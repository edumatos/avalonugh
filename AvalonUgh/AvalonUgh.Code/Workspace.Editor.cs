using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
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

			public EditorPort(ConstructorArguments args)
			{
				this.Toolbar = new EditorToolbar(args.Selectors);
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

				this.Loaded +=
					delegate
					{
						this.View.EditorSelectorNextSize += () => this.Toolbar.EditorSelectorNextSize();
						this.View.EditorSelectorPreviousSize += () => this.Toolbar.EditorSelectorPreviousSize();
					};





			}
		}


		private void InitializeMenuEditorButton()
		{
			Lobby.Menu.Editor +=
				 delegate
				 {
					 // maybe send others a pre loading message too?

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

									 this.LocalIdentity.Locals.ForEach(
										k => this.Sync_TeleportTo(
											this.LocalIdentity.Locals, 
											this.Editor.PortIdentity, 
											k.IdentityLocal, 100, 100, 0, 0)
									 );
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

			this.Editor.LoadWindow.Click +=
				NextLevelForEditor =>
				{
					this.Editor.LoadWindow.Hide();

					// send early warning
					this.Editor.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							//this.LevelReference = NextLevelForEditor;

							this.Editor.WhenLoadedQueue.Enqueue(
								delegate
								{
									// we need to bring back all the players!

									this.Editor.Window.ColorOverlay.Opacity = 0;
								}
							);

							this.Sync_LoadLevel(
								this.Editor.PortIdentity, NextLevelForEditor.Location.Embedded.AnimationFrame, ""
							);
						}
					);

				};
		}


	}
}
