using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;

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

			this.LocationTracker.LocationChanged +=
				delegate
				{
					this.Flashlight.MoveTo(
						this.LocationTracker.X + (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2,
						this.LocationTracker.Y + (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2
					);
				};

			// if the level is less in height than the view then dock to bottom
			// to support the statusbar over there which might or might not be there
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
