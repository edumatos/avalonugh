using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class CaveSelector : TileSelectorBase
	{
		public const string Identifier = "C";

		public override string GetIdentifier()
		{
			return Identifier;
		}

		public CaveSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "cave",
					Index = 0,
					Width = 2,
					Height = 2,
					Extension = "png"
				};

			this.Sizes =
				new View.SelectorInfo[]
				{
					new Size_2x2(2),
				};
		}
	

		[Script]
		private class Size_2x2 : TileSelector.Named
		{

			public Size_2x2(int variations)
				: base(2, 2, variations, "cave")
			{
			
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);

				{
					// the stone next to a cave has a window
					var TriggerPosition = Position[-2, -1];

					var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 2, 3);

					var trigger = Level.KnownStones.FirstOrDefault(k => k.ToObstacle().Equals(o_trigger));

					if (trigger != null)
					{
						Level.KnownStones.Remove(trigger);

						var Size_2x3 = new StoneSelector.Size_Generic(2, 3, 0);
						Size_2x3.Name.Index = 200;
						Size_2x3.CreateTo(Level, TriggerPosition);
					}
				}

				var u = new Cave(Level, this)
				{
					Position = Position,
					Variation = new Tile.VariationElement {Value = Name.Index },
					Image = ToImage(Level, Position)
				};

				Level.KnownCaves.Add(u);

				Name.Index = (Name.Index + 1) % Name.IndexCount;
			}
		}

	
	}
}
