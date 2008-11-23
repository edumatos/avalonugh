using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public static class StoneSelector
	{
		public const string Identifier = "S";


		internal static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_1x1(),
				new Size_2x2(),
				new Size_4x2(),
				new Size_2x4(),
				new Size_2x3(),
				new Size_2x1(),
			};

	
		[Script]
		internal class Size_1x1 : TileSelector
		{
			public Size_1x1() { }


			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);
				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
					}
				};

				u.Image
				.WithZoom(Level.Zoom)
				.MoveTo(Position.ContentX, Position.ContentY)
				.SizeTo(this.Width, this.Height);

				Level.KnownStones.Add(u);
			}
		}


		[Script]
		internal class Size_2x2 : TileSelector
		{
			public Size_2x2() : base(2, 2) { }

			int Alternate = 0;

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);
				Alternate++;

				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone" + (Alternate % 2) + "_2x2.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
					}
				};

				u.Image
				.WithZoom(Level.Zoom)
				.MoveTo(Position.ContentX, Position.ContentY)
				.SizeTo(this.Width, this.Height);

				Level.KnownStones.Add(u);
			}
		}

		[Script]
		internal class Size_4x2 : TileSelector
		{
			public Size_4x2() : base(4, 2) { }

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);
				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_4x2.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
					}
				};

				u.Image
				.WithZoom(Level.Zoom)
				.MoveTo(Position.ContentX, Position.ContentY)
				.SizeTo(this.Width, this.Height);

				Level.KnownStones.Add(u);
			}
		}



		[Script]
		internal class Size_2x4 : TileSelector
		{
			public Size_2x4() : base(2, 4) { }

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);
				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x4.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
					}
				};

				u.Image
				.WithZoom(Level.Zoom)
				.MoveTo(Position.ContentX, Position.ContentY)
				.SizeTo(this.Width, this.Height);

				Level.KnownStones.Add(u);
			}
		}

		[Script]
		internal class Size_2x3 : TileSelector
		{
			public Size_2x3() : base(2, 3) { }

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);
				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x3.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
					}
				};

				u.Image
				.WithZoom(Level.Zoom)
				.MoveTo(Position.ContentX, Position.ContentY)
				.SizeTo(this.Width, this.Height);

				Level.KnownStones.Add(u);
			}
		}

		[Script]
		internal class Size_2x1 : TileSelector
		{
			public Size_2x1() : base(2, 1) { }

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemovePlatforms(this, Level, Position);

				var u = new Stone(Level, this)
				{
					Position = Position,
					Image = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x1.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
					}
				};

				u.Image
				.WithZoom(Level.Zoom)
				.MoveTo(Position.ContentX, Position.ContentY)
				.SizeTo(this.Width, this.Height);

				Level.KnownStones.Add(u);
			}
		}



		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			var Selector = Sizes.SingleOrDefault(
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
					TileX = Position.X,
					TileY = Position.Y,
					ContentX = Position.X * PrimitiveTile.Width,
					ContentY = Position.Y * PrimitiveTile.Heigth,
				}
			);
		}
	}
}
