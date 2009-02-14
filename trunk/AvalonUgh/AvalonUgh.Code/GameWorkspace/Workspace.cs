using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.GameWorkspace
{
	[Script]
	public partial class Workspace : ISupportsContainer, IDisposable
	{

		// workspace contains
		// multiple views to multiple concurrent
		// levels
		// workspace provides syncing, console and input

		public Canvas Container { get; set; }


		//public Canvas Overlay { get; set; }

		public readonly KnownSelectors Selectors = new KnownSelectors();
		public readonly KnownLevels KnownLevels = new KnownLevels();

		public readonly BindingList<LevelReference> EmbeddedLevels = new BindingList<LevelReference>();
		public readonly BindingList<LevelReference> SavedLevels = new BindingList<LevelReference>();
		public readonly BindingList<LevelReference> DiscoveredLevels = new BindingList<LevelReference>();

		public IEnumerable<LevelReference> LoadableLevels
		{
			get
			{
				return EmbeddedLevels.Concat(SavedLevels).Concat(DiscoveredLevels).Where(k => k.Data != null);
			}
		}



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

		public readonly ConstructorArguments Arguments;

		readonly AudioLoop Music;


		public Workspace(ConstructorArguments args)
		{
			this.Arguments = args;

			this.LocalIdentity = new PlayerIdentity
			{
				Name = "LocalPlayer",
				SyncFramePaused = true,
				SyncFrameRate = DefaultFramerate
			};


			this.Music = new AudioLoop
			{
				Volume = 0.6,
				Loop = (AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3"),
				
			};

			this.EmbeddedLevels.AddRange(
				KnownLevels.Levels
			);

			//this.SavedLevels.AddRange(
			//    new LevelReference(),
			//    new LevelReference(),
			//    new LevelReference(),
			//    new LevelReference(),
			//    new LevelReference()
			//);

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

							// a port was loaded,
							// if we are displaying loading, we shall hide it now
							this.BackgroundLoading.Hide();

							NewPort.Level.KnownTryoperus.ForEachNewOrExistingItem(
								NewTryo =>
								{
									NewTryo.HandleFutureFrame = this.LocalIdentity.HandleFutureFrame;
								}
							);

							NewPort.Level.Physics.CollisionAtVelocity +=
								Velocity =>
								{
									if (Velocity < 0.1)
										return;

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

							// we will be rerouting this action over network
							NewPort.View.EditorSelectorDisabled = true;

							NewPort.View.EditorSelectorApplied +=
								(Selector, Position, EditorSelectorApplied_args) =>
								{
									if (Selector.Width > 0)
									{
										(Assets.Shared.KnownAssets.Path.Audio + "/place_tile.mp3").PlaySound();
									}

									this.Console.WriteLine("NewPort.View.EditorSelectorApplied");

									var Index = KnownSelectors.Index.Of(Selector, this.Selectors);

									this.Sync_EditorSelector(NewPort.PortIdentity, Index.Type, Index.Size, Position.ContentX, Position.ContentY);

							
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


			// the ingame status bar can support 1 or 2 players. The third player cannot be show ant this time.
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
					Height = args.PortHeight,
					Zoom = DefaultZoom,

					Selectors = this.Selectors,
					EmbeddedLevels = this.EmbeddedLevels,
					SavedLevels = this.SavedLevels,
					DiscoveredLevels = this.DiscoveredLevels,
				})
			{

				Zoom = DefaultZoom,

				Padding = args.WindowPadding,
				Width = args.PortWidth,

				Height = args.PortHeight,

				PortIdentity = PortIdentity_Editor,
			};



			this.Ports.Add(this.Editor);



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

			// add some "branding"



			this.Ports.Add(Lobby);

			this.Lobby.Menu.MaxPlayers = SupportedKeyboardInputs.Length;

			// each actor in players moves on each sync frame
			// players contain all locals and external players
			this.LocalIdentity.Locals.AttachTo(this.Players);


			// for every port whenever a level is loaded
			// for everypassanger whenever a passanger vehicle changes
			// play a sound
			this.Ports.ForEachNewOrExistingItem(
				p =>
				{
					p.Loaded +=
						delegate
						{
							p.Level.KnownPassengers.ForEachNewOrExistingItem(
								k =>
								{
									k.CurrentPassengerVehicleChanged +=
										delegate
										{
											SoundBoard.Default.enter();
										};
								}
							);
						};
				}
			);

			this.Players.ForEachNewOrExistingItem(
				NewPlayer =>
				{
					// here we create an actor for remote and local players

					NewPlayer.Actor = new Actor.man0(DefaultZoom)
					{
						RespectPlatforms = true,
						Memory_CanBeHitByVehicle = false,
					};

					NewPlayer.Actor.Jumping +=
						delegate
						{
							SoundBoard.Default.jump();

						};

					// every actor could act differently on gold collected
					NewPlayer.Actor.GoldStash.ForEachNewItem(
						gold =>
						{
							// play the sound only if it is in the same port
							SoundBoard.Default.treasure();

							// the yellow flash shall be displayed for local players only
						}
					);



					NewPlayer.Actor.CurrentVehicleChanged +=
						delegate
						{
							SoundBoard.Default.enter();
						};


					NewPlayer.Actor.EnterCave +=
						delegate
						{

							// are we trying to enter a cave?
							var NearbyCave = NewPlayer.Actor.NearbyCave;

							if (NearbyCave != null)
							{
								//AIDirector.WalkActorToTheCaveAndEnter(NewPlayer.Actor, NearbyCave,
								//    delegate
								//    {
								//        this.LocalIdentity.HandleFutureFrameInTime(1000,
								//            delegate
								//            {
								//                AIDirector.ActorExitCave(NewPlayer.Actor);
								//            }
								//        );
								//    }
								//);

								return;
							}

							if (NewPlayer.Actor.VelocityX == 0)
								if (NewPlayer.Actor.Animation != Actor.AnimationEnum.Talk)
								{
									NewPlayer.Actor.Animation = Actor.AnimationEnum.Talk;

									SoundBoard.Default.talk0_00();


									NewPlayer.Actor.KnownBubbles.Add(

										// show where shall we go

										new Actor.Bubble(DefaultZoom, this.LocalIdentity.SyncFrame % 6 - 1)
									);
								}
						};

					NewPlayer.Actor.EnterVehicle +=
						delegate
						{
							// exiting a vehicle is easy
							// entering is a bit harder
							// as we need to find it and reserve its use for us

							var ManAsObstacle = NewPlayer.Actor.ToObstacle();

							var NearbyVehicle = NewPlayer.Actor.CurrentLevel.KnownVehicles.Where(k => k.CurrentDriver == null).FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

							if (NearbyVehicle != null)
							{
								NearbyVehicle.CurrentDriver = NewPlayer.Actor;
							}
						};

					NewPlayer.Actor.Drop +=
						delegate
						{
							var CurrentVehicle = NewPlayer.Actor.CurrentVehicle;

							if (CurrentVehicle != null)
							{
								// can we drop a rock?

								if (CurrentVehicle.CurrentWeapon != null)
								{
									CurrentVehicle.CurrentWeapon.VelocityX = CurrentVehicle.VelocityX;
									CurrentVehicle.CurrentWeapon.VelocityY = 0.1;
									CurrentVehicle.CurrentWeapon = null;
								}
							}
						};

					NewPlayer.Actor.MoveTo(64, 64);
				}
			);
			this.Players.ForEachItemDeleted(
				DeletedPlayer =>
				{
					DeletedPlayer.Actor.CurrentLevel = null;
				}
			);


			this.Sync_EditorSelector =
				(int port, int type, int size, int x, int y) =>
				{
					this.Console.WriteLine("EditorSelector: " + new { port, type });

					var CurrentPort = this.Ports.SingleOrDefault(k => k.PortIdentity == port);

					var Selector = this.Selectors.Types[type].Sizes[size];
					var Position = new View.SelectorPosition { ContentX = x, ContentY = y };

					Selector.CreateTo(CurrentPort.Level, Position);
				};

		

			this.InitializeSync();

			this.Sync_RemoveLocalPlayer =
				(BindingList<PlayerInfo> a, int local) =>
				{
					var p = a.SingleOrDefault(k => k.IdentityLocal == local);

					if (p != null)
					{
						p.Input.Keyboard = null;


						a.Remove(p);
					}
				};

			Action Sync_LocalsIncrease = null;
			Action Sync_LocalsDecrease = null;

			var Interactive_Players = 0;


			Sync_LocalsDecrease =
				delegate
				{
					Interactive_Players--;
					this.Sync_RemoveLocalPlayer(this.LocalIdentity.Locals, this.LocalIdentity.Locals.Last().IdentityLocal);

				};

			Sync_LocalsIncrease =
				delegate
				{
					Interactive_Players++;

					if (this.CurrentPort == this.Lobby)
					{
						var EntryPoint = this.Lobby.GetRandomEntrypoint((x, y) => new { x, y });

						// this function call may need to be synchronized in time
						// in network mode
						this.Sync_TeleportTo(
							this.LocalIdentity.NetworkNumber,
							this.CurrentPort.PortIdentity,
							this.LocalIdentity.Locals.Count,
							EntryPoint.x,
							EntryPoint.y,
							0,
							0
						);
					}
					else if (this.CurrentPort == this.Editor)
					{
						var t = this.Editor.GetRandomEntrypoint((x, y) => new { x, y });

						this.Sync_TeleportTo(
							this.LocalIdentity.NetworkNumber,
							this.CurrentPort.PortIdentity,
							this.LocalIdentity.Locals.Count,
							t.x,
							t.y,
							0,
							0
						);

					}
					else if (this.CurrentPort == this.PrimaryMission)
					{
						var t = this.PrimaryMission.GetRandomEntrypoint((x, y) => new { x, y });

						this.Sync_TeleportTo(
							this.LocalIdentity.NetworkNumber,
							this.CurrentPort.PortIdentity,
							this.LocalIdentity.Locals.Count,
							t.x,
							t.y,
							0,
							0
						);

					}
				};


			this.Lobby.Menu.PlayersChanged +=
				delegate
				{
					var Players_add = (this.Lobby.Menu.Players - Interactive_Players).Max(0);
					var Players_remove = (Interactive_Players - this.Lobby.Menu.Players).Max(0);

					Console.WriteLine(new { Players_add, Players_remove, Interactive_Players, this.Lobby.Menu.Players }.ToString());

					Players_remove.Times(Sync_LocalsDecrease);
					Players_add.Times(Sync_LocalsIncrease);
				};




			var Credits = @"
				 programmed
					 by
			   arvo sulakatko
					with
				jsc compiler
				    in c#
			===
				dos  version
				 programmed
					 by
			   mario knezovic
					with
			  carsten neubauer
			===
				   levels 
				  designed
					 by
				peter schmitz
					 and
				  björn roy
			===
				dos version
				intros coded 
					 by
			   mario knezovic
					with
			   claudia scholz
			===
			      original
			    amiga version
			      programmed
			         by
			   thomas klinger
			        and
			     björn roy
			===
				  original
			   amiga graphics
				   drawn
					 by
			   thomas klinger
			===
			   pc graphics
					by
			  michael detert
				   with
			  carsten neubauer
			   mario knezovic
			===

				music and fx
					by
				maiko ruttmann
			";

			var TextContainers = new List<Dialog>();

			Credits.Split(k => k.Trim() == "===").ForEach(
				(string Text) =>
				{
					var d = new Dialog
					{
						Width = args.PortWidth,
						Height = args.PortHeight,
						Zoom = DefaultZoom,
						Text = Text.Trim(),
						VerticalAlignment = VerticalAlignment.Center,
					};

					TextContainers.Add(d);


				}
			);






			this.CurrentPort = this.Lobby;

			this.InitializePlayButton();
			this.InitializeMenuEditorButton();
			this.InitializeBackgroundLoading();
			this.InitializeKeyboardFocus();

			this.Lobby.Menu.AnyClick +=
				delegate
				{
					SoundBoard.Default.enter();

				};


			Lobby.LevelReference = KnownLevels.DefaultLobbyLevel;

			this.StartThinking();


			InitializeSplashCredits(TextContainers);
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

		#region IDisposable Members

		public void Dispose()
		{
			this.ThinkTimer.Stop();
		}

		#endregion

	}
}
