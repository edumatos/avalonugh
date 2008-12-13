using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class TopFiveDialog : Dialog
	{
		[Script]
		public class Entry
		{
			public readonly string Name;
			public readonly int Score;

			
			public Entry(string Name, int Score)
			{
				this.Name = Name;
				this.Score = Score;
			}
			const int NameMaxLength = 12;
			const int ScoreMaxLength = 7;

			public override string ToString()
			{
				// name 12
				// score 7

				var _Name = Name;

				if (_Name.Length > NameMaxLength)
					_Name = _Name.Substring(0, NameMaxLength);
				else
					_Name = _Name.PadRight(NameMaxLength, ' ');

				var _Score = Score.ToString();

				if (_Score.Length > ScoreMaxLength)
					_Score = _Score.Substring(0, ScoreMaxLength);
				else
					_Score = _Score.PadLeft(ScoreMaxLength, ' ');

				return _Name + " " + _Score;
			}
		}

		public TopFiveDialog()
		{
			this.Scores = new[] 
			{
				new Entry("ingognitus", 12345),
				new Entry("ingognitus", 12345),
				new Entry("ingognitus", 12345),
				new Entry("ingognitus", 12345),
				new Entry("ingognitus", 12345),
			};
		}

		Entry[] _Scores;
		public Entry[] Scores
		{
			get
			{
				return _Scores;
			}
			set
			{
				_Scores = value;
				Update();

			}
		}
		void Update()
		{
			var s = new StringBuilder();

			s.AppendLine("-");
			s.AppendLine("top five");
			s.AppendLine("-");

			if (_Scores != null)
				for (int i = 0; i < 5; i++)
				{
					if (i < _Scores.Length )
						s.AppendLine(_Scores[i].ToString());
				}

			this.Text = s.ToString();
		}
	}
}
