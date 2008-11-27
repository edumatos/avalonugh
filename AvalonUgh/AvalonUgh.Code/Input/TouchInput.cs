using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Threading;

namespace AvalonUgh.Code.Input
{
	[Script]
	public class TouchInput
	{
		// our touch input should be relative to content
		public double OffsetX;
		public double OffsetY;

		public double X;
		public double Y;

		public event Action Click;
		public event Action DoubleClick;

		public bool IsPressed;

		public TouchInput(Canvas Container)
		{
			var Offset = new { X = 0.0, Y = 0.0 }.ToDefault();

			var EnableClick = default(DispatcherTimer);

			Container.MouseLeftButtonDown +=
				(sender, args) =>
				{
					var p = args.GetPosition(Container);

					Offset = new { p.X, p.Y };
					IsPressed = true;

					this.X = p.X - OffsetX;
					this.Y = p.Y - OffsetY;
				};


			Container.MouseMove +=
				(sender, args) =>
				{
					if (Offset == null)
						return;

					var p = args.GetPosition(Container);


					this.X = p.X - OffsetX;
					this.Y = p.Y - OffsetY;
				};


			Container.MouseLeftButtonUp +=
				(sender, args) =>
				{
					if (Offset == null)
						return;

					IsPressed = false;

					var p = args.GetPosition(Container);

					var CurrentOffset = new { p.X, p.Y };

					var DeltaX = Math.Abs(CurrentOffset.X - Offset.X);
					var DeltaY = Math.Abs(CurrentOffset.Y - Offset.Y);

					if (DeltaX > 12)
						return;
					if (DeltaY > 12)
						return;



					if (EnableClick == null)
					{
						EnableClick = 500.AtDelay(
							delegate
							{
								EnableClick = null;

								if (IsPressed)
									return;

								if (this.Click != null)
									this.Click();

							}
						);
					}
					else
					{
						EnableClick.Stop();
						EnableClick = null;

						if (this.DoubleClick != null)
							this.DoubleClick();
					}



					Offset = null;
				};
		}
	}
}
