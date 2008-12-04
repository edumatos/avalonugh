using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.Nonoba.api;

namespace AvalonUgh.NetworkCode.Client.ActionScript
{
	[Script]
	public class NonobaClient : Shared.NetworkClient
	{
		public const int NonobaChatWidth = 200;

		public const int DefaultWidth = 600 + NonobaChatWidth;
		public const int DefaultHeight = 600;

		public NonobaClient()
		{
			this.Container.InvokeWhenStageIsReady(
				stage =>
				{
					var c = NonobaAPI.MakeMultiplayer(stage
						//, "192.168.3.102"
						//, "192.168.1.119"
					);

				}
			);
		}
	}
}
