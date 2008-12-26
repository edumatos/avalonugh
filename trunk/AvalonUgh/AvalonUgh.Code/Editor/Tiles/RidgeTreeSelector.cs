﻿using System;
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
	public class RidgeTreeSelector : SelectorBase
	{
		public const string Identifier = "T";

		public readonly View.SelectorInfo 
			Size_1x1 = new Size_Generic(1, 1, 3);
			//Size_2x1 = new Size_Generic(2, 1, 1),
			//Size_2x2 = new Size_Generic(2, 2, 3),
			//Size_2x3 = new Size_Generic(2, 3, 2),
			//Size_3x2 = new Size_Generic(3, 2, 2),
			//Size_3x3 = new Size_Generic(3, 3, 1),
			//Size_4x4,
			//Size_5x5,
			//Size_5x4;




		public RidgeTreeSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "ridgetree",
					Index = 500,
					Extension = "png"
				};

			//this.Size_5x5 =
			//    new TileSelector.Composite(5, 5,
			//        (Level, Position) =>
			//        {
			//            this.Size_1x1.CreateTo(Level, Position[2, 2]);
			//            this.Size_3x2.CreateTo(Level, Position[0, 0]);
			//            this.Size_2x3.CreateTo(Level, Position[0, 2]);
			//            this.Size_2x3.CreateTo(Level, Position[3, 0]);
			//            this.Size_3x2.CreateTo(Level, Position[2, 3]);
			//        }
			//    );

			//this.Size_5x4 =
			//    new TileSelector.Composite(5, 4,
			//        (Level, Position) =>
			//        {
			//            this.Size_2x2.CreateTo(Level, Position[0, 0]);
			//            this.Size_3x2.CreateTo(Level, Position[2, 0]);
			//            this.Size_3x2.CreateTo(Level, Position[0, 2]);
			//            this.Size_2x2.CreateTo(Level, Position[3, 2]);
			//        }
			//    );

			//this.Size_4x4 =
			//    new TileSelector.Composite(4, 4,
			//        (Level, Position) =>
			//        {
			//            this.Size_2x1.CreateTo(Level, Position[0, 0]);
			//            this.Size_2x3.CreateTo(Level, Position[0, 1]);
			//            this.Size_2x3.CreateTo(Level, Position[2, 0]);
			//            this.Size_2x1.CreateTo(Level, Position[2, 3]);
			//        }
			//    );

			this.Sizes =
				new View.SelectorInfo[]
				{
					Size_1x1,
					//Size_2x1,
					//Size_2x2,
					//Size_2x3,
					//Size_3x2,
					//Size_3x3,

					//Size_4x4,
					//Size_5x4,
					//Size_5x5
				};
		}

		

		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "ridgetree")
			{
			
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				if (Name.IndexCount > 0)
					Name.Index = (Name.Index + 1) % Name.IndexCount;

				var Name_Index = Name.Index;

				{
					// first ridgetree is shown as a cut off tree
					var TriggerPosition = Position[0, -1];

					var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 1);

					var trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(o_trigger));

					if (trigger == null)
					{
						Name.Index = 500;
					}
				}

				RemovePlatforms(this, Level, Position);
				RemoveEntities(this, Level, Position);

				var u = new RidgeTree(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownRidgeTrees.Add(u);





				Name.Index = Name_Index;
			}
		}

		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			var Selector = new RidgeTreeSelector().Sizes.SingleOrDefault(
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