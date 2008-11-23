using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
namespace AvalonUgh.Code.Editor
{
	[Script]
	public class DemolishSelector
	{
		public static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_1x1(),
				new Size_2x2(),
				new Size_4x4(),
				//new Size_2x4(),
				//new Size_2x3(),
				//new Size_2x1(),
			};

		[Script]
		internal class Size_1x1 : Tiles.TileSelector
		{
			public Size_1x1() : base(1, 1)
			{
				Invoke = (v, p) => InternalInvoke(v, this, p);
			}
		}

		[Script]
		internal class Size_2x2 : Tiles.TileSelector
		{
			public Size_2x2()
				: base(2, 2)
			{

				Invoke = (v, p) => InternalInvoke(v, this, p);
			}

		
		}


		[Script]
		internal class Size_4x4 : Tiles.TileSelector
		{
			public Size_4x4()
				: base(4, 4)
			{

				Invoke = (v, p) => InternalInvoke(v, this, p);
			}


		}
	

		public static void InternalInvoke(View View, View.SelectorInfo Selector, View.SelectorPosition Position)
		{
			var z = View.Level.Zoom;
			var x = Position.ContentX * z;
			var y = Position.ContentY * z;

			var o = new Obstacle
			{
				Left = x,
				Top = y,
				Right = x + Selector.Width * z,
				Bottom = y + Selector.Height * z
			};

			// we will remove the first object we 
			var a = View.Level.GetRemovableEntities().Where(k => k.Obstacle.Intersects(o)).ToArray();

			if (a.Any())
			{
				a.ForEach(k => k.Dispose());

				return;
			}

			View.Level.GetRemovablePlatforms().Where(k => k.Obstacle.Intersects(o)).ToArray().ForEach(k => k.Dispose());

		}
	}
}
