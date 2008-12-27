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
		public readonly View.SelectorInfo Size_2x3 = new Size_Generic(2, 3, 3);
		public readonly View.SelectorInfo Size_3x2 = new Size_Generic(3, 2, 1);
		public readonly View.SelectorInfo Size_4x2 = new Size_Generic(4, 2, 1);
		public readonly View.SelectorInfo Size_2x4 = new Size_Generic(2, 4, 1);

		public readonly View.SelectorInfo
			Size_3x3,
			Size_3x4,
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

			this.Size_3x3 =
				new TileSelector.Composite(3, 3,
					(Level, Position) =>
					{
						this.Size_1x2.CreateTo(Level, Position[0, 0]);
						this.Size_2x3.CreateTo(Level, Position[1, 0]);
						this.Size_1x1.CreateTo(Level, Position[0, 2]);
					}
				);

			this.Size_3x4 =
				new TileSelector.Composite(3, 4,
					(Level, Position) =>
					{
						this.Size_1x2.CreateTo(Level, Position[0, 0]);
						this.Size_2x4.CreateTo(Level, Position[1, 0]);
						this.Size_1x2.CreateTo(Level, Position[0, 2]);
					}
				);

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

					this.Size_3x3,
					this.Size_3x4,
					this.Size_6x3,
					this.Size_5x4,
					this.Size_5x5,
					this.Size_6x6,
					this.Size_8x4,
				};
		}




		[Script]
		public class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, ImageName)
			{

			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{


				if (Name.IndexCount > 0)
					Name.Index = (Name.Index + 1) % Name.IndexCount;

				var Name_Index = Name.Index;

				RemovePlatforms(this, Level, Position);

				// are we going for a special L shape?
				if (PrimitiveTileCountX == 2)
					if (PrimitiveTileCountY == 3)
					{
						// yay, is there a 1x2 tile to the west?
						var TriggerPosition = Position[-1, 0];

						var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 2);

						var trigger = Level.KnownStones.FirstOrDefault(k => k.ToObstacle().Equals(o_trigger));

						if (trigger != null)
						{
							// our tile will look special
							Name.Index = 100;

							Level.KnownStones.Remove(trigger);

							var Size_1x2 = new Size_Generic(1, 2, 0);
							Size_1x2.Name.Index = 100;
							Size_1x2.CreateTo(Level, TriggerPosition);
						}
					}

				if (PrimitiveTileCountX == 2)
					if (PrimitiveTileCountY == 4)
					{
						// yay, is there a 1x2 tile to the west?
						var TriggerPosition = Position[-1, 0];

						var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 2);

						var trigger = Level.KnownStones.FirstOrDefault(k => k.ToObstacle().Equals(o_trigger));

						if (trigger != null)
						{
							// our tile will look special
							Name.Index = 100;

							Level.KnownStones.Remove(trigger);

							var Size_1x2 = new Size_Generic(1, 2, 0);
							Size_1x2.Name.Index = 100;
							Size_1x2.CreateTo(Level, TriggerPosition);
						}
					}


				if (PrimitiveTileCountX == 1)
					if (PrimitiveTileCountY == 1)
					{
						// yay, is there a 1x2 tile to the west?
						var TriggerPosition = Position[0, -3];

						var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 2, 3);

						var trigger = Level.KnownStones.FirstOrDefault(k => k.ToObstacle().Equals(o_trigger));

						if (trigger != null)
						{
							// our tile will look special
							Name.Index = 101;

							Level.KnownStones.Remove(trigger);

							var Size_2x3 = new Size_Generic(2, 3, 0);
							Size_2x3.Name.Index = 101;
							Size_2x3.CreateTo(Level, TriggerPosition);
						}
					}


				if (PrimitiveTileCountX == 2)
					if (PrimitiveTileCountY == 3)
					{
						// the stone next to a cave has a window
						var TriggerPosition = Position[-2, 1];

						var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 2, 2);

						var trigger = Level.KnownCaves.FirstOrDefault(k => k.ToObstacle().Equals(o_trigger));

						if (trigger != null)
						{
							// our tile will look special
							Name.Index = 200;
						}
					}

				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownStones.Add(u);

				Name.Index = Name_Index;
			}
		}





		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			TileSelector.AttachToLevel(new StoneSelector().Sizes, Position, Tile, Level);
		}
	}
}
