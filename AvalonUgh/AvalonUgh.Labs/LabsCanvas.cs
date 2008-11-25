using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Code;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Editor.Tiles;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Labs.Shared
{

	[Script]
	public class LabsCanvas : Canvas
	{
		// the game should include all original levels

		public const int Zoom = 2;

		public const int DefaultWidth = 640;
		public const int DefaultHeight = 400;

		public const int StatusbarZoom = 2;

		public LabsCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			this.ClipToBounds = true;

			new[]
			{
				Colors.Black,
				Colors.Yellow,
				Colors.Red,
				Colors.Black
			}.ToGradient(DefaultHeight / 4).Select(
				(c, i) =>
					new Rectangle
					{
						Fill = new SolidColorBrush(c),
						Width = DefaultWidth,
						Height = 4,
					}.MoveTo(0, i * 4).AttachTo(this)
			).ToArray();

			var GameContent = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);


			var CurrentLevel = KnownAssets.Path.Assets + "/level08.txt";

			Console.WriteLine("loading: " + CurrentLevel);

			CurrentLevel.ToStringAsset(
				LevelText =>
				{
					Console.WriteLine("loading done: " + CurrentLevel);
					Console.WriteLine("loading done: " + LevelText.Length + " chars");
					Console.WriteLine(LevelText);

					var Level = new Level(LevelText, Zoom);

					// subtract statusbar
					var View = new View(DefaultWidth, DefaultHeight - 9 * StatusbarZoom, Level);

					View.Flashlight.Visible = true;

					Console.WriteLine(
						new
						{
							Text = Level.AttributeText.Value,
							Code = Level.AttributeCode.Value,
							Background = Level.AttributeBackground.Value,
							Water = Level.AttributeWater.Value,
							Level.Map.Width,
							Level.Map.Height
						}.ToString()
					);

					#region x
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


							}.MoveTo(x * w, y * h).AttachTo(View.Platforms);
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


							}.MoveTo(x * w, y * h).AttachTo(View.Platforms);
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


							}.MoveTo(x * w, y * h).AttachTo(View.Platforms);
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


							}.MoveTo(x * w, y * h).AttachTo(View.Platforms);
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


							}.MoveTo(x * w, y * h).AttachTo(View.Platforms);
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


							}.MoveTo(x * w, y * h).AttachTo(View.Platforms);
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
					

						bridge0 = CreateTile.FixLastParam("bridge0"),
						bridge0left = CreateTile.FixLastParam("bridge0left"),
						bridge0right = CreateTile.FixLastParam("bridge0right"),


						flower0 = CreateSprite.FixLastParam("flower0"),
						fish0 = CreateSprite.FixLastParam("fish0"),
						fish1 = CreateSprite.FixLastParam("fish1"),

					
						weed0_2x2 = CreateSprite_2x2.FixLastParam("weed0_00_2x2"),

					

					};
					#endregion

				







					#region Background
					Level.Map.ForEach(
						k =>
						{
							var Tile = new ASCIITileSizeInfo(k);

							// ridge
							if (k.Value == RidgeSelector.Identifier)
							{
								RidgeSelector.AttachToLevel(k, Tile, Level);

								return;
							}

							// cave
							if (k.Value == CaveSelector.Identifier)
							{
								CaveSelector.AttachToLevel(k, Tile, Level);

								return;
							}

							// fence
							if (k.Value == FenceSelector.Identifier)
							{
								FenceSelector.AttachToLevel(k, Tile, Level);

								return;
							}

							// stone
							if (k.Value == StoneSelector.Identifier)
							{
								StoneSelector.AttachToLevel(k, Tile, Level);

								return;
							}

							// platform
							if (k.Value == PlatformSelector.Identifier)
							{
								PlatformSelector.AttachToLevel(k, Tile, Level);

								return;
							}

							// bridge
							if (k.Value == BridgeSelector.Identifier)
							{
								BridgeSelector.AttachToLevel(k, Tile, Level);

								return;
							}
							

						}
					);

					#endregion


					//Create.weed0_2x2(0, 14);
					//Create.weed0_2x2(2, 14);

					//Create.flower0(0, 6);

					var fish_x = -PrimitiveTile.Width;

					new[]
					{
						Create.fish0(2, 12),
						Create.fish1(2, 12)
					}.AsCyclicEnumerable().ForEach(
						(fish, next) =>
						{
							fish_x = (fish_x + 1 * Zoom) % View.ContentActualWidth;


							fish.MoveTo(fish_x, Level.WaterTop + Level.WaterHeight / 3);

							fish.Show();

							(1000 / 15).AtDelay(
								delegate
								{
									fish.Hide();
									next();
								}
							);
						}
					);

					#region sprites

					var KnownActors = new List<Actor>();

					//new Actor[]
					//{
					//    new Actor.man1(Zoom),
					//    new Actor.man0(Zoom),
					//    new Actor.woman0(Zoom),
					//}.ForEach(
					//    (actor, index) =>
					//    {
					//        actor.AttachContainerTo(View.Entities);

					//        var cave = Level.KnownCaves.ToArray().AtModulus(index);

					//        actor.MoveTo(
					//            (cave.Position.TileX + 1) * PrimitiveTile.Width * Zoom,
					//            (cave.Position.TileY + 1) * PrimitiveTile.Heigth * Zoom
					//        );

					//        KnownActors.Add(actor);
					//    }
					//);



					var KnownBirds = new List<Bird>();




					Level.KnownTrees.ToArray().AttachContainerTo(View.Entities);
					Level.KnownSigns.ToArray().AttachContainerTo(View.Entities);
					Level.KnownRocks.ToArray().AttachContainerTo(View.Entities);



					#endregion

					#region bird
					{
						var bird1 = new Bird(Zoom);
						var bird1_x = DefaultWidth + bird1.Width / 2;
						var bird1_y = Zoom * 32;

						bird1.AttachContainerTo(View.Entities).MoveTo(bird1_x, bird1_y);
						KnownBirds.Add(bird1);

						(1000 / 30).AtInterval(
							delegate
							{
								bird1_x -= 4;

								if (bird1_x < -(bird1.Width / 2))
									bird1_x = View.ContentActualWidth + bird1.Width / 2;

								bird1.MoveTo(bird1_x, bird1_y);
							}
						);


						var bird2 = new Bird(Zoom);
						var bird2_x = DefaultWidth / 2;
						var bird2_y = Zoom * 70;

						bird2.AttachContainerTo(View.Entities).MoveTo(bird2_x, bird2_y);
						KnownBirds.Add(bird2);


						(1000 / 30).AtInterval(
							delegate
							{
								bird2_x -= 2;
								if (bird2_x < -(bird2.Width / 2))
									bird2_x = View.ContentActualWidth + bird2.Width / 2;

								bird2.MoveTo(bird2_x, bird2_y);
							}
						);

					}
					#endregion





					#region vehicle


					var xveh = new Vehicle(Zoom);
					xveh.ColorStripe = Colors.Red;
					xveh.AttachContainerTo(View.Entities);
					xveh.MoveTo(DefaultWidth / 2, DefaultHeight / 2);

					var twin = new Vehicle(Zoom);
					twin.ColorStripe = Colors.Blue;

					twin.AttachContainerTo(View.Entities);
					twin.MoveTo(DefaultWidth / 3, DefaultHeight / 4);

					var twin2 = new Vehicle(Zoom);
					twin2.ColorStripe = Colors.Yellow;
					twin2.Density = 1.11;
					twin2.AttachContainerTo(View.Entities);
					twin2.MoveTo(DefaultWidth * 2 / 3, DefaultHeight / 2);

					var ph = new Physics
					{
						Level = Level,
						Vehicles = new[]
							{
								xveh,
								twin2,
								twin
							}.AsEnumerable(),
						Birds = KnownBirds,
						Actors = KnownActors
					};



					GameContent.FocusVisualStyle = null;
					GameContent.Focusable = true;
					GameContent.Focus();

					GameContent.MouseLeftButtonDown +=
						(sender, args) =>
						{
							GameContent.Focus();
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

							InputControl = GameContent,
							Vehicle = xveh,
							View = View
						}
					);

					k1.Drop +=
						delegate
						{
							var rock = xveh.CurrentWeapon;

							if (rock == null)
								return;

							rock.MoveTo(xveh.X, xveh.Y);
							rock.VelocityX = xveh.VelocityX;
							rock.VelocityY = xveh.VelocityY;

							rock.Container.Show();
							rock.PhysicsDisabled = false;
							rock.Stability = 0;

							xveh.CurrentWeapon = null;


						};

					var FlashlightTracker = View.LocationTracker;

					FlashlightTracker.Target = xveh;

					k1.Enter +=
						delegate
						{
							if (xveh.IsUnmanned)
							{
								xveh.IsUnmanned = false;
								FlashlightTracker.Target = xveh;
							}
							else
							{
								xveh.IsUnmanned = true;
								var actor5 = new Actor.man0(Zoom)
								{
									Animation = Actor.AnimationEnum.Panic,
									RespectPlatforms = true,
									Level = Level
								};
								FlashlightTracker.Target = actor5;

								actor5.MoveTo(xveh.X, xveh.Y);

								actor5.AttachContainerTo(View.Entities);

								KnownActors.Add(actor5);
							}
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

							InputControl = GameContent,
							Vehicle = twin,
							View = View
						}
					);





					(1000 / 40).AtInterval(
						delegate
						{
							k1.Tick();
							k2.Tick();

							ph.Apply();

						}
					);



					#endregion




					View.Flashlight.Visible = false;

					var et = new EditorToolbar(this);

					et.MoveContainerTo((DefaultWidth - et.Width) / 2, DefaultHeight - et.Padding * 2 - PrimitiveTile.Heigth * 2);

					et.AttachContainerTo(this);

					et.EditorSelectorChanged +=
						() => View.EditorSelector = et.EditorSelector;

					View.EditorSelector = et.EditorSelector;

					et.LevelText.GotFocus +=
						delegate
						{
							et.LevelText.Text = Level.ToString();
						};

					GameContent.KeyUp +=
						(sender, args) =>
						{
							if (args.Key == Key.F)
								View.Flashlight.Visible = !View.Flashlight.Visible;

							if (args.Key == Key.G)
								View.IsFilmScratchEffectEnabled = !View.IsFilmScratchEffectEnabled;

							if (args.Key == Key.T)
							{
								et.Container.ToggleVisible();
								View.EditorSelectorRectangle.ToggleVisible();
							}
						};

					View.AttachContainerTo(GameContent);


					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Assets + "/statusbar.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = 320 * StatusbarZoom,
						Height = 9 * StatusbarZoom,
					}.AttachTo(GameContent).MoveTo((DefaultWidth - 320 * StatusbarZoom) / 2, DefaultHeight - 9 * StatusbarZoom);


				


					(Assets.Shared.KnownAssets.Path.Audio + "/newlevel.mp3").PlaySound();
				}
			);


		}
	}


}
