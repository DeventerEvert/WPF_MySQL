using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_MySQL.Models
{
	public class Question : INotifyPropertyChanged
	{
		private List<Answer> _answers;

		public int idQuestion { get; set; }
		public string questionText { get; set; }
		public string typeName { get; set; }
		public string image { get; set; }
		public int idQuiz { get; set; }
		public bool Type { get; set; }

		public List<Answer> Answers
		{
			get { return _answers; }
			set
			{
				_answers = value;
				NotifyPropertyChanged(nameof(Answers));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}