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
			#region CreateCustom
			Func<int, int, string, string, Image> CreateCustom =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					return new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w,
						Height = h,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};
			#endregion

			#region CreateCustom
			Func<int, int, string, string, Image> CreateCustom_2x2 =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					return new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w * 2,
						Height = h * 2,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};
			#endregion


			var CreateTile = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateSprite = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);
			var CreateSprite_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);


			var Create = new
			{
				fence0 = CreateTile.FixLastParam("fence0"),

				stone0 = CreateTile.FixLastParam("stone0"),
				stone1_2x2 = CreateTile_2x2.FixLastParam("stone1_2x2"),

				bridge0 = CreateTile.FixLastParam("bridge0"),
				bridge0left = CreateTile.FixLastParam("bridge0left"),
				bridge0right = CreateTile.FixLastParam("bridge0right"),


				platform0 = CreateTile.FixLastParam("platform0"),
				platform0_2x2 = CreateTile_2x2.FixLastParam("platform0_2x2"),
				platform1 = CreateTile.FixLastParam("platform1"),

				ridge0 = CreateTile.FixLastParam("ridge0"),
				ridge0_2x2 = CreateTile_2x2.FixLastParam("ridge0_2x2"),

				sign2 = CreateSprite.FixLastParam("sign2"),

				rock0 = CreateSprite.FixLastParam("rock0"),
				rock1 = CreateSprite.FixLastParam("rock1"),

				tree0_2x2 = CreateSprite_2x2.FixLastParam("tree0_2x2"),
				tree1_2x2 = CreateSprite_2x2.FixLastParam("tree1_2x2"),
			};

			#region Background
			for (int x = 2; x < 20; x++)
				Create.stone0(x, 13);

			for (int x = 2; x < 20; x += 2)
				Create.stone1_2x2(x, 14);
			for (int x = 2; x < 20; x += 2)
				Create.stone1_2x2(x, 16);
			for (int x = 2; x < 20; x += 2)
				Create.stone1_2x2(x, 18);

			for (int y = 14; y < 20; y += 2)
				Create.ridge0_2x2(0, y);


			for (int x = 0; x < 6; x++)
				Create.platform0(x, 7);

			Create.platform0_2x2(0, 12);
			Create.platform1(2, 12);
			Create.bridge0left(3, 12);
			for (int x = 4; x < 18; x++)
				Create.bridge0(x, 12);


			Create.platform0(19, 12);
			Create.bridge0right(18, 12);

			Create.ridge0(19, 13);
			#endregion

			Create.sign2(4, 11);

			Action<Image, Image> Blink =
				(a, b) =>
				{
					(1000 / 10).AtIntervalWithCounter(
						c =>
						{
							if (c % 12 == 0)
							{
								b.Hide();
								a.Show();

								return;
							}

							a.Hide();
							b.Show();
						}
					);
				};

			Action<Image, Image> SlowFrameChange =
				(a, b) =>
				{
					(5000).AtIntervalWithCounter(
						c =>
						{
							if (c % 2 == 0)
							{
								b.Hide();
								a.Show();

								return;
							}

							a.Hide();
							b.Show();
						}
					);
				};


			SlowFrameChange(
				Create.rock0(8, 11),
				Create.rock1(8, 11)
			);

			Blink(
				Create.tree1_2x2(13, 10),
				Create.tree0_2x2(13, 10)
			);
		}
	}
}
