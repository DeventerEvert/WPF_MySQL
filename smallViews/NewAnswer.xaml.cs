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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_MySQL.smallViews
{
    /// <summary>
    /// Interaction logic for NewAnswer.xaml
    /// </summary>
    public partial class NewAnswer : UserControl
    {
        public int question_idQuestion { get; set; }
        public int idAnswer { get; set; }
        public string answerText { get; set; }
        public string image { get; set; }
        public bool correct { get; set; }
        public NewAnswer()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnDeleteAnswer_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
        }

        private void txtAnswer_TextInput(object sender, TextCompositionEventArgs e)
        {
            answerText = txtAnswer.Text;
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
    }
}