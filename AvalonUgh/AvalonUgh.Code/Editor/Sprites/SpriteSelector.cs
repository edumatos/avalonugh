using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public abstract class SpriteSelector : View.SelectorInfo
	{
		public SpriteSelector()
		{
			PercisionX = PrimitiveTile.Width / 2;
			PercisionY = PrimitiveTile.Heigth;
		}

		public static void RemoveEntities(View.SelectorInfo Selector, Level Level, View.SelectorPosition Position)
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


			Level.GetRemovableEntities().Where(k => k.Obstacle.Intersects(o)).ToArray().ForEach(k => k.Dispose());
		}
	}
}
