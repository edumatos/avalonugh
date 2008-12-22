using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.ComponentModel;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class PlayerIdentity
	{
		public int Number;

		public string Name;

		public readonly BindingList<PlayerInfo> Locals = new BindingList<PlayerInfo>();

		public int SyncFrameLatency;
		public int SyncFrameRate;
		public bool SyncFramePaused;
		public bool SyncFramePausedSkip;
		public event Action SyncFrameChanged;
		int InternalSyncFrame;
		public int SyncFrame
		{
			get
			{
				return InternalSyncFrame;
			}
			set
			{
				InternalSyncFrame = value;
				if (SyncFrameChanged != null)
					SyncFrameChanged();
			}
		}

		public PlayerIdentity()
		{
			this.Locals.ForEachNewItem(
				i =>
				{
					i.IdentityLocal = this.Locals.Count;
				}
			);
		}

		public override string ToString()
		{
			return new { Number, Name }.ToString();
		}

		public PlayerInfo this[int IdentityLocal]
		{
			get
			{
				return this.Locals.Single(k => k.IdentityLocal == IdentityLocal);
			}
		}
	}
}
