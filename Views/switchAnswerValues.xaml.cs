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
using System.Diagnostics;

namespace WPF_MySQL.Views
{
	/// <summary>
	/// Interaction logic for switchAnswerValues.xaml
	/// </summary>
	public partial class switchAnswerValues : Window
	{
		private List<Question> allQuestions;
		public switchAnswerValues(List<Question> allQuestions)
		{
			InitializeComponent();
			if (allQuestions == null)
			{
				MessageBox.Show("Error: No questions available.");
				return;
			}

			this.allQuestions = allQuestions;

			for (int i = 0; i < allQuestions.Count; i++)
			{
				TextBlock questionLabel = new TextBlock
				{
					FontWeight = FontWeights.Bold,
					Margin = new Thickness(0, 0, 0, 75),
					Foreground = Brushes.White,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Left
				};

				questionLabel.Text = $"Vraag {allQuestions[i].questionText}:  ";
				labelsContainer.Children.Add(questionLabel);

				for (int j = 0; j < allQuestions[i].Answers.Count; j++)
				{
					TextBox answerTextBox = new TextBox
					{
						Margin = new Thickness(0, 0, 0, 5),
						Foreground = Brushes.White,
						Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c")),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Left
					};
					answerTextBox.Text = $"{allQuestions[i].Answers[j].answerText}";
					textBoxContainer.Children.Add(answerTextBox);

					Label idLabel = new Label
					{
						Width = 0,
						Height = 0
					};

					idLabel.Content = allQuestions[i].Answers[j].idAnswer;
					idBox.Children.Add(idLabel);

					TextBlock imagePath = new TextBlock
					{
						Margin = new Thickness(0, 0, 0, 7),
						Foreground = Brushes.White,
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Left
					};
					imagePath.Text = $"{allQuestions[i].Answers[j].image}";
					imagePathContainer.Children.Add(imagePath);

					Image imageBox = new Image
					{
						Margin = new Thickness(0, 0, 0, 3),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Left,
						Width = 20,
						Height = 20
					};

					try
					{
						// Attempt to set the image source from the provided path
						imageBox.Source = new BitmapImage(new Uri(allQuestions[i].Answers[j].image));
					}
					catch (Exception ex)
					{
						// Handle the exception (e.g., image path is null or invalid)
						Console.WriteLine($"Error loading image: {ex.Message}");

						// Set a placeholder image or take other appropriate action
						imageBox.Source = null;
					}
					CheckBox checkBox = new CheckBox
					{
						Content = "Select",
						Foreground = Brushes.White,
						Margin = new Thickness(0, 0, 0, 8),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Left,
						IsChecked = allQuestions[i].Answers[j].correct  // Set IsChecked based on 'correct' property
					};

					// Create local variables to capture the current values of i and j
					int currentI = i;
					int currentJ = j;

					checkBox.Checked += (sender, args) =>
					{
						Answer selectedAnswer = allQuestions[currentI].Answers[currentJ];
						selectedAnswer.correct = true;
					};

					checkBox.Unchecked += (sender, args) =>
					{
						Answer selectedAnswer = allQuestions[currentI].Answers[currentJ];
						selectedAnswer.correct = false;
					};

					// Add the RadioButton to the container
					radioButtonContainer.Children.Add(checkBox);

					// Add the Image to the container
					imageBoxContainer.Children.Add(imageBox);

					Button newImageButton = new Button
					{
						Content = "Change image",
						Foreground = Brushes.White,
						Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c")),
						Margin = new Thickness(0, 0, 0, 3),
						VerticalAlignment = VerticalAlignment.Center,
						HorizontalAlignment = HorizontalAlignment.Left
					};

					newImageButton.Click += (sender, e) => newImageButtonClick(sender, e, imageBox, imagePath);

					imageButtonContainer.Children.Add(newImageButton);
				}

			}
		}

		private void newImageButtonClick(object sender, RoutedEventArgs e, Image imageBox, TextBlock imagePath)
		{
			Button clickedButton = sender as Button;

			int index = imageButtonContainer.Children.IndexOf(clickedButton);

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

			bool? result = ofd.ShowDialog();

			if (result.HasValue && result.Value)
			{
				imageBox.Source = new BitmapImage(new Uri(ofd.FileName));
				imagePath.Text = ofd.FileName;
			}
		}

		private void submitBtn_Click(object sender, RoutedEventArgs e)
		{
			SQL sqlConnector = new SQL();

			for (int i = 0; i < allQuestions.Count; i++)
			{
				for (int j = 0; j < allQuestions[i].Answers.Count; j++)
				{
					// Retrieve controls for the current question and answer
					TextBox answerTextBox = textBoxContainer.Children.OfType<TextBox>().ElementAt(i * allQuestions[i].Answers.Count + j);
					Label idLabel = idBox.Children.OfType<Label>().ElementAt(i * allQuestions[i].Answers.Count + j);
					TextBlock imagePath = imagePathContainer.Children.OfType<TextBlock>().ElementAt(i * allQuestions[i].Answers.Count + j);
					Image imageBox = imageBoxContainer.Children.OfType<Image>().ElementAt(i * allQuestions[i].Answers.Count + j);
					CheckBox checkBox = radioButtonContainer.Children.OfType<CheckBox>().ElementAt(i * allQuestions[i].Answers.Count + j);

					Answer currentAnswer = allQuestions[i].Answers[j];

					string answerQuery = "UPDATE `answer` SET `answerText` = @answerText, `image` = @image, `isCorrect` = @isCorrect WHERE `idAnswer` = @idAnswer";
					using (MySqlCommand answerCmd = new MySqlCommand(answerQuery, sqlConnector.Connection))
					{
						answerCmd.Parameters.AddWithValue("@idAnswer", currentAnswer.idAnswer);
						answerCmd.Parameters.AddWithValue("@answerText", answerTextBox.Text);
						if(imagePath.Text == "")
						{
							answerCmd.Parameters.AddWithValue("@image", null);
						}
						else
						{
							answerCmd.Parameters.AddWithValue("@image", imagePath.Text);
						}
						answerCmd.Parameters.AddWithValue("@isCorrect", checkBox.IsChecked);
						answerCmd.ExecuteNonQuery();
					}
				}
			}

			MessageBox.Show("Answers updated!");
			this.Close();
		}

	}
}