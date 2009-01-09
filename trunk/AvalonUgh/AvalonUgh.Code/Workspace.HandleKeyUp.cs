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
		public readonly KeyboardInput[] SupportedKeyboardInputs;

		void EnableKeyboardFocus()
		{
			// we are going for the keyboard input
			// we want to enable the tilde console feature
			this.Container.FocusVisualStyle = null;
			this.Container.Focusable = true;
			this.Container.Focus();

			// at this time we should add a local player
			this.Container.MouseLeftButtonDown +=
				(sender, key_args) =>
				{
					this.Container.Focus();
				};
		}

		public static bool IsUnpauseKey(Key k)
		{
			if (k == Key.P)
				return true;
			if (k == Key.Enter)
				return true;
			if (k == Key.Space)
				return true;
			if (k == Key.Escape)
				return true;

			return false;
		}

		void HandleKeyUp(object sender, KeyEventArgs args)
		{



			// oem7 will trigger the console
			if (args.Key == Key.Oem7)
			{
				args.Handled = true;

				Console.BringContainerToFront();

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

			// allow single frame step
			if (args.Key == Key.PageUp)
			{
				if (this.LocalIdentity.SyncFramePaused)
					this.LocalIdentity.SyncFramePausedSkip = true;
				else
					this.LocalIdentity.SyncFramePaused = true;

				this.Console.WriteLine(this.LocalIdentity);
			}

			// instant pause
			if (args.Key == Key.PageDown)
			{
				this.LocalIdentity.SyncFramePaused = !this.LocalIdentity.SyncFramePaused;

				this.Console.WriteLine(this.LocalIdentity);
			}


			if (this.LocalIdentity.SyncFramePaused)
			{
				// if the game is paused
				// we cannot handle a future frame and thus we need to unpause momentarily

				if (IsUnpauseKey(args.Key))
				{
					args.Handled = true;
					this.Sync_SetPause(false, "you");
				}


				return;
			}

			this.Lobby.Menu.HandleKeyUp(args);

			if (args.Handled)
				return;

			if (this.Lobby.Menu.EnteringPassword != null)
				return;


			if (args.Key == Key.M)
			{
				this.Music.Enabled = !this.Music.Enabled;
			}

			if (args.Key == Key.P)
			{
				this.Sync_SetPause(true, "you");
			}


			if (args.Key == Key.Insert)
			{
				// more locals!
				this.Lobby.Menu.Players++;
			}

			if (args.Key == Key.Delete)
			{
				// less locals!
				this.Lobby.Menu.Players--;
			}

			if (this.Selectors.DefaultKeyShortcut.ContainsKey(args.Key))
			{
				this.Editor.Toolbar.SelectorType = this.Selectors.DefaultKeyShortcut[args.Key]();
			}

			if (args.Key == Key.Escape)
			{
				args.Handled = true;

				// if we are inside a mission, submission or editor this will bring us back

				//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Lobby);

				// if all players quit the game
				// we would be able to start another level
				if (this.CurrentPort != this.Lobby)
				{
					// looks like we bailed an active game, it will continue until someone is in there
					if (this.PrimaryMission.Level != null)
						this.Lobby.Menu.PlayText = "resume";

					this.Lobby.Window.ColorOverlay.Opacity = 1;
					this.CurrentPort.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							// entering lobby

							// just jump randomly in
							foreach (var k in this.LocalIdentity.Locals)
							{
								k.Actor.MoveTo(
									(this.Lobby.View.ContentActualWidth / 4) +
									(this.Lobby.View.ContentActualWidth / 2).Random(),
									this.Lobby.View.ContentActualHeight / 2);

								k.Actor.CurrentLevel = this.Lobby.Level;
							}


							this.CurrentPort = this.Lobby;
							this.CurrentPort.Players.AddRange(this.LocalIdentity.Locals.ToArray());

							this.Lobby.Window.BringContainerToFront();
							this.Lobby.Window.ColorOverlay.Opacity = 0;
						}
					);
				}
			}

		}
	}
}
