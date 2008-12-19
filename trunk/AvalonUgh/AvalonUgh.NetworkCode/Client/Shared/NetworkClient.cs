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

			this.CoPlayers = new BindingList<PlayerIdentity>();
			this.CoPlayers.ForEachNewItem(
				i =>
				{
					i.Locals.ForEachNewItem(p => this.Content.Players.Add(p));
					i.Locals.ForEachItemDeleted(p => this.Content.Players.Remove(p));
				}
			);

			#endregion

			Content = new TargetCanvas().AttachTo(this);
			Content.Console.WriteLine("starting networking...");

			Content.Console.WriteLine("InitializeEvents");

			this.Events.Server_Hello +=
				e =>
				{
					// yay, the server tells me my name. lets atleast remember it.
					this.Content.LocalIdentity.Number = e.user;
					this.Content.LocalIdentity.Name = e.name;

					Content.Console.WriteLine("Server_Hello " + e);

					//this.Content.DefaultPlayerInput.Keyboard.KeyStateChanged +=
					//    (Key, State) =>
					//    {
					//        this.Messages.KeyStateChanged((int)Key, Convert.ToInt32(State));
					//    };
				};

			this.Events.Server_UserJoined +=
				e =>
				{
					Content.Console.WriteLine("Server_UserJoined " + e);

					this.Messages.UserHello(e.user, this.Content.LocalIdentity.Name);

					this.CoPlayers.Add(
						new PlayerIdentity { Name = e.name, Number = e.user }
					);

					//this.Messages.UserTeleportTo(e.user,
					//    this.Content.DefaultPlayer.Actor.X,
					//    this.Content.DefaultPlayer.Actor.Y,
					//    this.Content.DefaultPlayer.Actor.VelocityX,
					//    this.Content.DefaultPlayer.Actor.VelocityY
					//);

					//AddRemotePlayer(e.user, e.name);
				};



			this.Events.Server_UserLeft +=
				e =>
				{
					//var c = this[e.user];

					//Content.Console.WriteLine("Server_UserLeft " + e + " - " + c);

					//this.Content.Players.Remove(c);
				};

			this.Events.UserHello +=
				e =>
				{
					Content.Console.WriteLine("UserHello " + e);

					this.CoPlayers.Add(
						new PlayerIdentity { Name = e.name, Number = e.user }
					);

					//this.Messages.UserTeleportTo(e.user,
					//    this.Content.DefaultPlayer.Actor.X,
					//    this.Content.DefaultPlayer.Actor.Y,
					//    this.Content.DefaultPlayer.Actor.VelocityX,
					//    this.Content.DefaultPlayer.Actor.VelocityY
					//);

					//AddRemotePlayer(e.user, e.name);
				};


			this.Events.UserKeyStateChanged +=
				e =>
				{
					//var c = this[e];

					//Content.Console.WriteLine("UserKeyStateChanged " + e + " - " + c);

					//c.Input.Keyboard.KeyState[(Key)e.key] =  Convert.ToBoolean(e.state);
				};



			10000.AtInterval(
				delegate
				{
					this.Content.Console.WriteLine("sending our position");

					foreach (var p in this.Content.LocalIdentity.Locals)
					{
						this.Messages.TeleportTo(
							p.IdentityLocal,
							p.Actor.X,
							p.Actor.Y,
							p.Actor.VelocityX,
							p.Actor.VelocityY
						);
					}
				}
			);

			this.Events.UserTeleportTo +=
				e =>
				{
					var p = this[e][e.local]; 

					Content.Console.WriteLine("UserTeleportTo " + e + " - " + p);

					p.Actor.MoveTo(e.x, e.y);
					p.Actor.VelocityX = e.vx;
					p.Actor.VelocityY = e.vy;
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
				Local => this.Messages.LocalPlayers_Increase()
			);

			this.Content.LocalIdentity.Locals.ForEachItemDeleted(
				Local => this.Messages.LocalPlayers_Decrease()
			);
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
