using WPF_MySQL.Models;
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
using System.Windows.Shapes;
using System.Xml;
using WPF_MySQL.Controllers;
using MySqlConnector;
using WPF_MySQL;
using Microsoft.Win32;

namespace WPF_MySQL
{


	public partial class switchQuizValues : Window
	{
		public Quiz _newquiz = new Quiz();
		private Quiztime quizValue;
		int activeQuizId;
		int selectedIndex;
		changeQuiz changeQuizWindow;
		public switchQuizValues(string selectedValue, string imageValue, int activeQuizId, int selectedIndex)
		{
			InitializeComponent();
			selectedTitleValue.Text = selectedValue;
			selectedImageValue.Text = imageValue;
			selectedQuizId.Content = activeQuizId;
			this.selectedIndex = selectedIndex;
			imgQuiz.Source = new BitmapImage(new Uri(imageValue));
			DataContext = _newquiz;
		}

		private void changeImgButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

			bool? result = ofd.ShowDialog();

			if (result.HasValue && result.Value)
			{
				imgQuiz.Source = new BitmapImage(new Uri(ofd.FileName));
				selectedImageValue.Text = ofd.FileName;
			}
		}

		private void quizSubmitButton_Click(object sender, RoutedEventArgs e)
		{
			SQL sqlConnector = new SQL();

			string query = "UPDATE `quiz` SET `Quizname` = @quizName, `Image` = @quizImage WHERE `idQuiz` = @quizId";
			using (MySqlCommand cmd = new MySqlCommand(query, sqlConnector.Connection))
			{
				cmd.Parameters.AddWithValue("@quizId", selectedQuizId.Content);
				cmd.Parameters.AddWithValue("@quizImage", selectedImageValue.Text);
				cmd.Parameters.AddWithValue("@quizName", selectedTitleValue.Text);
				cmd.ExecuteNonQuery();
			}

			MessageBox.Show("Quiz geupdate!");
			//changeQuiz
			//changeQuiz changeQuiz = new changeQuiz();
			//changeQuiz.cmbCombo.SelectedIndex = selectedIndex;
			//this.Visibility = Visibility.Hidden;
			//changeQuiz.Show();
		}

		private void quizWindow_Close(object sender, System.ComponentModel.CancelEventArgs e)
		{

				// Create an instance of changeQuiz
				changeQuiz changeQuiz = new changeQuiz();
				changeQuiz.cmbCombo.SelectedIndex = selectedIndex;

				// Hide the current window instead of closing it
				this.Visibility = Visibility.Hidden;

				// Show the changeQuiz window
				changeQuiz.Show();
		}
	}
}
