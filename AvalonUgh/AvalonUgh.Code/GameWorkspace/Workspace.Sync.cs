using System;
using System.ComponentModel;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		// these delegates need to be overriden to enable
		// network synchronization

		public Action<bool, string> Sync_SetPause;


		[Script]
		public delegate void DelegateTeleportTo(BindingList<PlayerInfo> a, int port, int local, double x, double y, double vx, double vy);
		public DelegateTeleportTo Sync_TeleportTo;

		[Script]
		public delegate void DelegateRemoveLocalPlayer(BindingList<PlayerInfo> a, int local);
		public DelegateRemoveLocalPlayer Sync_RemoveLocalPlayer;

		[Script]
		public delegate void DelegateLoadLevel(int port, int level, string custom);
		public DelegateLoadLevel Sync_LoadLevel;

		public void Sync_LoadLevelEx(Port port, LevelReference level)
		{
			// this is a "serializer" method
			if (level.Location.Embedded == null)
				this.Sync_LoadLevel(port.PortIdentity, -1, level.Data);
			else
				this.Sync_LoadLevel(port.PortIdentity, level.Location.Embedded.AnimationFrame, "");
		}


		[Script]
		public delegate void DelegateLoadLevelHint(int port);
		public DelegateLoadLevelHint Sync_RemoteOnly_LoadLevelHint;

		[Script]
		public delegate void DelegateEditorSelector(int port, int type, int size, int x, int y);
		public DelegateEditorSelector Sync_EditorSelector;


		[Script]
		public delegate void DelegateMouseMove(int port, double x, double y);
		public DelegateMouseMove Sync_RemoteOnly_MouseMove;


		[Script]
		public delegate void DelegateMissionStartHint(int user, int difficulty);
		public DelegateMissionStartHint Sync_MissionStartHint;
	}
}
