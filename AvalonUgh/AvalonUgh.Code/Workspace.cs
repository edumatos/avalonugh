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


		public readonly BindingList<Port> Ports = new BindingList<Port>();
		public readonly BindingList<PlayerInfo> Players = new BindingList<PlayerInfo>();

		public GameConsole Console { get; set; }

		const int DefaultZoom = 2;


		public Port EditorPort;
		public Port LobbyPort;

		public readonly ModernMenu Menu;

		public readonly EditorToolbar EditorToolbar;

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


			this.Menu = new ModernMenu(DefaultZoom, args.PortWidth, args.PortHeight);
			this.Menu.PlayersChanged +=
				delegate
				{
					Console.WriteLine("players: " + this.Menu.Players);
				};

			this.EditorToolbar = new EditorToolbar(this.Selectors);

			EditorToolbar.DragContainer = this.Container;
			EditorToolbar.Hide();
			EditorToolbar.AttachContainerTo(this.Container);

			var EditorToolbar_LoadLevel = new LoadWindow(this.Levels)
			{
				DragContainer = this.Container,
				Visibility = Visibility.Hidden
			};

			EditorToolbar_LoadLevel.AttachContainerTo(this);
			EditorToolbar_LoadLevel.MoveToCenter(this.Container);

			// move it to bottom center
			EditorToolbar.MoveContainerTo(
				(args.DefaultWidth - EditorToolbar.Width) / 2,
				args.DefaultHeight - EditorToolbar.Padding * 3 - PrimitiveTile.Heigth * 4
			);

			EditorToolbar.EditorSelectorChanged +=
				delegate
				{
					this.EditorPort.View.EditorSelector = EditorToolbar.EditorSelector;
				};

			EditorToolbar.LoadClicked +=
					delegate
					{
						EditorToolbar_LoadLevel.BringContainerToFront();
						EditorToolbar_LoadLevel.Show(EditorToolbar_LoadLevel.Visibility == Visibility.Hidden);

					};

			// serialize current level
			EditorToolbar.LevelText.GotFocus +=
				delegate
				{
					EditorToolbar.LevelText.Text = EditorPort.Level.ToString();
				};

			

			var LevelIntro = new LevelIntroDialog
			{
				AnimatedOpacity = 0,
				Width = args.DefaultWidth,
				Height = args.DefaultHeight,
				Zoom = DefaultZoom,
				AnimatedOpacityContentMultiplier = 1
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

	

			this.LobbyPort = new Port
			{
				Padding = args.WindowPadding,
				Selectors = this.Selectors,

				Zoom = DefaultZoom,

				Width = args.PortWidth,
				Height = args.PortHeight,

				PortIdentity = PortIdentity_Lobby,


			};

			Menu.AttachContainerTo(this.LobbyPort.Window.OverlayContainer);

			this.LobbyPort.WhenLoaded(
				delegate
				{
					Menu.BringContainerToFront();
				}
			);

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

			Menu.EnteringPasswordChanged +=
				delegate
				{
					Local0.Input.Keyboard.Disabled = Menu.EnteringPassword != null;
				};
			Action OpenEditor = null;

			Menu.Editor +=
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

			this.LocalIdentity.Locals.Add(Local0);


			LobbyPort.Loaded +=
				delegate
				{
					Console.WriteLine("adding an actor to lobby");

					Local0.Actor.MoveTo(
						(LobbyPort.View.ContentActualWidth / 4) +
						(LobbyPort.View.ContentActualWidth / 2).Random(),
						LobbyPort.View.ContentActualHeight / 2);

					Local0.Actor.CurrentLevel = LobbyPort.Level;

				};

			LobbyPort.LevelReference = new LevelReference(0);

			this.Players.Add(Local0);

			this.Ports.Add(LobbyPort);



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
								Menu.PlayText = "resume";

							Menu.Show();

							EditorToolbar.Hide();
							EditorToolbar_LoadLevel.Hide();

							// re-entering lobby


							Local0.Actor.CurrentLevel = LobbyPort.Level;
							Local0.Actor.MoveTo(
								(LobbyPort.View.ContentActualWidth / 4) +
								(LobbyPort.View.ContentActualWidth / 2).Random(),
								LobbyPort.View.ContentActualHeight / 2);

							LobbyPort.Window.BringContainerToFront();

						}


						Menu.HandleKeyUp(key_args);

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
					Console.WriteLine("focusing...");

					this.Container.Focus();
				};

			(1000 / 50).AtInterval(Think);

			this.Levels.AddRange(
				new KnownLevels().Levels
			);

			Menu.Play +=
				delegate
				{
					var Resumeable = this.Ports.FirstOrDefault(k => k.PortIdentity == PortIdentity_Mission);

					if (Resumeable != null)
					{
						Menu.Hide();

						this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Mission);

						Local0.Actor.CurrentLevel = Resumeable.Level;

						Console.WriteLine("resume...");

						return;
					}

					if (this.Levels.Any(k => k.Data == null))
					{
						Console.WriteLine("loading...");
						return;
					}

					var NextLevel = this.Levels.FirstOrDefault(k => k.Code.ToLower() == Menu.Password.ToLower());

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
							Height = args.PortHeight - 18,

							PortIdentity = PortIdentity_Mission,

							LevelReference = NextLevel,

						};

					NextLevelPort.Hide();

					this.Ports.Add(NextLevelPort);

					LevelIntro.BringContainerToFront();
					LevelIntro.AnimatedOpacity = 1;


					this.LocalIdentity.HandleFutureFrame(
						100,
						delegate
						{
							Local0.Actor.CurrentLevel = NextLevelPort.Level;

							Menu.Hide();

							NextLevelPort.Show();

							//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Mission);

							LevelIntro.AnimatedOpacity = 0;
						}
					);
				};

			EditorToolbar_LoadLevel.Click +=
				NextLevelForEditor =>
				{
					EditorPort.LevelReference = NextLevelForEditor;

					EditorToolbar_LoadLevel.Hide();
				};

			OpenEditor =
				delegate
				{

					this.LocalIdentity.HandleFutureFrame(
						delegate
						{
							LobbyPort.Level.KnownActors.Remove(Local0.Actor);

							if (this.EditorPort == null)
							{
								this.EditorPort = new Port
								{
									Selectors = this.Selectors,

									Zoom = DefaultZoom,

									Padding = args.WindowPadding,
									Width = args.PortWidth,
									StatusbarHeight = 18,
									Height = args.PortHeight ,

									PortIdentity = PortIdentity_Editor,
								};

								this.EditorPort.Loaded +=
									delegate
									{
										Local0.Actor.CurrentLevel = EditorPort.Level;
										
										this.EditorPort.View.LocationTracker.Target = Local0;

										if (this.EditorPort.Level.KnownCaves.Count == 0)
										{
											Local0.Actor.MoveTo(
												(EditorPort.View.ContentActualWidth / 4) +
												(EditorPort.View.ContentActualWidth / 2).Random(),
												EditorPort.View.ContentActualHeight / 2);

										}
										else
										{
											var EntryPointCave = this.EditorPort.Level.KnownCaves.Random();

											Local0.Actor.MoveTo(
												EntryPointCave.X,
												EntryPointCave.Y
											);
										}


										this.EditorPort.View.EditorSelectorNextSize += () => EditorToolbar.EditorSelectorNextSize();
										this.EditorPort.View.EditorSelectorPreviousSize += () => EditorToolbar.EditorSelectorPreviousSize();

									};

								this.Ports.Add(this.EditorPort);


								this.EditorPort.LevelReference = new LevelReference(0);
							}
							else
							{
								Local0.Actor.CurrentLevel = EditorPort.Level;
								Local0.Actor.MoveTo(
											(EditorPort.View.ContentActualWidth / 4) +
											(EditorPort.View.ContentActualWidth / 2).Random(),
											EditorPort.View.ContentActualHeight / 2);


							}

							//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Editor);

							this.EditorPort.Window.BringContainerToFront();

							// we are entering the editor
							// if anyone is there
							// then we need to sync
							Menu.Hide();
							EditorToolbar.Show();



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
