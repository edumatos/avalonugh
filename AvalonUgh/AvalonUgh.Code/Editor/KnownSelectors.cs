using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class KnownSelectors
	{
		public readonly SelectorBase[] Types;

		public readonly ArrowSelector Arrow = new ArrowSelector();
		public readonly Sprites.TryoperusSelector Tryoperus = new Sprites.TryoperusSelector();
		public readonly Sprites.RockSelector Rock = new Sprites.RockSelector();

		public KnownSelectors()
		{
			this.Types =
				new SelectorBase[]
				{
					Arrow,

					new Sprites.TreeSelector(),
					new Sprites.GoldSelector(),
					Rock,
					new Sprites.VehicleSelector(),
					new Sprites.SignSelector(),
					new Sprites.DinoSelector(),
					Tryoperus,

					new Tiles.StoneSelector(),
					new Tiles.RidgeSelector(),
					new Tiles.RidgeTreeSelector(),
					new Tiles.PlatformSelector(),
					new Tiles.CaveSelector(),
					new Tiles.FenceSelector(),
					new Tiles.BridgeSelector(),

					new DemolishSelector(),
					new WaterLevelSelector(),
					new BackgroundSelector()
				};
		}



		[Script]
		public class Index
		{
			public int Type;
			public int Size;

			public static Index Of(View.SelectorInfo e, KnownSelectors Selectors)
			{
				var n = new Index { Type = -1, Size = -1 };

				foreach (var i in Selectors.Types)
				{
					n.Type++;
					n.Size = Array.IndexOf(i.Sizes, e);

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
