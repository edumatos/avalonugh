using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Diagnostics;
using AvalonUgh.Code.Dialogs;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class StatusbarType : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public int Width
		{
			get
			{
				return 320 * Arguments.Zoom;
			}
		}

		public int Height
		{
			get
			{
				return 7 * Arguments.Zoom;
			}
		}

		[Script]
		public class StatusbarWindow : Window
		{
			public readonly StatusbarType Statusbar = new StatusbarType();

			public StatusbarWindow()
			{
				ClientWidth = Statusbar.Width;
				ClientHeight = Statusbar.Height;

				Statusbar.AttachContainerTo(this.ContentContainer);

				DraggableArea.BringToFront();

				this.BackgroundColor = Colors.Black;
			}
		}

		public StatusbarType()
			: this(null)
		{

		}

		[Script]
		public class ConstructorArguments
		{
			public int Zoom = 2;
		}

		public readonly ConstructorArguments Arguments;
		public StatusbarType(ConstructorArguments Arguments)
		{
			if (Arguments == null)
				Arguments = new ConstructorArguments();

			this.Arguments = Arguments;

			this.Container = new Canvas
			{
				Width = Width,
				Height = Height
			};

			var BackgroundSolo = new Image
			{
				Width = Width,
				Height = Height,
				Stretch = System.Windows.Media.Stretch.Fill,
				Source = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Statusbar,
					Name = "duo",
					Index = 0,
					Extension = "png"
				}
			};

			BackgroundSolo.AttachTo(this);

			Func<int, string, DialogTextBox> f =
				(x, text) =>
				{
					var t = new DialogTextBox
					{
						FontWidth = 5,
						FontHeigth = 5,
						Zoom = Arguments.Zoom,
						Color = Colors.White,
						Text = text,
					};

					t.AttachContainerTo(this);
					t.MoveContainerTo(x * Arguments.Zoom, 1 * Arguments.Zoom);
					t.TouchOverlay.MoveTo(x * Arguments.Zoom, 1 * Arguments.Zoom);

					return t;
				};

			//f(33, "00");
			//f(118, "000000");
			//f(177, "00");
			//f(244, "0000");
			//f(290, "00");


			//new Rectangle
			//{
			//    Fill = new SolidColorBrush(Color.FromRgb(0xe0, 0xec, 0x98)),
			//    Width = 32 * Arguments.Zoom,
			//    Height = 3 * Arguments.Zoom
			//}.AttachTo(this).MoveTo(64 * Arguments.Zoom, 2 * Arguments.Zoom);

			LivesTextBox = f(33, "00");
			HighScoreTextBox = f(114, "000000");
			HeadCountTextBox = f(169, "00");

			HeadCountTextBox.HoverBehaviorEnabled = false;

			CurrentFareScoreTextBox = f(232, "0000");
			MultiplierTextBox = f(305, "00");


			this.LevelTimeRectangle = new Rectangle
			{
				Fill = new SolidColorBrush(Color.FromRgb(0xe0, 0xec, 0x98)),
				Width = 32 * Arguments.Zoom,
				Height = 3 * Arguments.Zoom
			}.AttachTo(this).MoveTo(64 * Arguments.Zoom, 2 * Arguments.Zoom);

			var SignTemplate = new NameFormat
			{
				Path = KnownAssets.Path.Statusbar,
				Extension = "png",
				Zoom = 2
			};
			Func<int, int, Action<int>> BuildSetSign =
				(x, y) =>
				{
					var cache = new Dictionary<int, Image>
					{
						{-1, SignTemplate.ToName("question").ToImage(12, 6)},
						{0, SignTemplate.ToName("sign").ToIndex(0).ToImage(12, 6)},
						{1, SignTemplate.ToName("sign").ToIndex(1).ToImage(12, 6)},
						{2, SignTemplate.ToName("sign").ToIndex(2).ToImage(12, 6)},
						{3, SignTemplate.ToName("sign").ToIndex(3).ToImage(12, 6)},
						{4, SignTemplate.ToName("sign").ToIndex(4).ToImage(12, 6)},
						{5, SignTemplate.ToName("sign").ToIndex(5).ToImage(12, 6)}
					};

					var selection = new BindingList<Image>().WithEvents(
						i =>
						{
							i.AttachTo(this);

							return delegate
							{
								i.Orphanize();
							};
						}
					);

					return
						value =>
						{
							selection.Source.RemoveAll();

							if (cache.ContainsKey(value))
								cache[value].MoveTo(x * Arguments.Zoom, y * Arguments.Zoom).AddTo(selection.Source);
						};
				};

			this.SetLeftSign = BuildSetSign(199, 0);
			this.SetRightSign = BuildSetSign(275, 0);

			// E0EC98

		}

		public Action<int> SetLeftSign;
		public Action<int> SetRightSign;

		public readonly DialogTextBox LivesTextBox;
		public readonly DialogTextBox MultiplierTextBox;


		public readonly DialogTextBox HeadCountTextBox;
		int InternalHeadCount;
		public int HeadCount
		{
			set
			{
				InternalHeadCount = value;

				var x = value % 100;
				var s = x.ToString();

				HeadCountTextBox.Text = new string('0', 2 - s.Length) + s;
			}
			get
			{
				return InternalHeadCount;
			}
		}

		public bool DesignMode
		{
			set
			{
				LivesTextBox.Show(!value);
				HighScoreTextBox.Show(!value);
				CurrentFareScoreTextBox.Show(!value);
				LevelTimeRectangle.Show(!value);
				MultiplierTextBox.Show(!value);

				HeadCountTextBox.TouchOverlay.Orphanize();

				if (value)
					HeadCountTextBox.TouchOverlay.AttachTo(this);
			}
		}

		readonly Rectangle LevelTimeRectangle;
		int InternalLevelTime;
		public int LevelTime
		{
			set
			{
				InternalLevelTime = value.Max(0).Min(32);
				LevelTimeRectangle.Width = value * Arguments.Zoom;
			}
			get
			{
				return InternalLevelTime;
			}
		}

		public readonly DialogTextBox CurrentFareScoreTextBox;
		public int InternalCurrentFareScore;
		public int CurrentFareScore
		{
			set
			{
				InternalCurrentFareScore = value;

				var x = value % 10000;
				var s = x.ToString();

				CurrentFareScoreTextBox.Text = new string('0', 4 - s.Length) + s;
			}
			get
			{
				return InternalCurrentFareScore;
			}
		}

		public readonly DialogTextBox HighScoreTextBox;
		public int InternalHighScore;
		public int HighScore
		{
			set
			{
				InternalHighScore = value;

				var x = value % 1000000;
				var s = x.ToString();

				HighScoreTextBox.Text = new string('0', 6 - s.Length) + s;
			}
			get
			{
				return InternalHighScore;
			}
		}

	}
}
