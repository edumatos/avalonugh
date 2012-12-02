using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor.Tiles;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class SelectorBase
	{
		public int ImageWidth;
		public int ImageHeight;

		public NameFormat ToolbarImage;
		public View.SelectorInfo[] Sizes;

		public SelectorBase()
		{
			this.Sizes = new View.SelectorInfo[0];
		}


	}

	[Script]
	public abstract class TileSelectorBase : SelectorBase
	{

		public abstract string GetIdentifier();

		public void AttachTileToLevel(ASCII_ImageEntry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			
			var Selector = Sizes.SingleOrDefault(
				k => k.Equals(Tile)
			);

			if (Selector == null)
			{
				Console.WriteLine(
					new { InvalidSize = new { Tile.Width, Tile.Height }, Tile.Value, Position.X, Position.Y }.ToString()
				);

				return;
			}

			if (Tile.Variation != null)
			{
				var NamedSelector = Selector as TileSelector.Named;
				if (NamedSelector != null)
				{
					NamedSelector.Name.Index = Tile.Variation.Value;
				}
			}
			
			Selector.CreateTo(Level,
				new View.SelectorPosition
				{
					ContentX = Position.X * PrimitiveTile.Width,
					ContentY = Position.Y * PrimitiveTile.Heigth,
				}
			);
		
		}
	}
}
