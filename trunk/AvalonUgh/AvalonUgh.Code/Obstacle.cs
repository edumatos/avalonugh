using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code
{
	[Script]
	public class Obstacle
	{
		public ASCIIImage.Entry Position;
		public ASCIITileSizeInfo Tile;

		public ISupportsVelocity SupportsVelocity;


		public double Left { get; set; }
		public double Top { get; set; }
		public double Right { get; set; }
		public double Bottom { get; set; }

		public double X
		{
			get
			{
				return Left + (Right - Left) / 2;
			}
		}

		public double Y
		{
			get
			{
				return Top + (Bottom - Top) / 2;
			}
		}

		public bool Intersects(Obstacle e)
		{
			if (e.Right > this.Left)
				if (e.Left < this.Right)
					if (e.Bottom > this.Top)
						if (e.Top < this.Bottom)
							return true;


			return false;
		}
	}
}
