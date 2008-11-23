﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class Stone : ISupportsObstacle
	{
		public View.SelectorPosition Position;
		public Image Image;
		public TileSelector Selector { get; set; }
		
		public readonly Level Level;

		public Stone(Level Level, TileSelector Selector)
		{
			this.Level = Level;
			this.Selector = Selector;
		}

		#region ISupportsObstacle Members

		public double X
		{
			get { return (Position.ContentX + Selector.HalfWidth) * Level.Zoom; }
		}

		public double Y
		{
			get { return (Position.ContentY + Selector.HalfHeight) * Level.Zoom; }
		}

		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - Selector.HalfWidth * Level.Zoom,
				Top = y - Selector.HalfHeight * Level.Zoom,
				Right = x + Selector.HalfWidth * Level.Zoom,
				Bottom = y + Selector.HalfHeight * Level.Zoom,
			};
		}

		#endregion
	}
}
