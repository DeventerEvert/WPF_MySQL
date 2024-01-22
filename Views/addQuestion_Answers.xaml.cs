using WPF_MySQL.Models;
using System;
using System.Diagnostics;
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
using WPF_MySQL.smallViews;

namespace WPF_MySQL.Views
{
	/// <summary>
	/// Interaction logic for addQuestion_Answers.xaml
	/// </summary>
	public partial class addQuestion_Answers : Window
	{ 
    public Quiz quiz;
	public List<Quiz> Quizzes;
	public Question _newquestion = new Question();
	public Quiztime quiztimeInstance = new Quiztime();
	public string questionImage;
	public int questionIndex;
	public bool IsEdit;
	public int activeQuizId;
	public int selectedIndexValue;

		public addQuestion_Answers(int activeQuizId)
	{
		InitializeComponent();
		activeQuiz.Content = activeQuizId;
		this.activeQuizId = activeQuizId;
	}

	private void btnQuestionImage_Click(object sender, RoutedEventArgs e)
	{
		OpenFileDialog ofd = new OpenFileDialog();
		ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

		bool? result = ofd.ShowDialog();

		if (result.HasValue && result.Value)
		{
			questionImageBox.Source = new BitmapImage(new Uri(ofd.FileName));
			lblQuestionImage.Content = ofd.FileName;
		}
	}

	private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
	{
		NewAnswer newAnswer = new NewAnswer();
	}

	private void CheckBox_Checked(object sender, RoutedEventArgs e)
	{
		_newquestion.Type = true;
	}

		#region onSubmit
		private void btnSubmit_click(object sender, RoutedEventArgs e)
		{
			_newquestion.questionText = lblQuestionName.Text;
			_newquestion.image = (string)lblQuestionImage.Content;
			Debug.WriteLine("functioning");

				SQL sqlConnector = new SQL();

				// Insert question
				string insertQuestionQuery = "INSERT INTO question (idQuestion, questionText, image, Type, idQuiz) VALUES (@idQuestion, @questionText, @image, @Type, @idQuiz); ";
				using (MySqlCommand cmd = new MySqlCommand(insertQuestionQuery, sqlConnector.Connection))
				{
					cmd.Parameters.AddWithValue("@idQuiz", activeQuiz.Content);
					cmd.Parameters.AddWithValue("@idQuestion", _newquestion.idQuestion);
					cmd.Parameters.AddWithValue("@questionText", _newquestion.questionText);
					if(_newquestion.image == "")
					{
						cmd.Parameters.AddWithValue("@image", null);
					}
					else
					{
						cmd.Parameters.AddWithValue("@image", _newquestion.image);

					}

					if(_newquestion.Type == true)
					{
						cmd.Parameters.AddWithValue("@Type", 1);
					}
					else
					{
						cmd.Parameters.AddWithValue("@Type", 0);
					}

					cmd.ExecuteNonQuery();
				}

				// Get the ID of the last inserted question
				string getLastInsertedIdQuery = "SELECT LAST_INSERT_ID()";
				int questionId;
				using (MySqlCommand cmd = new MySqlCommand(getLastInsertedIdQuery, sqlConnector.Connection))
				{
					questionId = Convert.ToInt32(cmd.ExecuteScalar());
				}

				List<Answer> answers = new List<Answer>();
				foreach (NewAnswer newAnswer in grdAnswers.Children.OfType<NewAnswer>())
				{
					Answer answer = new Answer();
					answer.answerText = newAnswer.txtAnswer.Text;
					answer.image = newAnswer.lblAnswerImage.Content.ToString();
					answer.correct = (bool)newAnswer.cbxCorrect.IsChecked;

					answers.Add(answer);
				}
				_newquestion.Answers = answers;

				// Insert answers
				foreach (Answer answer in _newquestion.Answers)
					{
						string insertAnswerQuery = "INSERT INTO answer (question_idQuestion, answerText, image, isCorrect) VALUES (@idQuestion, @answerText, @image, @correct)";
						using (MySqlCommand cmd = new MySqlCommand(insertAnswerQuery, sqlConnector.Connection))
						{
							cmd.Parameters.AddWithValue("@idQuestion", questionId);
							cmd.Parameters.AddWithValue("@answerText", answer.answerText);
						if(answer.image == "")
						{
								cmd.Parameters.AddWithValue("@image", null);
						}
						else
						{
								cmd.Parameters.AddWithValue("@image", answer.image);

						}
							
							cmd.Parameters.AddWithValue("@correct", answer.correct);

							cmd.ExecuteNonQuery();


						}
				 }
					_newquestion.Answers = quiztimeInstance.Answers(_newquestion.idQuestion);
					

			if (!IsEdit)
				{
				//vragen hoe standaard neer te zetten hier		
				quiz.Questions.Add(_newquestion);
				}

			    Quizzes = quiztimeInstance.getQuizzes();
				MessageBox.Show("Vraag toegevoegd!");
				this.Close();			
			}
		#endregion

	}
}



