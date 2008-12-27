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

			public int Width;
			public int Height;

			public int Zoom;
		}

		public readonly BindingList<Port> Ports = new BindingList<Port>();

		public Workspace()
		{
			this.Container = new Canvas
			{
				Background = Brushes.Gray,
				Width = 640,
				Height = 400
			};

			const string LobbyLevel = Assets.Shared.KnownAssets.Path.Levels + "/level0_21.txt";

			LobbyLevel.ToStringAsset(
				LevelText =>
				{
					{
						var Level = new Level(LevelText, 2, this.Selectors);
						var View = new View(320, 200, Level);

						View.AttachContainerTo(this).MoveContainerTo(32, 32);

						Ports.Add(
							new Port
							{
								Level = Level,
								View = View,
							}
						);
					}

					{
						var w = new Window
						{
							DragContainer = this.Container,
							Width = 256 + 10,
							Height = 256 + 10
						};

						var Level = new Level(LevelText, 2, this.Selectors);
						var View = new View(256, 256, Level);

						w.Update();

						View.MoveContainerTo(w.Padding, w.Padding).AttachContainerTo(w);

						w.DraggableArea.BringToFront();
						w.MoveContainerTo(360, 64).AttachContainerTo(this);

						Ports.Add(
							new Port
							{
								Level = Level,
								View = View,
							}
						);
					}

					{
						var Level = new Level(LevelText, 2, this.Selectors);
						var View = new View(196, 128, Level);

						View.AttachContainerTo(this).MoveContainerTo(32, 230);
						View.AutoscrollEnabled = true;
						View.Flashlight.Visible = true;
						View.LocationTracker.Target = Level.KnownTryoperus.FirstOrDefault();

						Ports.Add(
							new Port
							{
								Level = Level,
								View = View,
							}
						);
					}
				}
			);

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

			//if (this.LocalIdentity.SyncFrame % 30 == 0)
			//    if (View.IsShakerEnabled)
			//        View.Level.AttributeWater.Value++;

			// some animations need to be synced by frame
			//foreach (var dino in Level.KnownDinos)
			//{
			//    dino.Animate(this.LocalIdentity.SyncFrame);
			//}

			//// we could pause the game here
			//foreach (var p in Players)
			//{
			//    p.AddAcceleration();
			//}

			foreach (var p in this.Ports)
			{
				foreach (var t in p.Level.KnownTryoperus)
				{
					t.Think();
				}

				p.Level.Physics.Apply();
			}
	

			this.LocalIdentity.SyncFrame++;
		}
	}
}
