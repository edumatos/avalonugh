using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Nonoba.Generic;

namespace AvalonUgh.NetworkCode.Shared
{
	[Script]
	public class VirtualPlayer : ServerPlayerBase<Communication.IEvents, Communication.IMessages>
	{
		// the player on the serverside
		// really does not do anything 

		public void UserLeft()
		{
			//this.Data["AvalonUgh"]["PropertyBag"] = "you lost the bag!";

		}


		public void UserJoined()
		{
			var k = this.Data["avalonugh"]["propertybag"][0];

			Console.WriteLine(k.GetCombinedKey());

			k.Value = "you got a bag alright!";

		}

	}
}
