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


			this.PrimaryMission =
				new MissionPort(
					new MissionPort.ConstructorArguments
					{
						Width = args.PortWidth,
						Height = args.PortHeight,
						Zoom = DefaultZoom,
					})
				{
					Selectors = this.Selectors,

					Padding = args.WindowPadding,



					PortIdentity = PortIdentity_Mission,


				};

			this.Ports.Add(PrimaryMission);


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

						AIDirector.WalkActorToTheCaveAndEnter(Local0.Actor, NearbyCave,
							delegate
							{
								if (CurrentPort.PortIdentity == PortIdentity_CaveMission)
								{
									this.PrimaryMission.Window.ColorOverlay.Opacity = 1;
									this.CurrentPort.Window.ColorOverlay.SetOpacity(1,
										delegate
										{

											this.PrimaryMission.BringContainerToFront();
											this.CurrentPort = this.PrimaryMission;

											var EntryPointCave = this.PrimaryMission.Level.KnownCaves.AtModulus(
												Local0.Actor.CurrentLevel.KnownCaves.IndexOf(NearbyCave)
											);

											AIDirector.ActorExitCaveFast(Local0.Actor);
											Local0.Actor.CurrentLevel = this.PrimaryMission.Level;
											AIDirector.ActorExitAnyCave(Local0.Actor, EntryPointCave);


											this.PrimaryMission.Window.ColorOverlay.Opacity = 0;

										}
									);

									return;
								}

								if (CurrentPort.PortIdentity == PortIdentity_Mission)
								{
									this.CaveMission.Window.ColorOverlay.Opacity = 1;
									this.CurrentPort.Window.ColorOverlay.SetOpacity(1,
										delegate
										{
											this.CaveMission.BringContainerToFront();
											this.CurrentPort = this.CaveMission;

											if (this.CaveMission.LevelReference == null)
											{
												this.CaveMission.LevelReference = KnownLevels.DefaultCaveLevel;
											}

											this.CaveMission.WhenLoaded(
												delegate
												{

													this.CaveMission.View.Flashlight.Visible = true;
													this.CaveMission.View.Flashlight.Container.Opacity = 0.7;

													this.CaveMission.View.LocationTracker.Target = Local0;


													var EntryPointCave = this.CaveMission.Level.KnownCaves.AtModulus(
														Local0.Actor.CurrentLevel.KnownCaves.IndexOf(NearbyCave)
													);

													AIDirector.ActorExitCaveFast(Local0.Actor);
													Local0.Actor.CurrentLevel = this.CaveMission.Level;
													AIDirector.ActorExitAnyCave(Local0.Actor, EntryPointCave);

													this.CaveMission.Window.ColorOverlay.Opacity = 0;

												}
											);

											Console.WriteLine("we should spawn a submission now");
										}
									);
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

							this.CurrentPort.Window.ColorOverlay.SetOpacity(1,
								delegate
								{
									Lobby.Window.ColorOverlay.Opacity = 0;

									this.Editor.Toolbar.Hide();
									this.Editor.LoadWindow.Hide();

									// re-entering lobby

									Local0.Actor.CurrentLevel = Lobby.Level;
									Local0.Actor.MoveTo(
										(Lobby.View.ContentActualWidth / 4) +
										(Lobby.View.ContentActualWidth / 2).Random(),
										Lobby.View.ContentActualHeight / 2);

									this.CurrentPort = this.Lobby;
									Lobby.Window.BringContainerToFront();
								}
							);

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





			this.Levels.AddRange(
				KnownLevels.Levels
			);

			Lobby.Menu.Play +=
				delegate
				{
					// fade this to black
					// switch the ports
					// fade in the intro
					// load
					// fade to black
					// fade to view
					this.Lobby.Window.ColorOverlay.Element.BringToFront();
					this.PrimaryMission.Window.ColorOverlay.Opacity = 1;
					this.Lobby.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							var NextLevel2 = this.Levels.FirstOrDefault(k => k.Code.ToLower() == Lobby.Menu.Password.ToLower());

							if (NextLevel2 == null)
							{
								// password does not match
								NextLevel2 = this.Levels.FirstOrDefault(k => k.Code.ToLower() == "cavity");
							}

							if (NextLevel2 == null)
							{
								Console.WriteLine("no next level...");
								return;
							}


							//Menu.Hide();

							Console.WriteLine("loading level - " + NextLevel2.Text);

							// fade in the level start menu
							// create and load new port
							// hide other ports
							// fade out

							// if we are in multyplayer
							// we need to do this in sync


							//PrimaryMission.LevelReference = NextLevel;
							PrimaryMission.Intro.LevelNumber = NextLevel2.Location.Embedded.AnimationFrame;
							PrimaryMission.Intro.LevelTitle = NextLevel2.Text;
							PrimaryMission.Intro.LevelPassword = NextLevel2.Code;

							PrimaryMission.Intro.Show();
							PrimaryMission.Fail.Hide();

							this.CurrentPort = this.PrimaryMission;

							PrimaryMission.BringContainerToFront();
							PrimaryMission.Window.ColorOverlay.SetOpacity(0,
								delegate
								{
									if (PrimaryMission.LevelReference == null)
										PrimaryMission.LevelReference = NextLevel2;

									PrimaryMission.WhenLoaded(
										delegate
										{
											PrimaryMission.Window.ColorOverlay.SetOpacity(1,
												delegate
												{
													Local0.Actor.CurrentLevel = PrimaryMission.Level;

													var StartPositionStone = PrimaryMission.Level.KnownStones.Random(k => k.Selector.PrimitiveTileCountX > 1 && k.Selector.PrimitiveTileCountY > 1);

													Local0.Actor.CurrentVehicle = new Vehicle(DefaultZoom).AddTo(PrimaryMission.Level.KnownVehicles);
													Local0.Actor.CurrentVehicle.MoveTo(StartPositionStone.X, StartPositionStone.Y);

													PrimaryMission.Intro.Hide();
													PrimaryMission.Window.ColorOverlay.Opacity = 0;
												}
											);
										}
									);
								}
							);
						}
					);



				};



			OpenEditor =
				delegate
				{
					this.Editor.Window.ColorOverlay.Opacity = 1;
					this.Lobby.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							this.CurrentPort = this.Editor;

							this.Editor.BringContainerToFront();
							this.Editor.Toolbar.BringContainerToFront();
							this.Editor.Toolbar.Show();

							this.Editor.LevelReference = new LevelReference(0);
							this.Editor.WhenLoaded(
								delegate
								{
									this.Editor.Window.ColorOverlay.Opacity = 0;

									Local0.Actor.CurrentLevel = this.Editor.Level;
								}
							);
						}
					);


				};


			this.EnableKeyboardFocus();

			(1000 / 50).AtInterval(Think);
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
