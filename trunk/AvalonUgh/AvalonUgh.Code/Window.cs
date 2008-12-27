using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Controls;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class Window : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
		public int BorderWidth { get; set; }
		public int Padding { get; set; }

		public readonly Action Update;

		public Window()
		{
			Width = 360;
			Height = 200;
			BorderWidth = 1;
			Padding = BorderWidth + 4;

			this.Container = new Canvas
			{
			};

			#region borders
			var ThreeD_Top = new Rectangle
			{
				Fill = Brushes.LightGreen,
			}.AttachTo(this.Container);

			var ThreeD_Left = new Rectangle
			{
				Fill = Brushes.LightGreen,
			}.AttachTo(this.Container);

			var ThreeD_Right = new Rectangle
			{
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.Container);

			var ThreeD_Bottom = new Rectangle
			{
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.Container);

			var ThreeD_Fill = new Rectangle
			{
				Fill = Brushes.Green,
				Opacity = 0.8
			}.AttachTo(this.Container);
			#endregion

			Update =
				delegate
				{
					Container.SizeTo(
						Width,
						Height
					);

					ThreeD_Top.SizeTo(
						Width,
						BorderWidth
					).MoveTo(0, 0);

					ThreeD_Left.SizeTo(
						BorderWidth,
						Height - BorderWidth * 2
					).MoveTo(0, BorderWidth);

					ThreeD_Right.SizeTo(
						BorderWidth,
						Height - BorderWidth * 2
					).MoveTo(Width - BorderWidth, BorderWidth);
						
					ThreeD_Bottom.SizeTo(
						Width,
						BorderWidth
					).MoveTo(0, Height - BorderWidth);

					ThreeD_Fill.SizeTo(
						Width - BorderWidth * 2,
						Height - BorderWidth * 2
					).MoveTo(BorderWidth, BorderWidth);

					this.DraggableArea.SizeTo(Width, Height);
				};

			this.DraggableArea = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black,
				Opacity = 0
			}.AttachTo(this).MoveTo(0, 0);

			Update();
		}

		readonly public Rectangle DraggableArea;

		Canvas InternalDragContainer;

		public Canvas DragContainer
		{
			get
			{
				return InternalDragContainer;
			}
			set
			{
				if (InternalDragContainer != null)
					throw new ArgumentException();

				InternalDragContainer = value;

				var Drag = new DragBehavior(DraggableArea, Container, DragContainer)
				{
					SnapX = x => x.Max(Padding - Width).Min(DragContainer.Width - Padding),
					SnapY = y => y.Max(Padding - Height).Min(DragContainer.Height - Padding)
				};
			}
		}

		public Visibility Visibility
		{
			get
			{
				return this.Container.Visibility;
			}
			set
			{
				this.Container.Visibility = value;
			}
		}

		public void MoveToCenter(Canvas c)
		{
			this.MoveContainerTo(
				Convert.ToInt32((c.Width - this.Width) / 2),
				Convert.ToInt32((c.Height - this.Height) / 2)
			);
		}
	}
}
