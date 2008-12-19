using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Game.Shared;

namespace AvalonUgh.NetworkCode.Client.Shared
{
	using TargetCanvas = AvalonUgh.Game.Shared.GameCanvas;
	using AvalonUgh.Code;
	using AvalonUgh.Code.Input;
	using System.Windows.Input;

	[Script]
	public class NetworkClient : VirtualClient, ISupportsContainer
	{
		// this code is shared between
		// javascript, actionscript, c#

		public const int DefaultWidth = TargetCanvas.DefaultWidth;
		public const int DefaultHeight = TargetCanvas.DefaultHeight;
		public const int Zoom = TargetCanvas.Zoom;

		public Canvas Container { get; set; }

		//public readonly Action<string> WriteLine;
		public TargetCanvas Content;

		public NetworkClient()
		{
			this.Container = new Canvas
			{
				Background = Brushes.Black,
				Width = DefaultWidth,
				Height = DefaultHeight
			};

			Content = new TargetCanvas().AttachTo(this);
			Content.Console.WriteLine("starting networking...");

		}

		public void Connect()
		{
			Content.Console.WriteLine("Connect");
		}

		public void Disconnect()
		{
			Content.Console.WriteLine("Disconnect");

		}


		public void InitializeEvents()
		{
			Content.Console.WriteLine("InitializeEvents");

			this.Events.Server_Hello +=
				e =>
				{
					this.Content.DefaultPlayer.Identity = e.user;
					this.Content.DefaultPlayer.Name = e.name;

					Content.Console.WriteLine("Server_Hello " + e);

					this.Content.DefaultPlayerInput.Keyboard.KeyStateChanged +=
						(Key, State) =>
						{
							this.Messages.KeyStateChanged((int)Key, Convert.ToInt32(State));
						};
				};

			this.Events.Server_UserJoined +=
				e =>
				{
					Content.Console.WriteLine("Server_UserJoined " + e);

					this.Messages.UserHello(e.user, this.Content.DefaultPlayer.Name);
					this.Messages.UserTeleportTo(e.user,
						this.Content.DefaultPlayer.Actor.X,
						this.Content.DefaultPlayer.Actor.Y,
						this.Content.DefaultPlayer.Actor.VelocityX,
						this.Content.DefaultPlayer.Actor.VelocityY
					);

					AddRemotePlayer(e.user, e.name);
				};

			this.Events.Server_UserLeft +=
				e =>
				{
					var c = this[e.user];

					Content.Console.WriteLine("Server_UserLeft " + e + " - " + c);

					this.Content.Players.Remove(c);
				};

			this.Events.UserHello +=
				e =>
				{
					Content.Console.WriteLine("UserHello " + e);

					this.Messages.UserTeleportTo(e.user,
						this.Content.DefaultPlayer.Actor.X,
						this.Content.DefaultPlayer.Actor.Y,
						this.Content.DefaultPlayer.Actor.VelocityX,
						this.Content.DefaultPlayer.Actor.VelocityY
					);

					AddRemotePlayer(e.user, e.name);
				};


			this.Events.UserKeyStateChanged +=
				e =>
				{
					var c = this[e];

					Content.Console.WriteLine("UserKeyStateChanged " + e + " - " + c);

					c.Input.Keyboard.KeyState[(Key)e.key] =  Convert.ToBoolean(e.state);
				};

			this.Events.UserTeleportTo +=
				e =>
				{
					var c = this[e];

					Content.Console.WriteLine("UserTeleportTo " + e + " - " + c);

					c.Actor.MoveTo(e.x, e.y);
					c.Actor.VelocityX = e.vx;
					c.Actor.VelocityY = e.vy;
				};
		}

		public PlayerInfo this[int user]
		{
			get
			{
				return this.Content.Players.First(k => user == k.Identity);
			}
		}

		public PlayerInfo this[Communication.RemoteEvents.WithUserArguments u]
		{
			get
			{
				return this[u.user];
			}
		}


		public PlayerInfo AddRemotePlayer(int user, string name)
		{
			var n = new PlayerInfo
			{
				Identity = user,
				Name = name,

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

			this.Content.Players.Add(n);

			return n;
		}
	}
}
