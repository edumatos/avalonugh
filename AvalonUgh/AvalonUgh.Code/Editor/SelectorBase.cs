using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;

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

		public void AttachTileToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
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
