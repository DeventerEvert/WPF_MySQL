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

	public partial class reviewQuestionPage : Page
	{
		public Question question;
		public reviewQuestionPage(Question question)
		{
			InitializeComponent();
			this.question = question;
			questionTitle.Content = question.questionText;
			answerTextBtn.Text = question.Answers[0].answerText;
			answerTextBtn1.Text = question.Answers[1].answerText;
			answerTextBtn2.Text = question.Answers[2].answerText;
			answerTextBtn3.Text = question.Answers[3].answerText;
			if (question.Answers[0].image != "geen afbeelding")
			{
				answerImageBtn.Source = new BitmapImage(new Uri(question.Answers[0].image));
				answerImageBtn1.Source = new BitmapImage(new Uri(question.Answers[1].image));
				answerImageBtn2.Source = new BitmapImage(new Uri(question.Answers[2].image));
				answerImageBtn3.Source = new BitmapImage(new Uri(question.Answers[3].image));
			}
			if (question.Answers[1].image != "geen afbeelding")
			{
				answerImageBtn.Source = new BitmapImage(new Uri(question.Answers[0].image));
				answerImageBtn1.Source = new BitmapImage(new Uri(question.Answers[1].image));
				answerImageBtn2.Source = new BitmapImage(new Uri(question.Answers[2].image));
				answerImageBtn3.Source = new BitmapImage(new Uri(question.Answers[3].image));
			}
			else if (question.Answers[2].image != "geen afbeelding")
			{
				answerImageBtn.Source = new BitmapImage(new Uri(question.Answers[0].image));
				answerImageBtn1.Source = new BitmapImage(new Uri(question.Answers[1].image));
				answerImageBtn2.Source = new BitmapImage(new Uri(question.Answers[2].image));
				answerImageBtn3.Source = new BitmapImage(new Uri(question.Answers[3].image));
			}
			else if (question.Answers[3].image != "geen afbeelding")
			{
				answerImageBtn.Source = new BitmapImage(new Uri(question.Answers[0].image));
				answerImageBtn1.Source = new BitmapImage(new Uri(question.Answers[1].image));
				answerImageBtn2.Source = new BitmapImage(new Uri(question.Answers[2].image));
				answerImageBtn3.Source = new BitmapImage(new Uri(question.Answers[3].image));
			}
			if (question.image != "geen afbeelding")
			{
				questionImage.Source = new BitmapImage(new Uri(question.image));
			}

		}
	}
}
