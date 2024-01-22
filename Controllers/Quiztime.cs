using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using WPF_MySQL.Models;

namespace WPF_MySQL.Controllers
{
	public class Quiztime : SQL, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private Quiz _activeQuiz;
		private List<Quiz> _quizzes = new List<Quiz>();
		private Question _activeQuestion;
		private int _myProperty;

		// create constructor
		public Quiztime()
		{
			_quizzes = getQuizzes();
			NotifyPropertyChanged(nameof(Quizzes));
		}

		public void UpdateViewModel()
		{
			NotifyPropertyChanged(nameof(Quizzes));
			NotifyPropertyChanged(nameof(ActiveQuiz));
			NotifyPropertyChanged(nameof(ActiveQuestion));
			NotifyPropertyChanged(nameof(MyProperty));
		}

		private List<Question> Questions(int idQuiz)
		{
			List<Question> questions = new List<Question>();
			string query = @"SELECT * FROM question WHERE idQuiz = @idQuiz";
			using (MySqlCommand cmd = new MySqlCommand(query, Connection))
			{
				cmd.Parameters.AddWithValue("@idQuiz", idQuiz);
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					Question question = new Question();
					question.idQuestion = reader.GetInt32(0);
					question.questionText = reader.GetString(1);
					if (!reader.IsDBNull(2))
					{
						question.image = reader.GetString(2);
					}
					question.idQuiz = reader.GetInt32(3);
					question.Type = reader.GetBoolean(4);
					questions.Add(question);
				}
				reader.Close();
				reader.Dispose();
			}
			foreach (Question question in questions)
			{
				try { question.Answers = Answers(question.idQuestion); } catch { }
			}
			return questions;
		}

		public List<Answer> Answers(int idQuestion)
		{
			string query = @"SELECT * FROM answer WHERE question_idQuestion = @idQuestion ";
			List<Answer> answers = new List<Answer>();
			using (MySqlCommand cmd = new MySqlCommand(query, Connection))
			{
				cmd.Parameters.AddWithValue("@idQuestion", idQuestion);
				MySqlDataReader reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					Answer answer = new Answer();
					answer.idAnswer = reader.GetInt32(0);
					answer.answerText = reader.GetString(1);
					if (!reader.IsDBNull(2))
					{
						answer.image = reader.GetString(2);
					}
					answer.correct = reader.GetBoolean(3);
					answer.question_idQuestion = reader.GetInt32(4);
					answers.Add(answer);
				}

				reader.Close();
				reader.Dispose();
			}
			return answers;
		}

		public List<Quiz> getQuizzes()
		{
			List<Quiz> quizzes = new List<Quiz>();
			string query = "SELECT * FROM quiz";

			using (MySqlCommand cmd = new MySqlCommand(query, Connection))
			{
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					Quiz quiz = new Quiz();
					quiz.idQuiz = reader.GetInt32(0);
					quiz.Quizname = reader.GetString(1);
					if (!reader.IsDBNull(2))
					{
						quiz.Image = reader.GetString(2);
					}
					quiz.dateCreated = reader.GetDateTime(3);
					quizzes.Add(quiz);
				}
				reader.Close();
				reader.Dispose();
			}
			Quizzes = quizzes;
			NotifyPropertyChanged(nameof(Quizzes));
			return quizzes;
		}

		public List<Quiz> Quizzes
		{
			get { return _quizzes; }
			set
			{
				_quizzes = value;
				NotifyPropertyChanged(nameof(Quizzes));
			}
		}

		public Quiz ActiveQuiz
		{
			get { return _activeQuiz; }
			set
			{
				if (_activeQuiz != value)
				{
					_activeQuiz = value;

					if (_activeQuiz != null)
					{
						// Initialize questions for the active quiz
						_activeQuiz.Questions = Questions(_activeQuiz.idQuiz);
					}

					NotifyPropertyChanged(nameof(ActiveQuiz));
				}
			}
		}


		public Question ActiveQuestion
		{
			get
			{
				return _activeQuestion;
			}
			set
			{
				_activeQuestion = value;
				NotifyPropertyChanged(nameof(ActiveQuestion));
			}
		}

		public int MyProperty
		{
			get { return _myProperty; }
			set
			{
				_myProperty = value;
				NotifyPropertyChanged(nameof(MyProperty));
			}
		}

		public void DoNotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void NotifyPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		public class ImageButtonTag
		{
			public Image ImageBox { get; set; }
			public TextBlock ImagePath { get; set; }
		}
	}
}