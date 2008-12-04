using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using Nonoba.GameLibrary;
using AvalonUgh.NetworkCode.Shared;

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
	public class NonobaGame : NonobaGame<NonobaGameUser<VirtualPlayer>>
	{
		public override void GameStarted()
		{
		}

		public override void GotMessage(NonobaGameUser<VirtualPlayer> user, Message message)
		{
		}

		public override void UserJoined(NonobaGameUser<VirtualPlayer> user)
		{
		}

		public override void UserLeft(NonobaGameUser<VirtualPlayer> user)
		{
		}
	}
}
