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
	public class RidgeSelector : SelectorInfo
	{
		public const string Identifier = "R";

		public readonly View.SelectorInfo 
			Size_1x1 = new Size_Generic(1, 1, 2),
			Size_2x1 = new Size_Generic(2, 1, 1),
			Size_2x2 = new Size_Generic(2, 2, 3),
			Size_2x3 = new Size_Generic(2, 3, 1),
			Size_3x2 = new Size_Generic(3, 2, 2),
			Size_4x4,
			Size_5x5,
			Size_5x4;
		

		

		public RidgeSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "ridge",
					Index = 0,
					Width = 2,
					Height = 2,
					Extension = "png"
				};

			this.Size_5x5 =
				new TileSelector.Composite(5, 5,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[2, 2]);
						this.Size_3x2.CreateTo(Level, Position[0, 0]);
						this.Size_2x3.CreateTo(Level, Position[0, 2]);
						this.Size_2x3.CreateTo(Level, Position[3, 0]);
						this.Size_3x2.CreateTo(Level, Position[2, 3]);
					}
				);

			this.Size_5x4 =
				new TileSelector.Composite(5, 4,
					(Level, Position) =>
					{
						this.Size_2x2.CreateTo(Level, Position[0, 0]);
						this.Size_3x2.CreateTo(Level, Position[2, 0]);
						this.Size_3x2.CreateTo(Level, Position[0, 2]);
						this.Size_2x2.CreateTo(Level, Position[3, 2]);
					}
				);

			this.Size_4x4 =
				new TileSelector.Composite(4, 4,
					(Level, Position) =>
					{
						this.Size_2x1.CreateTo(Level, Position[0, 0]);
						this.Size_2x3.CreateTo(Level, Position[0, 1]);
						this.Size_2x3.CreateTo(Level, Position[2, 0]);
						this.Size_2x1.CreateTo(Level, Position[2, 3]);
					}
				);

			this.Sizes =
				new View.SelectorInfo[]
				{
					Size_1x1,
					Size_2x1,
					Size_2x2,
					Size_2x3,
					Size_3x2,

					Size_4x4,
					Size_5x4,
					Size_5x5
				};
		}

		

		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "ridge")
			{
			
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				Name.Index = (Name.Index + 1) % Name.IndexCount;

				RemovePlatforms(this, Level, Position);
				RemoveEntities(this, Level, Position);

				var u = new Ridge(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownRidges.Add(u);
			}
		}

		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			var Selector = new RidgeSelector().Sizes.SingleOrDefault(
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
					ContentX = Position.X * PrimitiveTile.Width,
					ContentY = Position.Y * PrimitiveTile.Heigth,
				}
			);
		}
	}
}
