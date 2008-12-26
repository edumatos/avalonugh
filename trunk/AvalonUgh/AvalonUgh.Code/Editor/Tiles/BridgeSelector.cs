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
	public class BridgeSelector : SelectorBase
	{
		public const string Identifier = "B";

		public readonly View.SelectorInfo
			Size_1x1 = new Size_Generic(1, 1, 1),
			Size_2x1,
			Size_3x1,
			Size_4x1;

		public BridgeSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "bridge",
					Index = 0,
					Extension = "png"
				};

			this.Size_2x1 =
				new TileSelector.Composite(2, 1,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[1, 0]);
					}
				);

			this.Size_3x1 =
				new TileSelector.Composite(3, 1,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[1, 0]);
						this.Size_1x1.CreateTo(Level, Position[2, 0]);
					}
				);

			this.Size_4x1 =
				new TileSelector.Composite(4, 1,
					(Level, Position) =>
					{
						this.Size_1x1.CreateTo(Level, Position[0, 0]);
						this.Size_1x1.CreateTo(Level, Position[1, 0]);
						this.Size_1x1.CreateTo(Level, Position[2, 0]);
						this.Size_1x1.CreateTo(Level, Position[3, 0]);
					}
				);

			this.Sizes =
				new View.SelectorInfo[]
				{
					Size_1x1,
					Size_2x1,
					Size_3x1,
					Size_4x1
				};
		}



		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "bridge")
			{

			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				CreateTo(Level, Position, true, true);
			}

			void CreateTo(Level Level, View.SelectorPosition Position, bool ForwardScanToLeft, bool ForwardScanToRight)
			{
				//Name.Index = (Name.Index + 1) % Name.IndexCount;

				RemovePlatforms(this, Level, Position);

				const int VariationLeft = 1;
				const int VariationMiddle = 0;
				const int VariationRight = 2;

				this.Name.Index = VariationMiddle;

				var z = Level.Zoom;
				var x = Position.ContentX * z;
				var y = Position.ContentY * z;

				var o = new Obstacle
				{
					Left = x,
					Top = y,
					Right = x + this.Width * z,
					Bottom = y + this.Height * z
				};

				#region scan to right
				var right = o.WithOffset(this.Width * z, 0);
				var right_down = o.WithOffset(this.Width * z, this.Height * z);

				// is there a bridge to my right?
				var right_p = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(right));
				if (right_p == null)
				{
					if (Level.KnownPlatforms.Any(k => k.ToObstacle().Intersects(right)))
					{
						this.Name.Index = VariationRight;
					}
					else if (Level.KnownRidges.Any(k => k.ToObstacle().Intersects(right)))
					{
						this.Name.Index = VariationRight;
					}
					else if (Level.KnownPlatforms.Any(k => k.ToObstacle().Intersects(right_down)))
					{
						this.Name.Index = VariationRight;
					}
					else if (Level.KnownRidges.Any(k => k.ToObstacle().Intersects(right_down)))
					{
						this.Name.Index = VariationRight;
					}
				}
				#endregion

				#region scan to left
				var left = o.WithOffset(-this.Width * z, 0);
				var left_down = o.WithOffset(-this.Width * z, this.Height * z);

				// is there a bridge to my right?
				var left_p = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(left));
				if (left_p == null)
				{
					if (Level.KnownPlatforms.Any(k => k.ToObstacle().Intersects(left)))
					{
						if (this.Name.Index == VariationRight)
							this.Name.Index = VariationMiddle;
						else
							this.Name.Index = VariationLeft;
					}
					else if (Level.KnownRidges.Any(k => k.ToObstacle().Intersects(left)))
					{
						if (this.Name.Index == VariationRight)
							this.Name.Index = VariationMiddle;
						else
							this.Name.Index = VariationLeft;
					}
					else if (Level.KnownPlatforms.Any(k => k.ToObstacle().Intersects(left_down)))
					{
						if (this.Name.Index == VariationRight)
							this.Name.Index = VariationMiddle;
						else
							this.Name.Index = VariationLeft;
					}
					else if (Level.KnownRidges.Any(k => k.ToObstacle().Intersects(left_down)))
					{
						if (this.Name.Index == VariationRight)
							this.Name.Index = VariationMiddle;
						else
							this.Name.Index = VariationLeft;
					}

				}
				#endregion


				var u = new Bridge(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position),
					Name = this.Name.Clone()
				};

				Level.KnownBridges.Add(u);

				if (left_p != null)
					if (ForwardScanToLeft)
					{
						CreateTo(Level,
							new View.SelectorPosition
							{
								ContentX = Position.ContentX - this.Width,
								ContentY = Position.ContentY
							}, true, false);
					}

				if (right_p != null)
					if (ForwardScanToRight)
					{
						CreateTo(Level,
							new View.SelectorPosition
							{
								ContentX = Position.ContentX + this.Width,
								ContentY = Position.ContentY
							}, false, true);
					}
			}
		}




		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			TileSelector.AttachToLevel(new BridgeSelector().Sizes, Position, Tile, Level);
		}
	}
}
