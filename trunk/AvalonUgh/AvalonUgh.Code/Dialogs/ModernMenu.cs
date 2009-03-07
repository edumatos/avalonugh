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
using System.ComponentModel;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class ModernMenu : ISupportsContainer
	{
		public event Action AnyClick;

		public Canvas Container { get; set; }

		public readonly int Zoom;
		public readonly int Width;
		public readonly int Height;

		public readonly Rectangle Background;

		public event Action MoreGames;
		public event Action Shop;
		public event Action Play;
		public event Action Editor;

		public void RaiseEditor()
		{
			if (this.Editor != null)
				this.Editor();

		}

		public int DifficultyLevel;

		public int MaxPlayers = 3;

		int InternalPlayers;
		public int Players
		{
			get
			{
				return InternalPlayers;
			}

			set
			{
				// there can only be so many okayers on a single keyboard
				value = value.Max(0).Min(MaxPlayers);

				if (InternalPlayers == value)
					return;

				InternalPlayers = value;

				if (this.PlayersChanged != null)
					this.PlayersChanged();
			}
		}

		public event Action PlayersChanged;

		public string Password;

		readonly DialogTextBox Options_5;
		readonly Action Options_Click;
		readonly Action Options_3_Click;
		public readonly DialogTextBox Options_Play;

		public readonly DialogTextBox Options_CountDown;
		public readonly DialogTextBox Options_Options;
		public event Action CountDown;

		public string PlayText
		{
			get
			{
				return Options_Play.Text;
			}
			set
			{
				Options_Play.Text = value;
			}
		}

		public ModernMenu(int Zoom, int Width, int Height)
		{
			this.Password = "";

			this.Zoom = Zoom;
			this.Width = Width;
			this.Height = Height;

			this.DifficultyLevel = 1;

			this.Container = new Canvas
			{
				Width = Width,
				Height = Height,
				Name = "ModernMenu_Container",
			};

			this.Background = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black,
				Name = "ModernMenu_Background",
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

			var KnownOptions = new BindingList<DialogTextBox>();

			KnownOptions.ForEachNewOrExistingItem(
				NewOption =>
				{
					NewOption.TouchOverlay.MouseLeftButtonUp +=
						delegate
						{
							if (AnyClick != null)
								AnyClick();
						};

					NewOption.AttachContainerTo(this.Container);
					NewOption.TouchOverlay.AttachTo(this.Container);
				}
			);


			var Options_Y = (Height - PrimitiveFont.Heigth * Zoom) / 2;
			var Options = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "options"
			}.AddTo(KnownOptions).MoveContainerTo(0, Options_Y);
			this.Options_Options = Options;
			Options.TouchOverlay.MoveTo(0, Options_Y);


			var Options_CountDown_Y = Convert.ToInt32( Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * 0.5);
			this.Options_CountDown = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "3",
				Visibility = Visibility.Hidden
			}.AddTo(KnownOptions).MoveContainerTo(0, Options_CountDown_Y);

			Options_CountDown.TouchOverlay.MoveTo(0, Options_CountDown_Y);

			Options_CountDown.Click +=
				delegate
				{
					if (this.CountDown != null)
						this.CountDown();
				};


			var Options_Play_Y = Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * 1;
			this.Options_Play = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "play",
				Visibility = Visibility.Visible
			}.AddTo(KnownOptions).MoveContainerTo(0, Options_Play_Y);

			Options_Play.TouchOverlay.MoveTo(0, Options_Play_Y);

			Options_Play.Click +=
				delegate
				{
					RaisePlay();
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


			var Options_MoreGames_Y = Options_Y - Convert.ToInt32((PrimitiveFont.Heigth * Zoom + 4) * 4);
			var Options_MoreGames = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "more games",
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_MoreGames_Y);

			Options_MoreGames.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_MoreGames_Y);

			Options_MoreGames.Click +=
				delegate
				{
					if (this.MoreGames != null)
						this.MoreGames();
				};

			//var Options_Shop_Y = Options_Y - Convert.ToInt32((PrimitiveFont.Heigth * Zoom + 4) * 2.5);
			//var Options_Shop = new DialogTextBox
			//{
			//    Width = Width,
			//    Zoom = Zoom,
			//    TextAlignment = TextAlignment.Center,
			//    Text = "shop",
			//    Visibility = Visibility.Hidden
			//}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_Shop_Y);

			//Options_Shop.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_Shop_Y);

			//Options_Shop.Click +=
			//    delegate
			//    {
			//        if (this.Shop != null)
			//            this.Shop();
			//    };


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
			var Options_2_Y = Convert.ToInt32(Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * -1.5);
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


			var Options_4_Y = Convert.ToInt32(Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * -2.5);
			var Options_4 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_4_Y);

			Options_4.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_4_Y);

			Action Options_4_Update =
				delegate
				{
					if (this.Players == 0)
					{
						Options_4.Text = "just watching";
						return;
					}

					if (this.Players > 1)
					{
						Options_4.Text = this.Players + " players";
						return;
					}

					Options_4.Text = "1 player";
				};

			this.PlayersChanged +=
				delegate
				{
					Options_4_Update();
				};

			Options_4_Update();

			Options_4.Click +=
				delegate
				{

					Players = (Players + 1) % (MaxPlayers + 1);

				};


			var Options_6_Y = Convert.ToInt32(Options_Y - (PrimitiveFont.Heigth * Zoom + 4) * -4);
			var Options_6 = new DialogTextBox
			{
				Width = Width,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Center,
				Text = "editor",
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this.Container).MoveContainerTo(0, Options_6_Y);

			Options_6.TouchOverlay.AttachTo(this.Container).MoveTo(0, Options_6_Y);
			Options_6.Click +=
				delegate
				{
					if (this.Editor != null)
						this.Editor();

					Options_Click();
				};


			Options_3_Click =
				delegate
				{
					var Opening = Options.Visibility != Visibility.Hidden;

					Options_5.Show(Options.Visibility != Visibility.Hidden);

					Options_6.Show(Options.Visibility == Visibility.Hidden);
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

					if (EnteringPasswordChanged != null)
						EnteringPasswordChanged();
				};

			Options_3.Click += Options_3_Click;


			Options_Click =
				delegate
				{
					Options_6.Show(Options_Play.Visibility != Visibility.Hidden);

					//Options_Shop.Show(Options_Play.Visibility != Visibility.Hidden);
					Options_MoreGames.Show(Options_Play.Visibility != Visibility.Hidden);

					if (Options_Play.Visibility == Visibility.Visible)
					{
						AnimatedBackgroundOpacity = 0.5;
						Options_Play.Visibility = Visibility.Hidden;
						Options_3.Visibility = Visibility.Visible;
						//Options_Shop.Visibility = Visibility.Visible;
						Options_2.Visibility = Visibility.Visible;
						Options_4.Visibility = Visibility.Visible;
					}
					else
					{
						AnimatedBackgroundOpacity = 0;
						Options_Play.Visibility = Visibility.Visible;
						Options_3.Visibility = Visibility.Hidden;
						Options_2.Visibility = Visibility.Hidden;
						Options_4.Visibility = Visibility.Hidden;
					}
				};

			Options.Click += Options_Click;

		}

		public void RaisePlay()
		{
			if (this.Play != null)
				this.Play();
		}

		public Action EnteringPassword;
		public event Action EnteringPasswordChanged;

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
