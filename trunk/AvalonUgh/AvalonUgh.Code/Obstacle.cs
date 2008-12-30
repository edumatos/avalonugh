using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code
{
	[Script]
	public class Obstacle
	{

		public static Obstacle Of(AvalonUgh.Code.View.SelectorPosition p, int Zoom, int PrimitiveTileCountX, int PrimitiveTileCountY)
		{
			return new Obstacle
			{
				Left = p.ContentX * Zoom,
				Top = p.ContentY * Zoom,
				Width = PrimitiveTile.Width * Zoom * PrimitiveTileCountX,
				Height = PrimitiveTile.Heigth * Zoom * PrimitiveTileCountY
			};
		}

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


		public double Height
		{
			get
			{
				return Bottom - Top;
			}
			set
			{
				Bottom = Top + value;
			}
		}

		public double Width
		{
			get
			{
				return Right - Left;
			}
			set
			{
				Right = Left + value;
			}
		}



		public bool Equals(Obstacle e)
		{
			if (e.Right == this.Right)
				if (e.Left == this.Left)
					if (e.Bottom == this.Bottom)
						if (e.Top == this.Top)
							return true;

			return false;
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

		public Obstacle WithOffset(double x, double y)
		{
			return new Obstacle
			{
				Left = this.Left + x,
				Right = this.Right + x,
				Top = this.Top + y,
				Bottom = this.Bottom + y,
			};
		}
	}
}
