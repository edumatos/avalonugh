﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;

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

		public ASCII_ImageEntry Position;
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

		public Obstacle ToPercision(double e)
		{
			var Left = Math.Round(this.Left / e) * e;
			var Top = Math.Round(this.Top / e) * e;

			return new Obstacle
			{
				Left = Left,
				Top = Top,
				Right = Left + Width,
				Bottom = Top + Height
			};
		}

		public Obstacle GrowTo(Obstacle e)
		{
			return new Obstacle
			{
				Left = this.Left.Min(e.Left),
				Top = this.Top.Min(e.Top),
				Right = this.Right.Max(e.Right),
				Bottom = this.Bottom.Max(e.Bottom),

			};
		}

		public bool Contains(int x, int y)
		{
			if (x < Left)
				return false;

			if (x > Right)
				return false;

			if (y < Top)
				return false;

			if (y > Bottom)
				return false;

			return true;
		}
	}
}
