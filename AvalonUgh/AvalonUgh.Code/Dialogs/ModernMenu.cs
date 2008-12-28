using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Tween;
using AvalonUgh.Assets.Shared;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Input;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class ModernMenu : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public readonly int Zoom;
		public readonly int Width;
		public readonly int Height;

		public readonly Rectangle Background;

		public event Action Play;

		public int DifficultyLevel;

		public int Players;
		public event Action PlayersChanged;

		public string Password;

		readonly DialogTextBox Options_5;
		readonly Action Options_3_Click;
		readonly DialogTextBox Options_1;

		public string PlayText
		{
			get
			{
				return Options_1.Text;
			}
			set
			{
				Options_1.Text = value;
			}
		}

		public ModernMenu(int Zoom, int Width, int Height)
		{
			this.Password = "";

			this.Zoom = Zoom;
			this.Width = Width;
			this.Height = Height;

			this.DifficultyLevel = 1;
			this.Players = 1;

			this.Container = new Canvas
			{
				Width = Width,
				Height = Height
			};

			this.Background = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black,
			}.AttachTo(this);

			this._SetBackgroundOpacity__ = NumericEmitter.Of(
				(x, y) =>
				{
					var Opacity = x * 0.01;

					if (Opacity == 0)
					{
						this.Background.Hide();
					}
					else
					{
						this.Background.Show();
					}

					this.Background.Opacity = Opacity;
				}
			);

			this.AnimatedBackgroundOpacity = 0;




			var Options_Y = (Height - PrimitiveFont.Heigth * Zoom) / 2;
			var Options = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "options"
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_Y);

			Options.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_Y);


			var Options_1_Y = Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * 1;
			this.Options_1 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "play",
				Visibility = Visibility.Visible
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_1_Y);

			Options_1.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_1_Y);

			Options_1.Click +=
				delegate
				{
					if (this.Play != null)
						this.Play();
				};


			var Options_3_Y = Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * 1;
			var Options_3 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "password",
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_3_Y);

			Options_3.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_3_Y);

			var Options_5_Y = Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * -1;
			this.Options_5 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "none",
				Visibility = Visibility.Hidden,
				Color = Colors.Blue,
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_5_Y);


			var DifficultyLevel_Labels = new[] { "easy", "medium", "hard" };
			var Options_2_Y = Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * -1;
			var Options_2 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = DifficultyLevel_Labels[DifficultyLevel],
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_2_Y);

			Options_2.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_2_Y);
			Options_2.Click +=
				delegate
				{

					DifficultyLevel++;
					DifficultyLevel = DifficultyLevel % DifficultyLevel_Labels.Length;

					Options_2.Text = DifficultyLevel_Labels[DifficultyLevel];
				};


			var Players_Labels = new[] { "just watching", "1 player", "2 players", "3 players" };
			var Options_4_Y = Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * -2;
			var Options_4 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = Players_Labels[Players],
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_4_Y);

			Options_4.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_4_Y);
			Options_4.Click +=
				delegate
				{

					Players++;
					Players = Players % Players_Labels.Length;

					Options_4.Text = Players_Labels[Players];

					if (this.PlayersChanged != null)
						this.PlayersChanged();
				};

			Options_3_Click =
				delegate
				{
					var Opening = Options.Visibility != Visibility.Hidden;

					Options_5.Show(Options.Visibility != Visibility.Hidden);

					Options_2.Show(Options.Visibility == Visibility.Hidden);
					Options_4.Show(Options.Visibility == Visibility.Hidden);
					Options.Show(Options.Visibility == Visibility.Hidden);

					if (EnteringPassword != null)
					{
						EnteringPassword();
						EnteringPassword = null;
					}

					if (Opening)
					{
						EnteringPassword = 200.AtIntervalWithCounter(
							Counter =>
							{
								if (Counter % 2 == 0)
								{
									Options_5.Text = this.Password;
								}
								else
								{
									Options_5.Text = this.Password + "?";
								}
							}
						).Stop;
					}
				};

			Options_3.Click += Options_3_Click;
				

			Options.Click +=
				delegate
				{
					if (Options_1.Visibility == Visibility.Visible)
					{
						AnimatedBackgroundOpacity = 0.5;
						Options_1.Visibility = Visibility.Hidden;
						Options_3.Visibility = Visibility.Visible;
						Options_2.Visibility = Visibility.Visible;
						Options_4.Visibility = Visibility.Visible;
					}
					else
					{
						AnimatedBackgroundOpacity = 0;
						Options_1.Visibility = Visibility.Visible;
						Options_3.Visibility = Visibility.Hidden;
						Options_2.Visibility = Visibility.Hidden;
						Options_4.Visibility = Visibility.Hidden;
					}
				};
		}

		public Action EnteringPassword;

		Action<int, int> _SetBackgroundOpacity__;

		double InternalAnimatedBackgroundOpacity;

		public double AnimatedBackgroundOpacity
		{
			get
			{
				return InternalAnimatedBackgroundOpacity;
			}
			set
			{
				InternalAnimatedBackgroundOpacity = value;
				_SetBackgroundOpacity__(Convert.ToInt32(value * 100), 0);
			}
		}

		public static string KeyToString(Key k)
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

		public void HandleKeyUp(KeyEventArgs args)
		{
			if (EnteringPassword != null)
			{
				var IsDelete = args.Key == Key.Delete;

				if (args.Key == Key.Back)
					IsDelete = true;

				if (IsDelete)
				{
					// backspace need special care for javascript
					// and isnt currently useble

					args.Handled = true;

					if (this.Password.Length > 0)
						this.Password = this.Password.Substring(0, this.Password.Length - 1);

					this.Options_5.Text = this.Password;

					return;
				}

				if (args.Key == Key.Enter)
				{
					args.Handled = true;

					if (string.IsNullOrEmpty(this.Password))
						this.Options_5.Text = "none";
					else
						this.Options_5.Text = this.Password;

					this.Options_3_Click();

					return;
				}

				var Text = KeyToString(args.Key);

				if (Text != null)
				{
					this.Password += Text;
					this.Options_5.Text = this.Password + "?";

					args.Handled = true;

					return;
				}
			}
		}

	}
}
