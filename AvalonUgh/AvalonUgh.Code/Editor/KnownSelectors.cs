using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public static class KnownSelectors
	{
		public static View.SelectorInfo[][] KnownTypes
		{
			get
			{
				return new[]
				{
					Sprites.GoldSelector.Sizes,
					Sprites.RockSelector.Sizes,
					Sprites.VehicleSelector.Sizes,
					Sprites.SignSelector.Sizes,
					Sprites.TreeSelector.Sizes,


					Tiles.CaveSelector.Sizes,
					Tiles.PlatformSelector.Sizes,
					Tiles.StoneSelector.Sizes,
					Tiles.RidgeSelector.Sizes,
					Tiles.FenceSelector.Sizes,
					Tiles.BridgeSelector.Sizes,

					DemolishSelector.Sizes,
					WaterLevelSelector.Sizes
				};
			}
		}

		[Script]
		public class Index
		{
			public int Type;
			public int Size;

			public static Index Of(View.SelectorInfo e)
			{
				var n = new Index { Type = -1, Size = -1 };

				foreach (var i in KnownTypes)
				{
					n.Type++;
					n.Size = Array.IndexOf(i, e);

					if (n.Size != -1)
						break;
				}

				if (n.Size == -1)
					n.Type = -1;

				return n;
			}

			public override string ToString()
			{
				return new { Type, Size }.ToString();
			}
		}


		
	}
}
