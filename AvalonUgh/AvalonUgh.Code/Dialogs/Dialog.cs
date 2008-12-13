using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class Dialog : ISupportsContainer
	{
		public Canvas Container { get; set; }


		public Image Background { get; set; }

		// background
		// - ugh image
		// - black

		// text
		// text-centered

		// inputbox

		public DialogTextBox Content { get; set; }

		int _Zoom;
		public int Zoom { get { return _Zoom; } set { _Zoom = value; Update(); } }

		int _Width;
		public int Width { get { return _Width; } set { _Width = value; Update(); } }

		int _Height;
		public int Height { get { return _Height; } set { _Height = value; Update(); } }

		bool _BackgroundVisible;
		public bool BackgroundVisible { get { return _BackgroundVisible; } set { _BackgroundVisible = value; Update(); } }

		public string Text
		{
			get { return Content.Text; }
			set { Content.Text = value; }
		}


		public TextAlignment TextAlignment
		{
			get { return Content.TextAlignment; }
			set { Content.TextAlignment = value; }
		}

		public Dialog()
		{
			this.Container = new Canvas
			{
			};

			this.Background = new Image
			{
				Stretch = Stretch.Fill,
				Source = (KnownAssets.Path.Backgrounds + "/005.png").ToSource()
			}.AttachTo(this);

			this.Content = new DialogTextBox
			{
				TextAlignment = TextAlignment.Center,
				Color = Colors.Brown
			}.AttachContainerTo(this);



			this.BackgroundVisible = true;
		
		}

		void Update()
		{
			this.Container.Width = Width;
			this.Container.Height = Height;

			this.Background.Width = Width;
			this.Background.Height = Height;
			this.Background.Show(this.BackgroundVisible);

			if (this.BackgroundVisible)
				this.Content.Container.MoveTo(0, PrimitiveFont.Heigth * Zoom * 3);
			else
				this.Content.Container.MoveTo(0, 0);

			this.Content.Width = Width;
			this.Content.Zoom = Zoom;
		}
	
	}
}
