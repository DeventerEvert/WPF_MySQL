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
using System.Windows.Threading;
using WPF_MySQL.Controllers;
using WPF_MySQL.Models;
using WPF_MySQL.Pages;

namespace WPF_MySQL.Views
{
	/// <summary>
	/// Interaction logic for startQuiz.xaml
	/// </summary>
	public partial class startQuiz : Window
	{
		private DispatcherTimer timer;
		private int secondsElapsed;
		private int progressionCount = 1;
		public List<questionPage> pages = new List<questionPage>();
		public List<questionPage> review = new List<questionPage>();
		public Quiztime quiztimeObject;
		private questionPage currentQuestionPage;
		private int currentPageIndex = 0;
		private int currentReviewPageIndex = 0;
		private Quiz selectedQuiz;
		private Question activeQuestion;


		public startQuiz(Controllers.Quiztime quiztimeObject, Quiz selectedQuiz)
		{
			InitializeComponent();
			this.quiztimeObject = quiztimeObject;
			this.selectedQuiz = selectedQuiz;
			activeQuestion = quiztimeObject.ActiveQuiz.Questions[currentPageIndex];
			quizTitel.Text = $"De geselecteerde quiz: {quiztimeObject.ActiveQuiz.Quizname}";
			progressionLabel.Content = $"Question {progressionCount} of {quiztimeObject.ActiveQuiz.Questions.Count}";
			initializeQuiz();
		}

		private void initializeQuiz()
		{
			foreach (Question question in quiztimeObject.ActiveQuiz.Questions)
			{
				questionPage questionPage = new questionPage(question);
				pages.Add(questionPage);
			}
			if (pages.Count > 0)
			{
				framePage.Navigate(pages[currentPageIndex]);
			}
		}

		public void initializeReviewQuiz()
		{
			review.Clear();

			foreach (Question question in quiztimeObject.ActiveQuiz.Questions)
			{
				questionPage reviewQuestionPage = new questionPage(question);
				review.Add(reviewQuestionPage);
			}
			if (review.Count > currentReviewPageIndex)
			{
				currentQuestionPage = review[currentReviewPageIndex];
				framePage.Navigate(review[currentReviewPageIndex]);
			}
		}

		public void PrevButtonClicked()
		{
			if (currentPageIndex > 0)
			{
				currentPageIndex--;
				if (progressionCount == 1)
				{
					MessageBox.Show("U bent al bij de eerste vraag");
				}
				else
				{
					progressionCount--;
				}
				progressionLabel.Content = $"Question {progressionCount} of {quiztimeObject.ActiveQuiz.Questions.Count}";
				initializeQuiz();
			}
		}

		public void progressReset()
		{
			progressionCount = 1;
			progressionLabel.Content = $"Question {progressionCount} of {quiztimeObject.ActiveQuiz.Questions.Count}";
		}

		public void NextButtonClicked()
		{
			if (currentPageIndex < pages.Count - 1)
			{
				if (currentPageIndex < quiztimeObject.ActiveQuiz.Questions.Count - 1)
					currentPageIndex++;
					progressionCount++;
					progressionLabel.Content = $"Question {progressionCount} of {quiztimeObject.ActiveQuiz.Questions.Count}";
					initializeQuiz();
			}
		}

		public void reviewBtnNextClicked()
		{
			if (currentReviewPageIndex < review.Count - 1)
			{ 
			Debug.WriteLine($"Review NextButtonClicked. Current index: {review.Count}");
			currentReviewPageIndex++;
			Debug.WriteLine($"Review PrevButtonClicked. New index: {currentReviewPageIndex}");
			Debug.WriteLine(progressionCount);
			initializeReviewQuiz();
			}
	}
		
	

		public void reviewBtnPrevClicked()
		{
			if (currentReviewPageIndex > 0)
			{
				currentReviewPageIndex--;
				initializeReviewQuiz();
			}
		}

		public int GetCorrectAnswerIndex()
		{
			int currentQuestionIndex = currentReviewPageIndex;


		if (currentQuestionIndex >= 0 && currentQuestionIndex < quiztimeObject.ActiveQuiz.Questions.Count)
			{
				Question currentQuestion = quiztimeObject.ActiveQuiz.Questions[currentQuestionIndex];

				for (int i = 0; i < currentQuestion.Answers.Count; i++)
				{
					if (currentQuestion.Answers[i].correct)
					{
						return i; 
					}
				}
			}

			return -1;
		}

	}
}
