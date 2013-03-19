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
	public partial class Window : ISupportsContainer
	{
		public object Tag { get; set; }


		public Canvas Container { get; set; }

		public Canvas BackgroundContainer { get; set; }
		public Canvas ContentContainer { get; set; }
		public Canvas OverlayContainer { get; set; }

		public readonly AnimatedOpacity<Rectangle> ColorOverlay;


		public int Width { get; set; }
		public int Height { get; set; }
		public int BorderWidth { get; set; }


		int InternalPadding;
		public int Padding { get { return InternalPadding; } set { InternalPadding = value; Update(); } }

		public event Action AfterUpdate;
		public readonly Action Update;


		public int ClientWidth
		{
			get
			{
				return this.Width - Padding * 2;
			}
			set
			{
				this.Width = value + Padding * 2;

				this.Update();
			}
		}

		public int ClientHeight
		{
			get
			{
				return this.Height - Padding * 2;
			}
			set
			{
				this.Height = value + Padding * 2;

				this.Update();

			}
		}

		public readonly Rectangle ThreeD_Top;
		public readonly Rectangle ThreeD_Left;
		public readonly Rectangle ThreeD_Bottom;
		public readonly Rectangle ThreeD_Right;
		public readonly Rectangle ThreeD_Fill;

		Color InternalBackgroundColor;

		public Color BackgroundColor
		{
			get
			{
				return InternalBackgroundColor;
			}
			set
			{
				InternalBackgroundColor = value;

				var g = new[]
				{
					Colors.White,
					value,
					Colors.Black
				}.ToGradient(5).ToArray();


				this.ThreeD_Left.Fill = new SolidColorBrush(g[1]);
				this.ThreeD_Top.Fill = new SolidColorBrush(g[1]);

				this.ThreeD_Right.Fill = new SolidColorBrush(g[3]);
				this.ThreeD_Bottom.Fill = new SolidColorBrush(g[3]);

				this.ThreeD_Fill.Fill = new SolidColorBrush(InternalBackgroundColor);
			}
		}

		public Window()
		{
			Width = 360;
			Height = 200;
			BorderWidth = 1;


			this.Container = new Canvas
			{
				Name = "Window_Container"
			};

			this.BackgroundContainer = new Canvas
			{
				Name = "Window_BackgroundContainer"
			}.AttachTo(this.Container);


			#region borders
			this.ThreeD_Top = new Rectangle
			{
				Fill = Brushes.LightGreen,
			}.AttachTo(this.BackgroundContainer);

			this.ThreeD_Left = new Rectangle
			{
				Fill = Brushes.LightGreen,
			}.AttachTo(this.BackgroundContainer);

			this.ThreeD_Right = new Rectangle
			{
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.BackgroundContainer);

			this.ThreeD_Bottom = new Rectangle
			{
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.BackgroundContainer);

			this.ThreeD_Fill = new Rectangle
			{
				Fill = Brushes.Green,
				Opacity = 0.8
			}.AttachTo(this.BackgroundContainer);
			#endregion

			this.ContentContainer = new Canvas
			{
				Name = "Window_ContentContainer"
			}.AttachTo(this.Container);


			Update =
				delegate
				{
					Container.SizeTo(
						Width,
						Height
					);

					BackgroundContainer.SizeTo(
						Width,
						Height
					);

					this.ContentContainer.SizeTo(this.ClientWidth, this.ClientHeight).MoveTo(this.Padding, this.Padding);
					this.OverlayContainer.SizeTo(this.ClientWidth, this.ClientHeight).MoveTo(this.Padding, this.Padding);
					this.ColorOverlay.Element.SizeTo(this.ClientWidth, this.ClientHeight).MoveTo(this.Padding, this.Padding);

					this.OverlayContainer.ClipTo(0, 0, this.ClientWidth, this.ClientHeight);

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

					if (AfterUpdate != null)
						AfterUpdate();
				};


			this.DraggableArea = new Rectangle
			{
				Name = "Window_DraggableArea",
				Width = Width,
				Height = Height,
				Fill = Brushes.Black,
				Opacity = 0
			}.AttachTo(this).MoveTo(0, 0);

			this.DraggableArea.MouseLeftButtonDown +=
				delegate
				{
					this.Container.BringToFront();
				};

			this.OverlayContainer = new Canvas
			{
				Name = "Window_OverlayContainer",

			}.AttachTo(this.Container);

			this.ColorOverlay = new Rectangle
			{
				Name = "Window_ColorOverlay",
				Fill = Brushes.Black,
				Opacity = 0,
				Visibility = Visibility.Hidden,
			}.AttachTo(this.Container).ToAnimatedOpacity();

			//this.ColorOverlay.Opacity = 0;

			InternalPadding = BorderWidth + 4;

			Update();
		}

		readonly public Rectangle DraggableArea;

		Canvas InternalDragContainer;

		public DragBehavior XDragBehavior { get; set; }

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

				if (XDragBehavior != null)
					throw new ArgumentException();

				InternalDragContainer = value;

				this.XDragBehavior = new DragBehavior(DraggableArea, Container, DragContainer)
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
