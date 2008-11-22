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
		[Script]
		public class Size_1x1 :  View.SelectorInfo
		{
			public Size_1x1()
			{
				Width = PrimitiveTile.Width;
				Height = PrimitiveTile.Heigth;
				PercisionX = PrimitiveTile.Width;
				PercisionY = PrimitiveTile.Heigth;

				Invoke = InternalInvoke;
			}
		}

		[Script]
		public class Size_2x2 : View.SelectorInfo
		{
			public Size_2x2()
			{
				Width = PrimitiveTile.Width * 2;
				Height = PrimitiveTile.Heigth *2;
				PercisionX = PrimitiveTile.Width;
				PercisionY = PrimitiveTile.Heigth;

				Invoke = InternalInvoke;
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
			View.Level.GetRemovableEntities().FirstOrDefault(k => k.Obstacle.Intersects(o)).Apply(k => k.Dispose());

		}
	}
}
