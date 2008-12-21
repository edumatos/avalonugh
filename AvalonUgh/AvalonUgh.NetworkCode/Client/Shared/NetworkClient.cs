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
using AvalonUgh.Game.Shared;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.ComponentModel;

namespace AvalonUgh.NetworkCode.Client.Shared
{

	using TargetCanvas = AvalonUgh.Game.Shared.GameCanvas;
	using AvalonUgh.Code.Editor;

	[Script]
	public class NetworkClient : VirtualClient, ISupportsContainer
	{
		// this code is shared between
		// javascript, actionscript, c#

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
		}

		public void Disconnect()
		{
			Content.Console.WriteLine("Disconnect");

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
				};

			this.Events.Server_UserJoined +=
				e =>
				{
					Content.Console.WriteLine("Server_UserJoined " + e);

					this.Messages.UserHello(e.user, this.Content.LocalIdentity.Name);

					this.CoPlayers.Add(
						new PlayerIdentity { Name = e.name, Number = e.user }
					);

					// there is some catching up to do
					// like we need to tell it about our locals

					foreach (var p in this.Content.LocalIdentity.Locals)
					{
						this.Messages.UserLocalPlayers_Increase(e.user);
						this.Messages.UserTeleportTo(e.user,
							p.IdentityLocal,
							p.Actor.X,
							p.Actor.Y,
							p.Actor.VelocityX,
							p.Actor.VelocityY
						);
					}
				};



			this.Events.Server_UserLeft +=
				e =>
				{
					var c = this[e.user];

					Content.Console.WriteLine("Server_UserLeft " + e + " - " + c);

					this.CoPlayers.Remove(c);
				};

			this.Events.UserHello +=
				e =>
				{
					Content.Console.WriteLine("UserHello " + e);

					this.CoPlayers.Add(
						new PlayerIdentity { Name = e.name, Number = e.user }
					);

				};

			5000.AtInterval(
				delegate
				{
					// at every so often
					// we need syncing 
					// be it outside or inside a vehicle
					this.Content.Console.WriteLine("sending our position");

					foreach (var p in this.Content.LocalIdentity.Locals)
					{
						if (p.Actor.CurrentVehicle == null)
						{
							this.Messages.TeleportTo(
								p.IdentityLocal,
								p.Actor.X,
								p.Actor.Y,
								p.Actor.VelocityX,
								p.Actor.VelocityY
							);
						}
						else
						{
							this.Messages.TeleportTo(
								p.IdentityLocal,
								p.Actor.CurrentVehicle.X,
								p.Actor.CurrentVehicle.Y,
								p.Actor.CurrentVehicle.VelocityX,
								p.Actor.CurrentVehicle.VelocityY
							);
						}
					}

					this.Content.View.Level.KnownVehicles.Where(k => k.CurrentDriver == null).ForEach(
						(Vehicle, Index) =>
						{
							this.Messages.Vehicle_TeleportTo(
								Index,
								Vehicle.X,
								Vehicle.Y,
								Vehicle.VelocityX,
								Vehicle.VelocityY
							);
						}
					);


				}
			);

			this.Events.UserKeyStateChanged +=
				e =>
				{
					var p = this[e][e.local];

					this.Content.Console.WriteLine("UserKeyStateChanged " + e + " - " + p);

					p.Input.Keyboard.KeyState[p.Input.Keyboard.FromDefaultTranslation((Key)e.key)] = Convert.ToBoolean(e.state);
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
									new KeyboardInput.Arguments
									{
										Left = Key.Left,
										Right = Key.Right,
										Up = Key.Up,
										Down = Key.Down,
										Drop = Key.Space,
										Enter = Key.Enter,

										// there wont be any device to listen to tho
										InputControl = null,
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
					this.Messages.LocalPlayers_Increase();
					this.Messages.TeleportTo(Local.IdentityLocal,
						Local.Actor.X,
						Local.Actor.Y,
						Local.Actor.VelocityX,
						Local.Actor.VelocityY
					);

					// ... while member apply this rule

					// sending local ingame player keystates
					Action<Key, bool> Local_KeyStateChanged =
						(key, state) =>
						{
							this.Messages.KeyStateChanged(Local.IdentityLocal, (int)Local.Input.Keyboard.ToDefaultTranslation(key), Convert.ToInt32(state));
						};

					this.Content.Console.WriteLine("event add KeyStateChanged " + Local);
					Local.Input.Keyboard.KeyStateChanged += Local_KeyStateChanged;

					// when do we want to stop broadcasting our key changes?
					// maybe when we remove that local player

					this.Content.LocalIdentity.Locals.ForEachItemDeleted(
						(DeletedLocal, Dispose) =>
						{
							if (DeletedLocal != Local)
								return;

							this.Content.Console.WriteLine("event remove KeyStateChanged " + Local);
							Local.Input.Keyboard.KeyStateChanged -= Local_KeyStateChanged;

							// we should not listen to that event anymore
							Dispose();
						}
					);
				}
			);

			this.Content.LocalIdentity.Locals.ForEachItemDeleted(
				Local => this.Messages.LocalPlayers_Decrease()
			);
			#endregion

			#region EditorSelectorApplied

			this.Content.View.EditorSelectorApplied +=
				(Selector, Position) =>
				{
					var Index = KnownSelectors.Index.Of(Selector);

					// unknown selector
					if (Index.Type == -1)
						return;

					this.Messages.EditorSelector(Index.Type, Index.Size, Position.ContentX, Position.ContentY);
				};

			this.Events.UserEditorSelector +=
				e =>
				{
					Content.Console.WriteLine("UserEditorSelector " + e);

					var Selector = KnownSelectors.KnownTypes[e.type][e.size];
					var Position = new View.SelectorPosition { ContentX = e.x, ContentY = e.y };

					Selector.CreateTo(this.Content.View.Level, Position);
				};
			#endregion
		}

		public PlayerIdentity this[int user]
		{
			get
			{
				return this.CoPlayers.First(k => user == k.Number);
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
