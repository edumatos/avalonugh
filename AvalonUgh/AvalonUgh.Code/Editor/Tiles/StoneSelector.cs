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
					(View, Selector, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = Selector.Width * View.Level.Zoom,
							Height = Selector.Height * View.Level.Zoom,
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
			{
				PrimitiveTileCountX = 2;
				PrimitiveTileCountY = 2;
				

				Invoke =
					(View, Selector, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone1_2x2.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = Selector.Width * View.Level.Zoom,
							Height = Selector.Height * View.Level.Zoom,
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
			{
				PrimitiveTileCountX = 4;
				PrimitiveTileCountY = 2;

				Invoke =
					(View, Selector, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_4x2.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = Selector.Width * View.Level.Zoom,
							Height = Selector.Height * View.Level.Zoom,
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
			{
				PrimitiveTileCountX = 2;
				PrimitiveTileCountY = 4;

				Invoke =
					(View, Selector, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x4.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = Selector.Width * View.Level.Zoom,
							Height = Selector.Height * View.Level.Zoom,
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
			{
				PrimitiveTileCountX = 2;
				PrimitiveTileCountY = 3;

				Invoke =
					(View, Selector, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x3.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = Selector.Width * View.Level.Zoom,
							Height = Selector.Height * View.Level.Zoom,
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
			{
				PrimitiveTileCountX = 2;
				PrimitiveTileCountY = 1;

				Invoke =
					(View, Selector, Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0_2x1.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = Selector.Width * View.Level.Zoom,
							Height = Selector.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}
		}


		public static void AttachToLevel(ASCIIImage.Entry k, ASCIITileSizeInfo Tile, Level Level)
		{
			
		}
	}
}
