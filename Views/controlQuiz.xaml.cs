using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPF_MySQL.Controllers;
using WPF_MySQL.Models;
using WPF_MySQL.Pages;
using System.Windows.Navigation;

namespace WPF_MySQL.Views
{
	/// <summary>
	/// Interaction logic for controlQuiz.xaml
	/// </summary>
	public partial class controlQuiz : Window
	{
		public Quiztime quiztimeObject;
		private startQuiz quizWindow;
		private int currentQuestionIndex = 0;
		int progressCountCounter = 1;
		private bool answerClicked = false;
		private bool reviewMode = false;
		private bool nextPrevForReview = false;
		private bool progressBool = false;
		public startQuiz QuizWindowInstance { get; set; }


		public controlQuiz(Controllers.Quiztime quiztimeObject, startQuiz quizWindow)
		{
			InitializeComponent();
			progressCount.Content = $"Vraag: {progressCountCounter}/{quiztimeObject.ActiveQuiz.Questions.Count}";
			this.quiztimeObject = quiztimeObject;
			this.quizWindow = quizWindow;
			QuizWindowInstance = quizWindow;
		}

		#region vorige en volgende buttons
		private void prevBtn_Click(object sender, RoutedEventArgs e)
		{
			if (nextPrevForReview == false)
			{
				if (quizWindow != null)
				{
					Question currentQuestion = quiztimeObject.ActiveQuiz.Questions[currentQuestionIndex];
					quizWindow.PrevButtonClicked();
					//clearAnswer();
					decrementQuestionIndex();
					//ShowCurrentQuestion();
				}
			}
			else if (nextPrevForReview == true)
			{
				progressBool = false;
				if (progressCountCounter > 1)
				{
					UpdateProgressCount();
					quizWindow.reviewBtnPrevClicked();
				}
			}
		}

		private void nextBtn_Click(object sender, RoutedEventArgs e)
		{
			if (nextPrevForReview == false)
			{
				if (quizWindow != null)
				{
					Question currentQuestion = quiztimeObject.ActiveQuiz.Questions[currentQuestionIndex];
					incrementQuestionIndex();
					quizWindow.NextButtonClicked();

					// If it's the last question, set reviewMode to true
					if (currentQuestionIndex == quiztimeObject.ActiveQuiz.Questions.Count - 1)
					{
						reviewMode = true;
					}
					if (reviewMode && currentQuestionIndex == quiztimeObject.ActiveQuiz.Questions.Count - 1)
					{
						reviewQuizBtn.Visibility = Visibility.Visible;

					}
				}
			}
			else if (nextPrevForReview == true)
			{
				progressBool = true;
				if(progressCountCounter < quiztimeObject.ActiveQuiz.Questions.Count) 
				{ 
				UpdateProgressCount();
				quizWindow.reviewBtnNextClicked();
				}
			}
		}





		private void incrementQuestionIndex()
		{
			if (currentQuestionIndex < quiztimeObject.ActiveQuiz.Questions.Count - 1)
			{
				currentQuestionIndex++;
				progressBool = true;
				UpdateProgressCount();
			}
		}

		private void decrementQuestionIndex()
		{
			if (currentQuestionIndex > 0)
			{
				currentQuestionIndex--;
				progressBool = false;
				UpdateProgressCount();
			}
		}

		private void UpdateProgressCount()
		{
			if (progressBool == true)
			{
				progressCountCounter++;
				progressCount.Content = $"Vraag: {progressCountCounter}/{quiztimeObject.ActiveQuiz.Questions.Count}";
			}
			else if (progressBool == false)
			{
				progressCountCounter--;
				progressCount.Content = $"Vraag: {progressCountCounter}/{quiztimeObject.ActiveQuiz.Questions.Count}";
			}
		}

		private void updateReviewCount()
		{
			progressCount.Content = $"Vraag: {progressCountCounter}/{quiztimeObject.ActiveQuiz.Questions.Count}";
		}


		#endregion

		#region antwoord tonen
		/*
			private void showAnswer_Click(object sender, RoutedEventArgs e)
		{
			// Check if the current question index is valid
			if (currentQuestionIndex >= 0 && currentQuestionIndex < quiztimeObject.ActiveQuiz.Questions.Count)
			{
				// Find the index of the correct answer
				int correctAnswerIndex = -1;
				for (int i = 0; i < quiztimeObject.ActiveQuiz.Questions[currentQuestionIndex].Answers.Count; i++)
				{
					if (quiztimeObject.ActiveQuiz.Questions[currentQuestionIndex].Answers[i].correct)
					{
						correctAnswerIndex = i;
						break;
					}
				}

				// Highlight the correct answer button
				if (correctAnswerIndex != -1)
				{
					HighlightCorrectAnswerButton(correctAnswerIndex);
				}
				else
				{
					// Handle the case where the correct answer index is not found
					MessageBox.Show("Correct answer index not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				// Handle the case where the current question index is out of bounds
				MessageBox.Show("Invalid question index.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void clearAnswer()
		{
			answerTextBtn.Background = Brushes.Transparent;
			answerTextBtn1.Background = Brushes.Transparent;
			answerTextBtn2.Background = Brushes.Transparent;
			answerTextBtn3.Background = Brushes.Transparent;
		}

		private void HighlightCorrectAnswerButton(int correctAnswerIndex)
		{
			// Reset background colors for all answer buttons
			answerTextBtn.Background = Brushes.Transparent;
			answerTextBtn1.Background = Brushes.Transparent;
			answerTextBtn2.Background = Brushes.Transparent;
			answerTextBtn3.Background = Brushes.Transparent;

			// Highlight the correct answer button with a green background
			switch (correctAnswerIndex)
			{
				case 0:
					answerTextBtn.Background = Brushes.Green;
					break;
				case 1:
					answerTextBtn1.Background = Brushes.Green;
					break;
				case 2:
					answerTextBtn2.Background = Brushes.Green;
					break;
				case 3:
					answerTextBtn3.Background = Brushes.Green;
					break;
				default:
					// Handle an invalid answer index (optional)
					break;
			}
		}
*/
		#endregion

		private void closeQuiz_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();

			quizWindow.Close();
			this.Close();
		}

		private void reviewQuizBtn_Click(object sender, RoutedEventArgs e)
		{

			if (QuizWindowInstance != null)
			{
				nextPrevForReview = true;		
				QuizWindowInstance.initializeReviewQuiz();
				progressCountCounter = 1;
				updateReviewCount();
				reviewQuizBtn.Visibility = Visibility.Collapsed;
				revealAnswer.Visibility = Visibility.Visible;
			}
		}

		private void revealAnswer_Click(object sender, RoutedEventArgs e)
		{
			int correctAnswerIndex = quizWindow.GetCorrectAnswerIndex();

			if (correctAnswerIndex != -1)
			{
				// Assuming framePage is a Frame control and Content is a questionPage
				if (quizWindow.framePage.Content is questionPage currentPage)
				{
					currentPage.HighlightCorrectAnswer(correctAnswerIndex);
					Debug.WriteLine($"Correct answer index: {correctAnswerIndex}");
				}
				else
				{
					// Handle the case where currentPage is not a questionPage
					MessageBox.Show("Current page is not a question page.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			else
			{
				// Handle the case where the correct answer index is not found
				MessageBox.Show("Correct answer index not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}