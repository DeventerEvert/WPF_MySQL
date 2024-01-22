using Microsoft.Win32;
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
using WPF_MySQL.Controllers;
using MySqlConnector;
using WPF_MySQL;
using System.Diagnostics;
using WPF_MySQL.Models;

namespace WPF_MySQL.Views
{
	public partial class answerAdd : Window
	{
		private int questionId; // Use a private class-level variable
		public answerAdd(int selectedId)
		{
			InitializeComponent();
			this.DataContext = this;
			Debug.WriteLine($"Selected ID: {selectedId}");

			// Set the class-level variable
			questionId = selectedId;
		}

		private void btnUploadImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

			bool? result = ofd.ShowDialog();

			if (result.HasValue && result.Value)
			{
				lblAnswerImage.Content = ofd.FileName;
				imgContent.Source = new BitmapImage(new Uri(ofd.FileName));
			}
		}

		private void addBtn_Click(object sender, RoutedEventArgs e)
		{
			SQL sqlConnector = new SQL();
			Debug.WriteLine($"Question ID: {questionId}");
			string query = "INSERT INTO answer (idAnswer, answerText, image, isCorrect, question_idQuestion) VALUES (@idAnswer, @answerText, @image, @isCorrect, @question_idQuestion);";
			using (MySqlCommand cmd = new MySqlCommand(query, sqlConnector.Connection))
			{
				cmd.Parameters.AddWithValue("@idAnswer", null);
				cmd.Parameters.AddWithValue("@answerText", txtAnswer.Text);
				cmd.Parameters.AddWithValue("@image", lblAnswerImage.Content.ToString());
				cmd.Parameters.AddWithValue("@isCorrect", lblCorrect.Content.ToString());
				cmd.Parameters.AddWithValue("@question_idQuestion", questionId);
				cmd.ExecuteNonQuery();
			}

			MessageBox.Show("Antwoord toegevoegd!");
			this.Close();
		}
	}
}