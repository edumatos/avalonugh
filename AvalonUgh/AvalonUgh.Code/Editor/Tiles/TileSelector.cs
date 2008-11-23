using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public abstract class TileSelector : View.SelectorInfo
	{
		public TileSelector()
			: this(1, 1)
		{

		}

		public TileSelector(int PrimitiveTileCountX, int PrimitiveTileCountY)
		{
			this.PrimitiveTileCountX = PrimitiveTileCountX;
			this.PrimitiveTileCountY = PrimitiveTileCountY;

			PercisionX = PrimitiveTile.Width;
			PercisionY = PrimitiveTile.Heigth;
		}




		public static void RemovePlatforms(View.SelectorInfo Selector, Level Level, View.SelectorPosition Position)
		{
			var z = Level.Zoom;
			var x = Position.ContentX * z;
			var y = Position.ContentY * z;

			var o = new Obstacle
			{
				Left = x,
				Top = y,
				Right = x + Selector.Width * z,
				Bottom = y + Selector.Height * z
			};


			Level.GetRemovablePlatforms().Where(k => k.Obstacle.Intersects(o)).ToArray().ForEach(k => k.Dispose());
		}
	}
}
