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
		public Canvas BackgroundContainer { get; set; }


		public Image Background { get; set; }

		// background
		// - ugh image
		// - black

		// text
		// text-centered

		// inputbox

		public DialogTextBox Content { get; set; }
		public DialogTextBox LabelContent { get; set; }
		public DialogTextBox InputContent { get; set; }

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

		public string LabelText
		{
			get { return LabelContent.Text; }
			set { LabelContent.Text = value; }
		}

		public string InputText
		{
			get { return InputContent.Text; }
			set { InputContent.Text = value; }
		}

		VerticalAlignment _VerticalAlignment;
		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return _VerticalAlignment;
			}
			set
			{
				_VerticalAlignment = value;
				Update();
			}
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

			this.BackgroundContainer = new Canvas
			{
				Background = Brushes.Black
			}.AttachTo(this.Container);


			this.Background = new Image
			{
				Stretch = Stretch.Fill,
				Source = (KnownAssets.Path.Backgrounds + "/005.png").ToSource()
			}.AttachTo(this.BackgroundContainer);

			this.Content = new DialogTextBox
			{
				TextAlignment = TextAlignment.Center,
				Color = Colors.Brown
			}.AttachContainerTo(this);

			this.LabelContent = new DialogTextBox
			{
				TextAlignment = TextAlignment.Left,
				Color = Colors.Brown
			}.AttachContainerTo(this);

			this.InputContent = new DialogTextBox
			{
				TextAlignment = TextAlignment.Left,
				Color = Colors.Blue
			}.AttachContainerTo(this);

			this.Content.TextChanged +=
				delegate
				{
					UpdatePositions();

				};

			this.LabelContent.TextChanged +=
				delegate
				{
					UpdatePositions();

				};

			this.BackgroundVisible = true;
		}

		int VerticalAlignmentOffset
		{
			get
			{
				if (this.VerticalAlignment == VerticalAlignment.Top)
					return 0;

				var h = this.Content.Height + this.LabelContent.Height + this.InputContent.Height;

				if (this.VerticalAlignment == VerticalAlignment.Center)
					return (Height - PaddingTop - h) / 2;

				return Height - PaddingTop - h;
			}
		}

		void Update()
		{
			this.Container.Width = Width;
			this.Container.Height = Height;

			this.BackgroundContainer.Width = Width;
			this.BackgroundContainer.Height = Height;

			this.Background.Width = Width;
			this.Background.Height = Height;
			this.Background.Show(this.BackgroundVisible);

			UpdatePositions();

			this.Content.Width = Width;
			this.Content.Zoom = Zoom;

			this.LabelContent.Width = Width;
			this.LabelContent.Zoom = Zoom;

			this.InputContent.Width = Width;
			this.InputContent.Zoom = Zoom;
		}

		public int PaddingTop
		{
			get
			{
				if (BackgroundVisible)
					return PrimitiveFont.Heigth * Zoom * 4;

				return 0;
			}
		}

		private void UpdatePositions()
		{
			this.Content.Container.MoveTo(0, VerticalAlignmentOffset + PaddingTop);

			this.LabelContent.Container.MoveTo(0, VerticalAlignmentOffset + PaddingTop + this.Content.Height);

			this.InputContent.Container.MoveTo(0, VerticalAlignmentOffset + PaddingTop + this.Content.Height + this.LabelContent.Height);
		}

	}
}
