using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;

namespace AvalonUgh.NetworkCode.Client.Shared
{
	[Script]
	public class NetworkClient : VirtualClient, ISupportsContainer
	{
		public Canvas Container { get; set; }

		public NetworkClient()
		{
			this.Container = new Canvas
			{
				Background = Brushes.Red,
				Width = 600,
				Height = 600
			};
		}
	}
}
