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
	public static class PlatformSelector
	{
		public const string Identifier = "P";


		internal static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_Generic(1, 1, 1),
				//new Size_Generic(1, 2, 1),
				new Size_Generic(2, 2, 1),
				new Size_Generic(4, 2, 1),
				//new Size_Generic(2, 4, 1),
				
				new Size_Generic(3, 2, 1),
				new Size_Generic(2, 3, 1),

				new Size_Generic(2, 1, 1),
			};

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
					BridgeSelector.DefaultSize.CreateTo(Level,
						new View.SelectorPosition
						{
							ContentX = Position.ContentX - PrimitiveTile.Width,
							ContentY = Position.ContentY
						}
					);
				}

				if (right_p != null)
				{
					BridgeSelector.DefaultSize.CreateTo(Level,
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
			TileSelector.AttachToLevel(Sizes, Position, Tile, Level);
		}
	}
}
