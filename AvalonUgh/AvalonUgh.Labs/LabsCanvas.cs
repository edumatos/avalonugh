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
using System.Windows.Input;
using AvalonUgh.Code;

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

			};

			var CurrentLevel = KnownAssets.Path.Assets + "/level04.txt";

			Console.WriteLine("loading: " + CurrentLevel);

			CurrentLevel.ToStringAsset(
				LevelText =>
				{
					Console.WriteLine("loading done: " + CurrentLevel);
					Console.WriteLine("loading done: " + LevelText.Length + " chars");
					Console.WriteLine(LevelText);

					var Level = new Level(LevelText, Zoom);

					Console.WriteLine(new { Level.Text, Level.Code, Level.Background, Level.Water, Level.Map.Width, Level.Map.Height }.ToString());

					var BorderSize = Zoom * 10;

					var Obstacles = new List<Obstacle>
					{
						#region Borders
						new Obstacle
						{
							Left = 0,
							Top = -BorderSize,
							Right = DefaultWidth,
							Bottom = -12 * Zoom
						},
						new Obstacle
						{
							Left = -BorderSize,
							Top = 0,
							Right = -16 * Zoom,
							Bottom = DefaultHeight
						},
						new Obstacle
						{
							Left = DefaultWidth + 16 * Zoom,
							Top = 0,
							Right = DefaultWidth + 16 * Zoom + BorderSize,
							Bottom = DefaultHeight
						},
						new Obstacle
						{
							Left = 0,
							Top = DefaultHeight ,
							Right = DefaultWidth,
							Bottom = DefaultHeight + BorderSize
						},
#endregion

					};





					#region Background
					Level.Map.ForEach(
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
								Obstacles.Add(
									new Obstacle
									{
										Tile = Tile,
										Position = k,
										Left = k.X * 16 * Zoom,
										Top = k.Y * 12 * Zoom,
										Right = (k.X + Tile.Width) * 16 * Zoom,
										Bottom = (k.Y + Tile.Height) * 12 * Zoom
									}
								);

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
								Obstacles.Add(
									new Obstacle
									{
										Tile = Tile,
										Position = k,
										Left = k.X * 16 * Zoom,
										Top = k.Y * 12 * Zoom,
										Right = (k.X + Tile.Width) * 16 * Zoom,
										Bottom = (k.Y + Tile.Height) * 12 * Zoom
									}
								);

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



					var actor0 = new Actor.man0(Zoom).AttachContainerTo(this);

					actor0.MoveToTile(16.5, 1);
					var actor1 = new Actor.man0(Zoom).AttachContainerTo(this);

					actor1.MoveToTile(14.5, 1);


					new Sign(Zoom) { Value = 3 }.AttachContainerTo(this).MoveToTile(9, 12);
					new Sign(Zoom) { Value = 2 }.AttachContainerTo(this).MoveToTile(3, 6);
					new Sign(Zoom) { Value = 1 }.AttachContainerTo(this).MoveToTile(14, 2);




					var KnownRocks = new List<Rock>();
					var KnownTrees = new List<Tree>();


					//var WaterHeight = 50 * Zoom;
					//var WaterTop = DefaultHeight - WaterHeight - 9 * Zoom;

					var KnownWater = new Water(
						new Water.Info
						{
							DefaultWidth = DefaultWidth,
							Zoom = Zoom,
							WaterHeight = Level.WaterHeight,
							WaterTop = Level.WaterTop,

						}
					);

					var tree1 = new Tree(Zoom).AttachContainerTo(this);

					tree1.MoveToTile(4.5, 5);
					KnownTrees.Add(tree1);

					#endregion

					#region bird
					{
						var bird1 = new Bird(Zoom);
						var bird1_x = DefaultWidth + bird1.Width / 2;
						var bird1_y = Zoom * 32;

						bird1.AttachContainerTo(this).MoveTo(bird1_x, bird1_y);


						(1000 / 30).AtInterval(
							delegate
							{
								bird1_x -= 4;
								if (bird1_x < -(bird1.Width / 2))
									bird1_x = DefaultWidth + bird1.Width / 2;

								bird1.MoveTo(bird1_x, bird1_y);
							}
						);


						var bird2 = new Bird(Zoom);
						var bird2_x = DefaultWidth / 2;
						var bird2_y = Zoom * 70;

						bird2.AttachContainerTo(this).MoveTo(bird2_x, bird2_y);


						(1000 / 30).AtInterval(
							delegate
							{
								bird2_x -= 2;
								if (bird2_x < -(bird2.Width / 2))
									bird2_x = DefaultWidth + bird2.Width / 2;

								bird2.MoveTo(bird2_x, bird2_y);
							}
						);

					}
					#endregion



					var rock1 = new Rock(Zoom);

					rock1.AttachContainerTo(this);
					rock1.MoveTo(DefaultWidth / 2, DefaultHeight / 3);

					KnownRocks.Add(rock1);

					#region vehicle


					var xveh = new Vehicle(Zoom);
					xveh.ColorStripe = Colors.Red;
					xveh.AttachContainerTo(this);
					xveh.MoveTo(DefaultWidth / 2, DefaultHeight / 2);

					var twin = new Vehicle(Zoom);
					twin.ColorStripe = Colors.Blue;

					twin.AttachContainerTo(this);
					twin.MoveTo(DefaultWidth / 3, DefaultHeight / 4);

					var twin2 = new Vehicle(Zoom);
					twin2.ColorStripe = Colors.Yellow;
					twin2.Density = 1.11;
					twin2.AttachContainerTo(this);
					twin2.MoveTo(DefaultWidth * 2 / 3, DefaultHeight / 2);

					var ph = new Physics
					{
						WaterTop = Level.WaterTop,
						Obstacles = Obstacles,
						Vehicles = new[]
							{
								xveh,
								twin2,
								twin
							}.AsEnumerable(),
						Rocks = KnownRocks,
						Trees = KnownTrees
					};



					this.FocusVisualStyle = null;
					this.Focusable = true;
					this.Focus();

					this.MouseLeftButtonDown +=
						(sender, args) =>
						{
							this.Focus();
						};

					var k1 = new KeyboardInput(
						new KeyboardInput.Arguments
						{
							Left = Key.Left,
							Right = Key.Right,
							Up = Key.Up,
							Down = Key.Down,
							Drop = Key.Space,
							Enter = Key.Enter,

							InputControl = this,
							Vehicle = xveh,
							Water = KnownWater
						}
					);

					k1.Drop +=
						delegate
						{
							var rock2 = new Rock(Zoom);

							rock2.AttachContainerTo(this);
							rock2.MoveTo(xveh.X, xveh.Y);
							rock2.VelocityX = xveh.VelocityX;
							rock2.VelocityY = xveh.VelocityY;

							KnownRocks.Add(rock2);
						};


					var k2 = new KeyboardInput(
						new KeyboardInput.Arguments
						{
							Left = Key.A,
							Right = Key.D,
							Up = Key.W,
							Down = Key.S,
							Drop = Key.Q,
							Enter = Key.E,

							InputControl = this,
							Vehicle = twin,
							Water = KnownWater
						}
					);

					k2.Drop +=
						delegate
						{
							var rock2 = new Rock(Zoom);

							rock2.AttachContainerTo(this);
							rock2.MoveTo(twin.X, twin.Y);
							rock2.VelocityX = twin.VelocityX;
							rock2.VelocityY = twin.VelocityY;

							KnownRocks.Add(rock2);
						};



					(1000 / 40).AtInterval(
						delegate
						{
							k1.Tick();
							k2.Tick();

							ph.Apply();

						}
					);



					#endregion



					KnownWater.AttachContainerTo(this);

					var ff = new Flashlight(Zoom, DefaultWidth, DefaultHeight - 9 * Zoom);

					ff.AttachContainerTo(this);
					ff.MoveTo(xveh.X, xveh.Y);
					ff.Visible = false;

					xveh.LocationChanged +=
						delegate
						{
							if (ff.Visible)
								ff.MoveTo(
									xveh.X,
									xveh.Y
								);
						};

					this.KeyUp +=
						(sender, args) =>
						{
							if (args.Key == Key.F)
								ff.Visible = !ff.Visible;
						};

					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Assets + "/statusbar.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = DefaultWidth,
						Height = 9 * Zoom,
					}.AttachTo(this).MoveTo(0, DefaultHeight - 9 * Zoom);

					(Assets.Shared.KnownAssets.Path.Audio + "/newlevel.mp3").PlaySound();
				}
			);


		}
	}


}
