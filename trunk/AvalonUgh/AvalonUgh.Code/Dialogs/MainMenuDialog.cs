using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Input;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class MainMenuDialog : Dialog
	{
		public MainMenuDialog()
		{
			this.TextAlignment = System.Windows.TextAlignment.Left;

			this.DifficultyLevels =
				new[]
				{
					"easy",
					"medium",
					"hard"
				};

			this.GameModes =
				new[]
				{
					"single player",
					"team mode",
					"multiplayer",
					"multiplayer team",
				};


			this.UpdateText();


			this.LabelText = "password:";
			this.InputText = "none";

		}

		private void UpdateText()
		{
			this.Text = @"-
F1: start game
F2: enter password
F3: " + this.DifficultyLevels.AtModulus(this.DifficultyLevel) + @"
F4: " + this.GameModes.AtModulus(this.GameMode) + @"
F5: control options
-";
		}



		public int DifficultyLevel;
		public object[] DifficultyLevels;
		public int GameMode;
		public object[] GameModes;

		public event Action StartGame;

		public Action EnteringPassword;
		public string Password;

		public void HandleKeyUp(KeyEventArgs args)
		{
			if (args.Key == Key.F1)
			{
				args.Handled = true;

				if (StartGame != null)
					StartGame();

				return;
			}

			if (args.Key == Key.F2)
			{
				args.Handled = true;

				if (EnteringPassword != null)
					return;

				EnteringPassword = 200.AtIntervalWithCounter(
					Counter =>
					{
						if (Counter % 2 == 0)
						{
							this.InputText = this.Password;
						}
						else
						{
							this.InputText = this.Password + "?";
						}
					}
				).Stop;

				return;
			}

			if (args.Key == Key.F3)
			{
				args.Handled = true;

				this.DifficultyLevel++;
				this.UpdateText();

				// easy
				// medium
				// hard

				return;
			}

			if (args.Key == Key.F4)
			{
				args.Handled = true;

				this.GameMode++;
				this.UpdateText();
				// single player
				// team mode
				// multiplayer
				// multiplayer team

				return;
			}

			if (EnteringPassword != null)
			{
				if (args.Key == Key.Delete)
				{
					// backspace need special care for javascript
					// and isnt currently useble

					args.Handled = true;

					if (this.Password.Length > 0)
						this.Password = this.Password.Substring(0, this.Password.Length - 1);

					this.InputText = this.Password;

					return;
				}

				if (args.Key == Key.Enter)
				{
					args.Handled = true;
					EnteringPassword();
					EnteringPassword = null;

					if (string.IsNullOrEmpty(this.Password))
						this.InputText = "none";
					else
						this.InputText = this.Password;
					return;
				}

				var Text = KeyToString(args.Key);

				if (Text != null)
				{
					this.Password += Text;
					this.InputText = this.Password + "?";

					args.Handled = true;

					return;
				}
			}
		}

		public string KeyToString(Key k)
		{
			if (k == Key.A) return "a";
			if (k == Key.B) return "b";
			if (k == Key.C) return "c";
			if (k == Key.D) return "d";
			if (k == Key.E) return "e";
			if (k == Key.F) return "f";
			if (k == Key.G) return "g";
			if (k == Key.H) return "h";
			if (k == Key.I) return "i";
			if (k == Key.J) return "j";
			if (k == Key.K) return "k";
			if (k == Key.L) return "l";
			if (k == Key.M) return "m";
			if (k == Key.N) return "n";
			if (k == Key.O) return "o";
			if (k == Key.P) return "p";
			if (k == Key.Q) return "q";
			if (k == Key.R) return "r";
			if (k == Key.S) return "s";
			if (k == Key.T) return "t";
			if (k == Key.U) return "u";
			if (k == Key.V) return "v";
			if (k == Key.W) return "w";
			if (k == Key.X) return "x";
			if (k == Key.Y) return "y";
			if (k == Key.Z) return "z";
			if (k == Key.Space) return " ";
			if (k == Key.D0) return "0";
			if (k == Key.D1) return "1";
			if (k == Key.D2) return "2";
			if (k == Key.D3) return "3";
			if (k == Key.D4) return "4";
			if (k == Key.D5) return "5";
			if (k == Key.D6) return "6";
			if (k == Key.D7) return "7";
			if (k == Key.D8) return "8";
			if (k == Key.D9) return "9";

			return null;
		}
	}
}
