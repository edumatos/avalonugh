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
			var Music = new AudioLoop
			{
				Volume = 0.3,
				Loop = (AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3"),
				Enabled = true,
			};


			var KnownLevels = new KnownLevels();

			this.Container = new Canvas
			{
				Width = args.DefaultWidth,
				Height = args.DefaultHeight,
				Background = Brushes.DarkGray
			};

			this.Ports.AttachTo(k => k.Window, this.Container);
			this.Ports.ForEachNewOrExistingItem(
				(NewPort, index) =>
				{
					Console.WriteLine("port added " + new { NewPort.Width, NewPort.Height, NewPort.StatusbarHeight });

					NewPort.Window.MoveContainerTo(NewPort.Window.Padding * index, NewPort.Window.Padding * index);

					NewPort.Window.DragContainer = this.Container;

					NewPort.Loaded +=
						delegate
						{
							Console.WriteLine("port loaded " + new { NewPort.Width, NewPort.Height, NewPort.StatusbarHeight });
					
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
						};
					
				}
			);


			#region setting up our console
			this.Console = new GameConsole();

			this.Console.SizeTo(args.DefaultWidth, args.DefaultHeight / 2);
			this.Console.WriteLine("Avalon Ugh! Console ready.");
			this.Console.AnimatedTop = -this.Console.Height;

			this.Console.AttachContainerTo(this.Container);
			#endregion


			this.SupportedKeyboardInputs = new[]
			{
				new KeyboardInput(
					new KeyboardInput.Arguments.Arrows
					{
						InputControl = this.Container
					}
				),
				new KeyboardInput(
					new KeyboardInput.Arguments.WASD
					{
						InputControl = this.Container
					}
				),
				new KeyboardInput(
					new KeyboardInput.Arguments.IJKL
					{
						InputControl = this.Container
					}
				),
			};

			this.PrimaryMission =
				new MissionPort(
					new MissionPort.ConstructorArguments
					{
						Padding = args.WindowPadding,
						Width = args.PortWidth,
						Height = args.PortHeight,
						Zoom = DefaultZoom,
					})
				{
					Selectors = this.Selectors,

					



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



			this.Sync_SetPause =
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

					this.SupportedKeyboardInputs.ForEach(k => k.Disabled = IsPaused);

				};


			#endregion

			this.Lobby = new LobbyPort(
				new LobbyPort.ConstructorArguments
				{
					Padding = args.WindowPadding,
					Zoom = DefaultZoom,
					Width = args.PortWidth,
					Height = args.PortHeight
				})
			{
				
				Selectors = this.Selectors,

				PortIdentity = PortIdentity_Lobby,
			};

			this.Ports.Add(Lobby);

			this.Lobby.Menu.MaxPlayers = SupportedKeyboardInputs.Length;

			// each actor in players moves on each sync frame
			// players contain all locals and external players
			this.LocalIdentity.Locals.AttachTo(this.Players);

			this.Players.ForEachNewOrExistingItem(
				NewPlayer =>
				{
					// here we create an actor for remote and local players

					NewPlayer.Actor = new Actor.man0(DefaultZoom)
					{
						RespectPlatforms = true,
						CanBeHitByVehicle = false,
					};

					NewPlayer.Actor.MoveTo(64, 64);
				}
			);

			this.Sync_LocalsIncrease =
				delegate
				{
					var p = new PlayerInfo
					{
						Identity = LocalIdentity,
						Input = new PlayerInput
						{
							Keyboard = this.SupportedKeyboardInputs[this.LocalIdentity.Locals.Count],
						}
					};

					this.LocalIdentity.Locals.Add(p);

					if (this.CurrentPort != null)
						if (this.CurrentPort.Level != null)
						{
							// where shall we spawn?
							//p.Actor.CurrentLevel = this.CurrentPort.Level;
							//p.Actor.MoveTo(
							//    (this.CurrentPort.View.ContentActualWidth / 4) +
							//    (this.CurrentPort.View.ContentActualWidth / 2).Random(),
							//    this.CurrentPort.View.ContentActualHeight / 2);

							this.CurrentPort.Players.Add(p);
						}

					
				};

			this.Sync_LocalsDecrease =
				delegate
				{
					var p = this.LocalIdentity.Locals.Last();

					p.Input.Keyboard = null;
					p.Actor.CurrentLevel = null;

					this.LocalIdentity.Locals.Remove(p);
				};

			this.Lobby.Menu.PlayersChanged +=
				delegate
				{
					var Players_add = (this.Lobby.Menu.Players - this.LocalIdentity.Locals.Count).Max(0);
					var Players_remove = (this.LocalIdentity.Locals.Count - this.Lobby.Menu.Players).Max(0);

					Console.WriteLine(new { Players_add, Players_remove, this.Lobby.Menu.Players }.ToString());

					Players_remove.Times(this.Sync_LocalsDecrease);
					Players_add.Times(this.Sync_LocalsIncrease);
				};

			Lobby.Menu.EnteringPasswordChanged +=
				delegate
				{
					this.SupportedKeyboardInputs.ForEach(k => k.Disabled = Lobby.Menu.EnteringPassword != null);
				};

			//this.Lobby.Menu.Players = 1;

			#region local0
			var Local0 =
				new PlayerInfo
				{
					Actor = new Actor.man0(DefaultZoom)
					{
						RespectPlatforms = true,
						CanBeHitByVehicle = false,
					},
	
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

													//this.CaveMission.View.Flashlight.Visible = true;
													//this.CaveMission.View.Flashlight.Container.Opacity = 0.7;

													//this.CaveMission.View.LocationTracker.Target = Local0;


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
						if (Local0.Actor.Animation != Actor.AnimationEnum.Talk)
						{
							Local0.Actor.Animation = Actor.AnimationEnum.Talk;

							(Assets.Shared.KnownAssets.Path.Audio + "/talk0_00.mp3").PlaySound();
						}

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

			#endregion

			//this.LocalIdentity.Locals.Add(Local0);



			//Lobby.Window.ColorOverlay.Opacity = 1;
			Lobby.WhenLoaded(
				delegate
				{
					Console.WriteLine("lobby loaded");

					// we should load lobby only once

					this.Lobby.Players.AddRange(this.LocalIdentity.Locals.ToArray());

					//foreach (var k in this.LocalIdentity.Locals)
					//{
					//    k.Actor.MoveTo(
					//        (Lobby.View.ContentActualWidth / 4) +
					//        (Lobby.View.ContentActualWidth / 2).Random(),
					//        Lobby.View.ContentActualHeight / 2);

					//    k.Actor.CurrentLevel = Lobby.Level;
					//}

					//Lobby.Window.ColorOverlay.Opacity = 0;
				}
			);

			

			this.CurrentPort = this.Lobby;

			//this.Players.Add(Local0);

			



			







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
								NextLevel2 = KnownLevels.DefaultMissionLevel;
							}

				
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
													PrimaryMission.Players.AddRange(this.LocalIdentity.Locals.ToArray());
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



			Lobby.Menu.Editor +=
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

							if (this.Editor.LevelReference == null)
								this.Editor.LevelReference = KnownLevels.DefaultLobbyLevel;

							this.Editor.WhenLoaded(
								delegate
								{
									// how shall locals enter the editor?

									// just jump randomly in

									this.Editor.Players.AddRange(this.LocalIdentity.Locals.ToArray());
									this.Editor.Window.ColorOverlay.Opacity = 0;
								}
							);
						}
					);


				};


			
			(1000 / DefaultFramerate).AtInterval(Think);

			this.EnableKeyboardFocus();
			this.Container.KeyUp += HandleKeyUp;

			Lobby.LevelReference = KnownLevels.DefaultLobbyLevel;
		}

		public const int DefaultFramerate = 55;

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
