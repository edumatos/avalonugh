using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Input;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		public readonly KeyboardInput[] SupportedKeyboardInputs;


		void InitializeKeyboardFocus()
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
					if (HandleKeyUpDisabled)
						return;

					this.Container.Focus();
				};

			Action<UIElement> TextBoxDisableHandleKeyUp =
				e =>
				{
					e.GotFocus +=
						delegate
						{
							HandleKeyUpDisabled = true;
							this.SupportedKeyboardInputs.ForEach(k => k.Disabled = true);
						};

					e.LostFocus +=
						delegate
						{
							HandleKeyUpDisabled = false;
							this.SupportedKeyboardInputs.ForEach(k => k.Disabled = false);
						};
				};

			TextBoxDisableHandleKeyUp.AsParamsAction()(
				this.Editor.SaveWindow.PropertyText.Value,
				this.Editor.SaveWindow.PropertyCode.Value,
				this.Editor.SaveWindow.PropertyNextLevelCode.Value,
				this.Editor.SaveWindow.LevelScriptTextBox
			);



			this.Lobby.Menu.EnteringPasswordChanged +=
				delegate
				{
					this.SupportedKeyboardInputs.ForEach(k => k.Disabled = Lobby.Menu.EnteringPassword != null);
				};

			this.Container.KeyUp += HandleKeyUp;
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

		bool HandleKeyUpDisabled;

		void HandleKeyUp(object sender, KeyEventArgs args)
		{
			if (HandleKeyUpDisabled)
				return;


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

			if (args.Key == Key.F)
			{
				this.Arguments.ToFullscreen();
			}

			if (args.Key == Key.M)
			{
				this.Audio_Music.Enabled = !this.Audio_Music.Enabled;
			}

			if (args.Key == Key.C)
			{
				// this is a cheat
				this.LocalIdentity.Locals.ForEach(
					k =>
					{
						k.XActor.VelocityY = -12;
					}
				);
			}

			if (args.Key == Key.R)
			{
				// this is a cheat
				// reload current port
				this.CurrentPort.XLevelReference = this.CurrentPort.XLevelReference;
			}

			if (args.Key == Key.V)
			{
				if (this.PrimaryMission.Level != null)
					this.PrimaryMission.Level.AttributeHeadCount.Value = 0;
			}

			if (args.Key == Key.P)
			{
				this.Sync_SetPause(true, "you");
			}

			if (args.Key == Key.N)
			{
				this.Ports.Select(k => k.Window.ColorOverlay).Where(k => k.Opacity == 1).ForEach(k => k.Opacity = 0.5);

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

			if (args.Key == Key.Enter)
			{
				// if are we at the lobby and we are pressing
				// enter we will want the game to begin
				if (this.CurrentPort == this.Lobby)
				{
					this.Lobby.Menu.RaisePlay();
				}
			}


			if (args.Key == Key.E)
			{
				// if are we at the lobby and we are pressing
				// enter we will want the game to begin
				if (this.CurrentPort == this.Lobby)
				{
					this.Lobby.Menu.RaiseEditor();
				}
			}


			if (args.Key == Key.Escape)
			{
				args.Handled = true;

				// if we are inside a mission, submission or editor this will bring us back

				//this.Ports.ForEach(k => k.Visible = k.PortIdentity == PortIdentity_Lobby);

				Action GoToLobby =
					delegate
					{
						this.Lobby.Window.ColorOverlay.Opacity = 1;
						this.CurrentPort.Window.ColorOverlay.SetOpacity(1,
							delegate
							{
								// entering lobby

								// remove all locals from all ports
								this.LocalIdentity.Locals.ForEach(
									k =>
									{
										var p = this.Lobby.GetRandomEntrypoint((x, y) => new { x, y });

										this.Sync_TeleportTo(
											this.LocalIdentity.NetworkNumber,
											this.Lobby.PortIdentity,
											k.IdentityLocal,
											p.x,
											p.y,
											0, 0
										);
									}
								);

								this.CurrentPort = this.Lobby;
								Audio_Music.Loop = (AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3");


								this.Lobby.Window.BringContainerToFront();
								this.Lobby.Window.ColorOverlay.Opacity = 0;
							}
						);
					};

				// if all players quit the game
				// we would be able to start another level
				if (this.CurrentPort == this.Editor)
				{
					GoToLobby();
				}
				else if (this.CurrentPort == this.PrimaryMission)
				{
					// looks like we bailed an active game, it will continue until someone is in there
					this.Lobby.Menu.PlayText = "watch others play";

					this.PrimaryMission.Window.ColorOverlay.SetOpacity(1,
						delegate
						{
							this.PrimaryMission.Fail.Show();
							(KnownAssets.Path.Audio + "/gameover.mp3").PlaySound();

							this.PrimaryMission.Window.ColorOverlay.SetOpacity(0, GoToLobby);
						}
					);

				}
			}

		}
	}
}
