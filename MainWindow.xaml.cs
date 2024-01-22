using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WPF_MySQL.Models;
using WPF_MySQL.Views;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System;
using WPF_MySQL.Controllers;

namespace WPF_MySQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Question _newquestion = new Question();
        public string questionImage;

        Controllers.Quiztime quiztimeObject;
        public MainWindow()
        {


            InitializeComponent();
            quiztimeObject = new Controllers.Quiztime(); // create instance of Quiztime class
            this.DataContext = quiztimeObject;

            // event handlers
            cmbCombo.SelectionChanged += CmbCombo_SelectionChanged;
        }

        private void CmbCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbCombo = (ComboBox)sender;
            if (!string.IsNullOrEmpty(quiztimeObject.ActiveQuiz.Image))
            {
                imgQuiz.Source = new BitmapImage(new Uri(quiztimeObject.ActiveQuiz.Image));
            }
            Quiz selectedQuiz = (Quiz)cmbCombo.SelectedItem;
            quiztimeObject.ActiveQuiz = selectedQuiz;

            listViewQuestions.Items.Clear();
            listAnswers.Items.Clear();

            //Quiz _selectedQuiz = (Quiz)cmbCombo.SelectedItem;
            //quiztimeObject.ActiveQuiz = _selectedQuiz;
            foreach (Question question in quiztimeObject.ActiveQuiz.Questions)
            {
                int AnswerCount = 0;
                if (question.Answers != null)
                {
                    Debug.WriteLine($"answers: {question.Answers.Count}");
                    if (question.Answers.Count > AnswerCount)
                    {
                        AnswerCount = question.Answers.Count;
                    }

                    question.typeName = question.Type ? "Meervoud" : "Standaard";

                    listViewQuestions.Items.Add(question);
                }
            }


        }

        private void btnQuestionClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Question Selected_Question = (Question)b.DataContext;
            quiztimeObject.ActiveQuestion = Selected_Question;


            //QuestionWindow questionWindow = new QuestionWindow(quiztimeObject); 
            //questionWindow.Show();



        }

        private void listViewQuestions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            listAnswers.Items.Clear();

            Question selectedQuestion = (Question)listViewQuestions.SelectedItem;

            if (selectedQuestion.Answers != null)
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
            else
            {
                MessageBox.Show("Er zijn geen antwoorden");
            }
        }


        private void changeQuiz_click(object sender, RoutedEventArgs e)
        {

            createQuiz CreateQuiz = new createQuiz();
            CreateQuiz.Show();
            this.Close();

        }


        private void changeQuiz_btn_Click(object sender, RoutedEventArgs e)
        {
            changeQuiz changeQuiz = new changeQuiz();
            changeQuiz.Show();
            this.Close();
        }

		private void startQuiz_click(object sender, RoutedEventArgs e)
		{
			if (cmbCombo.SelectedItem == null)
			{
				MessageBox.Show("Selecteer een quiz");
				return;
			}
			else
			{
				// Directly reference the cmbCombo from XAML
				ComboBox cmbCombo = this.cmbCombo;

				// Assuming that your ComboBox is bound to Quiz objects
				if (cmbCombo.SelectedItem is Quiz selectedQuiz)
				{

					startQuiz StartQuiz = new startQuiz(quiztimeObject, selectedQuiz);
					StartQuiz.Show();

					controlQuiz ControlQuiz = new controlQuiz(quiztimeObject, StartQuiz);
					ControlQuiz.Show();

					this.Close();
				}
			}
		}
	}
}
