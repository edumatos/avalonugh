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
using AvalonUgh.Labs.Shared;
using System.Windows.Shapes;
using AvalonUgh.Code.Dialogs;

namespace AvalonUgh.NetworkCode.Client.Shared
{
	[Script]
	public class NetworkClient : VirtualClient, ISupportsContainer
	{
		// this code is shared between
		// javascript, actionscript, c#

		public const int DefaultWidth = LabsCanvas.DefaultWidth;
		public const int DefaultHeight = LabsCanvas.DefaultHeight;
		public const int Zoom = LabsCanvas.Zoom;

		public Canvas Container { get; set; }

		public readonly Action<string> WriteLine;

		public NetworkClient()
		{
			this.Container = new Canvas
			{
				Background = Brushes.Black,
				Width = DefaultWidth,
				Height = DefaultHeight
			};

			var Content = new LabsCanvas().AttachTo(this);

			Content.IsMultiplayer = true;

		
		
			#region Log
			var LogBackground = new Rectangle
			{
				Width = DefaultWidth,
				Height = DefaultHeight / 4,
				Fill = Brushes.Black,
				Opacity = 0.2
			}.AttachTo(this);

			var Log = new TextBox
			{
				AcceptsReturn = true,
				Width = DefaultWidth,
				Height = DefaultHeight / 4,
				Background = Brushes.Transparent,
				Foreground = Brushes.Yellow,
				BorderThickness = new Thickness(0),
				FontFamily = new FontFamily("Courier New"),
				IsReadOnly = true
			}.AttachTo(this);
			#endregion

		
			#region WriteLine
			var LogQueue = new Queue<string>();


			this.WriteLine =
				Text =>
				{
					LogQueue.Enqueue(Text);

					if (LogQueue.Count > 10)
						LogQueue.Dequeue();

					Log.Text = LogQueue.Aggregate("",
						(Value, QueueText) =>
						{
							if (Value == "")
								return QueueText;

							return Value + Environment.NewLine + QueueText;
						}
					);
				};
			#endregion

			this.WriteLine("Ready to load");
		}

		public void Connect()
		{
			this.WriteLine("Connect");
		}

		public void Disconnect()
		{
			this.WriteLine("Disconnect");

		}

		public void InitializeEvents()
		{
			this.WriteLine("InitializeEvents");

			this.Events.Server_Hello +=
				e =>
				{
					this.WriteLine("Server_Hello " + e);
				};

			this.Events.Server_UserJoined +=
				e =>
				{
					this.WriteLine("Server_UserJoined " + e);
				};

			this.Events.Server_UserLeft +=
				e =>
				{
					this.WriteLine("Server_UserLeft " + e);
				};
		}
	}
}
