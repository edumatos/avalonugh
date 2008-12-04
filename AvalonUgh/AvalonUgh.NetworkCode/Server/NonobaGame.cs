using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using Nonoba.GameLibrary;

namespace AvalonUgh.NetworkCode.Server
{
	[Script]
	public class NonobaGameUser<TVirtualPlayer> : NonobaGameUser
	{
		public TVirtualPlayer Virtual { get; set; }

		public override Dictionary<string, string> GetDebugValues()
		{
			return new Dictionary<string, string> { };
		}
	}


	[Script]
	public class NonobaGame
	{
	}
}
