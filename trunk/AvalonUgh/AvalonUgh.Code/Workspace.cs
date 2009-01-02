using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Editor.Sprites;

namespace AvalonUgh.Code
{
	[Script]
	public partial class Workspace : ISupportsContainer
	{
		// workspace contains
		// multiple views to multiple concurrent
		// levels
		// workspace provides syncing, console and input

		public Canvas Container { get; set; }


		//public Canvas Overlay { get; set; }

		public readonly KnownSelectors Selectors = new KnownSelectors();
		public readonly BindingList<LevelReference> Levels = new BindingList<LevelReference>();



		/// <summary>
		/// This will reflect the clients name and number,
		/// We could be in control of none or multiple actors or vehicles
		/// within multiple views and levels
		/// </summary>
		public readonly PlayerIdentity LocalIdentity = new PlayerIdentity { Name = "LocalPlayer" };


		const int PortIdentity_Lobby = 1000;
		const int PortIdentity_Editor = 2000;
		const int PortIdentity_Mission = 3000;
		const int PortIdentity_CaveMission = 4000;


		public readonly BindingList<Port> Ports = new BindingList<Port>();
		public readonly BindingList<PlayerInfo> Players = new BindingList<PlayerInfo>();

		public GameConsole Console { get; set; }

		const int DefaultZoom = 2;

		public Port PrimaryMission;
		public Port CaveMission;
		public EditorPort Editor;
		public LobbyPort Lobby;

		//public readonly ModernMenu Menu;

		//public readonly EditorToolbar EditorToolbar;

		[Script]
		public class ConstructorArguments
		{
			public int WindowPadding;

			public int PortWidth;
			public int PortHeight;

			public int DefaultWidth;
			public int DefaultHeight;
		}

		public Workspace(ConstructorArguments args)
		{
			var KnownLevels = new KnownLevels();

			this.Container = new Canvas
			{
				Width = args.DefaultWidth,
				Height = args.DefaultHeight,
				Background = Brushes.DarkGray
			};

		
			this.Ports.ForEachNewOrExistingItem(
				(k, index) =>
				{

					k.Window.MoveContainerTo(k.Window.Padding * index, k.Window.Padding * index);

					k.Window.DragContainer = this.Container;

				
				}
			);
			this.Ports.AttachTo(k => k.Window, this.Container);

	

			this.Ports.ForEachNewOrExistingItem(
				NewPort =>
				{
					NewPort.WhenLoaded(
						delegate
						{
							NewPort.Level.KnownTryoperus.ForEachNewOrExistingItem(
								NewTryo =>
								{
									NewTryo.HandleFutureFrame = this.LocalIdentity.HandleFutureFrame;
								}
							);

							NewPort.Level.Physics.CollisionAtVelocity +=
								Velocity =>
								{
									var Volume = (Velocity / (NewPort.Level.Zoom * 3.0) + 0.3).Max(0).Min(1);


									
									if (Volume > 0)
									{
										var Sound = (Assets.Shared.KnownAssets.Path.Audio + "/bounce.mp3").ToSound();

										Sound.SetVolume(Volume);
										Sound.Start();
									}
								};

							NewPort.Level.Physics.WaterCollisionAtVelocity +=
								Velocity =>
								{
									var Volume = (Velocity / (NewPort.Level.Zoom * 7.0)).Max(0).Min(1);


									if (Volume > 0)
									{
										var Sound = (Assets.Shared.KnownAssets.Path.Audio + "/water_slpash.mp3").ToSound();

										Sound.SetVolume(Volume);
										Sound.Start();
									}


								};
						}
					);
				}
			);


			#region setting up our console
			this.Console = new GameConsole();

			this.Console.SizeTo(args.DefaultWidth, args.DefaultHeight / 2);
			this.Console.WriteLine("Avalon Ugh! Console ready.");
			this.Console.AnimatedTop = -this.Console.Height;

			this.Console.AttachContainerTo(this.Container);
			#endregion


			this.CaveMission = new Port()
			{
				Selectors = this.Selectors,

				Zoom = DefaultZoom,

				Padding = args.WindowPadding,
				Width = args.PortWidth,
				StatusbarHeight = 18,
				Height = args.PortHeight,

				PortIdentity = PortIdentity_CaveMission,
			};

			this.Ports.Add(this.CaveMission);

			this.Editor = new EditorPort(
									new EditorPort.ConstructorArguments
									{
										Selectors = this.Selectors,
										Levels = this.Levels
									})
			{

				Zoom = DefaultZoom,

				Padding = args.WindowPadding,
				Width = args.PortWidth,
				StatusbarHeight = 18,
				Height = args.PortHeight,

				PortIdentity = PortIdentity_Editor,
			};

		

			this.Ports.Add(this.Editor);

			this.Editor.Toolbar.DragContainer = this.Container;
			this.Editor.Toolbar.Hide();
			this.Editor.Toolbar.AttachContainerTo(this.Container);

			this.Editor.LoadWindow.DragContainer = this.Container;
			this.Editor.LoadWindow.Hide();
			this.Editor.LoadWindow.AttachContainerTo(this);
			this.Editor.LoadWindow.MoveToCenter(this.Container);

			// move it to bottom center
			this.Editor.Toolbar.MoveContainerTo(
				(args.DefaultWidth - this.Editor.Toolbar.Width) / 2,
				args.DefaultHeight - this.Editor.Toolbar.Padding * 3 - PrimitiveTile.Heigth * 4
			);

	
	
			

			var LevelIntro = new LevelIntroDialog
			{
				AnimatedOpacity = 0,
				Width = args.DefaultWidth,
				Height = args.DefaultHeight,
				Zoom = DefaultZoom,
				AnimatedOpacityContentMultiplier = 1,

				
			};

			LevelIntro.AttachContainerTo(this.Container);


			#region PauseDialog
			var PauseDialog = new Dialog
			{
				Width = args.DefaultWidth,
				Height = args.DefaultHeight,
				Zoom = DefaultZoom,
				BackgroundVisible = false,
				VerticalAlignment = VerticalAlignment.Center,
				Text = @"
						   game was paused
						     by
							you
						",
				AnimatedOpacity = 0
			}.AttachContainerTo(this.Container);



			Action<bool, string> SetPause =
				(IsPaused, ByWhom) =>
				{
					this.LocalIdentity.SyncFramePaused = IsPaused;

					if (IsPaused)
					{
						PauseDialog.BringContainerToFront();
						PauseDialog.Text = @"
								   game was paused
									 by
									" + ByWhom;

						ActiveDialog = PauseDialog;
					}
					else
					{
						ActiveDialog = null;
					}
				};


			#endregion

	

			this.Lobby = new LobbyPort(
				new LobbyPort.ConstructorArguments
				{
					Zoom = DefaultZoom,
					Width = args.PortWidth,
					Height = args.PortHeight
				})
			{
				Padding = args.WindowPadding,
				Selectors = this.Selectors,

				PortIdentity = PortIdentity_Lobby,
			};

			this.Lobby.Menu.PlayersChanged +=
				delegate
				{
					Console.WriteLine("players: " + this.Lobby.Menu.Players);
				};

			var Local0 =
				new PlayerInfo
				{
					Actor = new Actor.man0(DefaultZoom)
					{
						RespectPlatforms = true,
						CanBeHitByVehicle = false,
					},
					Input = new PlayerInput
					{
						Keyboard = new KeyboardInput(
							new KeyboardInput.Arguments.Arrows
							{
								InputControl = this.Container
							}
						)
					}
				};

			Lobby.Menu.EnteringPasswordChanged +=
				delegate
				{
					Local0.Input.Keyboard.Disabled = Lobby.Menu.EnteringPassword != null;
				};
			Action OpenEditor = null;

			Lobby.Menu.Editor +=
				delegate
				{
					OpenEditor();
				};

			// we need to play jumping sound
			Local0.Actor.Jumping +=
				delegate
				{
					(Assets.Shared.KnownAssets.Path.Audio + "/jump.mp3").PlaySound();
				};

			Local0.Actor.CurrentVehicleChanged +=
				delegate
				{
					(Assets.Shared.KnownAssets.Path.Audio + "/enter.mp3").PlaySound();
				};

			Local0.Actor.WaterCollision +=
				delegate
				{
					Console.WriteLine("yay, water!");
				};

			Local0.Actor.EnterVehicle +=
				delegate
				{
					// exiting a vehicle is easy
					// entering is a bit harder
					// as we need to find it and reserve its use for us

					var ManAsObstacle = Local0.Actor.ToObstacle();

					var NearbyVehicle = Local0.Actor.CurrentLevel.KnownVehicles.Where(k => k.CurrentDriver == null).FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

					if (NearbyVehicle != null)
					{
						NearbyVehicle.CurrentDriver = Local0.Actor;
					}
				};

			Local0.Actor.Drop +=
				delegate
				{
					var CurrentVehicle = Local0.Actor.CurrentVehicle;

					if (CurrentVehicle != null)
					{
						// can we drop a rock?

						CurrentVehicle.CurrentWeapon = null;
					}
				};

			Local0.Actor.EnterCave +=
				delegate
				{
				

					var ManAsObstacle = Local0.Actor.ToObstacle();

					// are we trying to enter a cave?
					var NearbyCave = Local0.Actor.CurrentLevel.KnownCaves.FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

					if (NearbyCave != null)
					{
						// we need to align us in front of the cave
						// and show entering animation

						Console.WriteLine("entering a cave");

						var CurrentPort = this.Ports.Single(k => k.Level == Local0.Actor.CurrentLevel);

					

						AIDirector.WalkActorToTheCaveAndEnter(Local0.Actor, NearbyCave,
							delegate
							{
								if (CurrentPort.PortIdentity == PortIdentity_CaveMission)
								{
									Console.WriteLine("we should exit a submission now");
									AIDirector.ActorExitCaveFast(Local0.Actor);

									Local0.Actor.CurrentLevel = PrimaryMission.Level;
									PrimaryMission.BringContainerToFront();

									return;
								}

								if (CurrentPort.PortIdentity == PortIdentity_Mission)
								{
									if (this.CaveMission.LevelReference == null)
									{
										this.CaveMission.LevelReference = KnownLevels.DefaultCaveLevel;
									}

									this.CaveMission.BringContainerToFront();

									this.CaveMission.WhenLoaded(
										delegate
										{
											this.CaveMission.View.Flashlight.Visible = true;
											this.CaveMission.View.Flashlight.Container.Opacity = 0.7;

											this.CaveMission.View.LocationTracker.Target = Local0;

											AIDirector.ActorExitCaveFast(Local0.Actor);
											Local0.Actor.CurrentLevel = this.CaveMission.Level;

											var EntryPointCave = this.CaveMission.Level.KnownCaves.Random();

											Local0.Actor.MoveTo(
												EntryPointCave.X,
												EntryPointCave.Y
											);
										}
									);

									Console.WriteLine("we should spawn a submission now");
									return;
								}

								Console.WriteLine("inside a cave");

								// should we load another level and enter that?
								// for the first version lets keep it simple
								// lets just exit another cave

								if (Local0.Actor.CurrentLevel.KnownCaves.Count == 0)
								{
									// whatif the cave is destroyed?
									AIDirector.ActorExitCaveFast(Local0.Actor);
									return;
								}

								var NextCave = Local0.Actor.CurrentLevel.KnownCaves.Next(k => k == NearbyCave);


								AIDirector.ActorExitAnyCave(Local0.Actor, NextCave);
							}
						);


						return;
					}
					else
					{
						Local0.Actor.Animation = Actor.AnimationEnum.Talk;
					}
				};

			// every actor could act differently on gold collected
			Local0.Actor.GoldStash.ForEachNewItem(
				gold =>
				{
					var CurrentPort = this.Ports.Single(k => k.Level == Local0.Actor.CurrentLevel);

					(Assets.Shared.KnownAssets.Path.Audio + "/treasure.mp3").PlaySound();

					CurrentPort.View.ColorOverlay.Background = Brushes.Yellow;
					CurrentPort.View.ColorOverlay.Opacity = 0.7;
					CurrentPort.View.ColorOverlay.Show();
					CurrentPort.View.ColorOverlay.FadeOut();
				}
			);

			this.LocalIdentity.Locals.Add(Local0);

			this.Editor.Loaded +=
				delegate
				{
					Local0.Actor.CurrentLevel = Editor.Level;

					this.Editor.View.LocationTracker.Target = Local0;

					if (this.Editor.Level.KnownCaves.Count == 0)
					{
						Local0.Actor.MoveTo(
							(Editor.View.ContentActualWidth / 4) +
							(Editor.View.ContentActualWidth / 2).Random(),
							Editor.View.ContentActualHeight / 2);

					}
					else
					{
						var EntryPointCave = this.Editor.Level.KnownCaves.Random();

						Local0.Actor.MoveTo(
							EntryPointCave.X,
							EntryPointCave.Y
						);
					}



				};


			Lobby.Loaded +=
				delegate
				{
					Console.WriteLine("adding an actor to lobby");

					Local0.Actor.MoveTo(
						(Lobby.View.ContentActualWidth / 4) +
						(Lobby.View.ContentActualWidth / 2).Random(),
						Lobby.View.ContentActualHeight / 2);

					Local0.Actor.CurrentLevel = Lobby.Level;

				};

			Lobby.LevelReference = new LevelReference(0);

			this.Players.Add(Local0);

			this.Ports.Add(Lobby);



			this.Container.KeyUp += HandleKeyUp;

			this.Container.KeyUp +=
				(sender, key_args) =>
				{



					if (this.LocalIdentity.SyncFramePaused)
					{
						// if the game is paused
						// we cannot handle a future frame and thus we need to unpause momentarily

						if (key_args.Key == Key.P)
						{
							key_args.Handled = true;
							SetPause(false, "you");

							return;
						}

					}
					else
					{
						if (key_args.Key == Key.Escape)
						{
							key_args.Handled = true;

							// if we are inside a mission, submission or editor this will bring us back

							//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Lobby);

							// if all players quit the game
							// we would be able to start another level
							if (this.Ports.Any(k => k.PortIdentity == PortIdentity_Mission))
								Lobby.Menu.PlayText = "resume";

							Lobby.Menu.Show();

							this.Editor.Toolbar.Hide();
							this.Editor.LoadWindow.Hide();

							// re-entering lobby

							Local0.Actor.CurrentLevel = Lobby.Level;
							Local0.Actor.MoveTo(
								(Lobby.View.ContentActualWidth / 4) +
								(Lobby.View.ContentActualWidth / 2).Random(),
								Lobby.View.ContentActualHeight / 2);

							Lobby.Window.BringContainerToFront();

						}


						Lobby.Menu.HandleKeyUp(key_args);

						if (key_args.Handled)
							return;



						if (key_args.Key == Key.P)
						{
							SetPause(true, "you");
						}
					}


				};

			// we are going for the keyboard input
			// we want to enable the tilde console feature
			this.Container.FocusVisualStyle = null;
			this.Container.Focusable = true;
			this.Container.Focus();

			// at this time we should add a local player
			this.Container.MouseLeftButtonDown +=
				(sender, key_args) =>
				{
					this.Container.Focus();
				};

			(1000 / 50).AtInterval(Think);


			this.Levels.AddRange(
				KnownLevels.Levels
			);

			Lobby.Menu.Play +=
				delegate
				{
					var Resumeable = this.Ports.FirstOrDefault(k => k.PortIdentity == PortIdentity_Mission);

					if (Resumeable != null)
					{
						Lobby.Menu.Hide();

						//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Mission);


						Local0.Actor.CurrentLevel = Resumeable.Level;

						Resumeable.BringContainerToFront();

						Console.WriteLine("resume...");

						return;
					}

					if (this.Levels.Any(k => k.Data == null))
					{
						Console.WriteLine("loading...");
						return;
					}

					var NextLevel = this.Levels.FirstOrDefault(k => k.Code.ToLower() == Lobby.Menu.Password.ToLower());

					if (NextLevel == null)
					{
						// password does not match
						NextLevel = this.Levels.FirstOrDefault(k => k.Code.ToLower() == "cavity");
					}

					if (NextLevel == null)
					{
						Console.WriteLine("no next level...");
						return;
					}


					//Menu.Hide();

					Console.WriteLine("loading level - " + NextLevel.Text);

					LevelIntro.LevelNumber = NextLevel.Location.Embedded.AnimationFrame;
					LevelIntro.LevelTitle = NextLevel.Text;
					LevelIntro.LevelPassword = NextLevel.Code;

			
					// fade in the level start menu
					// create and load new port
					// hide other ports
					// fade out

					// if we are in multyplayer
					// we need to do this in sync

					var NextLevelPort =
						new Port
						{
							Selectors = this.Selectors,

							Zoom = DefaultZoom,

							Padding = args.WindowPadding,
							Width = args.PortWidth,
							StatusbarHeight = 18,
							Height = args.PortHeight,

							PortIdentity = PortIdentity_Mission,

							LevelReference = NextLevel,

						};

					PrimaryMission = NextLevelPort;

					NextLevelPort.Hide();

					this.Ports.Add(NextLevelPort);

					LevelIntro.BringContainerToFront();
					LevelIntro.AnimatedOpacity = 1;


					this.LocalIdentity.HandleFutureFrame(
						100,
						delegate
						{
							Local0.Actor.CurrentLevel = NextLevelPort.Level;

							var StartPositionStone = NextLevelPort.Level.KnownStones.Random(k => k.Selector.PrimitiveTileCountX > 1 && k.Selector.PrimitiveTileCountY > 1);


							Local0.Actor.CurrentVehicle =
								new Vehicle(DefaultZoom).AddTo(NextLevelPort.Level.KnownVehicles);
							Local0.Actor.CurrentVehicle.MoveTo(StartPositionStone.X, StartPositionStone.Y);

							Lobby.Menu.Hide();

							NextLevelPort.Show();


							LevelIntro.AnimatedOpacity = 0;
						}
					);
				};

			this.Editor.LoadWindow.Click +=
				NextLevelForEditor =>
				{
					Editor.LevelReference = NextLevelForEditor;

					this.Editor.LoadWindow.Hide();
				};

			OpenEditor =
				delegate
				{

					this.LocalIdentity.HandleFutureFrame(
						delegate
						{
							Lobby.Level.KnownActors.Remove(Local0.Actor);

							if (this.Editor.LevelReference == null)
							{
								


								this.Editor.LevelReference = new LevelReference(0);
							}
							else
							{
								Local0.Actor.CurrentLevel = Editor.Level;
								Local0.Actor.MoveTo(
											(Editor.View.ContentActualWidth / 4) +
											(Editor.View.ContentActualWidth / 2).Random(),
											Editor.View.ContentActualHeight / 2);


							}

							//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Editor);

							this.Editor.BringContainerToFront();
							this.Editor.Toolbar.BringContainerToFront();

							// we are entering the editor
							// if anyone is there
							// then we need to sync
							Lobby.Menu.Hide();
							this.Editor.Toolbar.Show();



						}
					);
				};

		}


		Dialog InternalActiveDialog;
		public Dialog ActiveDialog
		{
			get
			{
				return InternalActiveDialog;
			}
			set
			{
				if (InternalActiveDialog != null)
					InternalActiveDialog.AnimatedOpacity = 0;

				InternalActiveDialog = value;

				if (InternalActiveDialog != null)
					InternalActiveDialog.AnimatedOpacity = 0.5;
			}
		}
	}
}
