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

	using AvalonUgh.Code.Editor;
	using System.Diagnostics;

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

		public const int DefaultFramerate = Workspace.DefaultFramerate;

		public const int PortWidth = 640;
		public const int PortHeight = 400;

#if DEBUG
		public const int WindowPadding = 4;
		public const int DefaultWidth = 700;
		public const int DefaultHeight = 500;
#else
		public const int WindowPadding = 0;
		public const int DefaultWidth = PortWidth;
		public const int DefaultHeight = PortHeight;
#endif

		public Canvas Container { get; set; }

		public Workspace Content;

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
					c.Locals.AttachTo(this.Content.Players);

				}
			);

			this.CoPlayers.ForEachItemDeleted(
				c =>
				{
					// if coplayers leave at different times it will cause desync
					this.Content.Console.WriteLine("removing all locals for " + c);


					while (c.Locals.Count > 0)
						c.Locals.RemoveAt(0);
				}
			);

			#endregion

			Content = new Workspace(
				new Workspace.ConstructorArguments
				{
					WindowPadding = WindowPadding,

					PortWidth = PortWidth,
					PortHeight = PortHeight,

					DefaultWidth = DefaultWidth,
					DefaultHeight = DefaultHeight,

					Paused = true
				}
			).AttachContainerTo(this);

			Content.Console.WriteLine("InitializeEvents");

			var Server_Hello_UserSynced = new BindingList<PlayerIdentity>();

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
					if (e.others == 0)
					{
						// we do not have to sync to others
						this.Content.LocalIdentity.SyncFramePaused = false;
					}
					else
					{
						Server_Hello_UserSynced.ForEachNewItem(
							delegate 
							{
								if (Server_Hello_UserSynced.Count == e.others)
								{
									// we know everybody now
									var NextSyncFrame = Server_Hello_UserSynced.Min(k => k.SyncFrame);

									this.Content.Console.WriteLine("synced and ready to unpause from frame " + NextSyncFrame);
									this.Content.LocalIdentity.SyncFrame = NextSyncFrame;

									// unpause
									this.Content.LocalIdentity.SyncFramePaused = false;
								}
							}
						);
					}
				};


			#region Server_UserJoined
			this.Events.Server_UserJoined +=
				e =>
				{
					//Content.Console.WriteLine("Server_UserJoined " + new { e, this.Content.LocalIdentity.SyncFrame });

					this.Messages.UserHello(
						e.user,
						this.Content.LocalIdentity.Name,
						this.Content.LocalIdentity.SyncFrame
					);

					var LowestSyncFrame = this.Content.LocalIdentity.SyncFrame;

					if (this.CoPlayers.Any())
						LowestSyncFrame = this.CoPlayers.Min(k => k.SyncFrame) - this.Content.LocalIdentity.SyncFrameWindow;

					this.CoPlayers.Add(
						new PlayerIdentity
						{
							Name = e.name,
							Number = e.user,
							// that new client is paused
							// we need to run out of frames in order to pause correctly
							SyncFrame = LowestSyncFrame
						}
					);



					// the new player needs to be synced
					// lets pause for now to figure out how to do that

					var NextSyncFrameLimit = this.CoPlayers.Min(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
					// we can only increase the limiter
					this.Content.LocalIdentity.SyncFrameLimit = this.Content.LocalIdentity.SyncFrameLimit.Max(NextSyncFrameLimit);


					if (this.Content.LocalIdentity.SyncFrame == this.Content.LocalIdentity.SyncFrameLimit)
					{
						Content.Console.WriteLine("will sync to new joined client at frame " + this.Content.LocalIdentity.SyncFrame);
					}
					else
					{
						Content.Console.WriteLine("new client joined at frame " + this.Content.LocalIdentity.SyncFrame);
						Content.Console.WriteLine("will sync to that client at future frame " + this.Content.LocalIdentity.SyncFrameLimit);

					}

					this.Content.LocalIdentity.HandleFrame(
						this.Content.LocalIdentity.SyncFrameLimit,
						delegate
						{
							// this event happens at a later timepoint
							// if someone joins after us
							// there is some catching up to do
							// like we need to tell it about our locals

							foreach (var Local in this.Content.LocalIdentity.Locals)
							{
								// we should pause the new joiner until
								// we have told him where all of our items are

								// !!! we need to run out of frames
								// enter a wait for this new joiner
								// send our positions
								// and resume...

								this.Messages.UserTeleportTo(e.user,
									this.Content.LocalIdentity.SyncFrame,

									Local.IdentityLocal,

									// which port are our local players in?
									this.Content.CurrentPort.PortIdentity,

									Local.Actor.X,
									Local.Actor.Y,
									Local.Actor.VelocityX,
									Local.Actor.VelocityY
								);

								// we need to sync user inputs too
								foreach (var k in Local.Input.Keyboard.KeyState)
								{
									this.Messages.UserKeyStateChanged(
										e.user,
										Local.IdentityLocal,
										this.Content.LocalIdentity.SyncFrame,
										(int)Local.Input.Keyboard.ToDefaultTranslation(k.Key),
										Convert.ToInt32(k.Value)
									);
								}
							}

							this.Messages.UserSynced(
								e.user,
								this.Content.LocalIdentity.SyncFrame
							);

							this.Content.Console.WriteLine("syncing to new client done at frame " +
								this.Content.LocalIdentity.SyncFrame + " with limiter " +
								this.Content.LocalIdentity.SyncFrameLimit);



						}
					);
				};
			#endregion

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

					if (!this.Content.LocalIdentity.SyncFramePaused)
					{
						this.Content.Console.WriteLine("error: got UserHello while unpaused");
					}

				};

			this.Events.UserSynced +=
				e =>
				{
					var c = this[e.user];

					Server_Hello_UserSynced.Add(c);
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
						this.Content.LocalIdentity.SyncFrameLimit = this.CoPlayers.Min(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
					}
				};


			//(1000).AtInterval(
			//    delegate
			//    {
			//        // we are on our own
			//        if (this.CoPlayers.Count == 0)
			//            return;

			//        var AvgSyncFrame = this.CoPlayers.Average(k => k.SyncFrame);
			//        var AvgSyncFrameLatency = this.CoPlayers.Average(k => k.SyncFrameLatency);
			//        var AvgSyncFrameDelta = AvgSyncFrame - this.Content.LocalIdentity.SyncFrame;

			//        this.Content.Console.WriteLine(
			//                new
			//                {
			//                    avgframe = AvgSyncFrame,
			//                    frame = this.Content.LocalIdentity.SyncFrame,
			//                    framerate = this.Content.LocalIdentity.SyncFrameRate,
			//                    avglag = AvgSyncFrameLatency,
			//                    delta = AvgSyncFrameDelta
			//                }
			//            );
			//    }
			//);

			#region broadcast current frame
			this.Content.LocalIdentity.SyncFrameChanged +=
				delegate
				{
					this.Messages.SyncFrame(
						this.Content.LocalIdentity.SyncFrame,
						0
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


					// if we are paused we will not try to recalculate our new limit
					var NextSyncFrameLimit = this.CoPlayers.Min(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
					// we can only increase the limiter
					this.Content.LocalIdentity.SyncFrameLimit = this.Content.LocalIdentity.SyncFrameLimit.Max(NextSyncFrameLimit);


					// lets send the same data back to calculate lag
					this.Messages.UserSyncFrameEcho(e.user, e.frame, 0);
				};
			#endregion



			this.Events.UserKeyStateChanged +=
				e =>
				{
					var c = this[e.user];

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							var p = c[e.local];

							// if the remote frame is less than here
							// then we are in the future
							// otherwise they are in the future
							var key = p.Input.Keyboard.FromDefaultTranslation((Key)e.key);
							var state = Convert.ToBoolean(e.state);



							p.Input.Keyboard.KeyState[key] = state;
						},
						delegate
						{
							this.Content.Console.WriteLine("UserKeyStateChanged desync " + e);
						}
					);
				};

			#region networked Sync_TeleportTo
			// save the local implementation
			var Sync_TeleportTo = this.Content.Sync_TeleportTo;

			this.Events.UserTeleportTo +=
				e =>
				{
					var c = this[e.user];

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							Sync_TeleportTo(c.Locals, e.port, e.local, e.x, e.y, e.vx, e.vy);
						},
						delegate
						{
							this.Content.Console.WriteLine("UserTeleportTo desync " + e);
						}
					);
				};

			this.Content.Sync_TeleportTo =
				(BindingList<PlayerInfo> a, int port, int local, double x, double y, double vx, double vy) =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							// do a local teleport in the future
							Sync_TeleportTo(a, port, local, x, y, vx, vy);
						}
					);

					this.Messages.TeleportTo(FutureFrame, local, port, x, y, vx, vy);
				};

			#endregion

			#region Sync_RemoveLocalPlayer
			var Sync_RemoveLocalPlayer = this.Content.Sync_RemoveLocalPlayer;

			this.Events.UserRemoveLocalPlayer +=
				e =>
				{
					var c = this[e.user];

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							Sync_RemoveLocalPlayer(c.Locals, e.local);
						},
						delegate
						{
							this.Content.Console.WriteLine("UserRemoveLocalPlayer desync " + e);
						}
					);
				};

			this.Content.Sync_RemoveLocalPlayer =
				(BindingList<PlayerInfo> a, int local) =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							// do a local teleport in the future
							Sync_RemoveLocalPlayer(a, local);
						}
					);

					this.Messages.RemoveLocalPlayer(FutureFrame, local);
				};
			#endregion


			this.Content.LocalIdentity.Locals.ForEachNewOrExistingItem(
				Local =>
				{
					// ... while member apply this rule

					var ConnectedKeyboard = Local.Input.Keyboard;
					var LatencyKeyboard = new KeyboardInput(new KeyboardInput.Arguments.Arrows());

					Local.Input.Keyboard = LatencyKeyboard;

					// sending local ingame player keystates
					Action<Key, bool> Local_KeyStateChanged =
						(key, state) =>
						{
							//var FutureFrame = this.Content.LocalIdentity.SyncFrame + this.Content.LocalIdentity.SyncFrameWindow;

							var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(0,
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


							this.Messages.KeyStateChanged(
								Local.IdentityLocal,
								FutureFrame,
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

							/// we should delete our copies on the net too...

							this.Content.Console.WriteLine("event remove KeyStateChanged " + Local);
							ConnectedKeyboard.KeyStateChanged -= Local_KeyStateChanged;

							DeletedLocal.Input.Keyboard = ConnectedKeyboard;

							// we should not listen to that event anymore
							Dispose();


						}
					);
				}
			);

			//this.Content.LocalIdentity.Locals.ForEachItemDeleted(
			//    Local => this.Messages.LocalPlayers_Decrease(0)
			//);

			//#endregion

			//#region EditorSelectorApplied

			//this.Content.View.EditorSelectorDisabled = true;

			//this.Content.View.EditorSelectorApplied +=
			//    (Selector, Position) =>
			//    {
			//        var FutureFrame = this.Content.LocalIdentity.SyncFrame + this.Content.LocalIdentity.SyncFrameWindow;

			//        this.Content.LocalIdentity.HandleFrame(FutureFrame,
			//            delegate
			//            {
			//                Selector.CreateTo(this.Content.View.Level, Position);
			//            }
			//        );

			//        var Index = KnownSelectors.Index.Of(Selector, this.Content.Selectors);

			//        // unknown selector
			//        if (Index.Type == -1)
			//            return;

			//        this.Messages.EditorSelector(FutureFrame, Index.Type, Index.Size, Position.ContentX, Position.ContentY);
			//    };

			//this.Events.UserEditorSelector +=
			//    e =>
			//    {
			//        Content.Console.WriteLine("UserEditorSelector " + e);

			//        this.Content.LocalIdentity.HandleFrame(
			//            e.frame,
			//            delegate
			//            {
			//                var Selector = this.Content.Selectors.Types[e.type].Sizes[e.size];
			//                var Position = new View.SelectorPosition { ContentX = e.x, ContentY = e.y };

			//                Selector.CreateTo(this.Content.View.Level, Position);
			//            },
			//            delegate
			//            {
			//                this.Content.Console.WriteLine("error: desync!");
			//            }
			//        );
			//    };
			//#endregion

			//#region SetShakerEnabled
			//// we are overriding default behaviour
			//// as we need to act upon events in the future
			//this.Content.SetShakerEnabled =
			//    value =>
			//    {
			//        var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
			//            delegate
			//            {
			//                this.Content.View.IsShakerEnabled = value;
			//            }
			//        );

			//        this.Messages.SetShakerEnabled(FutureFrame, Convert.ToInt32(value));
			//    };

			//this.Events.UserSetShakerEnabled +=
			//    e =>
			//    {
			//        this.Content.LocalIdentity.HandleFrame(e.frame,
			//            delegate
			//            {
			//                this.Content.View.IsShakerEnabled = Convert.ToBoolean(e.value);
			//            }
			//        );
			//    };
			//#endregion


			#region pause
			var SetPause = this.Content.Sync_SetPause;
			this.Content.Sync_SetPause =
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


			//var LoadEmbeddedLevel = this.Content.LoadEmbeddedLevel;
			//this.Content.LoadEmbeddedLevel =
			//    LevelNumber =>
			//    {
			//        var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
			//            delegate
			//            {
			//                LoadEmbeddedLevel(LevelNumber);
			//            }
			//        );

			//        this.Messages.LoadEmbeddedLevel(FutureFrame, LevelNumber);
			//    };

			//this.Events.UserLoadEmbeddedLevel +=
			//    e =>
			//    {
			//        LoadEmbeddedLevel(e.level);
			//    };

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
