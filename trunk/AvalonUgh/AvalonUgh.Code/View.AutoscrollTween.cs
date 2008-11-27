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
	partial class View
	{


		Action<int, int> AutoscrollTween;

		public bool AutoscrollEnabled { get; set; }

		void Autoscroll()
		{
			if (!AutoscrollEnabled)
				return;

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

						var dx = Convert.ToDouble(this.ContainerWidth - w);
						var dy = Convert.ToDouble(this.ContainerHeight - h);

						if (dx > 0)
							px = 0.5;

						if (dy > 0)
							py = 0.5;

						MoveContentTo(px * dx, py * dy);
					}
				);


			AutoscrollTween(x * TweenPercision, y * TweenPercision);


		}

	}
}
