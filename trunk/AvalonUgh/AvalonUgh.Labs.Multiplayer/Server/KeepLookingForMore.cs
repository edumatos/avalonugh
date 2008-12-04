using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonUgh.NetworkCode.Server;
using ScriptCoreLib;

namespace AvalonUgh.Labs.Multiplayer.Server
{

	[Script]
	sealed class KeepLookingForMore
	{
		// jsc will find this type and only then look into the referenced assemblies

		// our dll's actual entry type is this:
		// global::AvalonUgh.NetworkCode.Server.NonobaGame

		public NonobaGame GetNonobaGame()
		{
			// a dummy method
			return null;
		}
	}
}
