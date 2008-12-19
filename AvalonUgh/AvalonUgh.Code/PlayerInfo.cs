using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Code
{
	[Script]
	public class PlayerInfo
	{
		// player can have
		// a cave (where he is in)
		// a man (as which he can move)
		// a vehicle (in which he can fly)
		// input

		/// <summary>
		/// Each client connected to the server gets its unique number,
		/// Name of the player or name of the team that is playing on this client
		/// </summary>
		public PlayerIdentity Identity = new PlayerIdentity();


		/// <summary>
		/// Each client can create multiple players for splitview game mode
		/// </summary>
		public int IdentityLocal;

		


		public override string ToString()
		{
			return new { IdentityLocal, Identity.Number, Identity.Name }.ToString();
		}


		public Actor Actor;


		public ISupportsPlayerInput InputRegistrant;
		public PlayerInput Input;

		public void AddAcceleration()
		{
			if (InputRegistrant == null)
				return;

			if (Input == null)
				return;

			InputRegistrant.AddAcceleration(Input);
		}
	}
}
