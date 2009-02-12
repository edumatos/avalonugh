using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Avalon;
using System.Windows.Media;
using AvalonUgh.Code.Dialogs;
using System.Windows.Shapes;

namespace AvalonUgh.Code
{
    [Script]
    public class Statusbar : ISupportsContainer
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
            public readonly Statusbar Statusbar = new Statusbar();

            public StatusbarWindow()
            {
                ClientWidth = Statusbar.Width;
                ClientHeight = Statusbar.Height;

                Statusbar.AttachContainerTo(this.ContentContainer);

                DraggableArea.BringToFront();

                this.BackgroundColor = Colors.Black;
            }
        }

        public Statusbar()
            : this(null)
        {

        }

        [Script]
        public class ConstructorArguments
        {
            public int Zoom = 2;
        }

        public readonly ConstructorArguments Arguments;
        public Statusbar(ConstructorArguments Arguments)
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
                    return new DialogTextBox
                    {
                        FontWidth = 5,
                        FontHeigth = 5,
                        Zoom = Arguments.Zoom,
                        Color = Colors.White,
                        Text = text,
                    }.AttachContainerTo(this).MoveContainerTo(x * Arguments.Zoom, 1 * Arguments.Zoom);
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

			f(33, "00");
			HighScoreTextBox = f(114, "000000");
			f(169, "00");
			CurrentFareScoreTextBox = f(232, "0000");
			f(305, "00");


			new Rectangle
			{
				Fill = new SolidColorBrush(Color.FromRgb(0xe0, 0xec, 0x98)),
				Width = 32 * Arguments.Zoom,
				Height = 3 * Arguments.Zoom
			}.AttachTo(this).MoveTo(64 * Arguments.Zoom, 2 * Arguments.Zoom);

            // E0EC98
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
