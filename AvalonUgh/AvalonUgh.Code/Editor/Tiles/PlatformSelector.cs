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
	public class PlatformSelector : SelectorBase
	{
		public const string Identifier = "P";

		public readonly View.SelectorInfo
			Size_1x1 = new Size_Generic(1, 1, 1),
			Size_2x1 = new Size_Generic(2, 1, 1),
			Size_2x2 = new Size_Generic(2, 2, 1),
			Size_3x1 = new Size_Generic(3, 1, 1),
			Size_4x1 = new Size_Generic(4, 1, 1),
			Size_4x2 = new Size_Generic(4, 2, 1),
			Size_3x2 = new Size_Generic(3, 2, 2),
			Size_2x3 = new Size_Generic(2, 3, 1),
			Size_5x1,
			Size_5x2;

		public PlatformSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Tiles,
					Name = "platform",
					Index = 0,
					Width = 2,
					Height = 2,
					Extension = "png"
				};

			this.Size_5x1 =
				new TileSelector.Composite(5, 1,
					(Level, Position) =>
					{
						this.Size_2x1.CreateTo(Level, Position[0, 0]);
						this.Size_3x1.CreateTo(Level, Position[2, 0]);
					}
				);

			this.Size_5x2 =
				new TileSelector.Composite(5, 2,
					(Level, Position) =>
					{
						this.Size_2x2.CreateTo(Level, Position[0, 0]);
						this.Size_3x2.CreateTo(Level, Position[2, 0]);
					}
				);

			this.Sizes = new View.SelectorInfo[]
			{
				Size_1x1,
				Size_2x1,
				Size_2x2,
				Size_3x1,
				Size_4x1,
				Size_4x2,
				Size_3x2,
				Size_2x3,
				Size_5x1,
				Size_5x2,

			};
		}



		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "platform")
			{

			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				Name.Index = (Name.Index + 1) % Name.IndexCount;

				RemovePlatforms(this, Level, Position);

				var u = new Platform(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownPlatforms.Add(u);

				var z = Level.Zoom;
				var x = Position.ContentX * z;
				var y = Position.ContentY * z;


				var o = new Obstacle
				{
					Left = x,
					Top = y,
					Right = x + PrimitiveTile.Width * z,
					Bottom = y + PrimitiveTile.Heigth * z
				};

				#region scan to right
				var right = o.WithOffset(PrimitiveTile.Width * z, 0);

				// is there a bridge to my right?
				var right_p = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(right));

				#endregion

				#region scan to left
				var left = o.WithOffset(-PrimitiveTile.Width * z, 0);

				// is there a bridge to my right?
				var left_p = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(left));
				#endregion

				if (left_p != null)
				{
					new BridgeSelector().Size_1x1.CreateTo(Level,
						new View.SelectorPosition
						{
							ContentX = Position.ContentX - PrimitiveTile.Width,
							ContentY = Position.ContentY
						}
					);
				}

				if (right_p != null)
				{
					new BridgeSelector().Size_1x1.CreateTo(Level,
						new View.SelectorPosition
						{
							ContentX = Position.ContentX + this.Width,
							ContentY = Position.ContentY
						}
					);
				}
			}
		}




		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			TileSelector.AttachToLevel(new PlatformSelector().Sizes, Position, Tile, Level);
		}
	}
}
