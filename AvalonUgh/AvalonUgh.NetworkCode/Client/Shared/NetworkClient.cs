using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Code;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Input;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.ComponentModel;

namespace AvalonUgh.NetworkCode.Client.Shared
{

	using TargetCanvas = Workspace;
	using AvalonUgh.Code.Editor;

	[Script]
	public class NetworkClient : VirtualClient, ISupportsContainer
	{
		// this code is shared between
		// javascript, actionscript, c#

		// read about network programming:
		// http://trac.bookofhook.com/bookofhook/trac.cgi/wiki/GameDesign
		// http://www.jakeworld.org/JakeWorld/main.php?main=Articles/Networking%20for%20Games%20101.php
		// http://trac.bookofhook.com/bookofhook/trac.cgi/wiki/Quake3Networking
		// http://trac.bookofhook.com/bookofhook/trac.cgi/wiki/IntroductionToMultiplayerGameProgramming

		public const int DefaultWidth = TargetCanvas.DefaultWidth;
		public const int DefaultHeight = TargetCanvas.DefaultHeight;
		public const int Zoom = TargetCanvas.Zoom;

		public Canvas Container { get; set; }

		public TargetCanvas Content;

		public NetworkClient()
		{
			this.Container = new Canvas
			{
				Background = Brushes.Black,
				Width = DefaultWidth,
				Height = DefaultHeight
			};



		}

		public void Connect()
		{
			Content.Console.WriteLine("Connect");

			IsConnected = true;
		}

		public bool IsConnected;

		public void Disconnect()
		{
			Content.Console.WriteLine("Disconnect");

			IsConnected = false;
		}

		public BindingList<PlayerIdentity> CoPlayers;

		public void InitializeEvents()
		{
			#region CoPlayers

			// coplayers are remoted locals
			this.CoPlayers = new BindingList<PlayerIdentity>();
			this.CoPlayers.ForEachNewItem(
				c =>
				{
					c.Locals.ForEachNewItem(
						p =>
						{
							this.Content.Players.Add(p);

							// at this point we should add listeners to events specific to
							// this remote local

							//this.Content.Console.WriteLine("attaching to remote local events " + p);



						}
					);

					c.Locals.ForEachItemDeleted(p => this.Content.Players.Remove(p));


				}
			);

			this.CoPlayers.ForEachItemDeleted(
				c =>
				{
					this.Content.Console.WriteLine("removing all locals for " + c);


					while (c.Locals.Count > 0)
						c.Locals.RemoveAt(0);
				}
			);

			#endregion

			Content = new TargetCanvas().AttachTo(this);
			Content.Console.WriteLine("InitializeEvents");

			this.Events.Server_Hello +=
				e =>
				{
					// yay, the server tells me my name. lets atleast remember it.
					this.Content.LocalIdentity.Number = e.user;
					this.Content.LocalIdentity.Name = e.name;

					Content.Console.WriteLine("Server_Hello " + e);

					// we have joined the server
					// now we need to sync up the frames
					// if we are alone in the server we do not sync yet
					if (e.others > 0)
					{
						// lets pause first
						//this.Content.LocalIdentity.SyncFramePaused = true;
					}
				};

			this.Events.Server_UserJoined +=
				e =>
				{
					Content.Console.WriteLine("Server_UserJoined " + e);

					this.Messages.UserHello(
						e.user,
						this.Content.LocalIdentity.Name,
						this.Content.LocalIdentity.SyncFrame
					);

					this.CoPlayers.Add(
						new PlayerIdentity { Name = e.name, Number = e.user }
					);

					// there is some catching up to do
					// like we need to tell it about our locals

					foreach (var p in this.Content.LocalIdentity.Locals)
					{
						this.Messages.UserLocalPlayers_Increase(e.user, 0);
						this.Messages.UserTeleportTo(e.user,
							p.IdentityLocal,
							p.Actor.X,
							p.Actor.Y,
							p.Actor.VelocityX,
							p.Actor.VelocityY
						);
					}

					// the new player needs to be synced
					// lets pause for now to figure out how to do that

					//this.Content.LocalIdentity.SyncFramePaused = true;
				};



			this.Events.Server_UserLeft +=
				e =>
				{
					var c = this[e.user];

					Content.Console.WriteLine("Server_UserLeft " + e + " - " + c);

					this.CoPlayers.Remove(c);

					// if we are again alone on the server
					// and we are not in sync 
					// we can just proceed as we do not need to sync
					if (this.CoPlayers.Count == 0)
					{
						this.Content.LocalIdentity.SyncFramePaused = false;
						this.Content.LocalIdentity.SyncFrameLimit = 0;
					}
					else
					{
						this.Content.LocalIdentity.SyncFrameLimit = this.CoPlayers.Max(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
					}
				};

			this.Events.UserHello +=
				e =>
				{
					Content.Console.WriteLine("UserHello " + e);

					this.CoPlayers.Add(
						new PlayerIdentity
						{
							Name = e.name,
							Number = e.user,
							SyncFrame = e.frame
						}
					);

					this.Content.LocalIdentity.SyncFrame = this.Content.LocalIdentity.SyncFrame.Max(e.frame);
					this.Content.LocalIdentity.SyncFrameLimit = this.CoPlayers.Max(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
				};


			(1000).AtInterval(
				delegate
				{
					// we are on our own
					if (this.CoPlayers.Count == 0)
						return;

					var AvgSyncFrame = this.CoPlayers.Average(k => k.SyncFrame);
					var AvgSyncFrameLatency = this.CoPlayers.Average(k => k.SyncFrameLatency);
					var AvgSyncFrameDelta = AvgSyncFrame - this.Content.LocalIdentity.SyncFrame;

					this.Content.Console.WriteLine(
							new
							{
								avgframe = AvgSyncFrame,
								frame = this.Content.LocalIdentity.SyncFrame,
								framerate = this.Content.LocalIdentity.SyncFrameRate,
								avglag = AvgSyncFrameLatency,
								delta = AvgSyncFrameDelta
							}
						);
				}
			);

			this.Content.LocalIdentity.SyncFrameChanged +=
				delegate
				{
					this.Messages.SyncFrame(
						this.Content.LocalIdentity.SyncFrame,
						this.Content.LocalIdentity.SyncFrameRate
					);
				};

			this.Events.UserSyncFrameEcho +=
				e =>
				{
					var c = this[e];

					if (c == null)
						return;

					c.SyncFrameLatency = this.Content.LocalIdentity.SyncFrame - e.frame;
				};

			this.Events.UserSyncFrame +=
				e =>
				{
					var c = this[e];

					if (c == null)
					{
						// we do not know yet about this user
						return;
					}

					c.SyncFrame = e.frame;
					c.SyncFrameRate = e.framerate;

					this.Content.LocalIdentity.SyncFrameLimit = this.CoPlayers.Max(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;

					// lets send the same data back to calculate lag
					this.Messages.UserSyncFrameEcho(e.user, e.frame, e.framerate);
				};



			this.Events.UserKeyStateChanged +=
				e =>
				{
					var p = this[e][e.local];

					this.Content.Console.WriteLine("UserKeyStateChanged " + e);


					// if the remote frame is less than here
					// then we are in the future
					// otherwise they are in the future
					var key = p.Input.Keyboard.FromDefaultTranslation((Key)e.key);
					var state = Convert.ToBoolean(e.state);

					if (p.KeyState_Sequence > 0)
					{
						if (p.KeyState_Sequence + 1 != e.sequence)
							this.Content.Console.WriteLine("error: KeyState_Sequence " + new { p.KeyState_Sequence, e.sequence });
					}

					p.KeyState_Sequence = e.sequence;


					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							p.Input.Keyboard.KeyState[key] = state;
						},
						delegate
						{
							this.Content.Console.WriteLine("UserKeyStateChanged desync " + e);
						}
					);



				};

			this.Events.UserTeleportTo +=
				e =>
				{
					var p = this[e][e.local];

					Content.Console.WriteLine("UserTeleportTo " + e + " - " + p);

					if (p.Actor.CurrentVehicle == null)
					{
						p.Actor.MoveTo(e.x, e.y);
						p.Actor.VelocityX = e.vx;
						p.Actor.VelocityY = e.vy;
					}
					else
					{
						p.Actor.CurrentVehicle.MoveTo(e.x, e.y);
						p.Actor.CurrentVehicle.VelocityX = e.vx;
						p.Actor.CurrentVehicle.VelocityY = e.vy;
					}
				};

			this.Events.UserVehicle_TeleportTo +=
				e =>
				{
					Content.Console.WriteLine("UserVehicle_TeleportTo " + e);

					var v = this.Content.View.Level.KnownVehicles.Where(k => k.CurrentDriver == null).AtModulus(e.index);

					v.MoveTo(e.x, e.y);
					v.VelocityX = e.vx;
					v.VelocityY = e.vy;
				};


			#region sync Locals
			this.Events.UserLocalPlayers_Decrease +=
				e =>
				{
					var c = this[e];

					Content.Console.WriteLine("UserLocalPlayers_Decrease " + e + " " + c);

					c.Locals.Remove(c.Locals.Last());
				};

			this.Events.UserLocalPlayers_Increase +=
				e =>
				{
					var c = this[e];

					Content.Console.WriteLine("UserLocalPlayers_Increase " + e + " " + c);

					var p = new PlayerInfo
					{
						Identity = c,
						// remote users get a dummy remoted input with the same keys
						Input =
							new PlayerInput
							{
								Keyboard = new KeyboardInput(
									new KeyboardInput.Arguments.Arrows
									{
										// there wont be any device to listen to tho
									}
								),
								Touch = null
							}

					};

					c.Locals.Add(p);


				};


		
			this.Content.LocalIdentity.Locals.ForEachNewOrExistingItem(
				Local =>
				{
					this.Messages.LocalPlayers_Increase(0);
					this.Messages.TeleportTo(Local.IdentityLocal,
						Local.Actor.X,
						Local.Actor.Y,
						Local.Actor.VelocityX,
						Local.Actor.VelocityY
					);

					// ... while member apply this rule

					var ConnectedKeyboard = Local.Input.Keyboard;
					var LatencyKeyboard = new KeyboardInput(new KeyboardInput.Arguments.Arrows());

					Local.Input.Keyboard = LatencyKeyboard;

					// sending local ingame player keystates
					Action<Key, bool> Local_KeyStateChanged =
						(key, state) =>
						{
							var FutureFrame = this.Content.LocalIdentity.SyncFrame + this.Content.LocalIdentity.SyncFrameWindow;

							this.Content.LocalIdentity.HandleFrame(FutureFrame,
								delegate
								{
									LatencyKeyboard.KeyState[
										LatencyKeyboard.FromDefaultTranslation(
											ConnectedKeyboard.ToDefaultTranslation(key)
										)
									] = state;
								},
								delegate
								{
									// can we be desynced?
								}
							);

							Local.KeyState_Sequence++;

							this.Messages.KeyStateChanged(
								Local.IdentityLocal,
								FutureFrame,
								Local.KeyState_Sequence,
								(int)ConnectedKeyboard.ToDefaultTranslation(key),
								Convert.ToInt32(state)
							);
						};


					this.Content.Console.WriteLine("event add KeyStateChanged " + Local);
					ConnectedKeyboard.KeyStateChanged += Local_KeyStateChanged;

					// when do we want to stop broadcasting our key changes?
					// maybe when we remove that local player

					this.Content.LocalIdentity.Locals.ForEachItemDeleted(
						(DeletedLocal, Dispose) =>
						{
							if (DeletedLocal != Local)
								return;

							this.Content.Console.WriteLine("event remove KeyStateChanged " + Local);
							ConnectedKeyboard.KeyStateChanged -= Local_KeyStateChanged;

							DeletedLocal.Input.Keyboard = ConnectedKeyboard;

							// we should not listen to that event anymore
							Dispose();
						}
					);
				}
			);

			this.Content.LocalIdentity.Locals.ForEachItemDeleted(
				Local => this.Messages.LocalPlayers_Decrease(0)
			);

			#endregion

			#region EditorSelectorApplied

			this.Content.View.EditorSelectorDisabled = true;

			this.Content.View.EditorSelectorApplied +=
				(Selector, Position) =>
				{
					var FutureFrame = this.Content.LocalIdentity.SyncFrame + this.Content.LocalIdentity.SyncFrameWindow;

					this.Content.LocalIdentity.HandleFrame(FutureFrame,
						delegate
						{
							Selector.CreateTo(this.Content.View.Level, Position);
						}
					);

					var Index = KnownSelectors.Index.Of(Selector, this.Content.Selectors);

					// unknown selector
					if (Index.Type == -1)
						return;

					this.Messages.EditorSelector(FutureFrame, Index.Type, Index.Size, Position.ContentX, Position.ContentY);
				};

			this.Events.UserEditorSelector +=
				e =>
				{
					Content.Console.WriteLine("UserEditorSelector " + e);

					this.Content.LocalIdentity.HandleFrame(
						e.frame,
						delegate
						{
							var Selector = this.Content.Selectors.Types[e.type].Sizes[e.size];
							var Position = new View.SelectorPosition { ContentX = e.x, ContentY = e.y };

							Selector.CreateTo(this.Content.View.Level, Position);
						},
						delegate
						{
							this.Content.Console.WriteLine("error: desync!");
						}
					);
				};
			#endregion

			#region SetShakerEnabled
			// we are overriding default behaviour
			// as we need to act upon events in the future
			this.Content.SetShakerEnabled =
				value =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							this.Content.View.IsShakerEnabled = value;
						}
					);

					this.Messages.SetShakerEnabled(FutureFrame, Convert.ToInt32(value));
				};

			this.Events.UserSetShakerEnabled +=
				e =>
				{
					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							this.Content.View.IsShakerEnabled = Convert.ToBoolean(e.value);
						}
					);
				};
			#endregion


			#region pause
			var SetPause = this.Content.SetPause;
			this.Content.SetPause =
				(IsPaused, ByWhom) =>
				{
					if (IsPaused)
					{
						var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
							delegate
							{
								SetPause(true, ByWhom);
							}
						);

						this.Messages.SetPaused(FutureFrame);
					}
					else
					{
						SetPause(false, ByWhom);
						this.Messages.ClearPaused();
					}
				};

			this.Events.UserSetPaused +=
				e =>
				{
					var c = this[e];

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							SetPause(true, c.Name);
						}
					);
				};

			this.Events.UserClearPaused +=
				e =>
				{
					var c = this[e];

					SetPause(false, c.Name);
				};
			#endregion


			var LoadEmbeddedLevel = this.Content.LoadEmbeddedLevel;
			this.Content.LoadEmbeddedLevel =
				LevelNumber =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							LoadEmbeddedLevel(LevelNumber);
						}
					);

					this.Messages.LoadEmbeddedLevel(FutureFrame, LevelNumber);
				};

			this.Events.UserLoadEmbeddedLevel +=
				e =>
				{
					LoadEmbeddedLevel(e.level);
				};

		}

		public PlayerIdentity this[int user]
		{
			get
			{
				return this.CoPlayers.FirstOrDefault(k => user == k.Number);
			}
		}

		public PlayerIdentity this[Communication.RemoteEvents.WithUserArguments u]
		{
			get
			{
				return this[u.user];
			}
		}


	}
}
