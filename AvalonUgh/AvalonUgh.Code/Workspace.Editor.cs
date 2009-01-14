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

namespace AvalonUgh.Code
{
	partial class Workspace
	{
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


			public EditorPort(ConstructorArguments args)
			{
				this.Toolbar = new EditorToolbar(args.Selectors);

				this.SaveWindow = new SaveWindow();

				this.LoadWindow = new LoadWindow(args.Levels);

				this.Selectors = args.Selectors;
				this.StatusbarHeight = 18;

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

				this.Toolbar.SaveClicked +=
					delegate
					{
						this.SaveWindow.BringContainerToFront();
						this.SaveWindow.Show(this.SaveWindow.Visibility == Visibility.Hidden);
					};

				this.Loaded +=
					delegate
					{
						this.View.EditorSelectorNextSize += () => this.Toolbar.EditorSelectorNextSize();
						this.View.EditorSelectorPreviousSize += () => this.Toolbar.EditorSelectorPreviousSize();

						this.Window.ColorOverlay.Opacity = 0;


						this.Arrows.ForEach(k => k.OrphanizeContainer().AttachContainerTo(this.View.ContentInfoOverlay));

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
				this.Arguments.DefaultHeight - this.Editor.Toolbar.Padding * 3 - PrimitiveTile.Heigth * 4
			);

			Lobby.Menu.Editor +=
				 delegate
				 {
					 if (this.Editor.LevelReference == null)
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
								 k => this.Sync_TeleportTo(this.LocalIdentity.Locals, 0, k.IdentityLocal, 0, 0, 0, 0)
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
											 this.LocalIdentity.Locals,
											 this.Editor.PortIdentity,
											 p.IdentityLocal, t.x, t.y, 0, 0
										 );
									 }


								 }
							 );

							 if (this.Editor.LevelReference == null)
							 {
								 this.Sync_LoadLevel(
									 this.Editor.PortIdentity, 0, ""
								 );
							 }
						 }
					 );


				 };

			this.Editor.Loaded +=
				delegate
				{
					this.Editor.View.TouchOverlay.MouseMove +=
						(_sender, _args) =>
						{
							var p = _args.GetPosition(this.Editor.View.TouchOverlay);

							if (this.Sync_RemoteOnly_MouseMove != null)
								this.Sync_RemoteOnly_MouseMove(this.Editor.PortIdentity, p.X, p.Y);
						};
				};


			#region LoadWindow
			this.Editor.LoadWindow.Click +=
				NextLevelForEditor =>
				{
					this.Editor.LoadWindow.Hide();

					// send early warning
					if (this.Sync_RemoteOnly_LoadLevelHint != null)
						this.Sync_RemoteOnly_LoadLevelHint(this.Editor.PortIdentity);

					this.Editor.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							// remove all locals from all ports
							this.LocalIdentity.Locals.ForEach(
								k => this.Sync_TeleportTo(this.LocalIdentity.Locals, 0, k.IdentityLocal, 0, 0, 0, 0)
							);

							this.Editor.WhenLoadedQueue.Enqueue(
								delegate
								{
									// we need to bring back all the players!

									foreach (var p in this.LocalIdentity.Locals)
									{
										var t = this.Editor.GetRandomEntrypoint((x, y) => new { x, y });

										this.Sync_TeleportTo(
											this.LocalIdentity.Locals,
											this.Editor.PortIdentity,
											p.IdentityLocal, t.x, t.y, 0, 0
										);
									}

								}
							);

							this.Sync_LoadLevel(
								this.Editor.PortIdentity, NextLevelForEditor.Location.Embedded.AnimationFrame, ""
							);
						}
					);

				};
			#endregion

		}


	}
}
