using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Editor;
using System.ComponentModel;
using AvalonUgh.Code.Dialogs;
using System.Windows.Input;
using ScriptCoreLib.Shared.Avalon.Tween;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using System.Windows.Shapes;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Code
{
	[Script]
	public class Workspace : ISupportsContainer
	{
		// workspace contains
		// multiple views to multiple concurrent
		// levels
		// workspace provides syncing, console and input

		public Canvas Container { get; set; }

		public Canvas Content { get; set; }

		public Canvas Overlay { get; set; }

		public readonly KnownSelectors Selectors = new KnownSelectors();
		public readonly BindingList<LevelReference> Levels = new BindingList<LevelReference>();



		/// <summary>
		/// This will reflect the clients name and number,
		/// We could be in control of none or multiple actors or vehicles
		/// within multiple views and levels
		/// </summary>
		public readonly PlayerIdentity LocalIdentity = new PlayerIdentity { Name = "LocalPlayer" };

		[Script]
		public class Port
		{
			public int PortIdentity;

			public Level Level;
			public View View;

			public Canvas Container;

			public int Left;
			public int Top;
			public int Width;
			public int Height;

			public int Zoom;

			public KnownSelectors Selectors;

			LevelReference InternalLevelReference;
			public LevelReference LevelReference
			{
				get
				{
					return InternalLevelReference;
				}
				set
				{
					if (Level != null)
						Level.Clear();

					if (View != null)
						View.OrphanizeContainer();


					Level = null;
					View = null;

					InternalLevelReference = value;

					Action<string> ApplyData =
						Data =>
						{
							if (Level != null)
								throw new Exception("InternalLevelReference");

							this.Level = new Level(Data, this.Zoom, this.Selectors);

							this.View = new View(Width, Height, this.Level);
							this.View.Show(this.InternalVisible);
							this.View.MoveContainerTo(this.Left, this.Top).AttachContainerTo(this.Container);

							if (this.Loaded != null)
								this.Loaded();

						};

					if (value.Data == null)
					{
						InternalLevelReference.Location.Embedded.ToString().ToStringAsset(
							ApplyData
						);
					}
					else
					{
						ApplyData(value.Data);
					}
				}
			}

			bool InternalVisible = true;
			public bool Visible
			{
				get
				{
					return InternalVisible;
				}
				set
				{
					InternalVisible = value;

					if (this.View != null)
						this.View.Show(value);
				}
			}

			public event Action Loaded;
		}

		const int PortIdentity_Lobby = 1000;
		const int PortIdentity_Editor = 2000;
		const int PortIdentity_Mission = 3000;


		public readonly BindingList<Port> Ports = new BindingList<Port>();
		public readonly BindingList<PlayerInfo> Players = new BindingList<PlayerInfo>();

		public GameConsole Console { get; set; }

		const int DefaultZoom = 2;

		public Workspace(int DefaultWidth, int DefaultHeight)
		{
			this.Container = new Canvas
			{
				Background = Brushes.Black,
				Width = DefaultWidth,
				Height = DefaultHeight
			};

			this.Content = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);

			this.Overlay = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);



			#region setting up our console
			this.Console = new GameConsole();

			this.Console.SizeTo(DefaultWidth, DefaultHeight / 2);
			this.Console.WriteLine("Avalon Ugh! Console ready.");
			this.Console.AnimatedTop = -this.Console.Height;

			this.Console.AttachContainerTo(this.Overlay);
			#endregion


			var Menu = new ModernMenu(DefaultZoom, DefaultWidth, DefaultHeight);


			Menu.AttachContainerTo(this.Overlay);

			var LevelIntro = new LevelIntroDialog
			{
				AnimatedOpacity = 0,
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = DefaultZoom,
				AnimatedOpacityContentMultiplier = 1
			};

			LevelIntro.AttachContainerTo(this.Overlay);


			#region PauseDialog
			var PauseDialog = new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = DefaultZoom,
				BackgroundVisible = false,
				VerticalAlignment = VerticalAlignment.Center,
				Text = @"
						   game was paused
						     by
							you
						",
				AnimatedOpacity = 0
			}.AttachContainerTo(this.Overlay);



			Action<bool, string> SetPause =
				(IsPaused, ByWhom) =>
				{
					this.LocalIdentity.SyncFramePaused = IsPaused;

					if (IsPaused)
					{
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

			var LobbyPort = new Port
			{
				Container = this.Content,

				Selectors = this.Selectors,

				Zoom = DefaultZoom,

				Width = DefaultWidth,
				Height = DefaultHeight - 18,

				PortIdentity = PortIdentity_Lobby,


			};


			var Local0 =
				new PlayerInfo
				{
					Actor = new Actor.man0(DefaultZoom),
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

			this.LocalIdentity.Locals.Add(Local0);


			LobbyPort.Loaded +=
				delegate
				{
					Console.WriteLine("adding an actor to lobby");

					Local0.Actor.MoveTo(
						(LobbyPort.View.ContentActualWidth / 4) +
						(LobbyPort.View.ContentActualWidth / 2).Random(),
						LobbyPort.View.ContentActualHeight / 2);


					LobbyPort.Level.KnownActors.Add(Local0.Actor);
				};

			LobbyPort.LevelReference = new LevelReference(0);

			this.Players.Add(Local0);

			this.Ports.Add(LobbyPort);



			this.Container.KeyUp +=
				(sender, args) =>
				{
					// oem7 will trigger the console
					if (args.Key == Key.Oem7)
					{
						args.Handled = true;

						if (Console.AnimatedTop == 0)
						{
							Console.AnimatedTop = -Console.Height;
						}
						else
						{
							Console.AnimatedTop = 0;
						}

						// the console is on top
						// of the game view
						// and under the transparent touch overlay
						// when the view is in editor mode
					}

					if (this.LocalIdentity.SyncFramePaused)
					{
						if (args.Key == Key.P)
						{
							args.Handled = true;
							SetPause(false, "you");

							return;
						}

					}
					else
					{
						if (args.Key == Key.Escape)
						{
							args.Handled = true;

							// if we are inside a mission, submission or editor this will bring us back

							this.Ports.Where(k => k.PortIdentity != PortIdentity_Lobby).ForEach(k => k.Visible = false);

							// if all players quit the game
							// we would be able to start another level
							Menu.PlayText = "resume";
							Menu.Show();
						}


						Menu.HandleKeyUp(args);

						if (args.Handled)
							return;



						if (args.Key == Key.P)
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
				(sender, args) =>
				{
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
						Resumeable.Visible = true;
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

					LevelIntro.AnimatedOpacity = 1;

					// fade in the level start menu
					// create and load new port
					// hide other ports
					// fade out

					// if we are in multyplayer
					// we need to do this in sync

					var NextLevelPort =
						new Port
						{
							Container = this.Content,

							Selectors = this.Selectors,

							Zoom = DefaultZoom,

							Width = DefaultWidth,
							Height = DefaultHeight - 18,

							PortIdentity = PortIdentity_Mission,

							LevelReference = NextLevel,

							Visible = false,
						};

					this.Ports.Add(NextLevelPort);

					this.LocalIdentity.HandleFutureFrame(
						100,
						delegate
						{
							Menu.Hide();

							NextLevelPort.Visible = true;

							LevelIntro.AnimatedOpacity = 0;
						}
					);
				};

		}

		void Think()
		{
			if (this.LocalIdentity.SyncFramePaused)
			{
				if (this.LocalIdentity.SyncFramePausedSkip)
				{
					this.LocalIdentity.SyncFramePausedSkip = false;
				}
				else
				{
					return;
				}
			}

			if (this.LocalIdentity.SyncFrameLimit > 0)
			{
				if (this.LocalIdentity.SyncFrameLimit <= this.LocalIdentity.SyncFrame)
				{
					return;
				}
			}

			if (this.Ports.Any(k => k == null))
				return;

			//if (this.LocalIdentity.SyncFrame % 30 == 0)
			//    if (View.IsShakerEnabled)
			//        View.Level.AttributeWater.Value++;



			//// we could pause the game here
			foreach (var p in Players)
			{
				p.AddAcceleration();
			}

			foreach (var p in this.Ports)
			{
				// some animations need to be synced by frame
				foreach (var dino in p.Level.KnownDinos)
				{
					dino.Animate(this.LocalIdentity.SyncFrame);
				}

				foreach (var t in p.Level.KnownTryoperus)
				{
					t.Think();
				}

				p.Level.Physics.Apply();
			}


			this.LocalIdentity.SyncFrame++;
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
