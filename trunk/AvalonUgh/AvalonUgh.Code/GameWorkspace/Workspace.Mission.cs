using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Cursors;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		[Script]
		public partial class MissionPort : Port
		{
			public readonly LevelIntroDialog Intro;
			public readonly Dialog Fail;

			[Script]
			public class ConstructorArguments
			{
				public int Padding;
				public int Width;
				public int Height;
				public int Zoom;
			}

			public readonly Statusbar Statusbar;

			public MissionPort(ConstructorArguments args)
			{
				this.Statusbar = new Statusbar(
					  new Statusbar.ConstructorArguments
					  {
						  Zoom = args.Zoom
					  }
				  );

				this.Padding = args.Padding;
				this.Zoom = args.Zoom;
				this.StatusbarHeight = Statusbar.Height + 2 * args.Zoom;

				this.Width = args.Width;
				this.Height = args.Height;

				Statusbar.MoveContainerTo(0, args.Height - Statusbar.Height - 1 * args.Zoom);
				Statusbar.AttachContainerTo(this.Window.ContentContainer);


				this.Intro = new LevelIntroDialog
				{
					Width = args.Width,
					Height = args.Height,
					Zoom = args.Zoom,
				};

				this.Intro.AttachContainerTo(this.Window.OverlayContainer);

				this.Fail = new Dialog
				{
					Width = args.Width,
					Height = args.Height,
					Zoom = args.Zoom,
					BackgroundVisible = false,
					VerticalAlignment = VerticalAlignment.Center,
					Text = @"
				   bad luck
				  you failed
				"
				};

				this.Fail.AttachContainerTo(this.Window.OverlayContainer);

				this.Loaded +=
					delegate
					{
						this.Statusbar.HeadCount = this.Level.AttributeHeadCount.Value;
						this.Level.AttributeHeadCount.Assigned +=
							delegate
							{
								this.Statusbar.HeadCount = this.Level.AttributeHeadCount.Value;
							};


						this.View.Memory_ScoreChanged +=
							delegate
							{
								this.Statusbar.HighScore = this.View.Memory_Score;
							};

						this.View.Memory_ScoreMultiplierChanged +=
							delegate
							{
								//this.Statusbar.HighScore = this.View.Memory_Score;
							};

						// we do not need to see the start position markers
						// it is useful only in the editor
						this.View.StartPositionsContainer.Hide();
						//this.View.ContentInfoOverlay.Hide();

						ReinitializeStatusbar();
					};



			}

	

			public Tuple GetRandomEntrypointForVehicle<Tuple>(Func<double, double, Tuple> CreateTuple)
			{
				if (this.Level == null)
					throw new Exception("Level has to be loaded before you can teleport into it");

				var s = LambdaExtensions.Random(
					from k in this.Level.KnownStones
					where k.Selector.PrimitiveTileCountX >= 2
					where k.Selector.PrimitiveTileCountY >= 2
					where k.Y < this.Level.WaterTop
					select k
				);

				return CreateTuple(s.X, s.Y);
			}

			public Tuple GetRandomEntrypoint<Tuple>(Func<double, double, Tuple> CreateTuple)
			{
				if (this.Level == null)
					throw new Exception("Level has to be loaded before you can teleport into it");

				if (this.Level.KnownCaves.Any())
				{
					var c = this.Level.KnownCaves.Random();

					return CreateTuple(c.X, c.Y);
				}

				return CreateTuple(100, 100);
			}
		}

		void InitializePlayButton()
		{
			this.Sync_MissionStartHint =
				(int user, int difficulty) =>
				{
					// sync difficulty
					this.Lobby.Menu.DifficultyLevel = difficulty;

					// if we are not in the lobby, 

					this.Lobby.Menu.Options_Options.Hide();
					this.Lobby.Menu.Options_Options.TouchOverlay.Hide();
					this.Lobby.Menu.Options_Play.Hide();
					this.Lobby.Menu.Options_Play.TouchOverlay.Hide();



					// we need protection for reentrancy


					this.Lobby.Menu.Options_CountDown.Show();

					Action<int> SetCounter =
						e =>
						{
							this.Lobby.Menu.Options_CountDown.Text = "" + e;

							SoundBoard.Default.enter();
						};


					SetCounter(3);

					// look, a time line! in code!
					this.LocalIdentity.HandleFutureFrameInTime(
						400 * 1, () => SetCounter(2)
					);

					this.LocalIdentity.HandleFutureFrameInTime(
						400 * 2, () => SetCounter(1)
					);

					this.LocalIdentity.HandleFutureFrameInTime(
						400 * 3,
						delegate
						{
							SetCounter(0);
							this.Lobby.Menu.Options_CountDown.Hide();
							this.Lobby.FadeToBlack();
						}
					);


					// in the future only the player that pressed the button 
					// will tell others what level to load

					this.LocalIdentity.HandleFutureFrameInTime(
						400 * 5,
						delegate
						{
							if (user == this.LocalIdentity.NetworkNumber)
							{
								// MissionStartHint continues here
								// only the initiator knows about the level to be loaded

								var NextLevel2 = this.LoadableLevels.FirstOrDefault(k => k.Code.ToLower() == Lobby.Menu.Password.ToLower());

								if (NextLevel2 == null)
								{
									// password does not match
									NextLevel2 = this.KnownLevels.DefaultMissionLevel;
								}

								this.Sync_LoadLevelEx(this.PrimaryMission, NextLevel2);
							}
						}
					);



					this.PrimaryMission.WhenLoaded(
						delegate
						{


							// we are showing black lobby
							// we now need to show the introducion to the level
							// and we need to teleport our guys over to the mission

							this.Console.WriteLine("ready for mission at frame " + this.LocalIdentity.SyncFrame);

							if (this.CurrentPort == this.Lobby)
							{
								EnterMission();
							}

							this.Lobby.Menu.Options_CountDown.Hide();

							this.Lobby.Menu.Options_Options.Show();
							this.Lobby.Menu.Options_Options.TouchOverlay.Show();
							this.Lobby.Menu.Options_Play.Show();
							this.Lobby.Menu.Options_Play.TouchOverlay.Show();

						}
					);
				};

			this.PrimaryMission.WhenLoaded(
				delegate
				{
					// either loaded before or after game start

					this.Lobby.Menu.Options_Play.Text = "watch others play";
				}
			);

			this.Lobby.Menu.Play +=
				delegate
				{
					if (this.PrimaryMission.LevelReference == null)
					{
						this.Sync_MissionStartHint(
							this.LocalIdentity.NetworkNumber,
							this.Lobby.Menu.DifficultyLevel
						);
					}
					else
					{
						// we are only going to observe
						this.Lobby.Menu.Players = 0;

						this.LocalIdentity.HandleFutureFrame(
							EnterMission
						);
					}
				};
		}

		private void EnterMission()
		{
			// we need to create vehicles for local players

			foreach (var i in this.LocalIdentity.Locals)
			{
				var p = this.PrimaryMission.GetRandomEntrypointForVehicle((x, y) => new { x, y });

				this.Sync_TeleportTo(
					this.LocalIdentity.NetworkNumber,
					this.PrimaryMission.PortIdentity,
					i.IdentityLocal,
					p.x,
					p.y,
					0, 0
				);

				this.Sync_Vehicalize(this.LocalIdentity.NetworkNumber, i.IdentityLocal);
			}

			var Caves = this.PrimaryMission.Level.KnownCaves.ToArray(Cave => new { Cave, Obstacle = Cave.ToObstacle() });
 
			this.PrimaryMission.Level.KnownPassengers.ForEach(
				(Passenger, index) =>
				{
					var PassengerObstacle = Passenger.ToObstacle();

					var PassengerCave = Caves.FirstOrDefault(k => k.Obstacle.Intersects(PassengerObstacle));

					if (PassengerCave != null)
					{
						// move the passangers into the cave
						Passenger.CurrentCave = PassengerCave.Cave;
						Passenger.Animation = Actor.AnimationEnum.Hidden;

						// only the first passanger will have to wait some frames
						// others will wait for their previous neighbour to reach
						// waiting point
						if (index > 0)
							Passenger.Memory_LogicState = Actor.Memory_LogicState_CaveLifeEnd - 1;
					}
				}
			);

			(KnownAssets.Path.Audio + "/newlevel.mp3").PlaySound();

			// user is indicating we are ready to play.

			// everybody who is in the lobby will be added to the game as players
			// who join later can only spectate for level already loaded
			// or join as passangers


			// fade this to black
			// switch the ports
			// fade in the intro
			// load
			// fade to black
			// fade to view
			//this.Lobby.Window.ColorOverlay.Element.BringToFront();

			#region Smoke And mirrors
			this.PrimaryMission.Window.ColorOverlay.Opacity = 1;
			this.Lobby.Window.ColorOverlay.SetOpacity(1,
				delegate
				{
					if (PrimaryMission.LevelReference.Location.Embedded != null)
						PrimaryMission.Intro.LevelNumber = PrimaryMission.LevelReference.Location.Embedded.AnimationFrame;
					else
						PrimaryMission.Intro.LevelNumber = 0;

					PrimaryMission.Intro.LevelTitle = PrimaryMission.LevelReference.Text;
					PrimaryMission.Intro.LevelPassword = PrimaryMission.LevelReference.Code;

					PrimaryMission.Intro.BringContainerToFront();
					PrimaryMission.Intro.Show();
					PrimaryMission.Fail.Hide();

					this.CurrentPort = this.PrimaryMission;
					Music.Loop = (AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music2.mp3");



					PrimaryMission.BringContainerToFront();
					PrimaryMission.Window.ColorOverlay.SetOpacity(0,
						delegate
						{
							// are we loading or rejoining?
							// this will freeze the game
							// and we will display non animated dialog for that time
							// but what if the load is way too fast?

							PrimaryMission.WhenLoaded(
								delegate
								{
									2000.AtDelay(
										delegate
										{
											PrimaryMission.Window.ColorOverlay.SetOpacity(1,
												delegate
												{
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
				}
			); 
			#endregion
		
		
			

		}
	}
}
