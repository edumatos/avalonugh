using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Input;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class KnownSelectors
	{
		public readonly SelectorBase[] Types;
		public readonly TileSelectorBase[] TileTypes;

		public readonly ArrowSelector Arrow = new ArrowSelector();
		public readonly WaterLevelSelector WaterLevel = new WaterLevelSelector();

		public readonly Sprites.PassengerSelector Passenger = new Sprites.PassengerSelector();

		public readonly Sprites.TryoperusSelector Tryoperus = new Sprites.TryoperusSelector();
		public readonly Sprites.RockSelector Rock = new Sprites.RockSelector();
		public readonly Sprites.VehicleSelector Vehicle = new Sprites.VehicleSelector();
		public readonly Sprites.SignSelector Sign = new Sprites.SignSelector();
		public readonly Sprites.DinoSelector Dino = new Sprites.DinoSelector();
		public readonly Sprites.TreeSelector Tree = new Sprites.TreeSelector();
		public readonly Sprites.BirdSelector Bird = new Sprites.BirdSelector();

		public readonly Tiles.StoneSelector Stone = new Tiles.StoneSelector();
		public readonly Tiles.RidgeSelector Ridge = new Tiles.RidgeSelector();
		public readonly Tiles.RidgeTreeSelector RidgeTree = new Tiles.RidgeTreeSelector();
		public readonly Tiles.PlatformSelector Platform = new Tiles.PlatformSelector();
		public readonly Tiles.CaveSelector Cave = new Tiles.CaveSelector();
		public readonly Tiles.FenceSelector Fence = new Tiles.FenceSelector();
		public readonly Tiles.BridgeSelector Bridge = new Tiles.BridgeSelector();

		public readonly Dictionary<Key, Func<SelectorBase>> DefaultKeyShortcut;

		public KnownSelectors()
		{
			this.TileTypes = new TileSelectorBase[]
				{
					Stone,
					Ridge,
					RidgeTree,
					Platform,
					Cave,
					Fence,
					Bridge,
				};

			this.Types =
				new SelectorBase[]
				{
					Arrow,

					Tree,
					Bird,
				
					Rock,
					Vehicle,
					Sign,
					Dino,
					Tryoperus,

					Stone,
					Ridge,
					RidgeTree,
					Platform,
					Cave,
					Fence,
					Bridge,

					
					new DemolishSelector(),
					WaterLevel,
					new SnowSelector(),
					new BackgroundSelector(),

					new Sprites.GoldSelector(),
					
					Passenger
				};

			ParamsFunc<SelectorBase, Func<SelectorBase>> f =
				e => e.AsCyclicEnumerator().Take;

			this.DefaultKeyShortcut = new Dictionary<Key, Func<SelectorBase>>
			{
				{ Key.D1, f(this.Stone, this.Cave, this.Fence)  },
				{ Key.D2, f(this.Ridge, this.RidgeTree) },
				{ Key.D3, f(this.Platform, this.Bridge) },
				{ Key.D4, f(this.Sign, this.Tree, this.Rock) },
				{ Key.D5, f(this.Dino, this.Tryoperus, this.Bird) },
				{ Key.D6, f(this.WaterLevel, this.Arrow) },
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
