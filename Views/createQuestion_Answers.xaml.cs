using System;
using WPF_MySQL.smallViews;
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
using WPF_MySQL.Models;
using Microsoft.Win32;
using System.Diagnostics;

namespace WPF_MySQL
{
	public partial class createQuestion_Answers : Window
	{
		public Quiz quiz;
		public Question _newquestion = new Question();
		public string questionImage;
		public int questionIndex;
		public bool IsEdit;

		public createQuestion_Answers()
		{
			InitializeComponent();
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

			// Enable or show the controls that allow multiple correct answers
			// For example, you can enable checkboxes in your Answer control.
			foreach (NewAnswer newAnswer in grdAnswers.Children.OfType<NewAnswer>())
			{
				newAnswer.cbxCorrect.IsEnabled = true;
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			_newquestion.Type = false;

			// Disable or hide the controls that allow multiple correct answers
			// For example, you can disable checkboxes in your Answer control.
			foreach (NewAnswer newAnswer in grdAnswers.Children.OfType<NewAnswer>())
			{
				newAnswer.cbxCorrect.IsEnabled = false;
			}
		}

		private void btnSubmit_click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("functioning");
			_newquestion.questionText = lblQuestionName.Text;
			_newquestion.image = (string)lblQuestionImage.Content;
			_newquestion.idQuiz = quiz.idQuiz;

			List<Answer> answers = new List<Answer>();
			foreach (NewAnswer newAnswer in grdAnswers.Children.OfType<NewAnswer>())
			{
				Answer answer = new Answer();
				answer.answerText = newAnswer.txtAnswer.Text;
				answer.image = newAnswer.lblAnswerImage.Content.ToString();

				// Check if Type is true to allow multiple correct answers
				if (_newquestion.Type == true)
				{
					answer.correct = (bool)newAnswer.cbxCorrect.IsChecked; 
				}
				else
				{
					// If Type is false, set correct based on a single correct answer logic
					answer.correct = (bool)newAnswer.cbxCorrect.IsChecked;

					// You may want to adjust this logic based on your requirements
					if (answer.correct)
					{
						// Clear other correct answers if only one is allowed
						foreach (var otherAnswer in answers)
						{
							if (otherAnswer != answer)
							{
								otherAnswer.correct = false;
							}
						}
					}
				}

				answers.Add(answer);
			}
			_newquestion.Answers = answers;

			if (!IsEdit)
			{
				quiz.Questions.Add(_newquestion);
			}
			this.Close();
		}
	}
}


