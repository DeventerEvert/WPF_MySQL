using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MySQL.Models
{
	public class Quiz : INotifyPropertyChanged
	{
		private int _idQuiz;
		private string _quizname;
		private string _image;
		private DateTime _dateCreated;
		private List<Question> _questions;

		public int idQuiz
		{
			get { return _idQuiz; }
			set
			{
				_idQuiz = value;
				NotifyPropertyChanged(nameof(idQuiz));
			}
		}

		public string Quizname
		{
			get { return _quizname; }
			set
			{
				_quizname = value;
				NotifyPropertyChanged(nameof(Quizname));
			}
		}

		public string Image
		{
			get { return _image; }
			set
			{
				_image = value;
				NotifyPropertyChanged(nameof(Image));
			}
		}

		public DateTime dateCreated
		{
			get { return _dateCreated; }
			set
			{
				_dateCreated = value;
				NotifyPropertyChanged(nameof(dateCreated));
			}
		}

		public List<Question> Questions
		{
			get { return _questions; }
			set
			{
				_questions = value;
				NotifyPropertyChanged(nameof(Questions));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}