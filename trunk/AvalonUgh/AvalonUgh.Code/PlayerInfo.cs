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

		public int Identity;
		public string Name;


		public override string ToString()
		{
			return new { Identity, Name }.ToString();
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
