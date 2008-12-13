using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code.Dialogs
{

	[Script]
	public class LevelIntroDialog : Dialog
	{
		int _LevelNumber;
		public int LevelNumber { get { return _LevelNumber; } set { _LevelNumber = value; LevelIntroDialog_Update(); } }
		string _LevelTitle;
		public string LevelTitle { get { return _LevelTitle; } set { _LevelTitle = value; LevelIntroDialog_Update(); } }
		string _LevelPassword;
		public string LevelPassword { get { return _LevelPassword; } set { _LevelPassword = value; LevelIntroDialog_Update(); } }

		void LevelIntroDialog_Update()
		{
			this.InputText = this.LevelPassword;

			var s = new StringBuilder();

			s.AppendLine();
			s.Append("Level ").Append(this._LevelNumber);
			s.AppendLine();
			s.AppendLine();
			s.AppendLine(this.LevelTitle);
			s.AppendLine();
			s.AppendLine();
			s.AppendLine();
			s.AppendLine();
			s.AppendLine();
			s.AppendLine();

			this.Content.Text = s.ToString();



		}

		public LevelIntroDialog()
		{
			BackgroundVisible = false;

			this.LabelContent.TextAlignment = System.Windows.TextAlignment.Center;
			this.LabelContent.Text = "password:";
		}
	}
}
