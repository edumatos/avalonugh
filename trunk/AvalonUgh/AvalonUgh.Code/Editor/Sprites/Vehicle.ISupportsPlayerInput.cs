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

			if (this.CurrentDriver == null)
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

			if (e.Touch != null)
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

			// limit the top speed a bit
			//this.VelocityX = this.VelocityX.Max(-2 * Zoom).Min(2 * Zoom);
			//this.VelocityY = this.VelocityY.Max(-2 * Zoom).Min(2 * Zoom);

			if (this.VelocityY == 0)
				this.VelocityX *= 0.7;

			if (!ExitIsBlocked)
				if (e.Keyboard.IsPressedEnter)
				{
					this.CurrentDriver.CurrentVehicle = null;
				}
		}

		#endregion
	}
}
