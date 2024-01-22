using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WPF_MySQL.Models;

namespace WPF_MySQL.Controllers
{
    public class Quiztime : SQL, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Models.Quiz _activeQuiz;
        private List<Models.Quiz> _quizzes = new List<Models.Quiz>();
        private Question _activeQuestion;

        // create constructor
        public Quiztime()
        {
            _quizzes = getQuizzes();
            NotifyPropertyChanged("Quizzes");
            
        }

        public void UpdateViewModel()
        {
            NotifyPropertyChanged("Quizzes");
            NotifyPropertyChanged("ActiveQuiz");
            NotifyPropertyChanged("ActiveQuestion");
            NotifyPropertyChanged("MyProperty");
           
        }


/// <summary>
/// Get questions for a quiz
/// </summary>
/// <param name="idQuiz">The quiz parameter</param>
/// <returns>Questions</returns>
        private List<Models.Question> Questions(int idQuiz)
        {
            List<Models.Question> questions = new List<Models.Question>();
            string query = "SELECT question.* FROM quiz " +
                "INNER JOIN quizquestion ON quiz.idQuiz = quizquestion.QuizID " +
                "INNER JOIN question ON quizquestion.QuestionID = question.idQuestion WHERE quiz.idQuiz = @idQuiz;";
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@idQuiz", idQuiz);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Models.Question question = new Models.Question();
                    question.idQuestion = reader.GetInt32(0);
                    question.Type = reader.GetInt32(1);
                    question.Desc = reader.GetString(2);
                    question.imagepath = reader.GetString(3);
                    questions.Add(question);
                }
                reader.Close();
                reader.Dispose();

            }
            return questions;
        }

        private List<Models.Quiz> getQuizzes()
        {
            List<Models.Quiz> quizzes = new List<Models.Quiz>();
            string query = "SELECT * FROM quiz";
         
            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Models.Quiz quiz = new Models.Quiz();
                    quiz.idQuiz = reader.GetInt32(0);
                    quiz.Date = reader.GetDateTime(1);
                    quiz.Name = reader.GetString(2);
                    quizzes.Add(quiz);
                }
                reader.Close();
                reader.Dispose();
            }
            return quizzes;
        }

        public void UpdateQuiz()
        {
            string query = "UPDATE quiz SET Name = @Name WHERE idQuiz = @idQuiz";
            //Quiz current_active = ActiveQuiz;

            using (MySqlCommand cmd = new MySqlCommand(query, Connection))
            {
                cmd.Parameters.AddWithValue("@Name", ActiveQuiz.Name);
                cmd.Parameters.AddWithValue("@idQuiz", ActiveQuiz.idQuiz);
                cmd.ExecuteNonQuery();
                // notify the views
                _quizzes = getQuizzes();
                //ActiveQuiz = current_active;
                ActiveQuiz = _quizzes.FirstOrDefault(q => q.idQuiz == ActiveQuiz.idQuiz);
                NotifyPropertyChanged("Quizzes");
            }
        }

        #region Properties


        // getter for the quizzes
        public List<Models.Quiz> Quizzes { 
            get {
                return _quizzes;
            } 
        }

        public Models.Quiz ActiveQuiz { 
            get { return _activeQuiz; } 
            set {
                // active quiz updated
                _activeQuiz = value;
                _activeQuiz.Questions = Questions(_activeQuiz.idQuiz);
                NotifyPropertyChanged("ActiveQuiz");
            } 
        }

        public Question ActiveQuestion { 
            get {
                return _activeQuestion; 
            }
            set {
                _activeQuestion = value;
                NotifyPropertyChanged();

            } 
        }

        public int MyProperty { get; set; }

        public Boolean MyTrueFalseProperty { get; set; }
        #endregion

        #region Binding notifiers

        public void DoNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
