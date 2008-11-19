using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;

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

		public Canvas Water { get; set; }

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

		public LocationTracker LocationTracker { get; set; }

		public Flashlight Flashlight { get; set; }

		public View(int width, int height, Level level)
		{
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

			this.Water = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.FlashlightContainer = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.Level.KnownWater.AttachContainerTo(this.Water);

			this.LocationTracker = new LocationTracker();

			// center bottom
			this.Content.MoveTo((width - ContentActualWidth) / 2, height - ContentActualHeight);

			this.Flashlight = new Flashlight(
				this.Level.Zoom,
				ContentActualWidth,
				ContentActualHeight
			).AttachContainerTo(this.FlashlightContainer);

			this.LocationTracker.LocationChanged +=
				delegate
				{
					this.Flashlight.MoveTo(this.LocationTracker.X, this.LocationTracker.Y);
				};
			// if the level is less in height than the view then dock to bottom
			// to support the statusbar over there which might or might not be there
		}
	}
}
