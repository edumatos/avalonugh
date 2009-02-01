using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.NetworkCode.Client.Shared;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib.CSharp.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Shapes;
using ScriptCoreLib.Shared.Lambda;
using System.Collections.Specialized;
using System.Diagnostics;

namespace AvalonUgh.Labs.Multiplayer
{
	class Program
	{
		// this project will build to
		// nonoba server dll
		// nonoba client flash
		// desktop inmemory multiplayer

		public const int Lag = 70;
		//public const int Lag = 50;


		static void VirtualLatency(Action e)
		{
			Lag.AtDelay(
				delegate
				{
					e();
				}
			);
		}

		[STAThread]
		static public void Main(string[] args)
		{
			// we will start an inmemory server and launch 3 clients
			Func<Communication.Bridge> Bridge =
				() =>
					new Communication.Bridge
					{
						//VirtualLatency = VirtualLatency

						VirtualLatency = e => e()
					};

			var Server = new VirtualGame
			{

			};

			Server.GameStarted();

			var UserId = 0;

			var DataStorage = new Stack<StringDictionary>();



			new ServerWindow(
				delegate
				{
					StringDictionary CurrentDataStorage = null;

					if (DataStorage.Count == 0)
						CurrentDataStorage = new StringDictionary();
					else
						CurrentDataStorage = DataStorage.Pop();

					Console.WriteLine("new client");

					var server_to_client = Bridge();
					var client_to_server = Bridge();


					var u = new VirtualPlayer
					{
						UserId = UserId++,
						FromPlayer = client_to_server,
						ToPlayer = server_to_client,
						Username = "guest" + UserId,
						SetData = (key, value) => CurrentDataStorage[key] = value,
						GetData = (key, value) => CurrentDataStorage.ContainsKey(key) ? CurrentDataStorage[key] : value
					};

					u.ToOthers =
						new Communication.RemoteMessages
						{
							VirtualTargets =
								() => Server.Users.Where(k => k != u).Select(k => k.ToPlayer)
						};

					new Communication.RemoteEvents.WithUserArgumentsRouter_Broadcast
					{
						user = u.UserId,
						Target = u.ToOthers
					}.CombineDelegates(client_to_server);

					new Communication.RemoteEvents.WithUserArgumentsRouter_Singlecast
					{
						user = u.UserId,
						Target =
							user => Server.Users.Where(k => k.UserId == user).Select(k => k.ToPlayer).SingleOrDefault()

					}.CombineDelegates(client_to_server);



					var c = new NetworkClient();

					c.Messages = client_to_server;
					c.Events = server_to_client;

					c.InitializeEvents();
					c.Connect();

					var w = c.ToWindow();

					w.Title += " - " + u.UserId + " " + u.Username;

					w.Show();

					Server.Users.Add(u);
					Server.UserJoined(u);
					u.UserJoined();

					w.Closed +=
						delegate
						{
							DataStorage.Push(CurrentDataStorage);

							Server.Users.Remove(u);
							Server.UserLeft(u);
							u.UserLeft();

							server_to_client.VirtualLatency =
								delegate
								{

								};

							client_to_server.VirtualLatency =
								delegate
								{

								};

							c.Content.Dispose();
						};
				}
			).ToWindow().ShowDialog();
		}

	}

	class ServerWindow : Canvas
	{
		public const int DefaultWidth = 300;
		public const int DefaultHeight = 200;

		public const int BackgroundDetail = 2;

		public ServerWindow(Action SpawnClient)
		{
			this.SizeTo(DefaultWidth, DefaultHeight);

			#region draw background
			new[]
			{
				Colors.Black,
				Colors.Yellow,
				Colors.Red,
				Colors.Black
			}.ToGradient(DefaultHeight / BackgroundDetail).Select(
				(c, i) =>
					new Rectangle
					{
						Fill = new SolidColorBrush(c),
						Width = DefaultWidth,
						Height = BackgroundDetail,
					}.MoveTo(0, i * BackgroundDetail).AttachTo(this)
			).ToArray();
			#endregion


			var b = new Button { Content = "Spawn client", Margin = new Thickness(16), Padding = new Thickness(4) }.AttachTo(this);
			var b2 = new Button { Content = "Spawn 2 clients", Margin = new Thickness(16), Padding = new Thickness(4) }.AttachTo(this);
			var b3 = new Button { Content = "Spawn 3 clients", Margin = new Thickness(16), Padding = new Thickness(4) }.AttachTo(this);

			Canvas.SetTop(b2, 50);
			Canvas.SetTop(b3, 100);

			Action<int> SpawnClients =
				Count =>
				{
					Enumerable.Range(0, Count).ForEach(
						(Index, SignalNext) =>
						{
							SpawnClient();
							50.AtDelay(SignalNext);
						}
					);
				};

			b.Click += delegate { SpawnClients(1); };
			b2.Click += delegate { SpawnClients(2); };
			b3.Click += delegate { SpawnClients(3); };

			50.AtDelay(() => SpawnClients(1));
		}
	}
}
