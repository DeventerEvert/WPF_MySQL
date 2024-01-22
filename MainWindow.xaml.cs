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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_MySQL.Models;
using WPF_MySQL.Controllers;
using System.ComponentModel;
using System.Windows.Media.Animation;
using WPF_MySQL.Views;

namespace WPF_MySQL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controllers.Quiztime quiztimeObject;

        TestWindow AudienceScreen;

        public MainWindow()
        {
      

            InitializeComponent();
            quiztimeObject = new Controllers.Quiztime(); // create instance of Quiztime class
            this.DataContext = quiztimeObject;
            
            // event handlers
            btnTest.Click += BtnTest_Click;
            btnOpenWindow.Click += BtnOpenWindow_Click;
            cmbCombo.SelectionChanged += CmbCombo_SelectionChanged;
            


        }

        private void BtnOpenWindow_Click(object sender, RoutedEventArgs e)
        {
           AudienceScreen = new TestWindow(quiztimeObject);
            // modal open window
            // tw.ShowDialog();
           AudienceScreen.Show();
        }

        private void CmbCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
            //Quiz _selectedQuiz = (Quiz)cmbCombo.SelectedItem;
            //quiztimeObject.ActiveQuiz = _selectedQuiz;
           


        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
           // Debug.WriteLine(quiztimeObject.ActiveQuiz.Name);
           // Debug.WriteLine(quiztimeObject.ActiveQuiz.Questions.Count.ToString());
           // create new datetime string
          

           quiztimeObject.ActiveQuiz.Name = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           quiztimeObject.UpdateQuiz();
            

        }

        private void btnQuestionClick(object sender, RoutedEventArgs e)
        {
             Button b = (Button)sender;
             Question Selected_Question = (Question)b.DataContext;
             quiztimeObject.ActiveQuestion = Selected_Question;


            //QuestionWindow questionWindow = new QuestionWindow(quiztimeObject); 
            //questionWindow.Show();

            AudienceScreen.setQuestion = Selected_Question;

            

        }
    }
}
