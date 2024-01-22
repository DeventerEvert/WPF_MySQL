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
    /// Interaction logic for switchQuestionValues.xaml
    /// </summary>
    public partial class switchQuestionValues : Window
    {
        private List<Question> allQuestions;
        public switchQuestionValues(List<Question> allQuestions)
        {
            InitializeComponent();

            if (allQuestions == null)
            {
                // Handle the case where allQuestions is null
                MessageBox.Show("Error: No questions available.");
                return;
            }

            this.allQuestions = allQuestions;

            for (int i = 0; i < allQuestions.Count; i++)
            {
                // Create a new label
                Label idLabel = new Label
                {
                    Width = 0,
                    Height = 0
                };

                TextBlock label = new TextBlock
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 0, 0, 7), // Adjust the margin as needed
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                TextBox textBox = new TextBox
                {
                    Margin = new Thickness(0, 0, 0, 5), // Adjust the margin as needed
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c")),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                TextBlock imageLabel = new TextBlock
                {
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 7), // Adjust the margin as needed
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Image imageBox = new Image
                {
                    Margin = new Thickness(0, 0, 0, 1), // Adjust the margin as needed
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 20,
                    Height = 20
                };

                Button newImageButton = new Button
                {
                    Content = "Change image",
                    Margin = new Thickness(0, 0, 0, 2), // Adjust the margin as needed
                    Foreground= Brushes.White,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2c2c2c")),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Left

                };

                newImageButton.Click += (sender, e) => newImageButtonClick(sender, e, imageBox, imageLabel);
                idLabel.Content = allQuestions[i].idQuestion;
                label.Text = $"Vraag {i + 1}:  ";
                textBox.Text = allQuestions[i].questionText;
                //imageLabel.Text = allQuestions[i].image;
                Debug.WriteLine(allQuestions[i].image);
                if (allQuestions[i].image == null)
                {
                    imageLabel.Text = "";
                }
                else
                {
                    imageBox.Source = new BitmapImage(new Uri(allQuestions[i].image));
                    imageLabel.Text = allQuestions[i].image;
                }

                // Add the label to your WPF window (assuming you have a container like a Grid)
                // Adjust the container based on your actual layout structure
                labelsContainer.Children.Add(label);
                textBoxContainer.Children.Add(textBox);
                imagePathContainer.Children.Add(imageLabel);
                imageBoxContainer.Children.Add(imageBox);
                imageButtonContainer.Children.Add(newImageButton);
                idBox.Children.Add(idLabel);


            }
        }
        private void newImageButtonClick(object sender, RoutedEventArgs e, Image imageBox, TextBlock imageLabel)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            bool? result = ofd.ShowDialog();

            if (result.HasValue && result.Value)
            {
                imageBox.Source = new BitmapImage(new Uri(ofd.FileName));
                imageLabel.Text = ofd.FileName;
            }
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            SQL sqlConnector = new SQL();

            // Iterate over each question
            for (int i = 0; i < allQuestions.Count; i++)
            {
                // Find the controls corresponding to the current question
                Label idLabel = idBox.Children.OfType<Label>().FirstOrDefault(l => l.Content.Equals(allQuestions[i].idQuestion));
                TextBlock imageLabel = imagePathContainer.Children.OfType<TextBlock>().ElementAt(i);
                TextBox textBox = textBoxContainer.Children.OfType<TextBox>().ElementAt(i);
                Image imageBox = imageBoxContainer.Children.OfType<Image>().ElementAt(i);

                string query = "UPDATE `question` SET `questionText` = @questionText, `image` = @image WHERE `idQuestion` = @idQuestion";
                using (MySqlCommand cmd = new MySqlCommand(query, sqlConnector.Connection))
                {
                    cmd.Parameters.AddWithValue("@idQuestion", idLabel.Content);
                    cmd.Parameters.AddWithValue("@questionText", textBox.Text);
                    cmd.Parameters.AddWithValue("@image", imageLabel.Text); 
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Questions updated!");
            this.Close();
        }
    }
}
