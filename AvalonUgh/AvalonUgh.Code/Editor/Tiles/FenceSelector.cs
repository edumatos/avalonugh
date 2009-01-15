using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class FenceSelector : SelectorBase
	{
		public const string Identifier = "F";

		public override string GetIdentifier()
		{
			return Identifier;
		}

		public readonly View.SelectorInfo
			Size_1x1 = new Size_Generic(1, 1, 1),
			Size_2x1,
			Size_1x2,
			Size_2x2;

		public FenceSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "fence",
					Index = 0,
					Extension = "png"
				};

			this.Size_1x1 = new Size_Generic(1, 1, 1);

			this.Size_2x1 =
				new TileSelector.Composite(2, 1,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[1, 0]);
					}
				);
			
			this.Size_1x2 =
				new TileSelector.Composite(1, 2,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[0, 1]);
					}
				);


			this.Size_2x2 =
				new TileSelector.Composite(2, 2,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[1, 0]);
						this.Size_1x1.CreateTo(Level, Position[0, 1]);
						this.Size_1x1.CreateTo(Level, Position[1, 1]);
					}
				);

			this.Sizes =
				new View.SelectorInfo[]
				{
					Size_1x1,
					Size_1x2,
					Size_2x1,
					Size_2x2
				};
		}


		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "fence")
			{

			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				Name.Index = (Name.Index + 1) % Name.IndexCount;

				RemovePlatforms(this, Level, Position);

				var u = new Fence(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownFences.Add(u);
			}
		}

		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			var Selector = new FenceSelector().Sizes.SingleOrDefault(
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
