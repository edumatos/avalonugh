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


		public static readonly View.SelectorInfo[] Sizes =
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
		public class Size_1x1 : TileSelector
		{
			public Size_1x1()
			{
				PrimitiveTileCountX = 1;
				PrimitiveTileCountY = 1;

				Invoke =
					(View, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = this.Width * View.Level.Zoom,
							Height = this.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}
		}


		[Script]
		public class Size_2x2 : TileSelector
		{
			public Size_2x2()
				: base(2, 2)
			{

				Invoke =
					(View, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone1_2x2.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = this.Width * View.Level.Zoom,
							Height = this.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}
		}

		[Script]
		public class Size_4x2 : TileSelector
		{
			public Size_4x2()
				: base(4, 2)
			{

				Invoke =
					(View, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_4x2.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = this.Width * View.Level.Zoom,
							Height = this.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}
		}



		[Script]
		public class Size_2x4 : TileSelector
		{
			public Size_2x4()
				: base(2, 4)
			{

				Invoke =
					(View, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x4.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = this.Width * View.Level.Zoom,
							Height = this.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}
		}

		[Script]
		public class Size_2x3 : TileSelector
		{
			public Size_2x3()
				: base(2, 3)
			{

				Invoke =
					(View, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x3.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = this.Width * View.Level.Zoom,
							Height = this.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}
		}

		[Script]
		public class Size_2x1 : TileSelector
		{
			public Size_2x1()
				: base(2, 1)
			{

			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var u = new Stone
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
				return;

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
