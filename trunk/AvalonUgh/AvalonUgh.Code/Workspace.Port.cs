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

			public readonly BindingList<PlayerInfo> Players = new BindingList<PlayerInfo>();

			public event Action<PlayerInfo> PlayerJoined;

			public Port()
			{
				this.Window.ContentContainer.Background = Brushes.Black;

				this.Players.ForEachNewOrExistingItem(
					k =>
					{
						if (PlayerJoined != null)
							PlayerJoined(k);

						Action DetectDeparture = null;

						DetectDeparture =
							delegate
							{
								if (k.Actor.CurrentLevel != this.Level)
								{
									k.Actor.CurrentLevelChanged -= DetectDeparture;
									this.Players.Remove(k);
								}
							};

						k.Actor.CurrentLevelChanged += DetectDeparture;
					}
				);
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

							this.View = new View(Width, Height - StatusbarHeight, this.Level);
							this.View.Show(this.InternalVisible);
							this.View.AttachContainerTo(this.Window.ContentContainer);


							// we are doing some advanced layering now
							var TouchContainerForViewContent = new Canvas
							{
								// we need to update this if the level changes
								// in size
								Width = View.ContentExtendedWidth,
								Height = View.ContentExtendedHeight
							}.AttachTo(this.Window.OverlayContainer);

							View.ContentExtendedContainerMoved +=
								(x, y) => TouchContainerForViewContent.MoveTo(x, y);

							// raise that event so we stay in sync
							View.MoveContentTo();
							View.TouchOverlay.Orphanize().AttachTo(TouchContainerForViewContent);



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

			public void WhenLoaded(Action e)
			{
				if (Level == null)
				{
					Action handler = null;

					handler =
						delegate
						{
							this.Loaded -= handler;

							e();
						};

					this.Loaded += handler;
				}
				else
				{
					e();
				}
			}
		}


		[Script]
		public class LobbyPort : Port
		{
			[Script]
			public class ConstructorArguments
			{
				public int Zoom;
				public int Width;
				public int Height;
			}

			public readonly ModernMenu Menu;

			public LobbyPort(ConstructorArguments args)
			{
				this.Zoom = DefaultZoom;

				this.Width = args.Width;
				this.Height = args.Height;

				this.Menu = new ModernMenu(args.Zoom, args.Width, args.Height);

				this.Menu.AttachContainerTo(this.Window.OverlayContainer);


				this.WhenLoaded(
					delegate
					{
						this.Menu.BringContainerToFront();
					}
				);
			}
		}


		[Script]
		public class EditorPort : Port
		{
			[Script]
			public class ConstructorArguments
			{
				public KnownSelectors Selectors;
				public BindingList<LevelReference> Levels;
			}

			public readonly EditorToolbar Toolbar;
			public readonly LoadWindow LoadWindow;

			public EditorPort(ConstructorArguments args)
			{
				this.Toolbar = new EditorToolbar(args.Selectors);
				this.LoadWindow = new LoadWindow(args.Levels);

				this.Selectors = args.Selectors;


				// serialize current level
				this.Toolbar.LevelText.GotFocus +=
					delegate
					{
						if (this.Level == null)
							return;

						this.Toolbar.LevelText.Text = this.Level.ToString();
					};

				this.Toolbar.EditorSelectorChanged +=
					delegate
					{
						if (this.View == null)
							return;

						this.View.EditorSelector = this.Toolbar.EditorSelector;
					};

				this.Toolbar.LoadClicked +=
					delegate
					{
						this.LoadWindow.BringContainerToFront();
						this.LoadWindow.Show(this.LoadWindow.Visibility == Visibility.Hidden);
					};

				this.WhenLoaded(
					delegate
					{
						this.View.EditorSelectorNextSize += () => this.Toolbar.EditorSelectorNextSize();
						this.View.EditorSelectorPreviousSize += () => this.Toolbar.EditorSelectorPreviousSize();
					}
				);

				Action<PlayerInfo> AttachPlayerToLevel =
					k =>
					{
						var c = this.Level.KnownCaves.FirstOrDefault();

						if (c == null)
						{
							k.Actor.MoveTo(
								(this.View.ContentActualWidth / 4) +
								(this.View.ContentActualWidth / 2).Random(),
								this.View.ContentActualHeight / 2);
						}
						else
						{
							k.Actor.MoveTo(c.X, c.Y);
						}


						k.Actor.CurrentLevel = this.Level;
					};

				this.PlayerJoined += AttachPlayerToLevel;

				this.LoadWindow.Click +=
					NextLevelForEditor =>
					{
						this.LoadWindow.Hide();

						this.Window.ColorOverlay.SetOpacity(1,
							delegate
							{
								this.LevelReference = NextLevelForEditor;

								this.WhenLoaded(
									delegate
									{
										// we need to bring back all the players!

										// just jump randomly in
										foreach (var k in this.Players)
										{
											AttachPlayerToLevel(k);
										}

										this.Window.ColorOverlay.Opacity = 0;
									}
								);
							}
						);

					};

			}
		}


		[Script]
		public class MissionPort : Port
		{
			public readonly LevelIntroDialog Intro;
			public readonly Dialog Fail;

			[Script]
			public class ConstructorArguments
			{
				public int Width;
				public int Height;
				public int Zoom;
			}

			public MissionPort(ConstructorArguments args)
			{
				this.Zoom = args.Zoom;
				this.StatusbarHeight = 18;

				this.Width = args.Width;
				this.Height = args.Height;


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


				this.WhenLoaded(
					delegate
					{
						this.Intro.BringContainerToFront();
					}
				);
			}
		}
	}
}
