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
using System.Windows.Input;
using System.Windows.Controls;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Diagnostics;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		[Script]
		public class EditorPort : Port
		{
			[Script]
			public class ConstructorArguments
			{
				public int Height;
				public int Zoom;

				public KnownSelectors Selectors;

				public BindingList<LevelReference> EmbeddedLevels;
				public BindingList<LevelReference> SavedLevels;
				public BindingList<LevelReference> DiscoveredLevels;
			}

			public readonly EditorToolbar Toolbar;
			public readonly LoadWindow LoadWindow;
			public readonly SaveWindow SaveWindow;

			[Script]
			public class Arrow : ArrowCursorControl
			{
				public PlayerIdentity Identity;

				public Action<double, double> AnimatedMoveTo;

				public Arrow()
				{
					this.AnimatedMoveTo = NumericEmitter.OfDouble((x, y) => this.MoveContainerTo(Convert.ToInt32(x), Convert.ToInt32(y)));
				}
			}

			public readonly BindingList<Arrow> Arrows = new BindingList<Arrow>();

			readonly StatusbarType Statusbar;


			public EditorPort(ConstructorArguments args)
			{
				this.Toolbar = new EditorToolbar(args.Selectors);

				#region Statusbar
				this.Statusbar = new StatusbarType(
					new StatusbarType.ConstructorArguments
					{
						Zoom = DefaultZoom
					}
				)
				{
					DesignMode = true
				};

				this.Statusbar.HeadCountTextBox.Click +=
					delegate
					{
						this.Statusbar.HeadCount = ((this.Statusbar.HeadCount + 1) % (this.Level.KnownPassengers.Count + 1)).Max(1);
						this.Level.AttributeHeadCount.Value = this.Statusbar.HeadCount;
					};


				this.Statusbar.MoveContainerTo(0, args.Height - Statusbar.Height - 1 * args.Zoom);
				this.Statusbar.AttachContainerTo(this.Window.OverlayContainer);
				this.StatusbarHeight = 18;

				#endregion



				this.SaveWindow = new SaveWindow();
				this.SaveWindow.TabSavedLevels.Items.MirrorTo(args.SavedLevels);


				this.LoadWindow = new LoadWindow();



				this.LoadWindow.EmbeddedLevels.Items.MirrorTo(args.EmbeddedLevels);
				this.LoadWindow.SavedLevels.Items.MirrorTo(args.SavedLevels);
				this.LoadWindow.DiscoveredLevels.Items.MirrorTo(args.DiscoveredLevels);


				this.Selectors = args.Selectors;

				// serialize current level


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

				this.SaveWindow.LevelScriptTextBox.GotFocus +=
					delegate
					{
						this.SaveWindow.LevelScriptTextBox.Text = this.Level.ToString();
					};

				this.Toolbar.SaveClicked +=
					delegate
					{
						if (this.SaveWindow.Visibility == Visibility.Hidden)
						{
							this.SaveWindow.BringContainerToFront();

							this.SaveWindow.PropertyCode.ValueText = this.Level.AttributeCode.Value;
							this.SaveWindow.PropertyNextLevelCode.ValueText = this.Level.AttributeNextCode.Value;
							this.SaveWindow.PropertyText.ValueText = this.Level.AttributeText.Value;

							this.SaveWindow.Preview.XLevelReference = new LevelReference
							{
								Data = this.Level.ToString()
							};

							this.SaveWindow.Show();

						}
						else
						{
							this.SaveWindow.Hide();
						}

					};

				this.Loaded +=
					delegate
					{
						this.Statusbar.HeadCount = this.Level.AttributeHeadCount.Value;
						this.Level.AttributeHeadCount.Assigned +=
							delegate
							{
								this.Statusbar.HeadCount = this.Level.AttributeHeadCount.Value;
							};


						this.View.EditorSelectorNextSize += () => this.Toolbar.EditorSelectorNextSize();
						this.View.EditorSelectorPreviousSize += () => this.Toolbar.EditorSelectorPreviousSize();
						this.View.EditorSelector = this.Toolbar.EditorSelector;

						this.Window.ColorOverlay.Opacity = 0;


						this.Arrows.ForEach(k => k.OrphanizeContainer().AttachContainerTo(this.View.ContentInfoOverlay));

						this.View.EditorSelectorApplied +=
							(Selector, Position, EditorSelectorApplied_args) =>
							{
								var Index = KnownSelectors.Index.Of(Selector, this.Selectors);

								if (this.Selectors.Types[Index.Type] == this.Selectors.Arrow)
								{
									// did we click on something useful?

									if (ArrowClick != null)
										ArrowClick(Position, EditorSelectorApplied_args);
								}
							};
					};


				this.Arrows.AttachTo(
					delegate
					{
						if (this.View == null)
							return null;

						return this.View.ContentInfoOverlay;
					}
				);


			}

			public event Action<View.SelectorPosition, MouseEventArgs> ArrowClick;


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

		[Script]
		public class MouseLocation : ISupportsLocationChanged
		{
			public double X { get; set; }
			public double Y { get; set; }

			#region ISupportsLocationChanged Members


			public event Action LocationChanged;

			#endregion

			public void RaiseLocationChanged()
			{
				if (this.LocationChanged != null)
					this.LocationChanged();
			}
		}

		public readonly MouseLocation Editor_LocalKnownMouseLocation = new MouseLocation();


		private void InitializeMenuEditorButton()
		{
			this.Editor.Toolbar.DragContainer = this.Container;
			this.Editor.Toolbar.Hide();
			this.Editor.Toolbar.AttachContainerTo(this.Container);

			this.Editor.LoadWindow.DragContainer = this.Container;
			this.Editor.LoadWindow.Hide();
			this.Editor.LoadWindow.AttachContainerTo(this);
			this.Editor.LoadWindow.MoveToCenter(this.Container);

			this.Editor.SaveWindow.DragContainer = this.Container;
			this.Editor.SaveWindow.Hide();
			this.Editor.SaveWindow.AttachContainerTo(this);
			this.Editor.SaveWindow.MoveToCenter(this.Container);

			// move it to bottom center
			this.Editor.Toolbar.MoveContainerTo(
				(this.Arguments.DefaultWidth - this.Editor.Toolbar.Width) / 2,
				-1
			);

			Lobby.Menu.Editor +=
				 delegate
				 {
					 Audio_Music.Loop = (AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/highscore_music.mp3");

					 if (this.Editor.XLevelReference == null)
					 {
						 // maybe send others a pre loading message too?

						 if (this.Sync_RemoteOnly_LoadLevelHint != null)
							 this.Sync_RemoteOnly_LoadLevelHint(this.Editor.PortIdentity);
					 }

					 this.Editor.Window.ColorOverlay.Opacity = 1;
					 this.Lobby.Window.ColorOverlay.SetOpacity(1,
						 delegate
						 {
							 this.Console.WriteLine("loading editor...");

							 this.CurrentPort = this.Editor;


							 // remove all locals from all ports
							 this.LocalIdentity.Locals.ForEach(
								 k => this.Sync_TeleportTo(this.LocalIdentity.NetworkNumber, 0, k.IdentityLocal, 0, 0, 0, 0)
							 );

							 // now we need to sync the editor

							 //if (this.Editor.LevelReference == null)
							 //    this.Editor.LevelReference = KnownLevels.DefaultLobbyLevel;


							 this.Editor.WhenLoaded(
								 delegate
								 {

									 //this.Editor.Players.AddRange(this.LocalIdentity.Locals.ToArray());


									 this.Editor.BringContainerToFront();
									 this.Editor.Toolbar.BringContainerToFront();
									 this.Editor.Toolbar.Show();
									 this.Editor.Window.ColorOverlay.Opacity = 0;


									 // how shall locals enter the editor?
									 // just jump randomly in

									 foreach (var p in this.LocalIdentity.Locals)
									 {
										 var t = this.Editor.GetRandomEntrypoint((x, y) => new { x, y });

										 this.Sync_TeleportTo(
											 this.LocalIdentity.NetworkNumber,
											 this.Editor.PortIdentity,
											 p.IdentityLocal, t.x, t.y, 0, 0
										 );
									 }


								 }
							 );

							 if (this.Editor.XLevelReference == null)
							 {
								 this.Sync_LoadLevel(
									 // load default editor level
									 this.Editor.PortIdentity, KnownLevels.DefaultEditorLevel, ""
								 );
							 }
						 }
					 );


				 };

			this.Editor.Loaded +=
				delegate
				{
					this.Editor.Level.KnownBirds.ForEachNewOrExistingItem(
						NewBird =>
						{
							SoundBoard.Default.bird_cry();
						}
					);

					this.Editor.View.TouchOverlay.MouseMove +=
						(_sender, _args) =>
						{
							var p = _args.GetPosition(this.Editor.View.TouchOverlay);

							// cursor has moved
							// if the view is followng the mouse
							// it should just do that

							this.Editor_LocalKnownMouseLocation.X = p.X;
							this.Editor_LocalKnownMouseLocation.Y = p.Y;
							this.Editor_LocalKnownMouseLocation.RaiseLocationChanged();

							if (this.Sync_RemoteOnly_MouseMove != null)
								this.Sync_RemoteOnly_MouseMove(this.Editor.PortIdentity, p.X, p.Y);
						};
				};

			this.Editor.SaveWindow.Click +=
				SaveTarget =>
				{
					// we can only save on an empty slot at this time
					if (SaveTarget.Data != null)
						return;

					this.Editor.Level.AttributeText.Value = this.Editor.SaveWindow.PropertyText.ValueText;
					this.Editor.Level.AttributeCode.Value = this.Editor.SaveWindow.PropertyCode.ValueText;

					this.Lobby.Menu.Password = this.Editor.SaveWindow.PropertyCode.ValueText;

					this.Editor.SaveWindow.Hide();

					SaveTarget.Data = this.Editor.Level.ToString();
				};

			#region LoadWindow
			this.Editor.LoadWindow.Click +=
				NextLevelForEditor =>
				{
					// we can not load an empty slot at this time
					if (NextLevelForEditor == null)
						return;

					if (NextLevelForEditor.Data == null)
						return;

					this.Editor.LoadWindow.Hide();

					// send early warning
					if (this.Sync_RemoteOnly_LoadLevelHint != null)
						this.Sync_RemoteOnly_LoadLevelHint(this.Editor.PortIdentity);

					this.Editor.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							// remove all locals from all ports
							this.LocalIdentity.Locals.ForEach(
								k => this.Sync_TeleportTo(this.LocalIdentity.NetworkNumber, 0, k.IdentityLocal, 0, 0, 0, 0)
							);

							this.Editor.WhenLoadedQueue.Enqueue(
								delegate
								{
									// we need to bring back all the players!

									foreach (var p in this.LocalIdentity.Locals)
									{
										var t = this.Editor.GetRandomEntrypoint((x, y) => new { x, y });

										this.Sync_TeleportTo(
											this.LocalIdentity.NetworkNumber,
											this.Editor.PortIdentity,
											p.IdentityLocal,
											t.x,
											t.y,
											0,
											0
										);
									}

								}
							);

							if (NextLevelForEditor.Location.Embedded != null)
							{
								this.Sync_LoadLevel(
									this.Editor.PortIdentity, NextLevelForEditor.Location.Embedded.AnimationFrame, ""
								);
							}
							else
							{
								this.Sync_LoadLevel(
									this.Editor.PortIdentity, -1, NextLevelForEditor.Data
								);
							}
						}
					);

				};
			#endregion

			InitializeEditorArrowClick();
		}

	

	}
}
