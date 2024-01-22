using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WPF_MySQL.Models;

namespace WPF_MySQL.Pages
{
	public partial class ReviewPage : Page
	{
		private Quiz quiz;
		private int currentQuestionIndex;
		private DispatcherTimer timer;
		private DispatcherTimer recapTimer;
		private List<string> allQuestions = new List<string>();
		private List<string> allCorrectAnswers = new List<string>();

		public ReviewPage(Quiz quiz)
		{
			InitializeComponent();
			this.quiz = quiz;
			currentQuestionIndex = 0;
			DisplayQuestion(currentQuestionIndex);
		}

		public void MoveToNextQuestion()
		{
			currentQuestionIndex++;
			DisplayQuestion(currentQuestionIndex);
		}

		public void MoveToPreviousQuestion()
		{
			if(currentQuestionIndex > 0)
			{
				currentQuestionIndex--;
				DisplayQuestion(currentQuestionIndex);
			}
		}

		public void WalkThroughQuestions()
		{
			currentQuestionIndex = 0;
		}

		public void displayRecap()
		{
			DisplayFullList();
		}

		private void RecapTimer_Tick(object sender, EventArgs e)
		{
			// Stop the recap timer
			recapTimer.Stop();

			// Display the final recap
			DisplayFullList();
		}
		private void DisplayQuestion(int questionIndex)
		{
			if (questionIndex < quiz.Questions.Count)
			{
				Question question = quiz.Questions[questionIndex];

				// Create a TextBlock for the question with larger font size
				TextBlock questionTextBlock = new TextBlock
				{
					Text = $"Question: {question.questionText}",
					Margin = new Thickness(0, 0, 0, 5),
					FontWeight = FontWeights.Bold,
					FontSize = 26,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};

				// Create an Image control for displaying the image
				Image imageControl = new Image();
				if (question.image != "geen afbeelding")
				{
					// Set the source of the Image control
					imageControl.Source = new BitmapImage(new Uri(question.image));
					imageControl.Height = 50;
					imageControl.Width = 50;

				}

				Answer correctAnswer = question.Answers.FirstOrDefault(a => a.correct);

				Image answerImage = new Image();
				if (question.Answers[0].image != "geen afbeelding")
				{
					// Set the source of the Image control
					answerImage.Source = new BitmapImage(new Uri(correctAnswer.image));
					answerImage.Height = 50;
					answerImage.Width = 50;
				}

				// Create a TextBlock for the correct answer with larger font size
				TextBlock correctAnswerTextBlock = new TextBlock
				{
					Text = $"Correct Answer: {GetCorrectAnswerText(question)}",
					Margin = new Thickness(0, 0, 0, 10),
					FontSize = 22,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};

				// Add the questions and correct answers to the lists
				allQuestions.Add(questionTextBlock.Text);
				allCorrectAnswers.Add(correctAnswerTextBlock.Text);

				// Add the controls to the StackPanel
				questionsPanel.Children.Clear();
				questionsPanel.Children.Add(questionTextBlock);
				if (!string.IsNullOrEmpty(question.image))
				{
					questionsPanel.Children.Add(imageControl);
				}
				questionsPanel.Children.Add(correctAnswerTextBlock);
				questionsPanel.Children.Add(answerImage);
			}
		}

		private void DisplayFullList()
		{
			// Create a TextBlock for the final recap with larger font size
			TextBlock finalRecapTextBlock = new TextBlock
			{
				Text = "Final Recap",
				Margin = new Thickness(0, 0, 0, 10),
				FontWeight = FontWeights.Bold,
				FontSize = 26,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			// Add the TextBlock to the StackPanel
			questionsPanel.Children.Clear();
			questionsPanel.Children.Add(finalRecapTextBlock);

			// Display all questions and their correct answers
			for (int i = 0; i < allQuestions.Count; i++)
			{
				Question question = quiz.Questions[i];

				TextBlock questionTextBlock = new TextBlock
				{
					Text = question.questionText,
					Margin = new Thickness(0, 0, 0, 5),
					FontWeight = FontWeights.Bold,
					FontSize = 18,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};

				// Create an Image control for displaying the image
				Image imageControl = new Image();
				if (question.image != "geen afbeelding")
				{
					// Set the source of the Image control
					imageControl.Source = new BitmapImage(new Uri(question.image));

					// Optionally set the size of the Image control
					imageControl.Width = 50; // Adjust the width as needed
					imageControl.Height = 50; // Adjust the height as needed
				}

				TextBlock correctAnswerTextBlock = new TextBlock
				{
					Text = $"Correct Answer: {GetCorrectAnswerText(question)}",
					Margin = new Thickness(0, 0, 0, 10),
					FontSize = 16,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};

				Image correctAnswerImage = new Image();
				Answer correctAnswer = question.Answers.FirstOrDefault(a => a.correct);
				if (question.Answers[0].image != "geen afbeelding")
				{
					correctAnswerImage.Source = new BitmapImage(new Uri(correctAnswer.image));

					correctAnswerImage.Width = 50; 
					correctAnswerImage.Height = 50; 
				}

				// Add the controls to the StackPanel
				questionsPanel.Children.Add(questionTextBlock);
				if (!string.IsNullOrEmpty(question.image))
				{
					questionsPanel.Children.Add(imageControl);
				}
				questionsPanel.Children.Add(correctAnswerTextBlock);
				questionsPanel.Children.Add(correctAnswerImage);
			}
		}

		private string GetCorrectAnswerText(Question question)
		{
			foreach (Answer answer in question.Answers)
			{
				if (answer.correct)
				{
					return answer.answerText;
				}
			}

			return "Correct answer not found";
		}
	}
}