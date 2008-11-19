using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonUgh.Code
{
	/// <summary>
	/// View enables to display maps that are larger than the provided
	/// client area by enabling autoscroll. In the editor mode the scroll
	/// should follow the mouse while in playmode it should follow
	/// the active player be it a vehicle or an actor
	/// </summary>
	[Script]
	public class View : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public Canvas Background { get; set; }

		public Canvas Content { get; set; }

		public Canvas Platforms { get; set; }

		public Canvas Entities { get; set; }

		public Canvas WaterContainer { get; set; }

		public Canvas FlashlightContainer { get; set; }


		public Canvas TouchOverlay { get; set; }

		public Level Level { get; set; }

		public int ContentActualWidth
		{
			get { return this.Level.ActualWidth; }
		}

		public int ContentActualHeight
		{
			get { return this.Level.ActualHeight; }
		}

		public int ContentExtendedWidth
		{
			get
			{
				return this.ContentActualWidth + (this.ContainerWidth - this.ContentActualWidth).Max(0) + MaxShakeSize * 2;
			}
		}

		public int ContentExtendedHeight
		{
			get
			{
				return this.ContentActualHeight + (this.ContainerHeight - this.ContentActualHeight).Max(0) + MaxShakeSize * 2;
			}
		}

		public LocationTracker LocationTracker { get; set; }

		public Flashlight Flashlight { get; set; }

		public int ContainerWidth { get; set; }
		public int ContainerHeight { get; set; }

		public const int MaxShakeSize = 4;

		public View(int width, int height, Level level)
		{
			this.ContainerWidth = width;
			this.ContainerHeight = height;

			// for the fist configuration we do need to know all the 
			// variables, even if we allow them to be changed later
			// like enable actual scrolling at a later point of time
			// or change the size of the view

			this.Container = new Canvas
			{
				Width = width,
				Height = height
			};

			this.Level = level;


			this.Content = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Container);

			this.Platforms = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.Entities = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.WaterContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.Container);

			this.FlashlightContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.Container);

			//this.Level.KnownWater.AttachContainerTo(this.Water);

			new Water(
				new Water.Info
				{
					DefaultWidth = this.ContentExtendedWidth,
					DefaultHeight = this.ContentExtendedHeight,

					WaterTop = this.Level.WaterTop + MaxShakeSize + (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2,
					Zoom = this.Level.Zoom,
					//Level = this.Level,

					// maybe the map should be able to set this color?
					WaterColorBottom = Colors.Green
				}
			).AttachContainerTo(this.WaterContainer);

			this.LocationTracker = new LocationTracker();

			// center bottom
			this.MoveContentTo(
				(width - ContentActualWidth) / 2,
				(height - ContentActualHeight) / 2
			);

			this.Flashlight = new Flashlight(
				this.Level.Zoom,
				ContentExtendedWidth,
				ContentExtendedHeight
			).AttachContainerTo(this.FlashlightContainer);

			this.Flashlight.Container.Opacity = 0.5;
			this.Flashlight.Visible = false;
			this.Flashlight.VisibleChanged +=
				delegate
				{
					// - no performance boost at all at this time

					//if (!this.Flashlight.Visible)
					//{
					//    this.Content.ClipTo(
					//        MaxShakeSize - (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2,
					//        MaxShakeSize - (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2,
					//        ContentExtendedWidth,
					//        ContentExtendedHeight
					//    );
					//}
				};

			this.LocationTracker.LocationChanged += new Action(Autoscroll);
			this.LocationTracker.LocationChanged +=
				delegate
				{

					if (this.Flashlight.Visible)
					{
						var x = this.LocationTracker.X + (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2;
						var y = this.LocationTracker.Y + (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2;

						// - no performance boost at all at this time
						//this.Content.ClipTo(
						//    Convert.ToInt32(this.LocationTracker.X - this.Flashlight.Size * this.Level.Zoom / 2),
						//    Convert.ToInt32(this.LocationTracker.Y - this.Flashlight.Size * this.Level.Zoom / 2),
						//    this.Flashlight.Size * this.Level.Zoom,
						//    this.Flashlight.Size * this.Level.Zoom
						//);

						this.Flashlight.MoveTo(
							x,
							y
						);
					}
				};

			// if the level is less in height than the view then dock to bottom
			// to support the statusbar over there which might or might not be there

		}

		Action<int, int> AutoscrollTween;

		void Autoscroll()
		{
			if (this.LocationTracker.Target == null)
				return;


			var w = this.ContentActualWidth;
			var h = this.ContentActualHeight;


			if (w < this.ContainerWidth)
				if (h < this.ContainerHeight)
					return;

			var x = Convert.ToInt32(this.LocationTracker.X);
			var y = Convert.ToInt32(this.LocationTracker.Y);

			const int TweenPercision = 2;

			if (AutoscrollTween == null)
				AutoscrollTween = NumericEmitter.Of(
					(ax_, ay_) =>
					{
						var a = new
						{
							x = ax_ / (double)TweenPercision,
							y = ay_ / (double)TweenPercision
						};

						//Console.WriteLine(
						//    a.ToString()
						//);

						// margin from the edge
						var mx = this.ContainerWidth / 4;
						var my = this.ContainerHeight / 4;


						var px = ((a.x - mx) / (w - mx * 2)).Max(0).Min(1);
						var py = ((a.y - my) / (h - my * 2)).Max(0).Min(1);

						var dx = this.ContainerWidth - w;
						var dy = this.ContainerHeight - h;

						MoveContentTo(px * dx, py * dy);
					}
				);

			AutoscrollTween(x * TweenPercision, y * TweenPercision);


		}

		public void MoveContentTo(double x, double y)
		{
			var ex = x - MaxShakeSize - (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2;
			var ey = y - MaxShakeSize - (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2;

			this.FlashlightContainer.MoveTo(ex, ey);
			this.WaterContainer.MoveTo(ex, ey);

			this.Content.MoveTo(
				x,
				y
			);
		}
	}
}
