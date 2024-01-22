using WPF_MySQL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using WPF_MySQL.Controllers;
using MySqlConnector;
using WPF_MySQL;
using Microsoft.Win32;

namespace WPF_MySQL.Views
{
    public partial class createQuiz : Window
    {
        public Quiz quiz;
        public bool IsEdit;


        public createQuiz()
        {
            InitializeComponent();
            quiz = new Quiz();
            txtboxQuizName.Text = "New Quiz";
            IsEdit = false;
        }

        public createQuiz(Quiz editQuiz)
        {
            InitializeComponent();
            quiz = editQuiz;
            IsEdit = true;
            Title = "Edit Quiz";
            if (quiz != null)
            {
                txtboxQuizName.Text = quiz.Quizname;
                if (!string.IsNullOrEmpty(quiz.Image))
                {
                    lblQuizImage.Content = null;
                    Uri imagePath = new Uri(quiz.Image);
                    BitmapImage bitmap = new BitmapImage(imagePath);
                    QuizImage.Source = bitmap;
                }
                foreach (Question question in quiz.Questions)
                {
                    int answerCount = 0;
                    if (question.Answers.Count > answerCount)
                    {
                        answerCount = question.Answers.Count;
                    }

                    this.listview_Questions.Items.Add(question);
                }
            }
            else
            {
                quiz = new Quiz();
                txtboxQuizName.Text = "New Quiz";
            }
        }



        private void btnNewQuestion_Click(object sender, RoutedEventArgs e)
        {
            createQuestion_Answers addQuestion = new createQuestion_Answers();
            addQuestion.quiz = quiz;
            addQuestion.IsEdit = false;
            if (quiz.Questions == null)
            {
                quiz.Questions = new List<Question>();
            }
            addQuestion.Closed += CreateQuestion_Closed;
            addQuestion.Show();
        }

        private void CreateQuestion_Closed(object sender, EventArgs e)
        {
            listview_Questions.Items.Clear();

            foreach (Question question in quiz.Questions)
            {
                int answerCount = question.Answers.Count;
                question.typeName = question.Type ? "Meervoud" : "Standaard";
                listview_Questions.Items.Add(question);
            }
        }
        private void btnQuizImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            bool? result = ofd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                Uri imagePath = new Uri(ofd.FileName);
                BitmapImage bitmap = new BitmapImage(imagePath);
                QuizImage.Source = bitmap;
                lblQuizImage.Content = null;
            }
        }

        private void btnFinalize_Click(object sender, RoutedEventArgs e)
        {
            if (quiz.Questions.Count < 0)
            {
                MessageBox.Show("A Quiz should have minimal 10 questions.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (IsEdit == false)
                {
                    SQL bla = new SQL();

                    long idQuiz = new long();
                    long idQuestion = new long();
                    long idAnswers = new long();

                    // Create Quiz
                    string query = "INSERT INTO `quiz` (`idQuiz`, `Quizname`, `Image`) " +
                                   "VALUES (NULL, @quizName, @quizImage)";
                    using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                    {
                        cmd.Parameters.AddWithValue("@quizName", txtboxQuizName.Text);
                        if (QuizImage.Source != null)
                        {
                            cmd.Parameters.AddWithValue("@quizImage", QuizImage.Source.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@quizImage", null);
                        }
                        cmd.ExecuteNonQuery();
                        idQuiz = cmd.LastInsertedId;
                    }
                    foreach (Question question in quiz.Questions)
                    {
                        //Create Question
                        query = "INSERT INTO question (idQuestion, questionText, image, idQuiz, type) " +
                                       "VALUES (NULL, @questionText, @questionImage, @idQuiz, @questionType)";

                        using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                        {
                            cmd.Parameters.AddWithValue("@questionText", question.questionText);
							if (question.image != "")
							{
								cmd.Parameters.AddWithValue("@questionImage", question.image);
							}
							else if (question.image == "")
							{
								cmd.Parameters.AddWithValue("@questionImage", null);
							}
							cmd.Parameters.AddWithValue("@idQuiz", idQuiz);
                            if (question.typeName == "Meervoud")
                            {
                                question.Type = true;
                            }
                            else
                            {
								question.Type = false;
							}
                            cmd.Parameters.AddWithValue("@questionType", question.Type);

                            cmd.ExecuteNonQuery();
                            idQuestion = cmd.LastInsertedId;
                        }

                        foreach (Answer answer in question.Answers)
                        {
                            //Create answer
                            query = "INSERT INTO answer (idAnswer, answerText, image, isCorrect, question_idQuestion) " +
                                    "VALUES (NULL, @answerText, @answerImage, @isCorrect, @question_idQuestion)";

                            using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                            {
                                cmd.Parameters.AddWithValue("@answerText", answer.answerText);
                                if(answer.image == "")
                                {
									cmd.Parameters.AddWithValue("@answerImage", null);
								}       
                                else
                                {
                                    cmd.Parameters.AddWithValue("@answerImage", answer.image);
                                }
                                cmd.Parameters.AddWithValue("@isCorrect", answer.correct);
                                cmd.Parameters.AddWithValue("@question_idQuestion", idQuestion);

                                cmd.ExecuteNonQuery();
                                idAnswers = cmd.LastInsertedId;
                            }
                        }
                    }

                    MessageBox.Show("Succesfully created Quiz!");
                    MainWindow main = new MainWindow();
                    quiz = null;
                    this.Close();
                    main.ShowDialog();
                }
                else
                {
                    SQL bla = new SQL();

                    long idQuiz = quiz.idQuiz;
                    long idQuestion = new long();
                    long idAnswers = new long();

                    // Update Quiz
                    string query = "UPDATE `quiz` SET `Quizname` = @quizName, `Image` = @quizImage WHERE `idQuiz` = @idQuiz";
                    using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                    {
                        cmd.Parameters.AddWithValue("@idQuiz", idQuiz);
                        cmd.Parameters.AddWithValue("@quizName", txtboxQuizName.Text);
                        if (QuizImage.Source != null)
                        {
                            cmd.Parameters.AddWithValue("@quizImage", QuizImage.Source.ToString());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@quizImage", null);
                        }
                        cmd.ExecuteNonQuery();
                    }

                    // Empty related question's idQuiz
                    query = "UPDATE question SET idQuiz = NULL WHERE idQuiz = @idQuiz";
                    using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                    {
                        cmd.Parameters.AddWithValue("@idQuiz", idQuiz);
                        cmd.ExecuteNonQuery();
                    }

                    foreach (Question question in quiz.Questions)
                    {
                        idQuestion = question.idQuestion;

                        //Update Question
                        if (idQuestion == 0)
                        {
                            query = "INSERT INTO question (idQuestion, questionText, image, idQuiz, type) " +
                                    "VALUES (NULL, @questionText, @questionImage, @idQuiz, @questionType)";
                        }
                        else
                        {
                            query = "UPDATE question SET questionText = @questionText, image = @questionImage, idQuiz = @idQuiz, type = @questionType WHERE idQuestion = @idQuestion";
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                        {
                            cmd.Parameters.AddWithValue("@questionText", question.questionText);
                            if (question.image != "")
                            {
								cmd.Parameters.AddWithValue("@questionImage", question.image);
                            }
                            else if(question.image == "")
                            {
								cmd.Parameters.AddWithValue("@questionImage", null);
							}
                            cmd.Parameters.AddWithValue("@idQuiz", idQuiz);
                            cmd.Parameters.AddWithValue("@questionType", question.Type);
                            if (idQuestion != 0)
                            {
                                cmd.Parameters.AddWithValue("@idQuestion", idQuestion);
                            }

                            cmd.ExecuteNonQuery();
                            if (idQuestion == 0)
                            {
                                idQuestion = cmd.LastInsertedId;
                            }
                        }


                        foreach (Answer answer in question.Answers)
                        {
                            bool update;
                            string checkQuery = "SELECT idAnswer FROM answer WHERE idAnswer = @answerid";
                            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, bla.Connection))
                            {
                                checkCmd.Parameters.AddWithValue("@answerid", answer.idAnswer);
                                object existingAnswerId = checkCmd.ExecuteScalar();

                                if (existingAnswerId != null)
                                {
                                    // Answer exists, update it
                                    query = "UPDATE answer SET answerText = @answerText, image = @answerImage WHERE idAnswer = @answerid";
                                    update = true;
                                }
                                else
                                {
                                    // Answer doesn't exist, insert it
                                    query = "INSERT INTO answer (idAnswer, answerText, image) VALUES (NULL, @answerText, @answerImage)";
                                    update = false;
                                }
                            }

                            using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                            {
                                cmd.Parameters.AddWithValue("@answerid", answer.idAnswer);
                                cmd.Parameters.AddWithValue("@answerText", answer.answerText);
                                cmd.Parameters.AddWithValue("@answerImage", answer.image);

                                cmd.ExecuteNonQuery();
                                idAnswers = cmd.LastInsertedId;

                            }

                            if (update == false)
                            {
                                query = "INSERT INTO `question_answer` (`idQuestion`, `idAnswer`, `correct`) " +
                                        "VALUES (@idQuestion, @idAnswer, @Correct)";

                                using (MySqlCommand cmd = new MySqlCommand(query, bla.Connection))
                                {
                                    cmd.Parameters.AddWithValue("@idQuestion", idQuestion);
                                    cmd.Parameters.AddWithValue("@idAnswer", idAnswers);
                                    cmd.Parameters.AddWithValue("@Correct", answer.correct);

                                    cmd.ExecuteNonQuery();
                                }
                            }

                        }

                    }
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }


            }
        }

        private void btnRemoveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (listview_Questions.SelectedItems.Count > 0)
            {
                quiz.Questions.RemoveAt(listview_Questions.SelectedIndex);
                listview_Questions.Items.RemoveAt(listview_Questions.SelectedIndex);
            }
        }

        private void btnEditQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (listview_Questions.SelectedItems.Count > 0)
            {
                createQuestion_Answers createQuestion = new createQuestion_Answers();
                createQuestion.quiz = quiz;
                createQuestion._newquestion = quiz.Questions[listview_Questions.SelectedIndex];
                createQuestion.questionIndex = listview_Questions.SelectedIndex;
                createQuestion.IsEdit = true;


                createQuestion.Title = "Edit Question";
                createQuestion.Closed += CreateQuestion_Closed;
                createQuestion.Show();
            }

        }
    }
}
