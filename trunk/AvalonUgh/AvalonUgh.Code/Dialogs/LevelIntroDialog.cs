using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;

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

			var y = 7;
			var MaxLength = this.Width / ((PrimitiveFont.Width + 1) * Zoom);

			#region word warp
			if (this.LevelTitle != null)
			{
				var t = this.LevelTitle.Trim().Split(" ").Aggregate("",
					(seed, k) =>
					{
						if (seed == "")
							return k;

						var x = seed + " " + k;

						if (x.Length > MaxLength)
						{
							y--;
							s.AppendLine(seed);
							return k;
						}

						return x;
					}
				);

				y--;
				s.AppendLine(t);
			}
			#endregion

			for (int i = 0; i < y; i++)
			{
				s.AppendLine();
			}


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
