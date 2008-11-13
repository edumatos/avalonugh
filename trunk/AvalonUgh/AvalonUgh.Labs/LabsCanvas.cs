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
using System.Windows.Input;

namespace AvalonUgh.Labs.Shared
{

	[Script]
	public class LabsCanvas : Canvas
	{
		public const int Zoom = 2;

		public const int DefaultWidth = 320 * Zoom;
		public const int DefaultHeight = 200 * Zoom;


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

			#region CreateCustom
			Func<int, int, string, string, Image> CreateCustom_2x1 =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					return new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w * 2,
						Height = h * 1,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};
			#endregion

			#region CreateCustom
			Func<int, int, string, string, Image> CreateCustom_4x2 =
				(x, y, tile, path) =>
				{
					var w = 16 * Zoom;
					var h = 12 * Zoom;

					return new Image
					{
						Source = (path + "/" + tile + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = w * 4,
						Height = h * 2,


					}.MoveTo(x * w, y * h).AttachTo(this);
				};
			#endregion


			var CreateTile = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x1 = CreateCustom_2x1.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x3 = CreateCustom_2x3.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_2x4 = CreateCustom_2x4.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);
			var CreateTile_4x2 = CreateCustom_4x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Tiles);

			var CreateSprite = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);
			var CreateSprite_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);


			var Create = new
			{
				fence0 = CreateTile.FixLastParam("fence0"),

				stone0 = CreateTile.FixLastParam("stone0"),
				stone0_2x1 = CreateTile_2x1.FixLastParam("stone0_2x1"),
				stone1_2x2 = CreateTile_2x2.FixLastParam("stone1_2x2"),
				stone0_2x3 = CreateTile_2x3.FixLastParam("stone0_2x3"),
				stone0_2x4 = CreateTile_2x4.FixLastParam("stone0_2x4"),
				stone0_4x2 = CreateTile_4x2.FixLastParam("stone0_4x2"),

				bridge0 = CreateTile.FixLastParam("bridge0"),
				bridge0left = CreateTile.FixLastParam("bridge0left"),
				bridge0right = CreateTile.FixLastParam("bridge0right"),


				platform0 = CreateTile.FixLastParam("platform0"),
				platform0_2x1 = CreateTile_2x1.FixLastParam("platform0_2x1"),
				platform0_2x2 = CreateTile_2x2.FixLastParam("platform0_2x2"),
				platform0_2x3 = CreateTile_2x3.FixLastParam("platform0_2x3"),
				platform0_4x2 = CreateTile_4x2.FixLastParam("platform0_4x2"),
				platform1 = CreateTile.FixLastParam("platform1"),

				ridge0 = CreateTile.FixLastParam("ridge0"),
				ridge0_2x2 = CreateTile_2x2.FixLastParam("ridge0_2x2"),

				cave0_2x2 = CreateTile_2x2.FixLastParam("cave0_2x2"),
				cave1_2x2 = CreateTile_2x2.FixLastParam("cave1_2x2"),

				sign1 = CreateSprite.FixLastParam("sign1"),
				sign2 = CreateSprite.FixLastParam("sign2"),
				sign3 = CreateSprite.FixLastParam("sign3"),
				sign4 = CreateSprite.FixLastParam("sign4"),

				rock0 = CreateSprite.FixLastParam("rock0"),
				rock1 = CreateSprite.FixLastParam("rock1"),

				tree0_2x2 = CreateSprite_2x2.FixLastParam("tree0_2x2"),
				tree1_2x2 = CreateSprite_2x2.FixLastParam("tree1_2x2"),


				man0_00_2x2 = CreateSprite_2x2.FixLastParam("man0_00_2x2"),
				man0_01_2x2 = CreateSprite_2x2.FixLastParam("man0_01_2x2"),
			};

			(KnownAssets.Path.Assets + "/level03.txt").ToStringAsset(
				LevelText =>
				{
					var Level = new ASCIIImage(LevelText);

					#region Background
					Level.ForEach(
						k =>
						{
							if (char.IsNumber(k.Value, 0))
								return;

							var Tile = new ASCIITileSizeInfo(k);

							var Is_2x1 = Tile.Width == 2 && Tile.Height == 1;
							var Is_2x2 = Tile.Width == 2 && Tile.Height == 2;
							var Is_2x3 = Tile.Width == 2 && Tile.Height == 3;
							var Is_2x4 = Tile.Width == 2 && Tile.Height == 4;
							var Is_4x2 = Tile.Width == 4 && Tile.Height == 2;






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

							// cave
							if (k.Value == "C")
							{

								if (Is_2x2)
								{
									//if ((k.X * k.Y) % 2 == 0)
									Create.cave0_2x2(k.X, k.Y);
									//else
									//    Create.cave1_2x2(k.X, k.Y);
									return;
								}

								return;
							}

							// platform
							if (k.Value == "P")
							{
								if (Is_2x3)
								{
									Create.platform0_2x3(k.X, k.Y);
									return;
								}

								if (Is_4x2)
								{
									Create.platform0_4x2(k.X, k.Y);
									return;
								}

								if (Is_2x1)
								{
									Create.platform0_2x1(k.X, k.Y);
									return;
								}

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
								if (Is_4x2)
								{
									Create.stone0_4x2(k.X, k.Y);
									return;
								}

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

								if (Is_2x1)
								{
									Create.stone0_2x1(k.X, k.Y);
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


							// fence
							if (k.Value == "F")
							{
								Create.fence0(k.X, k.Y);
								return;
							}

							if (k.Value != " ")
								Create.fence0(k.X, k.Y);
						}
					);

					#endregion




					#region sprites

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
						Create.rock0(6, 12),
						Create.rock1(6, 12)
					);

					FrameChange(500,
						Create.man0_00_2x2(16, 1),
						Create.man0_01_2x2(16, 1)
					);


					Create.sign3(9, 12);
					Create.sign2(3, 6);
					Create.sign1(14, 2);


					//Create.sign4(4, 11);



					Blink(
						Create.tree1_2x2(4, 5),
						Create.tree0_2x2(4, 5)
					);


					#endregion

					#region bird
					{
						var w = 16 * Zoom;
						var h = 12 * Zoom;
						var x = DefaultWidth;

						var birdc = new Canvas
						{

						}.MoveTo(6 * w, 2 * h).AttachTo(this);

						var bird = Enumerable.Range(0, 13).ToArray(
							index =>
								new Image
								{
									Source = (Assets.Shared.KnownAssets.Path.Sprites + "/bird0_" + ("" + index).PadLeft(2, '0') + "_2x3.png").ToSource(),
									Stretch = Stretch.Fill,
									Width = w * 2,
									Height = h * 3,
									Visibility = Visibility.Hidden
								}.AttachTo(birdc)
						);

						bird.AsCyclicEnumerable().ForEach(
							(Image value, Action SignalNext) =>
							{
								value.Visibility = Visibility.Visible;
								x -= 2;
								if (x < -(w * 2))
									x = DefaultWidth;

								birdc.MoveTo(x, Zoom * h);

								(1000 / 30).AtDelay(
									delegate
									{
										value.Visibility = Visibility.Hidden;
										SignalNext();
									}
								);
							}
						);
					}
					#endregion


					var WaterHeight = 24 * Zoom;
					var WaterTop = DefaultHeight - WaterHeight - 9 * Zoom;


					#region vehicle
					{
						var w = 16 * Zoom;
						var h = 12 * Zoom;
						var x = 10.0 * w;
						var y = 6.0 * h;

						var y_float = 0.0;
						var y_float_acc = 0.1;
						var y_float_max = 4.0;

						var vehc = new Canvas
						{

						}.MoveTo(x, y ).AttachTo(this);

						var veh = Enumerable.Range(0, 7).ToArray(
							index =>
								new Image
								{
									Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_" + ("" + index).PadLeft(2, '0') + "_2x2.png").ToSource(),
									Stretch = Stretch.Fill,
									Width = w * 2,
									Height = h * 2,
									Visibility = Visibility.Hidden
								}.AttachTo(vehc)
						);

						(1000 / 30).AtIntervalWithCounter(
							c =>
							{
								var yy = Math.Cos(c * 0.1);

								vehc.MoveTo(x, y + yy * Zoom * y_float);
							}
						);

						veh.AsCyclicEnumerable().ForEach(
							(Image value, Action SignalNext) =>
							{
								value.Visibility = Visibility.Visible;

								(1000 / 30).AtDelay(
									delegate
									{
										value.Visibility = Visibility.Hidden;
										SignalNext();
									}
								);
							}
						);


						this.Focusable = true;
						this.Focus();

						this.MouseLeftButtonDown +=
							(sender, args) =>
							{
								this.Focus();
							};

						var KeyState = new Dictionary<Key, bool>
						{
							{Key.Up, false},
							{Key.Down, false},
							{Key.Right, false},
							{Key.Left, false},
						};

						this.KeyDown +=
							(sender, args) =>
							{
								if (KeyState.ContainsKey(args.Key))
									KeyState[args.Key] = true;
							};


						this.KeyUp +=
							(sender, args) =>
							{
								if (KeyState.ContainsKey(args.Key))
									KeyState[args.Key] = false;
							};

						Func<Key, bool> IsKeyDown =
							k => KeyState[k];

						// http://www.regentsprep.org/Regents/physics/phys01/accgravi/index.htm
						// http://www.glenbrook.k12.il.us/GBSSCI/PHYS/Class/1DKin/U1L5b.html
						// http://www.regentsprep.org/Regents/physics/phys-topic.cfm?Course=PHYS&TopicCode=01a

						var speed_y = 0.0;
						var speed_y_Acc = 0.1;

						var speed_x = 0.0;
						var speed_x_Acc = 0.1;

						(1000 / 30).AtInterval(
							delegate
							{
								if (KeyState.Any(k => k.Value))
								{
									y_float = (y_float - y_float_acc).Max(0);
								}
								else
								{
									y_float = (y_float + y_float_acc / 2).Min(y_float_max);
								}

						
								if (IsKeyDown(Key.Up))
								{
									speed_y -= speed_y_Acc;
								}
								else if (IsKeyDown(Key.Down))
								{
									speed_y += speed_y_Acc;
								}
								else
								{
									if (speed_y > 0)
										speed_y -= speed_y_Acc / 2;
									else
										speed_y += speed_y_Acc / 2;



								}

								//if ((y - Zoom * 24) < WaterTop)
								//    speed_y += 0.1; // add gravity
								//else
								//    speed_y -= 0.2; // add water pressure


								y += speed_y * Zoom;

								if (IsKeyDown(Key.Left))
									speed_x -= speed_x_Acc;
								else if (IsKeyDown(Key.Right))
									speed_x += speed_x_Acc;
								else
								{
									if (speed_x > 0)
										speed_x -= speed_x_Acc / 2;
									else
										speed_x += speed_x_Acc / 2;
								}

								x += speed_x * Zoom;
							}
						);


					}
					#endregion


					#region water gradient
					// water with waves 
					// http://learnwpf.com/Posts/Post.aspx?postId=9b2c71c0-7136-4ee7-ab2a-f8eec62874af

					var WaterContainer = new Canvas
					{
						Width = DefaultWidth,
						Height = WaterHeight
					}.MoveTo(0, WaterTop).AttachTo(this);

					var WaterColorTop = Colors.Cyan;
					var WaterColorBottom = Colors.DarkCyan;

					WaterColorTop.A = 40;
					WaterColorBottom.A = 40;

					WaterColorTop.ToGradient(WaterColorBottom, WaterHeight / Zoom).Select(
						(c, i) =>
						{
							var Opacity = c.A / 255.0;

							c.A = 0xFF;

							return new Rectangle
							{
								Fill = new SolidColorBrush(c),
								Width = DefaultWidth,
								Height = Zoom,
								Opacity = Opacity
							}.MoveTo(0, i * Zoom).AttachTo(WaterContainer);
						}
					).ToArray();
					#endregion


					#region watersurface
					{

						var watersurfaces = Enumerable.Range(0, 3).ToArray(
							index =>
							{
								var frame = new Canvas
								{
									Width = DefaultWidth,
									Height = WaterHeight,
									//Visibility = Visibility.Hidden
								}.MoveTo(0, 0).AttachTo(WaterContainer);

								Action<double, int> CreateWater =
									(WaterOpacity, WaterIndex) =>
									{
										new Image
										{
											Source = (Assets.Shared.KnownAssets.Path.Assets + "/water" + index + ".png").ToSource(),
											Stretch = Stretch.Fill,
											Width = DefaultWidth,
											Height = 1 * Zoom,
											Opacity = WaterOpacity
										}.AttachTo(frame).MoveTo(0, WaterIndex * Zoom);
									};

								CreateWater.FixLastParamToIndex()(
									1,
									0.5,
									0.2
								);




								return frame;
							}
						);

						watersurfaces.AsCyclicEnumerable().ForEach(
							(value, SignalNext) =>
							{
								value.Visibility = Visibility.Visible;

								(300).AtDelay(
									delegate
									{
										value.Visibility = Visibility.Hidden;
										SignalNext();
									}
								);
							}
						);
					}
					#endregion

					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Assets + "/statusbar.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = DefaultWidth,
						Height = 9 * Zoom,
					}.AttachTo(this).MoveTo(0, DefaultHeight - 9 * Zoom);
				}
			);


		}
	}
}
