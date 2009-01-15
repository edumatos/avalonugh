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
	public class RidgeSelector : SelectorBase
	{
		public const string Identifier = "R";

		public override string GetIdentifier()
		{
			return Identifier;
		}

		public readonly View.SelectorInfo
			Size_1x1 = new Size_Generic(1, 1, 2),
			Size_1x2 = new Size_Generic(1, 2, 1),
			Size_1x3 = new Size_Generic(1, 3, 1),
			Size_2x1 = new Size_Generic(2, 1, 1),
			Size_2x2 = new Size_Generic(2, 2, 4),
			Size_2x3 = new Size_Generic(2, 3, 1),
			Size_3x2 = new Size_Generic(3, 2, 2),
			Size_3x3 = new Size_Generic(3, 3, 1),
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
					Size_1x2,
					Size_1x3,
					Size_2x1,
					Size_2x2,
					Size_2x3,
					Size_3x2,
				
					Size_3x3,

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
				if (Name.IndexCount > 0)
					Name.Index = (Name.Index + 1) % Name.IndexCount;

				var Name_Index = Name.Index;

				Action Later = null;

				if (PrimitiveTileCountX == 1)
					if (PrimitiveTileCountY == 1)
					{
						// showing roots
						var RidgeTree_TriggerPosition = Position[1, -2];
						var RidgeTree_TriggerObstacle = Obstacle.Of(RidgeTree_TriggerPosition, Level.Zoom, 1, 1);
						var RidgeTree_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(RidgeTree_TriggerObstacle));

						if (RidgeTree_Trigger != null)
						{

							var Ridge1_TriggerPosition = Position[1, 0];
							var Ridge1_TriggerObstacle = Obstacle.Of(Ridge1_TriggerPosition, Level.Zoom, 1, 1);
							var Ridge1_Trigger = Level.KnownRidges.FirstOrDefault(k => k.ToObstacle().Intersects(Ridge1_TriggerObstacle));

							if (Ridge1_Trigger != null)
							{

								var Ridge2_TriggerPosition = Position[1, -1];
								var Ridge2_TriggerObstacle = Obstacle.Of(Ridge2_TriggerPosition, Level.Zoom, 1, 1);
								var Ridge2_Trigger = Level.KnownRidges.FirstOrDefault(k => k.ToObstacle().Intersects(Ridge2_TriggerObstacle));

								if (Ridge2_Trigger != null)
								{
									Name.Index = 500;
								}
							}
						}
					}

				if (PrimitiveTileCountX == 2)
					if (PrimitiveTileCountY == 2)
					{
						// showing roots
						var Top0_TriggerPosition = Position[-1, -1];
						var Top0_TriggerObstacle = Obstacle.Of(Top0_TriggerPosition, Level.Zoom, 1, 1);
						var Top0_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(Top0_TriggerObstacle));


						var Top1_TriggerPosition = Position[0, -1];
						var Top1_TriggerObstacle = Obstacle.Of(Top1_TriggerPosition, Level.Zoom, 1, 1);
						var Top1_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(Top1_TriggerObstacle));

						var Top2_TriggerPosition = Position[1, -1];
						var Top2_TriggerObstacle = Obstacle.Of(Top2_TriggerPosition, Level.Zoom, 1, 1);
						var Top2_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(Top2_TriggerObstacle));


						if (Top0_Trigger == null)
							if (Top2_Trigger == null)
								if (Top1_Trigger != null)
								{
									Name.Index = 500;

									var Ridge1_TriggerPosition = Position[-1, 1];
									var Ridge1_TriggerObstacle = Obstacle.Of(Ridge1_TriggerPosition, Level.Zoom, 1, 1);
									var Ridge1_Trigger = Level.KnownRidges.FirstOrDefault(k => k.ToObstacle().Equals(Ridge1_TriggerObstacle));

									if (Ridge1_Trigger != null)
									{
										Later +=
											delegate
											{
												new RidgeSelector().Size_1x1.CreateTo(Level, Ridge1_TriggerPosition);
											};
									}
								}
					}

				if (PrimitiveTileCountX == 3)
					if (PrimitiveTileCountY == 2)
					{
						var Top0_TriggerPosition = Position[-1, -1];
						var Top0_TriggerObstacle = Obstacle.Of(Top0_TriggerPosition, Level.Zoom, 1, 1);
						var Top0_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(Top0_TriggerObstacle));


						// showing roots
						var Top1_TriggerPosition = Position[0, -1];
						var Top1_TriggerObstacle = Obstacle.Of(Top1_TriggerPosition, Level.Zoom, 1, 1);
						var Top1_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(Top1_TriggerObstacle));

						var Top2_TriggerPosition = Position[1, -1];
						var Top2_TriggerObstacle = Obstacle.Of(Top2_TriggerPosition, Level.Zoom, 1, 1);
						var Top2_Trigger = Level.KnownRidgeTrees.FirstOrDefault(k => k.ToObstacle().Intersects(Top2_TriggerObstacle));

						if (Top0_Trigger == null)
							if (Top2_Trigger == null)
							if (Top1_Trigger != null)
							{
								Name.Index = 500;

								var Ridge1_TriggerPosition = Position[-1, 1];
								var Ridge1_TriggerObstacle = Obstacle.Of(Ridge1_TriggerPosition, Level.Zoom, 1, 1);
								var Ridge1_Trigger = Level.KnownRidges.FirstOrDefault(k => k.ToObstacle().Equals(Ridge1_TriggerObstacle));

								if (Ridge1_Trigger != null)
								{
									Later +=
										delegate
										{
											new RidgeSelector().Size_1x1.CreateTo(Level, Ridge1_TriggerPosition);
										};
								}
							}
					}


				RemovePlatforms(this, Level, Position);
				RemoveEntities(this, Level, Position);

				var u = new Ridge(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownRidges.Add(u);



				{
					// bridge connected only by the corner will also have legs!
					var TriggerPosition = Position[-1, -1];

					var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 1);

					var trigger = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(o_trigger));

					if (trigger != null)
					{
						Level.KnownBridges.Remove(trigger);

						new BridgeSelector().Size_1x1.CreateTo(Level, TriggerPosition);
					}
				}

				{
					// bridge connected only by the corner will also have legs!
					var TriggerPosition = Position[this.PrimitiveTileCountX, -1];

					var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 1);

					var trigger = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(o_trigger));

					if (trigger != null)
					{
						Level.KnownBridges.Remove(trigger);

						new BridgeSelector().Size_1x1.CreateTo(Level, TriggerPosition);
					}
				}

				Name.Index = Name_Index;

				if (Later != null)
					Later();
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
