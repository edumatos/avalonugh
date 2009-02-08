using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AvalonUgh.Code;
using AvalonUgh.Code.Input;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.NetworkCode.Client.Shared
{

	using AvalonUgh.Code.Editor;
	using AvalonUgh.Code.GameWorkspace;

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

		/// <summary>
		/// state:String - Defines the state of the request using the values of the NonobaAPI's Public Constants
		/// success:Boolean - If true, the user bought the item.
		/// </summary>
		public Action<string, Action<string, bool>> ShowShop;

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



		public void InitializeEvents()
		{

			this.Content = new Workspace(
				new Workspace.ConstructorArguments
				{
					WindowPadding = WindowPadding,

					PortWidth = PortWidth,
					PortHeight = PortHeight,

					DefaultWidth = DefaultWidth,
					DefaultHeight = DefaultHeight,
				}
			).AttachContainerTo(this);

			Content.Console.WriteLine("binding to shop menu item");

			Content.Lobby.Menu.Shop +=
				delegate
				{
					Content.Console.WriteLine("invoking shop");

					if (this.ShowShop != null)
						this.ShowShop("saveten",
							delegate
							{
								// and reaction to the shop activity
								Content.Console.WriteLine("invoking shop done");
							}
						);
				};

			Content.Console.WriteLine("InitializeEvents");

			var Server_Hello_UserSynced = new BindingList<PlayerIdentity>();
			var Server_LoadLevel = new BindingList<string>();

			this.Events.Server_Message +=
				e =>
				{
					this.Content.Console.WriteLine("server: " + e.text);
				};

			this.Events.Server_Hello +=
				e =>
				{
					// yay, the server tells me my name. lets atleast remember it.
					this.Content.LocalIdentity.NetworkNumber = e.user;
					this.Content.LocalIdentity.Name = e.name;
					this.Content.LocalIdentity.SyncFrameWindow = e.framelimit;

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
									//this.Content.LocalIdentity.SyncFramePaused = false;
								}
							}
						);
					}

					// fixme: server says there are no slots for us to save to!
					Server_LoadLevel.ForEachNewOrExistingItem(
						IncomingData =>
						{
							var NewSlot = new LevelReference();

							this.Content.SavedLevels.Add(
								NewSlot
							);

							if (!string.IsNullOrEmpty(IncomingData))
							{
								// this slot has value
								NewSlot.Data = IncomingData;
							}

							if (Server_LoadLevel.Count == e.levels)
							{
								// all saved levels have been loaded

								this.Content.SavedLevels.ForEachNewOrExistingItem(
									level =>
									{
										// from an empty shell to a level with data
										// it must have been saved by level editor
										level.DataFuture.Continue(
											data =>
											{
												var index = this.Content.SavedLevels.IndexOf(level);

												this.Content.Console.WriteLine("save: " + new { index, data.Length });

												this.Messages.Server_LoadLevel(
													index,
													data
												);
											}
										);
									}
								);
							}
						}
					);
				};

			this.Events.Server_LoadLevel +=
				e =>
				{
					Content.Console.WriteLine("Server_LoadLevel " + e.index + " length " + e.data.Length);

					Server_LoadLevel.Add(e.data);
				};


			#region Server_UserJoined
			this.Events.Server_UserJoined +=
				e =>
				{
					//Content.Console.WriteLine("Server_UserJoined " + new { e, this.Content.LocalIdentity.SyncFrame });
					var EgoIsPrimate = this.Content.AllPlayers.Min(k => k.NetworkNumber) == this.Content.LocalIdentity.NetworkNumber;

					this.Messages.UserHello(
						e.user,
						this.Content.LocalIdentity.Name,
						this.Content.LocalIdentity.SyncFrame
					);

					var LowestSyncFrame = this.Content.LocalIdentity.SyncFrame;

					if (this.Content.CoPlayers.Any())
						LowestSyncFrame = this.Content.CoPlayers.Min(k => k.SyncFrame) - this.Content.LocalIdentity.SyncFrameWindow;

					this.Content.CoPlayers.Add(
						new PlayerIdentity
						{
							Name = e.name,
							NetworkNumber = e.user,
							// that new client is paused
							// we need to run out of frames in order to pause correctly
							SyncFrame = LowestSyncFrame
						}
					);



					// the new player needs to be synced
					// lets pause for now to figure out how to do that

					var NextSyncFrameLimit = this.Content.CoPlayers.Min(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
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


							#region EgoIsPrimate
							if (EgoIsPrimate)
							{
								Action<Workspace.Port> ReplicatePort =
									(Port) =>
									{
										if (Port.LevelReference == null)
											return;


										// ports are always dirty
										// because of the passengers

										this.Messages.UserLoadLevel(
											e.user,
											Port.PortIdentity,
											this.Content.LocalIdentity.SyncFrame,
											-1,
											Port.Level.ToString(Level.ToStringMode.ForSync)
										);
									};

								ReplicatePort(this.Content.PrimaryMission);
								ReplicatePort(this.Content.Editor);

							}
							#endregion


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

					this.Content.CoPlayers.Add(
						new PlayerIdentity
						{
							Name = e.name,
							NetworkNumber = e.user,
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

					this.Content.CoPlayers.Remove(c);

					// if we are again alone on the server
					// and we are not in sync 
					// we can just proceed as we do not need to sync
					if (this.Content.CoPlayers.Count == 0)
					{
						this.Content.LocalIdentity.SyncFramePaused = false;
						this.Content.LocalIdentity.SyncFrameLimit = 0;
					}
					else
					{
						this.Content.LocalIdentity.SyncFrameLimit = this.Content.CoPlayers.Min(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
					}
				};

			//var Checksum_Local = new List<PlayerIdentity.ChecksumHistoryItem>();
			//var Checksum_Remote = new List<PlayerIdentity.ChecksumHistoryItem>();

			//Action Checksum_DetectDesync =
			//    delegate
			//    {
			//        var Checksum_Local_Delete = Checksum_Local.Where(k => k.SyncFrame < (this.Content.LocalIdentity.SyncFrame - this.Content.LocalIdentity.SyncFrameWindow)).ToArray();
			//        var Checksum_Remote_Delete = Checksum_Remote.Where(k => k.SyncFrame < (this.Content.LocalIdentity.SyncFrame - this.Content.LocalIdentity.SyncFrameWindow)).ToArray();

			//        Checksum_Local_Delete.ForEach(k => Checksum_Local.Remove(k));
			//        Checksum_Remote_Delete.ForEach(k => Checksum_Remote.Remove(k));

			//        if (Checksum_Remote.Count > 0)
			//            if (Checksum_Local.Count > 0)
			//            {
			//                // we got something to compare to

			//            }
			//    };

			#region broadcast current frame
			this.Content.LocalIdentity.SyncFrameChanged +=
				delegate
				{
					var c = this.Content.InternalChecksumHistory.LastOrDefault();
					var crc = 0;

					if (c != null)
						crc = c.Checksum;

					this.Messages.SyncFrame(
						this.Content.LocalIdentity.SyncFrame,
						0,
						crc
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

					if (c.SyncFrame + 1 == e.frame)
					{
						c.SyncFrame = e.frame;

						var NextChecksumItem = new Workspace.ChecksumItem
						{
							Checksum = e.crc,
							NetworkNumber = e.user,
							SyncFrame = e.frame
						};

						this.Content.ExternalChecksumHistory.Enqueue(NextChecksumItem);

						if (this.Content.ExternalChecksumHistory.Count > this.Content.LocalIdentity.SyncFrameWindow)
							this.Content.ExternalChecksumHistory.Dequeue();
					}
					else
					{
						c.SyncFrame = e.frame;
					}


					// if we are paused we will not try to recalculate our new limit
					var NextSyncFrameLimit = this.Content.CoPlayers.Min(k => k.SyncFrame) + this.Content.LocalIdentity.SyncFrameWindow;
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

							if (p == null)
							{
								this.Content.Console.WriteLine("error: UserKeyStateChanged desync " + e);

								return;
							}
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

			#region UserMouseMove
			this.Content.Sync_RemoteOnly_MouseMove =
				(int port, double x, double y) =>
				{
					this.Messages.MouseMove(port, x, y);
				};

			this.Events.UserMouseMove +=
				e =>
				{
					var c = this[e];

					// e.port could be used to select a specific editor window
					// for now we ignore it

					var a = this.Content.Editor.Arrows.SingleOrDefault(k => k.Identity == c);

					if (a == null)
					{
						a = new Workspace.EditorPort.Arrow
						{
							Identity = c
						};

						this.Content.Editor.Arrows.Add(a);

						this.Content.CoPlayers.ForEachItemDeleted(
							DeletedIdentity =>
							{
								if (DeletedIdentity == c)
									this.Content.Editor.Arrows.Remove(a);
							}
						);
					}

					a.AnimatedMoveTo(e.x, e.y);
				};
			#endregion

			#region UserLoadLevelHint
			this.Content.Sync_RemoteOnly_LoadLevelHint =
				(int port) =>
				{
					this.Messages.LoadLevelHint(port);
				};

			this.Events.UserLoadLevelHint +=
				e =>
				{
					if (this.Content.CurrentPort.PortIdentity == e.port)
						this.Content.CurrentPort.Window.ColorOverlay.Opacity = 1;
					else
					{
						// remote player is going to reload 
						this.Content.BackgroundLoading.Show();
					}
				};
			#endregion


			#region Sync_LoadLevel
			var Sync_LoadLevel = this.Content.Sync_LoadLevel;

			this.Events.UserLoadLevel +=
				e =>
				{
					var c = this[e.user];

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							Sync_LoadLevel(e.port, e.level, e.custom);
						},
						delegate
						{
							this.Content.Console.WriteLine("UserTeleportTo desync " + e);
						}
					);
				};

			this.Content.Sync_LoadLevel =
				(int port, int level, string custom) =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							// do a local teleport in the future
							Sync_LoadLevel(port, level, custom);
						}
					);

					this.Messages.LoadLevel(FutureFrame, port, level, custom);
				};
			#endregion

			#region Sync_EditorSelector
			var Sync_EditorSelector = this.Content.Sync_EditorSelector;

			this.Content.Sync_EditorSelector =
				(int port, int type, int size, int x, int y) =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							// do a local teleport in the future
							Sync_EditorSelector(port, type, size, x, y);
						}
					);

					this.Messages.EditorSelector(FutureFrame, port, type, size, x, y);
				};

			this.Events.UserEditorSelector +=
				e =>
				{
					var c = this[e.user];

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							Sync_EditorSelector(e.port, e.type, e.size, e.x, e.y);
						},
						delegate
						{
							this.Content.Console.WriteLine("UserEditorSelector desync " + e);
						}
					);
				};
			#endregion


			#region networked Sync_TeleportTo
			// save the local implementation
			var Sync_TeleportTo = this.Content.Sync_TeleportTo;

			this.Events.UserTeleportTo +=
				e =>
				{
					var c = this[e.user];

					if (c == null)
					{
						this.Content.Console.WriteLine("timetravel: havent met " + e.user + " yet");
						return;
					}

					this.Content.LocalIdentity.HandleFrame(e.frame,
						delegate
						{
							Sync_TeleportTo(e.user, e.port, e.local, e.x, e.y, e.vx, e.vy);
						},
						delegate
						{
							this.Content.Console.WriteLine("UserTeleportTo desync " + e);
						}
					);
				};

			this.Content.Sync_TeleportTo =
				(int user, int port, int local, double x, double y, double vx, double vy) =>
				{
					var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
						delegate
						{
							// do a local teleport in the future
							Sync_TeleportTo(user, port, local, x, y, vx, vy);
						}
					);

					// we cannot teleoprt other player locals
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

			#region Sync_MissionStartHint
			{
				var Sync_MissionStartHint = this.Content.Sync_MissionStartHint;

				this.Content.Sync_MissionStartHint =
					(int user, int difficulty) =>
					{
						var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
							delegate
							{
								Sync_MissionStartHint(user, difficulty);
							}
						);

						this.Messages.MissionStartHint(FutureFrame, difficulty);
					};

				this.Events.UserMissionStartHint +=
					e =>
					{
						var c = this[e.user];

						this.Content.LocalIdentity.HandleFrame(e.frame,
							delegate
							{
								Sync_MissionStartHint(c.NetworkNumber, e.difficulty);
							}
						);
					};
			}
			#endregion


			#region Sync_Vehicalize
			{
				var Sync_Vehicalize = this.Content.Sync_Vehicalize;

				this.Content.Sync_Vehicalize =
					(int user, int local) =>
					{
						var FutureFrame = this.Content.LocalIdentity.HandleFutureFrame(
							delegate
							{
								Sync_Vehicalize(user, local);
							}
						);

						this.Messages.Vehicalize(FutureFrame, local);
					};

				this.Events.UserVehicalize +=
					e =>
					{
						var c = this[e.user];

						this.Content.LocalIdentity.HandleFrame(e.frame,
							delegate
							{
								Sync_Vehicalize(c.NetworkNumber, e.local);
							}
						);
					};
			}
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



		}

		public PlayerIdentity this[int user]
		{
			get
			{
				return this.Content.CoPlayers.FirstOrDefault(k => user == k.NetworkNumber);
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
