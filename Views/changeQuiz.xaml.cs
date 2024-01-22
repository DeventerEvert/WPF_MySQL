using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WPF_MySQL.Models;
using WPF_MySQL.Views;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using WPF_MySQL.Controllers;
using MySqlConnector;
using WPF_MySQL;
using WPF_MySQL.smallViews;

namespace WPF_MySQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class changeQuiz : Window
    {
        public Quiz quiz;
        public Question _newquestion = new Question();
        public string questionImage;
        private int selectedId;
        private bool closeBool = true;

        Controllers.Quiztime quiztimeObject;
        public changeQuiz()
        {


            InitializeComponent();

            quiztimeObject = new Controllers.Quiztime(); // create instance of Quiztime class
            quiz = new Quiz();
            this.DataContext = quiztimeObject;


            // event handlers
            cmbCombo.SelectionChanged += CmbCombo_SelectionChanged;
        }

        public changeQuiz(int activeQuizId)
        {


            InitializeComponent();

            quiztimeObject = new Controllers.Quiztime(); // create instance of Quiztime class
            quiz = new Quiz();
            this.DataContext = quiztimeObject;
            cmbCombo.DisplayMemberPath = quiz.Quizname;


            // event handlers
            cmbCombo.SelectionChanged += CmbCombo_SelectionChanged;
        }


        private void CmbCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(quiztimeObject.ActiveQuiz.Image))
            {
                imgQuiz.Source = new BitmapImage(new Uri(quiztimeObject.ActiveQuiz.Image));
            }
            else
            {
                imgQuiz.Source = new BitmapImage(new Uri("https://i.imgur.com/6ZaQW.jpg"));
            }
            idQuizValue.Content = quiztimeObject.ActiveQuiz.idQuiz.ToString();
            listViewQuestions.Items.Clear();
            listAnswers.Items.Clear();

            //Quiz _selectedQuiz = (Quiz)cmbCombo.SelectedItem;
            //quiztimeObject.ActiveQuiz = _selectedQuiz;
            foreach (Question question in quiztimeObject.ActiveQuiz.Questions)
            {
                int AnswerCount = 0;
                if (question.Answers != null)
                {
                    if (question.Answers.Count > AnswerCount)
                    {
                        AnswerCount = question.Answers.Count;
                    }


                    question.typeName = question.Type ? "Meervoud" : "Standaard";

                    if (question.image == null)
                    {
                        question.image = null;
                    }
                    listViewQuestions.Items.Add(question);
                }
            }


        }

        private void listViewQuestions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            listAnswers.Items.Clear();

            Question selectedQuestion = (Question)listViewQuestions.SelectedItem;
            selectedId = selectedQuestion?.idQuestion ?? 0; // Update the class-level variable
            Debug.WriteLine(selectedId);

            if (selectedQuestion != null)
            {
                foreach (Answer answer in selectedQuestion.Answers)
                {
                    if (answer.image == null)
                    {
                        this.listAnswers.Items.Add(answer.answerText);
                    }
                    else if (answer.image != null)
                    {
                        this.listAnswers.Items.Add(answer.image);
                    }
                }
            }
        }



        #region editQuiz
        private void quizEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (quiztimeObject.ActiveQuiz == null)
            {
                MessageBox.Show("Selecteer een quiz");
            }
            else
            {
                string selectedValue = lblTest.Content.ToString();
                if (imgQuiz.Source == null)
                {
                    imgQuiz.Source = new BitmapImage(new Uri("https://i.imgur.com/6ZaQW.jpg"));
                }

                closeBool = false;
                if(closeBool == false)
                { 
                string imageValue = imgQuiz.Source.ToString();
                int activeQuizId = Convert.ToInt32(idQuizValue.Content);
                int selectedIndex = cmbCombo.SelectedIndex;
                Debug.WriteLine(selectedValue);
                switchQuizValues SwitchQuizValues = new switchQuizValues(selectedValue, imageValue, activeQuizId, selectedIndex);
                SwitchQuizValues.Show();
                this.Close();
				}
			}
        }
        #endregion

        #region editQuestion
        private void questionEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (quiztimeObject.ActiveQuiz == null)
            {
                MessageBox.Show("Selecteer een quiz");
            }
            else
            {
                List<Question> allQuestions = new List<Question>();

                foreach (Question questionItem in listViewQuestions.Items)
                {
                    allQuestions.Add(questionItem);
                }

                switchQuestionValues SwitchQuestionValues = new switchQuestionValues(allQuestions);
                SwitchQuestionValues.Show();
            }
        }

        #endregion

        private void answerEditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (quiztimeObject.ActiveQuiz == null)
            {
                MessageBox.Show("Selecteer een quiz");
            }
            else
            {
                List<Question> allQuestions = new List<Question>();

                foreach (Question questionItem in listViewQuestions.Items)
                {
                    allQuestions.Add(questionItem);
                }

                switchAnswerValues SwitchAnswerValues = new switchAnswerValues(allQuestions);
                SwitchAnswerValues.Show();
            }
        }

        #region removeQuestion
        //Om vraag te verwijderen

        private void removeQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (listViewQuestions.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecteer een quiz");
            }
            else
            {
                if (listViewQuestions.SelectedItems.Count > 0)
                {
                    Question selectedQuestion = (Question)listViewQuestions.SelectedItem;
                    MessageBoxResult result = MessageBox.Show("Weet u zeker dat u de vraag wilt verwijderen?", "Bevestig verwijdering", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                    if (result == MessageBoxResult.OK)
                    {
                        if (RemoveQuestionFromDatabase(selectedQuestion.idQuestion))
                        {
                            int selectedIndex = listViewQuestions.SelectedIndex;

                            listViewQuestions.Items.RemoveAt(selectedIndex);
                        }
                        else
                        {
                            MessageBox.Show("Failed to remove the question from the database.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Verwijdering geannuleerd.");
                    }
                }
            }
        }

        private bool RemoveQuestionFromDatabase(int idQuestion)
        {
            try
            {
                SQL sqlConnector = new SQL();

                using (MySqlCommand deleteAnswersCmd = new MySqlCommand("DELETE FROM answer WHERE question_idQuestion = @QuestionId", sqlConnector.Connection))
                {
                    deleteAnswersCmd.Parameters.AddWithValue("@QuestionId", idQuestion);
                    deleteAnswersCmd.ExecuteNonQuery();
                }

                using (MySqlCommand deleteQuestionCmd = new MySqlCommand("DELETE FROM question WHERE idQuestion = @QuestionId", sqlConnector.Connection))
                {
                    deleteQuestionCmd.Parameters.AddWithValue("@QuestionId", idQuestion);
                    int rowsAffected = deleteQuestionCmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing question from the database: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region removeQuiz

        private void removeQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            if (quiztimeObject.ActiveQuiz == null)
            {
                MessageBox.Show("Selecteer een quiz");
            }
            else
            {
                int selectedQuizId = quiztimeObject.ActiveQuiz.idQuiz;
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u de quiz wilt verwijderen?", "Bevestig verwijdering", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.OK)
                {
                    if (RemoveQuizFromDatabase(selectedQuizId))
                    {
                        MessageBox.Show("Quiz is verwijderd");
                        changeQuiz changeQuiz = new changeQuiz();
                        changeQuiz.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove the quiz from the database.");
                    }
                }
                else
                {
                    MessageBox.Show("Verwijdering geannuleerd.");
                }
            }
        }



        //Om quiz te verwijderen
        private bool RemoveQuizFromDatabase(int selectedQuizId)
        {
            try
            {
                SQL sqlConnector = new SQL();

                List<int> questionIds = new List<int>();

                using (MySqlCommand getQuestionIdsCmd = new MySqlCommand("SELECT idQuestion FROM question WHERE idQuiz = @QuizId", sqlConnector.Connection))
                {
                    getQuestionIdsCmd.Parameters.AddWithValue("@QuizId", selectedQuizId);

                    using (MySqlDataReader reader = getQuestionIdsCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int questionId = reader.GetInt32("idQuestion");
                            questionIds.Add(questionId);
                        }
                    }
                }

                // Delete answers related to the questions
                using (MySqlCommand deleteAnswersCmd = new MySqlCommand("DELETE FROM answer WHERE question_idQuestion = @QuestionId", sqlConnector.Connection))
                {
                    foreach (int questionId in questionIds)
                    {
                        deleteAnswersCmd.Parameters.Clear();
                        deleteAnswersCmd.Parameters.AddWithValue("@QuestionId", questionId);
                        deleteAnswersCmd.ExecuteNonQuery();
                    }
                }

                // Delete questions
                using (MySqlCommand deleteQuestionsCmd = new MySqlCommand("DELETE FROM question WHERE idQuiz = @QuizId", sqlConnector.Connection))
                {
                    deleteQuestionsCmd.Parameters.AddWithValue("@QuizId", selectedQuizId);
                    deleteQuestionsCmd.ExecuteNonQuery();
                }

                // Delete quiz
                using (MySqlCommand deleteQuizCmd = new MySqlCommand("DELETE FROM quiz WHERE idQuiz = @QuizId", sqlConnector.Connection))
                {
                    deleteQuizCmd.Parameters.AddWithValue("@QuizId", selectedQuizId);
                    deleteQuizCmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing quiz from the database: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region removeAnswer

        //Om antwoorden te verwijderen
        private void removeAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(listAnswers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een antwoord");
            }
            else 
            { 
            if (listAnswers.SelectedItems.Count > 0)
            {
                // Get the selected answerText
                string selectedAnswerText = listAnswers.SelectedItems[0].ToString();

                // Find the corresponding Question object based on the selected answerText
                Question selectedQuestion = quiztimeObject.ActiveQuiz.Questions.FirstOrDefault(q => q.Answers != null && q.Answers.Any(a => a.answerText == selectedAnswerText));
                MessageBoxResult result = MessageBox.Show("Weet u zeker dat u het antwoord wilt verwijderen?", "Bevestig verwijdering", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    if (selectedQuestion != null)
                    {
                        if (selectedQuestion.Answers != null && selectedQuestion.Answers.Count > 0)
                        {
                            // Access the Answers property directly
                            List<Answer> answers = selectedQuestion.Answers;

                            // Now you can work with the 'answers' list as needed
                            Answer selectedAnswer = answers.FirstOrDefault(a => a.answerText == selectedAnswerText);

                            if (selectedAnswer != null)
                            {
                                int idAnswer = selectedAnswer.idAnswer;

                                if (removeAnswerFromDatabase(idAnswer))
                                {
                                    int selectedAnswerIndex = listAnswers.SelectedIndex;
                                    listAnswers.Items.RemoveAt(selectedAnswerIndex);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to remove the answer from the database.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Selected answer not found in the question's answers.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No answers selected.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selected answer not associated with any question.");
                    }
                }
                else
                {
                    MessageBox.Show("Verwijdering geannuleerd.");
                }
            }
          }
		}

		private bool removeAnswerFromDatabase(int idAnswer)
        {
            try
            {
                SQL sqlConnector = new SQL();

                using (MySqlCommand deleteAnswersCmd = new MySqlCommand("DELETE FROM answer WHERE idAnswer = @AnswerId", sqlConnector.Connection))
                {
                    deleteAnswersCmd.Parameters.AddWithValue("@AnswerId", idAnswer);
                    int rowsAffected = deleteAnswersCmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing answer from the database: {ex.Message}");
                return false;
            }
        }


        #endregion

        private void addQuestionBtn_click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(idQuizValue.Content);
            if (idQuizValue.Content == "")
            {
                MessageBox.Show("Selecteer een quiz");
            }
            else
            {
                int activeQuizId = Convert.ToInt32(idQuizValue.Content);
                int selectedIndexValue = (int)cmbCombo.SelectedIndex;
                addQuestion_Answers AddQuestion_Answers = new addQuestion_Answers(activeQuizId);
                AddQuestion_Answers.quiz = quiz;
                AddQuestion_Answers.IsEdit = false;
                if (quiz.Questions == null)
                {
                    quiz.Questions = new List<Question>();
                }

                AddQuestion_Answers.ShowDialog();
            }
        }



        private void btnAnswer_click(object sender, RoutedEventArgs e)
        {
            if (selectedId != 0) // Check if an ID is selected
            {
                answerAdd answerAdd = new answerAdd(selectedId);
                answerAdd.ShowDialog();
            }
            else
            {
                // Handle the case when no ID is selected
                MessageBox.Show("Selecteer eerst een vraag");
            }
        }
        private void closeChangeQuiz_Window(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closeBool == true) 
            { 

            // Create an instance of changeQuiz
            MainWindow mainWindow = new MainWindow();

            // Hide the current window instead of closing it
            this.Visibility = Visibility.Hidden;

            // Show the changeQuiz window
            mainWindow.Show();
			}
		}
    }
}

