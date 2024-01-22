using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MySQL.Models
{
	public class Answer : INotifyPropertyChanged
	{
		private int _question_idQuestion;
		private int _idAnswer;
		private string _answerText;
		private string _image;
		private bool _correct;

		public int question_idQuestion
		{
			get { return _question_idQuestion; }
			set
			{
				_question_idQuestion = value;
				NotifyPropertyChanged(nameof(question_idQuestion));
			}
		}

		public int idAnswer
		{
			get { return _idAnswer; }
			set
			{
				_idAnswer = value;
				NotifyPropertyChanged(nameof(idAnswer));
			}
		}

		public string answerText
		{
			get { return _answerText; }
			set
			{
				_answerText = value;
				NotifyPropertyChanged(nameof(answerText));
			}
		}

		public string image
		{
			get { return _image; }
			set
			{
				_image = value;
				NotifyPropertyChanged(nameof(image));
			}
		}

		public bool correct
		{
			get { return _correct; }
			set
			{
				_correct = value;
				NotifyPropertyChanged(nameof(correct));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}