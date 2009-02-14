using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Promotion;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		public Port CurrentPort;

		public MissionPort PrimaryMission;
		public Port CaveMission;
		public EditorPort Editor;
		public LobbyPort Lobby;

		[Script]
		public class Port : ISupportsContainer
		{
			public readonly Window Window = new Window();

			public readonly Canvas TouchContainerForViewContent;

			public Port()
			{
				this.Window.ContentContainer.Background = Brushes.Black;
				this.Window.ColorOverlay.Opacity = 1;

				// we are doing some advanced layering now
				this.TouchContainerForViewContent = new Canvas
				{
					Name = "Port_TouchContainerForViewContent",
					// we need to update this if the level changes
					// in size

					//Background = Brushes.Purple,
					//Opacity = 0.5
				}.AttachTo(this.Window.OverlayContainer);
			}



			public Canvas Container
			{
				get
				{
					return this.Window.Container;
				}
			}

			public int PortIdentity;

			public Level Level;
			public View View;

			public int StatusbarHeight;

			public int Padding { get { return this.Window.Padding; } set { this.Window.Padding = value; } }

			public int Width
			{
				get { return this.Window.ClientWidth; }
				set
				{
					this.Window.ClientWidth = value;
					this.Window.ColorOverlay.Element.Width = value;
				}
			}
			public int Height
			{
				get { return this.Window.ClientHeight; }
				set
				{
					this.Window.ClientHeight = value;
					this.Window.ColorOverlay.Element.Height = value;
				}
			}

			public int Zoom;

			public KnownSelectors Selectors;

			public bool IsLoading
			{
				get
				{

					if (this.LevelReference == null)
						return false;

					if (this.Level != null)
						return false;

					return true;
				}
			}


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
							this.WhenLoadedDelay = false;

							this.View = new View(Width, Height - StatusbarHeight, this.Level);
							this.View.Show(this.InternalVisible);
							this.View.AttachContainerTo(this.Window.ContentContainer);


							// we are doing some advanced layering now
							this.TouchContainerForViewContent.SizeTo(
								// we need to update this if the level changes
								// in size
								View.ContentExtendedWidth,
								View.ContentExtendedHeight
							);

							View.ContentExtendedContainerMoved +=
								(x, y) => TouchContainerForViewContent.MoveTo(x, y);

							// raise that event so we stay in sync
							View.MoveContentTo();
							View.TouchOverlay.Orphanize().AttachTo(TouchContainerForViewContent);



							if (this.Loaded != null)
								this.Loaded();

							while (WhenLoadedQueue.Count > 0)
								WhenLoadedQueue.Dequeue()();

						};

					value.DataFuture.Continue(ApplyData);

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

			public readonly Queue<Action> WhenLoadedQueue = new Queue<Action>();

			public bool WhenLoadedDelay;


			public void WhenLoaded(Action e)
			{
				if (WhenLoadedDelay)
				{
					WhenLoadedQueue.Enqueue(e);
					return;
				}

				if (Level == null)
				{
					WhenLoadedQueue.Enqueue(e);
					return;
				}

				e();
			}

			public void FadeToBlack()
			{
				this.Window.ColorOverlay.Opacity = 1;
			}
		}




	
	}
}
