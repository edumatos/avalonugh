using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public static class BridgeSelector
	{
		public const string Identifier = "B";


		internal static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_Generic(1, 1, 3),
				//new Size_Generic(1, 2, 1),
				//new Size_Generic(2, 2, 1),
				//new Size_Generic(4, 2, 1),
				////new Size_Generic(2, 4, 1),
				//new Size_Generic(2, 3, 1),
				//new Size_Generic(2, 1, 1),
			};

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

				// scan to right 
				var right = o.WithOffset(this.Width * z, 0);

				// is there a bridge to my right?
				var right_p = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(right));

				if (right_p != null)
				{
					this.Name.Index = VariationLeft;
				}

				// scan to left 
				var left = o.WithOffset(-this.Width * z, 0);

				// is there a bridge to my right?
				var left_p = Level.KnownBridges.FirstOrDefault(k => k.ToObstacle().Intersects(left));

				if (left_p != null)
				{
					if (this.Name.Index == VariationLeft)
					{
						this.Name.Index = VariationMiddle;
					}
					else
					{
						this.Name.Index = VariationRight;
					}
				}


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
			TileSelector.AttachToLevel(Sizes, Position, Tile, Level);
		}
	}
}
