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

namespace AvalonUgh.NetworkCode.Client.Shared
{
	[Script]
	public class NetworkClient : VirtualClient, ISupportsContainer
	{
		// this code is shared between
		// javascript, actionscript, c#

		public Canvas Container { get; set; }

		public readonly Action<string> WriteLine;

		public NetworkClient()
		{
			this.Container = new Canvas
			{
				Background = Brushes.Yellow,
				Width = 600,
				Height = 600
			};

			var Log = new TextBox
			{
				AcceptsReturn = true,
				Width = 600,
				Height = 600,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0),
				FontFamily = new FontFamily("Courier New")
			}.AttachTo(this);

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
			this.Events.ServerPlayerHello +=
				delegate
				{
					this.WriteLine("ServerPlayerHello");
				};
		}
	}
}
