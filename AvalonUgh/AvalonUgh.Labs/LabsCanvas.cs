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
using AvalonUgh.Labs.Shared.Extensions;

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

			#region CreateCustom
			Func<int, int, string, string, Image> CreateCustom_2x3 =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					return new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w * 2,
						Height = h * 3,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};
			#endregion

			#region CreateCustom
			Func<int, int, string, string, Image> CreateCustom_2x4 =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					return new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w * 2,
						Height = h * 4,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};
			#endregion


			var CreateTile = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x3 = CreateCustom_2x3.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x4 = CreateCustom_2x4.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateSprite = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);
			var CreateSprite_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);


			var Create = new
			{
				fence0 = CreateTile.FixLastParam("fence0"),

				stone0 = CreateTile.FixLastParam("stone0"),
				stone1_2x2 = CreateTile_2x2.FixLastParam("stone1_2x2"),
				stone0_2x3 = CreateTile_2x3.FixLastParam("stone0_2x3"),
				stone0_2x4 = CreateTile_2x4.FixLastParam("stone0_2x4"),

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


				man0_00_2x2 = CreateSprite_2x2.FixLastParam("man0_00_2x2"),
				man0_01_2x2 = CreateSprite_2x2.FixLastParam("man0_01_2x2"),
			};

			("assets/AvalonUgh.Labs/level01.txt").ToStringAsset(
				LevelText =>
				{
					var Level = new ASCIIImage(LevelText);


					Level.ForEach(
						k =>
						{
							if (k.Value == "4")
								return;

							if (k.Value == "3")
								return;

							if (k.Value == "2")
								return;

							var Is_2x2 = false;
							var Is_2x3 = false;
							var Is_2x4 = false;



							if (k[1, 0] == "2")
								if (k[0, 1] == "2")
									if (k[1, 1] == "2")
										Is_2x2 = true;

							if (k[1, 0] == "2")
								if (k[0, 1] == "3")
									if (k[1, 1] == "3")
										if (k[0, 2] == "3")
											if (k[1, 2] == "3")
												Is_2x3 = true;

							if (k[1, 0] == "2")
								if (k[0, 1] == "4")
									if (k[1, 1] == "4")
										if (k[0, 2] == "4")
											if (k[1, 2] == "4")
												if (k[0, 3] == "4")
													if (k[1, 3] == "4")
														Is_2x4 = true;

							// ridge
							if (k.Value == "R")
							{
								if (Is_2x2)
								{
									Create.ridge0_2x2(k.X, k.Y);
									return;
								}

								Create.ridge0(k.X, k.Y);
								return;
							}

							// platform
							if (k.Value == "P")
							{
								if (Is_2x2)
								{
									Create.platform0_2x2(k.X, k.Y);
									return;
								}

								Create.platform0(k.X, k.Y);
								return;
							}

							// stone
							if (k.Value == "S")
							{
								if (Is_2x4)
								{
									Create.stone0_2x4(k.X, k.Y);
									return;
								}

								if (Is_2x3)
								{
									Create.stone0_2x3(k.X, k.Y);
									return;
								}

								if (Is_2x2)
								{
									Create.stone1_2x2(k.X, k.Y);
									return;
								}

								Create.stone0(k.X, k.Y);
								return;
							}



							// bridge
							if (k.Value == "B")
							{
								if (k[-1, 0] != "B")
								{
									Create.bridge0left(k.X, k.Y);
									return;
								}

								if (k[1, 0] != "B")
								{
									Create.bridge0right(k.X, k.Y);
									return;
								}


								Create.bridge0(k.X, k.Y);
								return;
							}


							if (k.Value != " ")
								Create.fence0(k.X, k.Y);
						}
					);
				}
			);

			//#region Background
			//for (int x = 2; x < 20; x++)
			//    Create.stone0(x, 13);

			//for (int x = 2; x < 20; x += 2)
			//    Create.stone1_2x2(x, 14);
			//for (int x = 2; x < 20; x += 2)
			//    Create.stone1_2x2(x, 16);
			//for (int x = 2; x < 20; x += 2)
			//    Create.stone1_2x2(x, 18);

			//for (int y = 14; y < 20; y += 2)
			//    Create.ridge0_2x2(0, y);


			//for (int x = 2; x < 6; x++)
			//    Create.platform0(x, 7);

			//for (int y = 0; y <= 8; y += 2)
			//    Create.ridge0_2x2(0, y);


			//Create.platform0_2x2(0, 12);
			//Create.platform1(2, 12);
			//Create.bridge0left(3, 12);
			//for (int x = 4; x < 18; x++)
			//    Create.bridge0(x, 12);


			//Create.platform0(19, 12);
			//Create.bridge0right(18, 12);

			//Create.ridge0(19, 13);
			//#endregion

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

			Action<int, Image, Image> FrameChange =
				(f, a, b) =>
				{
					f.AtIntervalWithCounter(
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

			FrameChange(5000,
				Create.rock0(8, 11),
				Create.rock1(8, 11)
			);

			FrameChange(500,
				Create.man0_00_2x2(16, 10),
				Create.man0_01_2x2(16, 10)
			);


			Blink(
				Create.tree1_2x2(13, 10),
				Create.tree0_2x2(13, 10)
			);
		}
	}
}
