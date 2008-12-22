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
	public class StoneSelector : SelectorBase
	{
		public const string ImageName = "stone";
		public const string Identifier = "S";


		public readonly View.SelectorInfo Size_1x1 = new Size_Generic(1, 1, 1);
		public readonly View.SelectorInfo Size_1x2 = new Size_Generic(1, 2, 1);
		public readonly View.SelectorInfo Size_2x1 = new Size_Generic(2, 1, 1);
		public readonly View.SelectorInfo Size_2x2 = new Size_Generic(2, 2, 2);
		public readonly View.SelectorInfo Size_2x3 = new Size_Generic(2, 3, 4);
		public readonly View.SelectorInfo Size_3x2 = new Size_Generic(3, 2, 1);
		public readonly View.SelectorInfo Size_4x2 = new Size_Generic(4, 2, 1);
		public readonly View.SelectorInfo Size_2x4 = new Size_Generic(2, 4, 1);

		public readonly View.SelectorInfo
			Size_6x6, 
			Size_8x4, 
			Size_5x5,
			Size_5x4,
			Size_6x3;

		public StoneSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "stone",
					Index = 0,
					Width = 2,
					Height = 2,
					Extension = "png"
				};

			this.Size_6x6 =
				new TileSelector.Composite(6, 6,
					(Level, Position) =>
					{
						this.Size_2x2.CreateTo(Level, Position[2, 2]);
						this.Size_4x2.CreateTo(Level, Position[0, 0]);
						this.Size_2x4.CreateTo(Level, Position[4, 0]);
						this.Size_2x4.CreateTo(Level, Position[0, 2]);
						this.Size_4x2.CreateTo(Level, Position[2, 4]);
					}
				);

			this.Size_8x4 =
				new TileSelector.Composite(8, 4,
					(Level, Position) =>
					{
						this.Size_4x2.CreateTo(Level, Position[0, 0]);
						this.Size_4x2.CreateTo(Level, Position[4, 0]);
						this.Size_4x2.CreateTo(Level, Position[2, 2]);
						this.Size_2x2.CreateTo(Level, Position[0, 2]);
						this.Size_2x2.CreateTo(Level, Position[6, 2]);
					}
				);


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

			this.Size_6x3 =
				new TileSelector.Composite(6, 3,
					(Level, Position) =>
					{
						this.Size_2x2.CreateTo(Level, Position[0, 0]);
						this.Size_2x1.CreateTo(Level, Position[0, 2]);

						this.Size_2x3.CreateTo(Level, Position[2, 0]);

						this.Size_2x2.CreateTo(Level, Position[4, 1]);
						this.Size_2x1.CreateTo(Level, Position[4, 0]);
					}
				);
					
			this.Sizes =
				new View.SelectorInfo[]
				{
					this.Size_1x1,
					this.Size_1x2,
					this.Size_2x1,
					this.Size_2x2,
					this.Size_2x3,
					this.Size_3x2,
					this.Size_4x2,
					this.Size_2x4,

					this.Size_6x3,
					this.Size_5x4,
					this.Size_5x5,
					this.Size_6x6,
					this.Size_8x4,
				};
		}




		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, ImageName)
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

				var o = u.ToObstacle();

				Level.KnownStones.Add(u);
			}
		}





		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			TileSelector.AttachToLevel(new StoneSelector().Sizes, Position, Tile, Level);
		}
	}
}
