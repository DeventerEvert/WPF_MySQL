using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MySQL.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MySQL.smallViews;
using WPF_MySQL.Controllers;
using System.Diagnostics;

namespace WPF_MySQL.Pages
{
	/// <summary>
	/// Interaction logic for questionPage.xaml
	/// </summary>

	public partial class questionPage : Page
	{
		public Question question;
		public questionPage(Question question)
		{
			InitializeComponent();

			this.question = question;
			this.DataContext = question;

			questionTitle.Content = question.questionText;
			if (question.image != null)
			{
				questionImage.Source = new BitmapImage(new Uri(question.image));
			}

			if (question.Answers[0].image != null)
			{
				answerSpace0.answerImage.Source = new BitmapImage(new Uri(question.Answers[0].image));
				answerSpace1.answerImage.Source = new BitmapImage(new Uri(question.Answers[1].image));
				answerSpace2.answerImage.Source = new BitmapImage(new Uri(question.Answers[2].image));
				answerSpace3.answerImage.Source = new BitmapImage(new Uri(question.Answers[3].image));
			}
			answerSpace0.Answer = question.Answers[0].answerText;
			answerSpace1.Answer = question.Answers[1].answerText;
			answerSpace2.Answer = question.Answers[2].answerText;
			answerSpace3.Answer = question.Answers[3].answerText;
		}

		public void HighlightCorrectAnswer(int correctAnswerIndex, bool isImage = false)
		{
			// Letters resetten
			answerSpace0.Foreground = Brushes.Black;
			answerSpace1.Foreground = Brushes.Black;
			answerSpace2.Foreground = Brushes.Black;
			answerSpace3.Foreground = Brushes.Black;

			// Check if multiple correct answers are allowed
			if (question.Type == true)
			{
				// Iterate through all answers and highlight the correct ones
				foreach (var answerControl in new List<answerControl>
		{
			answerSpace0,
			answerSpace1,
			answerSpace2,
			answerSpace3
		})
				{
					int answerIndex = int.Parse(answerControl.Name.Substring("answerSpace".Length));

					if (question.Answers[answerIndex].correct)
					{
						answerControl.Foreground = Brushes.Green;
						answerControl.BorderThickness = new Thickness(2);
						answerControl.BorderBrush = Brushes.Green;
						answerControl.Background = Brushes.LightGreen;
					}
				}
			}
			else
			{
				// Highlight a single correct answer
				answerControl correctAnswerTextBlock = FindName($"answerSpace{correctAnswerIndex}") as answerControl;

				if (correctAnswerTextBlock != null)
				{
					correctAnswerTextBlock.Foreground = Brushes.Green;
					correctAnswerTextBlock.BorderThickness = new Thickness(2);
					correctAnswerTextBlock.BorderBrush = Brushes.Green;
					correctAnswerTextBlock.Background = Brushes.LightGreen;
				}
			}
		}
	}
}