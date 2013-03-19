using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using System.Windows;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public abstract class Tile : ISupportsObstacle
	{
		public View.SelectorPosition Position;
		public Image Image;
		public TileSelector Selector { get; set; }

		public readonly LevelType Level;

		[Script]
		public class VariationElement
		{
			public int Value;
		}

		public VariationElement Variation;

		public Tile(LevelType Level, TileSelector Selector)
		{
			this.Level = Level;
			this.Selector = Selector;
		}

		public NameFormat Name;

		#region ISupportsObstacle Members

		public double X
		{
			get { return (Position.ContentX + Selector.HalfWidth) * Level.Zoom; }
		}

		public double Y
		{
			get { return (Position.ContentY + Selector.HalfHeight) * Level.Zoom; }
		}

		public Point Location
		{
			get
			{
				return new Point { X = this.X, Y = this.Y };
			}
		}

		double ObstacleCache_X;
		double ObstacleCache_Y;
		Obstacle ObstacleCache_Value;

		public int ObstaclePaddingBottom;

		public Obstacle ToObstacle(double x, double y)
		{
			if (ObstacleCache_X == x)
				if (ObstacleCache_Y == y)
					if (ObstacleCache_Value != null)
						return ObstacleCache_Value;

			ObstacleCache_X = x;
			ObstacleCache_Y = y;
			ObstacleCache_Value = new Obstacle
			{
				Left = x - Selector.HalfWidth * Level.Zoom,
				Top = y - Selector.HalfHeight * Level.Zoom,
				Right = x + Selector.HalfWidth * Level.Zoom,
				Bottom = y + Selector.HalfHeight * Level.Zoom  - ObstaclePaddingBottom * Level.Zoom,
			};

			return ObstacleCache_Value;
		}

		#endregion


		public abstract string GetIdentifier();
	}
}
