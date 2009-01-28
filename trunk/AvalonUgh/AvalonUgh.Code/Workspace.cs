using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
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

        public readonly BindingList<LevelReference> EmbeddedLevels = new BindingList<LevelReference>();
        public readonly BindingList<LevelReference> SavedLevels = new BindingList<LevelReference>();



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

            public bool Paused;
        }

        public readonly ConstructorArguments Arguments;

        readonly AudioLoop Music;


        public Workspace(ConstructorArguments args)
        {
            this.Arguments = args;

            this.LocalIdentity.SyncFramePaused = args.Paused;

            this.Music = new AudioLoop
            {
                Volume = 0.3,
                Loop = (AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3"),
                //Enabled = true,
            };


            var KnownLevels = new KnownLevels();

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
                                (Selector, Position) =>
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
                    EmbeddedLevels = this.EmbeddedLevels,
                    SavedLevels = this.SavedLevels,
                })
            {

                Zoom = DefaultZoom,

                Padding = args.WindowPadding,
                Width = args.PortWidth,

                Height = args.PortHeight,

                PortIdentity = PortIdentity_Editor,
            };

            this.Editor.Loaded +=
                delegate
                {
                    // each time this a map in the editor is loaded we will update minimap

                    this.MiniLevel.LevelReference = this.Editor.LevelReference;
                    this.MiniLevel.BringContainerToFront();
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

            this.Players.ForEachNewOrExistingItem(
                NewPlayer =>
                {
                    // here we create an actor for remote and local players

                    NewPlayer.Actor = new Actor.man0(DefaultZoom)
                    {
                        RespectPlatforms = true,
                        CanBeHitByVehicle = false,
                    };

                    NewPlayer.Actor.Jumping +=
                        delegate
                        {
                            (Assets.Shared.KnownAssets.Path.Audio + "/jump.mp3").PlaySound();
                        };

                    // every actor could act differently on gold collected
                    NewPlayer.Actor.GoldStash.ForEachNewItem(
                        gold =>
                        {
                            // play the sound only if it is in the same port
                            (Assets.Shared.KnownAssets.Path.Audio + "/treasure.mp3").PlaySound();

                            // the yellow flash shall be displayed for local players only
                        }
                    );



                    NewPlayer.Actor.CurrentVehicleChanged +=
                        delegate
                        {
                            (Assets.Shared.KnownAssets.Path.Audio + "/enter.mp3").PlaySound();
                        };


                    NewPlayer.Actor.EnterCave +=
                        delegate
                        {
                            if (NewPlayer.Actor.VelocityX == 0)
                                if (NewPlayer.Actor.Animation != Actor.AnimationEnum.Talk)
                                {
                                    NewPlayer.Actor.Animation = Actor.AnimationEnum.Talk;

                                    (Assets.Shared.KnownAssets.Path.Audio + "/talk0_00.mp3").PlaySound();
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

                                CurrentVehicle.CurrentWeapon = null;
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

            this.Sync_LoadLevel =
                (int port, int level, string custom) =>
                {
                    var CurrentPort = this.Ports.SingleOrDefault(k => k.PortIdentity == port);

                    Console.WriteLine("loading level " + level + " for port " + port + " at frame " + this.LocalIdentity.SyncFrame);

                    if (level == -1)
                    {
                        // will load custom level instead

                        // we have just recieved a new level - we might want to save
                        // it into our store
                        // we need to take a hash of it tho

                        CurrentPort.LevelReference =
                            new LevelReference(new LevelReference.StorageLocation { Cookie = "editorlevel" })
                            {
                                Data = custom
                            };

                    }
                    else
                    {
                        CurrentPort.LevelReference = this.EmbeddedLevels.Single(k => k.Location.Embedded.AnimationFrame == level);
                    }

                };

            #region Sync_TeleportTo local implementation
            this.Sync_TeleportTo =
                (BindingList<PlayerInfo> a, int port, int local, double x, double y, double vx, double vy) =>
                {
                    var p = a.SingleOrDefault(k => k.IdentityLocal == local);

                    if (p == null)
                    {
                        p = new PlayerInfo
                        {
                            Identity = LocalIdentity,
                            Input = new PlayerInput
                            {

                            }
                        };

                        if (a == this.LocalIdentity.Locals)
                        {
                            p.Input.Keyboard = this.SupportedKeyboardInputs[local];
                        }
                        else
                            p.Input.Keyboard = new KeyboardInput(new KeyboardInput.Arguments.Arrows());

                        a.Add(p);

                        this.Console.WriteLine("created new player via teleport " + new { p.Identity, p.IdentityLocal });

                    }



                    p.IdentityLocal = local;

                    var CurrentPort = this.Ports.SingleOrDefault(k => k.PortIdentity == port);

                    if (CurrentPort == null)
                        p.Actor.CurrentLevel = null;
                    else
                        p.Actor.CurrentLevel = CurrentPort.Level;

                    p.Actor.MoveTo(x, y);
                    p.Actor.VelocityX = vx;
                    p.Actor.VelocityY = vy;

                    if (a == this.LocalIdentity.Locals)
                    {
                        // every actor could act differently on gold collected
                        p.Actor.GoldStash.ForEachNewItem(
                            gold =>
                            {
                                var _CurrentPort = this.Ports.Single(k => k.Level == p.Actor.CurrentLevel);

                                _CurrentPort.View.ColorOverlay.Background = Brushes.Yellow;
                                _CurrentPort.View.ColorOverlay.Opacity = 0.7;
                                _CurrentPort.View.ColorOverlay.Show();
                                _CurrentPort.View.ColorOverlay.FadeOut();
                            }
                        );

                    }
                };
            #endregion

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
                            this.LocalIdentity.Locals,
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
                            this.LocalIdentity.Locals,
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

                };


            #endregion


            Lobby.WhenLoaded(
                delegate
                {
                    Console.WriteLine("lobby loaded");

                    // we should load lobby only once

                    this.Lobby.Players.AddRange(this.LocalIdentity.Locals.ToArray());


                    Lobby.Window.ColorOverlay.Opacity = 0;

                    this.MiniLevel = new MiniLevelWindow
                    {
                        DragContainer = this.Container,
                        LevelReference = Lobby.LevelReference
                    };

                    this.MiniLevel.AttachContainerTo(this);

                    this.Console.AnimatedTopChanged +=
                        delegate
                        {
                            this.MiniLevel.BringContainerToFront();
                        };
                }
            );



            this.CurrentPort = this.Lobby;

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
                            var NextLevel2 = this.EmbeddedLevels.FirstOrDefault(k => k.Code.ToLower() == Lobby.Menu.Password.ToLower());

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



            InitializeMenuEditorButton();


            BackgroundLoading = new DialogTextBox
            {
                Zoom = 2,
                Text = "loading...",
                Visibility = Visibility.Hidden
            }.AttachContainerTo(this);

            var s = new Statusbar.StatusbarWindow
            {
                DragContainer = this.Container
            }.AttachContainerTo(this);




            BackgroundLoading.MoveContainerTo(0, args.DefaultHeight - BackgroundLoading.Height);

            this.EnableKeyboardFocus();


            Lobby.LevelReference = KnownLevels.DefaultLobbyLevel;
            this.StartThinking();
        }

        public readonly DialogTextBox BackgroundLoading;


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

        MiniLevelWindow MiniLevel;
    }
}
