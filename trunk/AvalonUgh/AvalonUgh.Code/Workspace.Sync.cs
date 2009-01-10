using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Editor;
using System.ComponentModel;
using AvalonUgh.Code.Dialogs;
using System.Windows.Input;
using ScriptCoreLib.Shared.Avalon.Tween;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using System.Windows.Shapes;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Code
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

		[Script]
		public delegate void DelegateLoadLevelHint(int port);
		public DelegateLoadLevelHint Sync_RemoteOnly_LoadLevelHint;

		[Script]
		public delegate void DelegateEditorSelector(int port, int type, int size, int x, int y);
		public DelegateEditorSelector Sync_EditorSelector;


		[Script]
		public delegate void DelegateMouseMove(int port, double x, double y);
		public DelegateMouseMove Sync_RemoteOnly_MouseMove;
	}
}
