using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
namespace AvalonUgh.Code.Editor
{
	[Script]
	public class WaterLevelSelector
	{
		public static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_Vertical()
			};



		[Script]
		internal class Size_Vertical : View.SelectorInfo
		{
			public Size_Vertical()
			{
				// we are setting a width that should be greater than the view width
				// we might add a support for -1 to expand to to view later
				this.Width = 1000;

				this.Height = 4;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var WaterHeight = PrimitiveTile.Heigth * Level.Map.Height - Position.ContentY;

				Console.WriteLine(
					"water: " +
					WaterHeight
				);

				// the view should listen to this event
				// and update our water animation at runtime
				Level.AttributeWater.Value = WaterHeight;
			}

		}




	}
}
