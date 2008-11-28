using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonUgh.Code.Input;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Sprites
{
	partial class Vehicle : ISupportsPlayerInput
	{
		#region ISupportsPlayerInput Members

		public void AddAcceleration(PlayerInput e)
		{

			if (this.IsUnmanned)
			{
				// why are we even called?
				// we should be controlling the caveman at this time already
				return;
			}


			if (e.HasInput)
			{
				this.IsAnimated = true;
			}
			else
			{
				this.IsAnimated = false;
			}

			var y = 0.0;

			if (e.Keyboard.IsPressedUp)
				y -= 2;

			if (e.Keyboard.IsPressedDown)
				y += 1;


			var x = 0.0;

			if (e.Keyboard.IsPressedLeft)
				x -= 1;

			if (e.Keyboard.IsPressedRight)
				x += 1;

			if (e.Touch.IsPressed)
			{
				var DeltaX = e.Touch.X - this.X;
				var DeltaY = e.Touch.Y - this.Y;

				var ay = (DeltaY / 64).Min(1).Max(-1);

				if (ay < 0)
					ay *= 2;

				x += (DeltaX / 64);
				y += ay;
			}

			if (y > 0)
			{
				// if we are under water, we cannot go down
				if (this.CurrentLevel != null)
				{
					if (this.Y > this.CurrentLevel.WaterTop)
					{
						y = 0;

						if (x == 0)
							this.IsAnimated = false;
					}
				}
			}



			this.VelocityX += x.Min(1).Max(-1) * this.Acceleration;
			this.VelocityY += y.Min(1).Max(-2) * this.Acceleration;

			if (this.VelocityY == 0)
				this.VelocityX *= 0.7;
		}

		#endregion
	}
}
