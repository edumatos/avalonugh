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
	public class RidgeTreeSelector : SelectorBase
	{
		public const string Identifier = "T";

		public readonly View.SelectorInfo
			Size_1x1 = new SelectorSize_1x1(3);
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
		private class SelectorSize_1x1 : TileSelector.Named
		{

			public SelectorSize_1x1(int variations)
				: base(1, 1, variations, "ridgetree")
			{

			}


			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				if (Name.IndexCount > 0)
					Name.Index = (Name.Index + 1) % Name.IndexCount;

				Action Later = null;

				var Name_Index = Name.Index;

				var Top_TriggerPosition = Position[0, -1];
				var Top_TriggerObstacle = Obstacle.Of(Top_TriggerPosition, Level.Zoom, 1, 1);
				var Top_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Equals(Top_TriggerObstacle));


				var Left_TriggerPosition = Position[-1, 0];
				var Left_TriggerObstacle = Obstacle.Of(Left_TriggerPosition, Level.Zoom, 1, 1);
				var Left_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Equals(Left_TriggerObstacle));

				var Right_TriggerPosition = Position[1, 0];
				var Right_TriggerObstacle = Obstacle.Of(Right_TriggerPosition, Level.Zoom, 1, 1);
				var Right_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Equals(Right_TriggerObstacle));


				var Bottom_TriggerPosition = Position[0, 1];
				var Bottom_TriggerObstacle = Obstacle.Of(Bottom_TriggerPosition, Level.Zoom, 1, 1);
				var Bottom_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Equals(Bottom_TriggerObstacle));

				if (Top_Trigger != null)
				{
					Level.KnownRidgeTrees.Remove(Top_Trigger);

					Later +=
						delegate
						{
							CreateTo(Level, Top_TriggerPosition);
						};
				}
				else
				{
					// cut off tree
					Name.Index = 500;
				}

				if (Left_Trigger != null)
				{
					if (Top_Trigger != null)
					{
						Name.Index = 520;
					}
					else
					{
						if (Bottom_Trigger != null)
						{
							Name.Index = 510;
						}
						else
						{
							Name.Index = 300 + Name_Index % 2;
						}
					}

					Level.KnownRidgeTrees.Remove(Left_Trigger);

					Later +=
						delegate
						{
							CreateTo(Level, Left_TriggerPosition);
						};
				}
				else
				{
					//if (Bottom_Trigger != null)
					//{
					//    Name.Index = 530;
					//}
					//else
					//{
						if (Right_Trigger != null)
						{
							Name.Index = 300 + Name_Index % 2;
						}
					//}
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

				if (Later != null)
					Later();
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
