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
			Size_1x1 = new SelectorSize_1x1(),
			//Size_2x1 = new Size_Generic(2, 1, 1),
			//Size_2x2 = new Size_Generic(2, 2, 3),
			//Size_2x3 = new Size_Generic(2, 3, 2),
			//Size_3x2 = new Size_Generic(3, 2, 2),
			//Size_3x3 = new Size_Generic(3, 3, 1),
			Size_1x2,
			Size_2x1;
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

			this.Size_1x2 =
				new TileSelector.Composite(1, 2,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[0, 1]);
					}
				);


			this.Size_2x1 =
				new TileSelector.Composite(2, 1,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[1, 0]);
					}
				);

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
					Size_1x2,
					Size_2x1,
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

			public SelectorSize_1x1()
				: base(1, 1, 0, "ridgetree")
			{

			}


			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				Name.Index++;


				var Name_Index = Name.Index;

				var TopRight_TriggerPosition = Position[1, -1];
				var TopRight_TriggerObstacle = Obstacle.Of(TopRight_TriggerPosition, Level.Zoom, 1, 1);
				var TopRight_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Equals(TopRight_TriggerObstacle));




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

				Func<int, int, Func<int>> ToLookupValue =
					(offset, count) =>
						() => this.Name.Index % count + offset;

				var Tiles = new
				{
					Triggers = new[]
					{
						// 2, 4, 8, 16
						Left_Trigger, 
						Top_Trigger, 
						Right_Trigger, 
						Bottom_Trigger
					},
					Vertical = ToLookupValue(0, 4),
					Horizontal = ToLookupValue(300, 2),
					Cut = ToLookupValue(500, 1),
					LeftToBottom = ToLookupValue(510, 1),
					LeftToTop = ToLookupValue(520, 2),
					RightToTop = ToLookupValue(530, 1),
					RightToBottom = ToLookupValue(540, 1),
					BottomToTopAndRight = ToLookupValue(550, 1),
				};

				var Lookup =
					new Dictionary<int, Func<int>>
					{
						{0, Tiles.Vertical},

						{4, Tiles.Vertical},
						{4 + 16, Tiles.Vertical},
						{4 + 16 + 8, Tiles.Vertical},

						{2, Tiles.Horizontal},
						{8, Tiles.Horizontal},
						{2 + 8, Tiles.Horizontal},

						{2 + 16, Tiles.LeftToBottom},
						{2 + 4, Tiles.LeftToTop},
						{8 + 16, Tiles.RightToBottom},
						{2 + 16 + 8, Tiles.RightToBottom},

						{4 + 8, Tiles.RightToTop},
						{16, Tiles.Cut},

						{-1, Tiles.BottomToTopAndRight}
					};


				var Flags = Tiles.Triggers.ToFlags();

				if (Lookup.ContainsKey(Flags))
				{
					if (Flags == 4 + 16 + 8)
					{
						if (TopRight_Trigger != null)
							Flags = -1;
					}

					Name.Index = Lookup[Flags]();
				}
				else
				{
					Name.Index = Tiles.Cut();
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

				// we are going to do the ripple update

				var self = this;

				// todo: jsc probably emits a base call here, which we do not need

				if (Top_Trigger != null)
				{
					self.CreateTo(Level, Top_TriggerPosition);
				}

				if (Left_Trigger != null)
				{
					self.CreateTo(Level, Left_TriggerPosition);
				}

				if (Left_Trigger != null)
				{
					var LeftBottom_Ridge_2x2_TriggerPosition = Position[-1, 1];
					var LeftBottom_Ridge_2x2_TriggerObstacle = Obstacle.Of(LeftBottom_Ridge_2x2_TriggerPosition, Level.Zoom, 2, 2);
					var LeftBottom_Ridge_2x2_Trigger = Level.KnownRidges.FirstOrDefault(k => k.ToObstacle().Equals(LeftBottom_Ridge_2x2_TriggerObstacle));


					if (LeftBottom_Ridge_2x2_Trigger != null)
					{
						new RidgeSelector().Size_2x2.CreateTo(Level, LeftBottom_Ridge_2x2_TriggerPosition);

					}

					var LeftBottom_Ridge_3x2_TriggerPosition = Position[-1, 1];
					var LeftBottom_Ridge_3x2_TriggerObstacle = Obstacle.Of(LeftBottom_Ridge_3x2_TriggerPosition, Level.Zoom, 3, 2);
					var LeftBottom_Ridge_3x2_Trigger = Level.KnownRidges.FirstOrDefault(k => k.ToObstacle().Equals(LeftBottom_Ridge_3x2_TriggerObstacle));


					if (LeftBottom_Ridge_3x2_Trigger != null)
					{
						new RidgeSelector().Size_3x2.CreateTo(Level, LeftBottom_Ridge_3x2_TriggerPosition);

					}
				}
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
