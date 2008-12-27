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

					InternalLevelReference.Location.Embedded.ToString().ToStringAsset(
						Data =>
						{
							if (Level != null)
								throw new Exception("InternalLevelReference");

							this.Level = new Level(Data, this.Zoom, this.Selectors);
							this.View = new View(Width, Height, this.Level);
							this.View.Show(this.InternalVisible);
							this.View.MoveContainerTo(this.Left, this.Top).AttachContainerTo(this.Container);
						}
					);
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
		}

		const int PortIdentity_Lobby = 1000;
		const int PortIdentity_Editor = 2000;
		const int PortIdentity_Mission = 3000;

		public readonly BindingList<Port> Ports = new BindingList<Port>();

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



			this.Ports.Add(
				new Port
				{
					Container = this.Content,

					Selectors = this.Selectors,

					Zoom = DefaultZoom,

					Width = DefaultWidth,
					Height = DefaultHeight - 18,

					PortIdentity = PortIdentity_Lobby,

					LevelReference = new LevelReference(0)
				}
			);




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

					if (args.Key == Key.D1)
					{
						args.Handled = true;
						foreach (var p in this.Ports)
						{
							p.Visible = p.PortIdentity == PortIdentity_Lobby;
						}
					}

					if (args.Key == Key.D2)
					{
						args.Handled = true;
						foreach (var p in this.Ports)
						{
							p.Visible = p.PortIdentity == PortIdentity_Editor;
						}
					}

					if (args.Key == Key.P)
					{
						SetPause(!this.LocalIdentity.SyncFramePaused, "you");
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

			foreach (var p in this.Ports)
			{
				if (p.Level == null)
					return;
			}

			//if (this.LocalIdentity.SyncFrame % 30 == 0)
			//    if (View.IsShakerEnabled)
			//        View.Level.AttributeWater.Value++;



			//// we could pause the game here
			//foreach (var p in Players)
			//{
			//    p.AddAcceleration();
			//}

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
