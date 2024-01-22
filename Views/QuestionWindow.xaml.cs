using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WPF_MySQL.Views
{
    /// <summary>
    /// Interaction logic for QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        

        Quiztime quiztimeObject; // use instance of Quiztime class 
        public QuestionWindow(Quiztime vm)
        {
            InitializeComponent();
            quiztimeObject = vm;
            this.DataContext = quiztimeObject;
                        
        }

       
    }
}
