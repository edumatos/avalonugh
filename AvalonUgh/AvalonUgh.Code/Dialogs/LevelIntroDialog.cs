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
		public int LevelNumber { get {return _LevelNumber; } set { _LevelNumber = value; Update(); }}
		string _LevelTitle;
		public string LevelTitle { get { return _LevelTitle; } set { _LevelTitle = value; Update(); } }
		string _LevelPassword;
		public string LevelPassword { get { return _LevelPassword; } set { _LevelPassword = value; Update(); } }

		void Update()
		{
			var s = new StringBuilder();

			s.AppendLine();
			s.AppendLine();
			s.Append("Level ").Append(this._LevelNumber);
			s.AppendLine();
			s.AppendLine();

		}

		public LevelIntroDialog()
		{
			BackgroundVisible = false;
		}
	}
}
