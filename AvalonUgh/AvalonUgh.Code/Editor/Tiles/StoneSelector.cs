using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public static class StoneSelector
	{
		public const string Identifier = "S";


		internal static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_Generic(1, 1, 1),
				new Size_Generic(1, 2, 1),
				new Size_Generic(2, 2, 2),
				new Size_Generic(4, 2, 1),
				new Size_Generic(2, 4, 1),
				new Size_Generic(2, 3, 1),
				new Size_Generic(2, 1, 1),
			};

		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "stone")
			{
			
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				Name.Index = (Name.Index + 1) % Name.IndexCount;

				RemovePlatforms(this, Level, Position);

				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownStones.Add(u);
			}
		}




		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			var Selector = Sizes.SingleOrDefault(
				k => k.Equals(Tile)
			);

			if (Selector == null)
			{
				Console.WriteLine(
					new { InvalidSize = new { Tile.Width, Tile.Height }, Identifier, Position.X, Position.Y }.ToString()
				);

				return;
			}

			Selector.CreateTo(Level,
				new View.SelectorPosition
				{
					TileX = Position.X,
					TileY = Position.Y,
					ContentX = Position.X * PrimitiveTile.Width,
					ContentY = Position.Y * PrimitiveTile.Heigth,
				}
			);
		}
	}
}
