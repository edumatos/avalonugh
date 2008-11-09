using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvalonUgh.Labs.Shared
{
	[Script]
	public class LabsCanvas : Canvas
	{
		public const int DefaultWidth = 640;
		public const int DefaultHeight = 480;

		public LabsCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			this.ClipToBounds = true;

			Colors.Black.ToGradient(Colors.Red, DefaultHeight / 4).Select(
				(c, i) =>
					new Rectangle
					{
						Fill = new SolidColorBrush(c),
						Width = DefaultWidth,
						Height = 4,
					}.MoveTo(0, i * 4).AttachTo(this)
			).ToArray();


			var Zoom = 2;

			Action<int, int, string, string> CreateCustom =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w,
						Height = h,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};

			var CreateTile = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateSprite = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);


			var Create = new
			{
				stone0 = CreateTile.FixLastParam("stone0"),
				bridge0 = CreateTile.FixLastParam("bridge0"),
				sign2 = CreateSprite.FixLastParam("sign2"),
				rock0 = CreateSprite.FixLastParam("rock0"),
			};

			#region Background
			for (int x = 0; x < 20; x++)
				for (int y = 13; y < 20; y++)
				{
					Create.stone0(x, y);
				}

			for (int x = 0; x < 20; x++)
				Create.bridge0(x, 12);
			#endregion

			Create.sign2(4, 11);
			Create.rock0(8, 11);
		}
	}
}
