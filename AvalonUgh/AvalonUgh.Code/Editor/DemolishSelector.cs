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
				//new Size_4x2(),
				//new Size_2x4(),
				//new Size_2x3(),
				//new Size_2x1(),
			};

		[Script]
		public class Size_1x1 : Tiles.TileSelector
		{
			public Size_1x1() : base(1, 1)
			{
				Invoke = (v, p) => InternalInvoke(v, this, p);
			}
		}

		[Script]
		public class Size_2x2 : Tiles.TileSelector
		{
			public Size_2x2()
				: base(2, 2)
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
				Right = x + Selector.Width * 2,
				Bottom = y + Selector.Height * 2
			};

			// we will remove the first object we 
			var a = View.Level.GetRemovableEntities().Where(k => k.Obstacle.Intersects(o)).ToArray();
			
			a.ForEach(k => k.Dispose());

		}
	}
}
