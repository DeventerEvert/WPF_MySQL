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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MySQL.Models;
using WPF_MySQL.smallViews;

namespace WPF_MySQL.smallViews
{
	/// <summary>
	/// Interaction logic for answerControl.xaml
	/// </summary>
	public partial class answerControl : UserControl
	{
		public static readonly DependencyProperty LeftLabelProperty =
		 DependencyProperty.Register("LeftLabel", typeof(string), typeof(answerControl));

		public string LeftLabel
		{
			get { return (string)GetValue(LeftLabelProperty); }
			set { SetValue(LeftLabelProperty, value); }
		}

		public answerControl()
		{
			InitializeComponent();
		}


		//Voeg een Dependency Property toe voor de Answer
		public string Answer
		{
			get { return (string)GetValue(AnswerProperty); }
			set { SetValue(AnswerProperty, value); }
		}

		public static readonly DependencyProperty AnswerProperty =
			DependencyProperty.Register("Answer", typeof(string), typeof(answerControl), new PropertyMetadata(string.Empty, OnSetAnswerChanged));

		private static void OnSetAnswerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			answerControl answerControl = d as answerControl;
			answerControl.OnSetAnswerChanged(e);
		}

		private void OnSetAnswerChanged(DependencyPropertyChangedEventArgs e)
		{
			answerText.Text = e.NewValue.ToString();
		}


		public string AnswerImage
		{
			get { return (string)GetValue(AnswerImageProperty); }
			set { SetValue(AnswerImageProperty, value); }
		}

		public static readonly DependencyProperty AnswerImageProperty =
			DependencyProperty.Register("AnswerImage", typeof(string), typeof(answerControl), new PropertyMetadata(string.Empty, OnSetAnswerImageChanged));

		private static void OnSetAnswerImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			answerControl answerControl = d as answerControl;
			answerControl.OnSetAnswerImageChanged(e);
		}

		private void OnSetAnswerImageChanged(DependencyPropertyChangedEventArgs e)
		{
			answerImage.Source = new BitmapImage(new Uri(e.NewValue.ToString()));
		}
	}
}