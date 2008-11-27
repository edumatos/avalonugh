using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Threading;

namespace AvalonUgh.Code
{
	[Script]
	public class TouchInput
	{
		public double DeltaX;
		public double DeltaY;

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

				
				};


			Container.MouseMove +=
				(sender, args) =>
				{
					if (Offset == null)
						return;

					var p = args.GetPosition(Container);

					var CurrentOffset = new { p.X, p.Y };

					this.DeltaX = CurrentOffset.X - Offset.X;
					this.DeltaY = CurrentOffset.Y - Offset.Y;
				};


			Container.MouseLeftButtonUp +=
				(sender, args) =>
				{
					if (Offset == null)
						return;

					IsPressed = false;

					var p = args.GetPosition(Container);

					var CurrentOffset = new { p.X, p.Y };

					this.DeltaX = CurrentOffset.X - Offset.X;
					this.DeltaY = CurrentOffset.Y - Offset.Y;

					if (DeltaX > 12)
						return;
					if (DeltaY > 12)
						return;

					if (EnableClick == null)
					{
						EnableClick = 500.AtDelay(
							delegate
							{
								if (this.Click != null)
									this.Click();

								EnableClick = null;
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
