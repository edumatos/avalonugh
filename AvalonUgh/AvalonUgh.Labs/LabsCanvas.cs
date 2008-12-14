using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Dialogs;

namespace AvalonUgh.Labs.Shared
{

	[Script]
	public class LabsCanvas : Canvas
	{
		// the game should include all original levels

		public const int Zoom = 2;

		//public const int Padding = 64;
		public const int Padding = 0;

		public const int DefaultWidth = 640 + Padding * 2;
		public const int DefaultHeight = 400 + Padding * 2;

		public const int StatusbarZoom = 2;


		public readonly Canvas GameContent;

		public LabsCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;
			Background = Brushes.Black;

			this.ClipToBounds = true;

			//var MusicVolumes = new[] { 0.1, 0.0, 0.1, 0.2 };

			// Func<double> NextMusicVolume = MusicVolumes.AsCyclicEnumerable().GetEnumerator().Take;
			Func<double> NextMusicVolume = new[] { 0.14, 0.07 }.AsCyclicEnumerator().Take;

			Action<double> SetMusicVolume = null;
			Action ApplyNextMusicVolume = () => SetMusicVolume(NextMusicVolume());

			(AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3").Apply(
				(Source, Retry) =>
				{
					var Music = Source.PlaySound();

					Music.PlaybackComplete += Retry;

					SetMusicVolume = Music.SetVolume;
					ApplyNextMusicVolume();

				}
			);

			//new[]
			//{
			//    Colors.Black,
			//    Colors.Yellow,
			//    Colors.Red,
			//    Colors.Black
			//}.ToGradient(DefaultHeight / 4).Select(
			//    (c, i) =>
			//        new Rectangle
			//        {
			//            Fill = new SolidColorBrush(c),
			//            Width = DefaultWidth,
			//            Height = 4,
			//        }.MoveTo(0, i * 4).AttachTo(this)
			//).ToArray();

			this.GameContent = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);

			var et = new EditorToolbar(this);

			et.MoveContainerTo((DefaultWidth - et.Width) / 2, DefaultHeight - et.Padding * 2 - PrimitiveTile.Heigth * 2);
			et.AttachContainerTo(this);

			this.MenuDialog = new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Left,
				Text = @"-
 F1: start game
 F2: enter password
 F3: medium
 F4: single player
 F5: control options
-",
				LabelText = "password:",
				InputText = "none?"
			}.AttachContainerTo(this);

			var MenuDummyDisabled = false;
			this.MenuDialog.Container.MouseLeftButtonUp +=
				delegate
				{
					if (MenuDummyDisabled)
						return;
					MenuDummyDisabled = true;
					this.MenuDialog.Container.FadeOut(
						delegate
						{
							this.MenuDialog.OrphanizeContainer();
							NewMethod(ApplyNextMusicVolume, et);
						}
					);

				};



		}

		Dialog MenuDialog;

		public bool IsMultiplayer
		{
			set
			{
				if (value)
					MenuDialog.Text = @"-
 F1: start game
 F2: enter password
 F3: medium
 F4: multiplayer
 F5: control options
-";
				else
					MenuDialog.Text = @"-
 F1: start game
 F2: enter password
 F3: medium
 F4: single player
 F5: control options
-";
			}
		}
		private void NewMethod(Action ApplyNextMusicVolume, EditorToolbar et)
		{

			//var CurrentLevel = KnownAssets.Path.Assets + "/level09.txt";
			var CurrentLevel = Assets.Shared.KnownAssets.Path.Levels + "/level0_02.txt";

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

					//View.Flashlight.Visible = true;

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

					//#region x
					//#region CreateCustom
					//Func<int, int, string, string, Image> CreateCustom =
					//    (x, y, tile, path) =>
					//    {
					//        var w = 16 * Zoom;
					//        var h = 12 * Zoom;

					//        return new Image
					//        {
					//            Source = (path + "/" + tile + ".png").ToSource(),
					//            Stretch = Stretch.Fill,
					//            Width = w,
					//            Height = h,


					//        }.MoveTo(x * w, y * h).AttachTo(View.Platforms);
					//    };
					//#endregion

					//#region CreateCustom
					//Func<int, int, string, string, Image> CreateCustom_2x2 =
					//    (x, y, tile, path) =>
					//    {
					//        var w = 16 * Zoom;
					//        var h = 12 * Zoom;

					//        return new Image
					//        {
					//            Source = (path + "/" + tile + ".png").ToSource(),
					//            Stretch = Stretch.Fill,
					//            Width = w * 2,
					//            Height = h * 2,


					//        }.MoveTo(x * w, y * h).AttachTo(View.Platforms);
					//    };
					//#endregion



					//var CreateSprite = CreateCustom.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);
					//var CreateSprite_2x2 = CreateCustom_2x2.FixLastParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Sprites);


					//var Create = new
					//{
					//    flower0 = CreateSprite.FixLastParam("flower0"),
					//    fish0 = CreateSprite.FixLastParam("fish0"),
					//    fish1 = CreateSprite.FixLastParam("fish1"),


					//    weed0_2x2 = CreateSprite_2x2.FixLastParam("weed0_00_2x2"),

					//};
					//#endregion










					//Create.weed0_2x2(0, 14);
					//Create.weed0_2x2(2, 14);

					//Create.flower0(0, 6);

					//var fish_x = -PrimitiveTile.Width;

					//new[]
					//{
					//    Create.fish0(2, 12),
					//    Create.fish1(2, 12)
					//}.AsCyclicEnumerable().ForEach(
					//    (fish, next) =>
					//    {
					//        fish_x = (fish_x + 1 * Zoom) % View.ContentActualWidth;


					//        fish.MoveTo(fish_x, Level.WaterTop + Level.WaterHeight / 3);

					//        fish.Show();

					//        (1000 / 15).AtDelay(
					//            delegate
					//            {
					//                fish.Hide();
					//                next();
					//            }
					//        );
					//    }
					//);

					#region sprites

					//var KnownActors = new List<Actor>();

					//var ActorPool = new Func<Actor>[]
					//{
					//    () => new Actor.man0(Zoom) {Level = Level },
					//    () => new Actor.man1(Zoom)  {Level = Level },
					//    () => new Actor.woman0(Zoom)  {Level = Level },
					//}.AsCyclicEnumerable().GetEnumerator();

					//Level.KnownCaves.ForEachNewOrExistingItem(
					//    cave =>
					//    {
					//        var actor = ActorPool.Take()();

					//        actor.AttachContainerTo(View.Entities);


					//        actor.MoveTo(
					//            (cave.Position.TileX + 1) * PrimitiveTile.Width * Zoom,
					//            (cave.Position.TileY + 1) * PrimitiveTile.Heigth * Zoom
					//        );

					//        KnownActors.Add(actor);
					//    }
					//);



					//var KnownBirds = new List<Bird>();





					#endregion

					//#region bird
					//{
					//    var bird1 = new Bird(Zoom);
					//    var bird1_x = DefaultWidth + bird1.Width / 2;
					//    var bird1_y = Zoom * 32;

					//    bird1.AttachContainerTo(View.Entities).MoveTo(bird1_x, bird1_y);
					//    KnownBirds.Add(bird1);

					//    (1000 / 30).AtInterval(
					//        delegate
					//        {
					//            bird1_x -= 4;

					//            if (bird1_x < -(bird1.Width / 2))
					//                bird1_x = View.ContentActualWidth + bird1.Width / 2;

					//            bird1.MoveTo(bird1_x, bird1_y);
					//        }
					//    );


					//    var bird2 = new Bird(Zoom);
					//    var bird2_x = DefaultWidth / 2;
					//    var bird2_y = Zoom * 70;

					//    bird2.AttachContainerTo(View.Entities).MoveTo(bird2_x, bird2_y);
					//    KnownBirds.Add(bird2);


					//    (1000 / 30).AtInterval(
					//        delegate
					//        {
					//            bird2_x -= 2;
					//            if (bird2_x < -(bird2.Width / 2))
					//                bird2_x = View.ContentActualWidth + bird2.Width / 2;

					//            bird2.MoveTo(bird2_x, bird2_y);
					//        }
					//    );

					//}
					//#endregion





					#region vehicle


					var xveh = new Vehicle(Zoom)
					{
						ColorStripe = Colors.Red,
						CurrentLevel = Level
					};

					Level.KnownVehicles.Add(xveh);

					xveh.AttachContainerTo(View.Entities);
					xveh.MoveTo(Level.ActualWidth / 2, xveh.HalfHeight);

					//var twin = new Vehicle(Zoom);
					//twin.ColorStripe = Colors.Blue;

					//twin.AttachContainerTo(View.Entities);
					//twin.MoveTo(Level.ActualWidth * 2 / 3, 0);

					//var twin2 = new Vehicle(Zoom);
					//twin2.ColorStripe = Colors.Yellow;
					//twin2.Density = 1.11;
					//twin2.AttachContainerTo(View.Entities);
					//twin2.MoveTo(Level.ActualWidth / 3, 0);



					Level.Physics.CollisionAtVelocity +=
						Velocity =>
						{
							var Volume = (Velocity / (Level.Zoom * 3.0)).Max(0).Min(1);

							Console.WriteLine(new { Volume, Velocity });

							if (Volume > 0)
								(Assets.Shared.KnownAssets.Path.Audio + "/bounce.mp3").PlaySound().SetVolume(Volume);

							//Console.WriteLine(Velocity);
						};


					GameContent.FocusVisualStyle = null;
					GameContent.Focusable = true;
					GameContent.Focus();

					GameContent.MouseLeftButtonDown +=
						(sender, args) =>
						{
							GameContent.Focus();
						};


					View.LocationTracker.Target = xveh;

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
							//Vehicle = xveh,
							//View = View
						}
					);

					var k3 = new PlayerInput
					{
						Keyboard = k1,
						Touch = View.TouchInput
					};

					ISupportsPlayerInput k3target = xveh;

					Actor xveh_man = new Actor.man0(Zoom)
					{
						Animation = Actor.AnimationEnum.Panic,
						RespectPlatforms = true,
						Level = Level,
						CanBeHitByVehicle = false
					};

					xveh_man.GoldStash.ForEachNewItem(
						gold =>
						{
							(Assets.Shared.KnownAssets.Path.Audio + "/treasure.mp3").PlaySound();

							View.ColorOverlay.Background = Brushes.Yellow;
							View.ColorOverlay.Opacity = 0.7;
							View.ColorOverlay.Show();
							View.ColorOverlay.FadeOut();
						}
					);

					xveh_man.CurrentVehicle = xveh;



					k3.Enter +=
						delegate
						{
							if (xveh.IsUnmanned)
							{
								if (xveh_man != null)
								{
									// AI is controlling our man
									// possibly entering the cave already
									if (xveh_man.AIInputEnabled)
										return;

									if (xveh_man.CurrentCave != null)
									{
										AIDirector.ActorExitAnyCave(xveh_man, Level.KnownCaves.Where(k => k != xveh_man.CurrentCave).Random());

										return;
									}

									var ManAsObstacle = xveh_man.ToObstacle();

									// are we trying to enter a cave?
									var NearbyCave = Level.KnownCaves.FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

									if (NearbyCave != null)
									{
										// we need to align us in front of the cave
										// and show entering animation

										AIDirector.WalkActorToTheCaveAndEnter(xveh_man, NearbyCave, null);

										Console.WriteLine("entering a cave");

										return;
									}


									if (!ManAsObstacle.Intersects(xveh.ToObstacle()))
									{
										Console.WriteLine("try geting closer to a vehicle!");
										return;
									}

									Level.KnownActors.Remove(xveh_man);

									xveh_man.CurrentVehicle = xveh;
									xveh_man.OrphanizeContainer();
								}

								xveh.IsUnmanned = false;
								View.LocationTracker.Target = xveh;
								k3target = xveh;
							}
							else
							{
								xveh.IsUnmanned = true;
								xveh_man.CurrentVehicle = null;


								View.LocationTracker.Target = xveh_man;
								k3target = xveh_man;

								xveh_man.MoveTo(xveh.X, xveh.Y - xveh_man.ToObstacle().Height / 2);

								xveh_man.AttachContainerTo(View.Entities);
								Level.KnownActors.Add(xveh_man);
							}
						};

					k3.Drop +=
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

					//var k2 = new KeyboardInput(
					//    new KeyboardInput.Arguments
					//    {
					//        Left = Key.A,
					//        Right = Key.D,
					//        Up = Key.W,
					//        Down = Key.S,
					//        Drop = Key.Q,
					//        Enter = Key.E,

					//        InputControl = GameContent,
					//        Vehicle = twin,
					//        View = View
					//    }
					//);




					(1000 / 40).AtInterval(
						delegate
						{
							k3target.AddAcceleration(k3);


							Level.Physics.Apply();

						}
					);



					#endregion




					View.Flashlight.Visible = false;



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
							// oem7 will trigger the console
							if (args.Key == Key.Oem7)
								View.Container.ToggleVisible();

							if (args.Key == Key.F)
								View.Flashlight.Visible = !View.Flashlight.Visible;

							if (args.Key == Key.G)
								View.IsFilmScratchEffectEnabled = !View.IsFilmScratchEffectEnabled;

							if (args.Key == Key.T)
							{
								et.Container.ToggleVisible();
								View.EditorSelectorRectangle.ToggleVisible();
							}

							if (args.Key == Key.M)
								ApplyNextMusicVolume();
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
