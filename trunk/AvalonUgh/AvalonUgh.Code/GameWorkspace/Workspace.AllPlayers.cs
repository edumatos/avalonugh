using System;
using System.ComponentModel;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;
using System.Linq;
using AvalonUgh.Code.Input;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Collections.Generic;
using AvalonUgh.Code.Editor.Sprites;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		/// <summary>
		/// This will reflect the clients name and number,
		/// We could be in control of none or multiple actors or vehicles
		/// within multiple views and levels
		/// </summary>
		public readonly PlayerIdentity LocalIdentity;

		public BindingList<PlayerIdentity> CoPlayers;

		public IEnumerable<PlayerIdentity> AllPlayers
		{
			get
			{
				return CoPlayers.ConcatSingle(this.LocalIdentity);
			}
		}

		public bool LocalIdentityIsPrimate
		{
			get
			{
				return this.AllPlayers.Min(k => k.NetworkNumber) == this.LocalIdentity.NetworkNumber;
			}
		}


	}
}
