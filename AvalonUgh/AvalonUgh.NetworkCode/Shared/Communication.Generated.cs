using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ScriptCoreLib.Shared.Nonoba;
using ScriptCoreLib;

namespace AvalonUgh.NetworkCode.Shared
{
    #region Communication
    [Script]
    [CompilerGenerated]
    public partial class Communication
    {
        #region Messages
        [Script]
        [CompilerGenerated]
        public enum Messages
        {
            None = 100,
            Server_Hello,
            Server_UserJoined,
            Server_UserLeft,
            Hello,
            UserHello,
            KeyStateChanged,
            UserKeyStateChanged,
            TeleportTo,
            UserTeleportTo,
            Vehicle_TeleportTo,
            UserVehicle_TeleportTo,
            LocalPlayers_Increase,
            UserLocalPlayers_Increase,
            LocalPlayers_Decrease,
            UserLocalPlayers_Decrease,
            EditorSelector,
            UserEditorSelector,
            SyncFrame,
            UserSyncFrame,
            SyncFrameEcho,
            UserSyncFrameEcho,
            SetShakerEnabled,
            UserSetShakerEnabled,
            SetPaused,
            UserSetPaused,
            ClearPaused,
            UserClearPaused,
            LoadEmbeddedLevel,
            UserLoadEmbeddedLevel,
            LoadCustomLevel,
            UserLoadCustomLevel,
        }
        #endregion

        #region IMessages
        [Script]
        [CompilerGenerated]
        public partial interface IMessages
        {
        }
        #endregion
        #region IEvents
        [Script]
        [CompilerGenerated]
        public partial interface IEvents
        {
            event Action<RemoteEvents.Server_HelloArguments> Server_Hello;
            event Action<RemoteEvents.Server_UserJoinedArguments> Server_UserJoined;
            event Action<RemoteEvents.Server_UserLeftArguments> Server_UserLeft;
            event Action<RemoteEvents.HelloArguments> Hello;
            event Action<RemoteEvents.UserHelloArguments> UserHello;
            event Action<RemoteEvents.KeyStateChangedArguments> KeyStateChanged;
            event Action<RemoteEvents.UserKeyStateChangedArguments> UserKeyStateChanged;
            event Action<RemoteEvents.TeleportToArguments> TeleportTo;
            event Action<RemoteEvents.UserTeleportToArguments> UserTeleportTo;
            event Action<RemoteEvents.Vehicle_TeleportToArguments> Vehicle_TeleportTo;
            event Action<RemoteEvents.UserVehicle_TeleportToArguments> UserVehicle_TeleportTo;
            event Action<RemoteEvents.LocalPlayers_IncreaseArguments> LocalPlayers_Increase;
            event Action<RemoteEvents.UserLocalPlayers_IncreaseArguments> UserLocalPlayers_Increase;
            event Action<RemoteEvents.LocalPlayers_DecreaseArguments> LocalPlayers_Decrease;
            event Action<RemoteEvents.UserLocalPlayers_DecreaseArguments> UserLocalPlayers_Decrease;
            event Action<RemoteEvents.EditorSelectorArguments> EditorSelector;
            event Action<RemoteEvents.UserEditorSelectorArguments> UserEditorSelector;
            event Action<RemoteEvents.SyncFrameArguments> SyncFrame;
            event Action<RemoteEvents.UserSyncFrameArguments> UserSyncFrame;
            event Action<RemoteEvents.SyncFrameEchoArguments> SyncFrameEcho;
            event Action<RemoteEvents.UserSyncFrameEchoArguments> UserSyncFrameEcho;
            event Action<RemoteEvents.SetShakerEnabledArguments> SetShakerEnabled;
            event Action<RemoteEvents.UserSetShakerEnabledArguments> UserSetShakerEnabled;
            event Action<RemoteEvents.SetPausedArguments> SetPaused;
            event Action<RemoteEvents.UserSetPausedArguments> UserSetPaused;
            event Action<RemoteEvents.ClearPausedArguments> ClearPaused;
            event Action<RemoteEvents.UserClearPausedArguments> UserClearPaused;
            event Action<RemoteEvents.LoadEmbeddedLevelArguments> LoadEmbeddedLevel;
            event Action<RemoteEvents.UserLoadEmbeddedLevelArguments> UserLoadEmbeddedLevel;
            event Action<RemoteEvents.LoadCustomLevelArguments> LoadCustomLevel;
            event Action<RemoteEvents.UserLoadCustomLevelArguments> UserLoadCustomLevel;
        }
        #endregion

        #region RemoteMessages
        [Script]
        [CompilerGenerated]
        public sealed partial class RemoteMessages : IMessages
        {
            public Action<SendArguments> Send;
            public Func<IEnumerable<IMessages>> VirtualTargets;
            #region SendArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class SendArguments
            {
                public Messages i;
                public object[] args;
            }
            #endregion
            public void Server_Hello(int user, string name, int others)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_Hello, args = new object[] { user, name, others } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_Hello(user, name, others);
                    }
                }
            }
            public void Server_UserJoined(int user, string name)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_UserJoined, args = new object[] { user, name } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_UserJoined(user, name);
                    }
                }
            }
            public void Server_UserLeft(int user, string name)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_UserLeft, args = new object[] { user, name } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_UserLeft(user, name);
                    }
                }
            }
            public void Hello(string name, int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Hello, args = new object[] { name, frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Hello(name, frame);
                    }
                }
            }
            public void UserHello(int user, string name, int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserHello, args = new object[] { user, name, frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserHello(user, name, frame);
                    }
                }
            }
            public void KeyStateChanged(int local, int frame, int sequence, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.KeyStateChanged, args = new object[] { local, frame, sequence, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.KeyStateChanged(local, frame, sequence, key, state);
                    }
                }
            }
            public void UserKeyStateChanged(int user, int local, int frame, int sequence, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserKeyStateChanged, args = new object[] { user, local, frame, sequence, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserKeyStateChanged(user, local, frame, sequence, key, state);
                    }
                }
            }
            public void TeleportTo(int frame, int local, int port, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.TeleportTo, args = new object[] { frame, local, port, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.TeleportTo(frame, local, port, x, y, vx, vy);
                    }
                }
            }
            public void UserTeleportTo(int user, int frame, int local, int port, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserTeleportTo, args = new object[] { user, frame, local, port, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserTeleportTo(user, frame, local, port, x, y, vx, vy);
                    }
                }
            }
            public void Vehicle_TeleportTo(int index, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Vehicle_TeleportTo, args = new object[] { index, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Vehicle_TeleportTo(index, x, y, vx, vy);
                    }
                }
            }
            public void UserVehicle_TeleportTo(int user, int index, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserVehicle_TeleportTo, args = new object[] { user, index, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserVehicle_TeleportTo(user, index, x, y, vx, vy);
                    }
                }
            }
            public void LocalPlayers_Increase(int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LocalPlayers_Increase, args = new object[] { frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LocalPlayers_Increase(frame);
                    }
                }
            }
            public void UserLocalPlayers_Increase(int user, int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLocalPlayers_Increase, args = new object[] { user, frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLocalPlayers_Increase(user, frame);
                    }
                }
            }
            public void LocalPlayers_Decrease(int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LocalPlayers_Decrease, args = new object[] { frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LocalPlayers_Decrease(frame);
                    }
                }
            }
            public void UserLocalPlayers_Decrease(int user, int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLocalPlayers_Decrease, args = new object[] { user, frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLocalPlayers_Decrease(user, frame);
                    }
                }
            }
            public void EditorSelector(int frame, int type, int size, int x, int y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.EditorSelector, args = new object[] { frame, type, size, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.EditorSelector(frame, type, size, x, y);
                    }
                }
            }
            public void UserEditorSelector(int user, int frame, int type, int size, int x, int y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserEditorSelector, args = new object[] { user, frame, type, size, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserEditorSelector(user, frame, type, size, x, y);
                    }
                }
            }
            public void SyncFrame(int frame, int framerate)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.SyncFrame, args = new object[] { frame, framerate } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.SyncFrame(frame, framerate);
                    }
                }
            }
            public void UserSyncFrame(int user, int frame, int framerate)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSyncFrame, args = new object[] { user, frame, framerate } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSyncFrame(user, frame, framerate);
                    }
                }
            }
            public void SyncFrameEcho(int frame, int framerate)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.SyncFrameEcho, args = new object[] { frame, framerate } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.SyncFrameEcho(frame, framerate);
                    }
                }
            }
            public void UserSyncFrameEcho(int user, int frame, int framerate)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSyncFrameEcho, args = new object[] { user, frame, framerate } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSyncFrameEcho(user, frame, framerate);
                    }
                }
            }
            public void SetShakerEnabled(int frame, int value)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.SetShakerEnabled, args = new object[] { frame, value } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.SetShakerEnabled(frame, value);
                    }
                }
            }
            public void UserSetShakerEnabled(int user, int frame, int value)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSetShakerEnabled, args = new object[] { user, frame, value } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSetShakerEnabled(user, frame, value);
                    }
                }
            }
            public void SetPaused(int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.SetPaused, args = new object[] { frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.SetPaused(frame);
                    }
                }
            }
            public void UserSetPaused(int user, int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSetPaused, args = new object[] { user, frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSetPaused(user, frame);
                    }
                }
            }
            public void ClearPaused()
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.ClearPaused, args = new object[] {  } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.ClearPaused();
                    }
                }
            }
            public void UserClearPaused(int user)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserClearPaused, args = new object[] { user } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserClearPaused(user);
                    }
                }
            }
            public void LoadEmbeddedLevel(int frame, int level)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LoadEmbeddedLevel, args = new object[] { frame, level } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LoadEmbeddedLevel(frame, level);
                    }
                }
            }
            public void UserLoadEmbeddedLevel(int user, int frame, int level)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLoadEmbeddedLevel, args = new object[] { user, frame, level } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLoadEmbeddedLevel(user, frame, level);
                    }
                }
            }
            public void LoadCustomLevel(int frame, string data)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LoadCustomLevel, args = new object[] { frame, data } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LoadCustomLevel(frame, data);
                    }
                }
            }
            public void UserLoadCustomLevel(int user, int frame, string data)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLoadCustomLevel, args = new object[] { user, frame, data } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLoadCustomLevel(user, frame, data);
                    }
                }
            }
        }
        #endregion

        #region RemoteEvents
        [Script]
        [CompilerGenerated]
        public sealed partial class RemoteEvents : IEvents
        {
            private readonly Dictionary<Messages, Action<IDispatchHelper>> DispatchTable;
            private readonly Dictionary<Messages, Converter<object, Delegate>> DispatchTableDelegates;
            [AccessedThroughProperty("BroadcastRouter")]
            private WithUserArgumentsRouter_Broadcast _BroadcastRouter;
            [AccessedThroughProperty("SinglecastRouter")]
            private WithUserArgumentsRouter_Singlecast _SinglecastRouter;
            #region DispatchHelper
            [Script]
            [CompilerGenerated]
            public partial class DispatchHelper
            {
                public Converter<uint, int> GetInt32 { get; set; }
                public Converter<uint, double> GetDouble { get; set; }
                public Converter<uint, string> GetString { get; set; }
                public Converter<uint, int[]> GetInt32Array { get; set; }
                public Converter<uint, double[]> GetDoubleArray { get; set; }
                public Converter<uint, string[]> GetStringArray { get; set; }
                public Converter<uint, byte[]> GetMemoryStream { get; set; }
            }
            #endregion
            public bool Dispatch(Messages e, IDispatchHelper h)
            {
                if (!DispatchTableDelegates.ContainsKey(e)) return false;
                if (DispatchTableDelegates[e](null) == null) return false;
                if (!DispatchTable.ContainsKey(e)) return false;
                DispatchTable[e](h);
                return true;
            }
            #region WithUserArguments
            [Script]
            [CompilerGenerated]
            public abstract partial class WithUserArguments
            {
                public int user;
            }
            #endregion
            #region WithUserArgumentsRouter_Broadcast
            [Script]
            [CompilerGenerated]
            public sealed partial class WithUserArgumentsRouter_Broadcast : WithUserArguments
            {
                public IMessages Target;

                #region Automatic Event Routing
                public void CombineDelegates(IEvents value)
                {
                    value.Hello += this.UserHello;
                    value.KeyStateChanged += this.UserKeyStateChanged;
                    value.TeleportTo += this.UserTeleportTo;
                    value.Vehicle_TeleportTo += this.UserVehicle_TeleportTo;
                    value.LocalPlayers_Increase += this.UserLocalPlayers_Increase;
                    value.LocalPlayers_Decrease += this.UserLocalPlayers_Decrease;
                    value.EditorSelector += this.UserEditorSelector;
                    value.SyncFrame += this.UserSyncFrame;
                    value.SyncFrameEcho += this.UserSyncFrameEcho;
                    value.SetShakerEnabled += this.UserSetShakerEnabled;
                    value.SetPaused += this.UserSetPaused;
                    value.ClearPaused += this.UserClearPaused;
                    value.LoadEmbeddedLevel += this.UserLoadEmbeddedLevel;
                    value.LoadCustomLevel += this.UserLoadCustomLevel;
                }

                public void RemoveDelegates(IEvents value)
                {
                    value.Hello -= this.UserHello;
                    value.KeyStateChanged -= this.UserKeyStateChanged;
                    value.TeleportTo -= this.UserTeleportTo;
                    value.Vehicle_TeleportTo -= this.UserVehicle_TeleportTo;
                    value.LocalPlayers_Increase -= this.UserLocalPlayers_Increase;
                    value.LocalPlayers_Decrease -= this.UserLocalPlayers_Decrease;
                    value.EditorSelector -= this.UserEditorSelector;
                    value.SyncFrame -= this.UserSyncFrame;
                    value.SyncFrameEcho -= this.UserSyncFrameEcho;
                    value.SetShakerEnabled -= this.UserSetShakerEnabled;
                    value.SetPaused -= this.UserSetPaused;
                    value.ClearPaused -= this.UserClearPaused;
                    value.LoadEmbeddedLevel -= this.UserLoadEmbeddedLevel;
                    value.LoadCustomLevel -= this.UserLoadCustomLevel;
                }
                #endregion

                #region Routing
                public void UserHello(HelloArguments e)
                {
                    Target.UserHello(this.user, e.name, e.frame);
                }
                public void UserKeyStateChanged(KeyStateChangedArguments e)
                {
                    Target.UserKeyStateChanged(this.user, e.local, e.frame, e.sequence, e.key, e.state);
                }
                public void UserTeleportTo(TeleportToArguments e)
                {
                    Target.UserTeleportTo(this.user, e.frame, e.local, e.port, e.x, e.y, e.vx, e.vy);
                }
                public void UserVehicle_TeleportTo(Vehicle_TeleportToArguments e)
                {
                    Target.UserVehicle_TeleportTo(this.user, e.index, e.x, e.y, e.vx, e.vy);
                }
                public void UserLocalPlayers_Increase(LocalPlayers_IncreaseArguments e)
                {
                    Target.UserLocalPlayers_Increase(this.user, e.frame);
                }
                public void UserLocalPlayers_Decrease(LocalPlayers_DecreaseArguments e)
                {
                    Target.UserLocalPlayers_Decrease(this.user, e.frame);
                }
                public void UserEditorSelector(EditorSelectorArguments e)
                {
                    Target.UserEditorSelector(this.user, e.frame, e.type, e.size, e.x, e.y);
                }
                public void UserSyncFrame(SyncFrameArguments e)
                {
                    Target.UserSyncFrame(this.user, e.frame, e.framerate);
                }
                public void UserSyncFrameEcho(SyncFrameEchoArguments e)
                {
                    Target.UserSyncFrameEcho(this.user, e.frame, e.framerate);
                }
                public void UserSetShakerEnabled(SetShakerEnabledArguments e)
                {
                    Target.UserSetShakerEnabled(this.user, e.frame, e.value);
                }
                public void UserSetPaused(SetPausedArguments e)
                {
                    Target.UserSetPaused(this.user, e.frame);
                }
                public void UserClearPaused(ClearPausedArguments e)
                {
                    Target.UserClearPaused(this.user);
                }
                public void UserLoadEmbeddedLevel(LoadEmbeddedLevelArguments e)
                {
                    Target.UserLoadEmbeddedLevel(this.user, e.frame, e.level);
                }
                public void UserLoadCustomLevel(LoadCustomLevelArguments e)
                {
                    Target.UserLoadCustomLevel(this.user, e.frame, e.data);
                }
                #endregion
            }
            #endregion
            #region WithUserArgumentsRouter_SinglecastView
            [Script]
            [CompilerGenerated]
            public sealed partial class WithUserArgumentsRouter_SinglecastView : WithUserArguments
            {
                public IMessages Target;
                #region Routing
                public void UserHello(string name, int frame)
                {
                    this.Target.UserHello(this.user, name, frame);
                }
                public void UserHello(UserHelloArguments e)
                {
                    this.Target.UserHello(this.user, e.name, e.frame);
                }
                public void UserKeyStateChanged(int local, int frame, int sequence, int key, int state)
                {
                    this.Target.UserKeyStateChanged(this.user, local, frame, sequence, key, state);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    this.Target.UserKeyStateChanged(this.user, e.local, e.frame, e.sequence, e.key, e.state);
                }
                public void UserTeleportTo(int frame, int local, int port, double x, double y, double vx, double vy)
                {
                    this.Target.UserTeleportTo(this.user, frame, local, port, x, y, vx, vy);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    this.Target.UserTeleportTo(this.user, e.frame, e.local, e.port, e.x, e.y, e.vx, e.vy);
                }
                public void UserVehicle_TeleportTo(int index, double x, double y, double vx, double vy)
                {
                    this.Target.UserVehicle_TeleportTo(this.user, index, x, y, vx, vy);
                }
                public void UserVehicle_TeleportTo(UserVehicle_TeleportToArguments e)
                {
                    this.Target.UserVehicle_TeleportTo(this.user, e.index, e.x, e.y, e.vx, e.vy);
                }
                public void UserLocalPlayers_Increase(int frame)
                {
                    this.Target.UserLocalPlayers_Increase(this.user, frame);
                }
                public void UserLocalPlayers_Increase(UserLocalPlayers_IncreaseArguments e)
                {
                    this.Target.UserLocalPlayers_Increase(this.user, e.frame);
                }
                public void UserLocalPlayers_Decrease(int frame)
                {
                    this.Target.UserLocalPlayers_Decrease(this.user, frame);
                }
                public void UserLocalPlayers_Decrease(UserLocalPlayers_DecreaseArguments e)
                {
                    this.Target.UserLocalPlayers_Decrease(this.user, e.frame);
                }
                public void UserEditorSelector(int frame, int type, int size, int x, int y)
                {
                    this.Target.UserEditorSelector(this.user, frame, type, size, x, y);
                }
                public void UserEditorSelector(UserEditorSelectorArguments e)
                {
                    this.Target.UserEditorSelector(this.user, e.frame, e.type, e.size, e.x, e.y);
                }
                public void UserSyncFrame(int frame, int framerate)
                {
                    this.Target.UserSyncFrame(this.user, frame, framerate);
                }
                public void UserSyncFrame(UserSyncFrameArguments e)
                {
                    this.Target.UserSyncFrame(this.user, e.frame, e.framerate);
                }
                public void UserSyncFrameEcho(int frame, int framerate)
                {
                    this.Target.UserSyncFrameEcho(this.user, frame, framerate);
                }
                public void UserSyncFrameEcho(UserSyncFrameEchoArguments e)
                {
                    this.Target.UserSyncFrameEcho(this.user, e.frame, e.framerate);
                }
                public void UserSetShakerEnabled(int frame, int value)
                {
                    this.Target.UserSetShakerEnabled(this.user, frame, value);
                }
                public void UserSetShakerEnabled(UserSetShakerEnabledArguments e)
                {
                    this.Target.UserSetShakerEnabled(this.user, e.frame, e.value);
                }
                public void UserSetPaused(int frame)
                {
                    this.Target.UserSetPaused(this.user, frame);
                }
                public void UserSetPaused(UserSetPausedArguments e)
                {
                    this.Target.UserSetPaused(this.user, e.frame);
                }
                public void UserClearPaused()
                {
                    this.Target.UserClearPaused(this.user);
                }
                public void UserClearPaused(UserClearPausedArguments e)
                {
                    this.Target.UserClearPaused(this.user);
                }
                public void UserLoadEmbeddedLevel(int frame, int level)
                {
                    this.Target.UserLoadEmbeddedLevel(this.user, frame, level);
                }
                public void UserLoadEmbeddedLevel(UserLoadEmbeddedLevelArguments e)
                {
                    this.Target.UserLoadEmbeddedLevel(this.user, e.frame, e.level);
                }
                public void UserLoadCustomLevel(int frame, string data)
                {
                    this.Target.UserLoadCustomLevel(this.user, frame, data);
                }
                public void UserLoadCustomLevel(UserLoadCustomLevelArguments e)
                {
                    this.Target.UserLoadCustomLevel(this.user, e.frame, e.data);
                }
                #endregion
            }
            #endregion
            #region WithUserArgumentsRouter_Singlecast
            [Script]
            [CompilerGenerated]
            public sealed partial class WithUserArgumentsRouter_Singlecast : WithUserArguments
            {
                public System.Converter<int, IMessages> Target;

                #region Automatic Event Routing
                public void CombineDelegates(IEvents value)
                {
                    value.UserHello += this.UserHello;
                    value.UserKeyStateChanged += this.UserKeyStateChanged;
                    value.UserTeleportTo += this.UserTeleportTo;
                    value.UserVehicle_TeleportTo += this.UserVehicle_TeleportTo;
                    value.UserLocalPlayers_Increase += this.UserLocalPlayers_Increase;
                    value.UserLocalPlayers_Decrease += this.UserLocalPlayers_Decrease;
                    value.UserEditorSelector += this.UserEditorSelector;
                    value.UserSyncFrame += this.UserSyncFrame;
                    value.UserSyncFrameEcho += this.UserSyncFrameEcho;
                    value.UserSetShakerEnabled += this.UserSetShakerEnabled;
                    value.UserSetPaused += this.UserSetPaused;
                    value.UserClearPaused += this.UserClearPaused;
                    value.UserLoadEmbeddedLevel += this.UserLoadEmbeddedLevel;
                    value.UserLoadCustomLevel += this.UserLoadCustomLevel;
                }

                public void RemoveDelegates(IEvents value)
                {
                    value.UserHello -= this.UserHello;
                    value.UserKeyStateChanged -= this.UserKeyStateChanged;
                    value.UserTeleportTo -= this.UserTeleportTo;
                    value.UserVehicle_TeleportTo -= this.UserVehicle_TeleportTo;
                    value.UserLocalPlayers_Increase -= this.UserLocalPlayers_Increase;
                    value.UserLocalPlayers_Decrease -= this.UserLocalPlayers_Decrease;
                    value.UserEditorSelector -= this.UserEditorSelector;
                    value.UserSyncFrame -= this.UserSyncFrame;
                    value.UserSyncFrameEcho -= this.UserSyncFrameEcho;
                    value.UserSetShakerEnabled -= this.UserSetShakerEnabled;
                    value.UserSetPaused -= this.UserSetPaused;
                    value.UserClearPaused -= this.UserClearPaused;
                    value.UserLoadEmbeddedLevel -= this.UserLoadEmbeddedLevel;
                    value.UserLoadCustomLevel -= this.UserLoadCustomLevel;
                }
                #endregion

                #region Routing
                public void UserHello(UserHelloArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserHello(this.user, e.name, e.frame);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserKeyStateChanged(this.user, e.local, e.frame, e.sequence, e.key, e.state);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserTeleportTo(this.user, e.frame, e.local, e.port, e.x, e.y, e.vx, e.vy);
                }
                public void UserVehicle_TeleportTo(UserVehicle_TeleportToArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserVehicle_TeleportTo(this.user, e.index, e.x, e.y, e.vx, e.vy);
                }
                public void UserLocalPlayers_Increase(UserLocalPlayers_IncreaseArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLocalPlayers_Increase(this.user, e.frame);
                }
                public void UserLocalPlayers_Decrease(UserLocalPlayers_DecreaseArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLocalPlayers_Decrease(this.user, e.frame);
                }
                public void UserEditorSelector(UserEditorSelectorArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserEditorSelector(this.user, e.frame, e.type, e.size, e.x, e.y);
                }
                public void UserSyncFrame(UserSyncFrameArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSyncFrame(this.user, e.frame, e.framerate);
                }
                public void UserSyncFrameEcho(UserSyncFrameEchoArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSyncFrameEcho(this.user, e.frame, e.framerate);
                }
                public void UserSetShakerEnabled(UserSetShakerEnabledArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSetShakerEnabled(this.user, e.frame, e.value);
                }
                public void UserSetPaused(UserSetPausedArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSetPaused(this.user, e.frame);
                }
                public void UserClearPaused(UserClearPausedArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserClearPaused(this.user);
                }
                public void UserLoadEmbeddedLevel(UserLoadEmbeddedLevelArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLoadEmbeddedLevel(this.user, e.frame, e.level);
                }
                public void UserLoadCustomLevel(UserLoadCustomLevelArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLoadCustomLevel(this.user, e.frame, e.data);
                }
                #endregion
            }
            #endregion
            #region Server_HelloArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Server_HelloArguments
            {
                public int user;
                public string name;
                public int others;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(", others = ").Append(this.others).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<Server_HelloArguments> Server_Hello;
            #region Server_UserJoinedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Server_UserJoinedArguments
            {
                public int user;
                public string name;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<Server_UserJoinedArguments> Server_UserJoined;
            #region Server_UserLeftArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Server_UserLeftArguments
            {
                public int user;
                public string name;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<Server_UserLeftArguments> Server_UserLeft;
            #region HelloArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class HelloArguments
            {
                public string name;
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ name = ").Append(this.name).Append(", frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<HelloArguments> Hello;
            #region UserHelloArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserHelloArguments : WithUserArguments
            {
                public string name;
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(", frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserHelloArguments> UserHello;
            #region KeyStateChangedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class KeyStateChangedArguments
            {
                public int local;
                public int frame;
                public int sequence;
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ local = ").Append(this.local).Append(", frame = ").Append(this.frame).Append(", sequence = ").Append(this.sequence).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<KeyStateChangedArguments> KeyStateChanged;
            #region UserKeyStateChangedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserKeyStateChangedArguments : WithUserArguments
            {
                public int local;
                public int frame;
                public int sequence;
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", local = ").Append(this.local).Append(", frame = ").Append(this.frame).Append(", sequence = ").Append(this.sequence).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserKeyStateChangedArguments> UserKeyStateChanged;
            #region TeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class TeleportToArguments
            {
                public int frame;
                public int local;
                public int port;
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", local = ").Append(this.local).Append(", port = ").Append(this.port).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<TeleportToArguments> TeleportTo;
            #region UserTeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserTeleportToArguments : WithUserArguments
            {
                public int frame;
                public int local;
                public int port;
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", local = ").Append(this.local).Append(", port = ").Append(this.port).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserTeleportToArguments> UserTeleportTo;
            #region Vehicle_TeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Vehicle_TeleportToArguments
            {
                public int index;
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ index = ").Append(this.index).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<Vehicle_TeleportToArguments> Vehicle_TeleportTo;
            #region UserVehicle_TeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserVehicle_TeleportToArguments : WithUserArguments
            {
                public int index;
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", index = ").Append(this.index).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserVehicle_TeleportToArguments> UserVehicle_TeleportTo;
            #region LocalPlayers_IncreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LocalPlayers_IncreaseArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<LocalPlayers_IncreaseArguments> LocalPlayers_Increase;
            #region UserLocalPlayers_IncreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLocalPlayers_IncreaseArguments : WithUserArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLocalPlayers_IncreaseArguments> UserLocalPlayers_Increase;
            #region LocalPlayers_DecreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LocalPlayers_DecreaseArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<LocalPlayers_DecreaseArguments> LocalPlayers_Decrease;
            #region UserLocalPlayers_DecreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLocalPlayers_DecreaseArguments : WithUserArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLocalPlayers_DecreaseArguments> UserLocalPlayers_Decrease;
            #region EditorSelectorArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class EditorSelectorArguments
            {
                public int frame;
                public int type;
                public int size;
                public int x;
                public int y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", type = ").Append(this.type).Append(", size = ").Append(this.size).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<EditorSelectorArguments> EditorSelector;
            #region UserEditorSelectorArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserEditorSelectorArguments : WithUserArguments
            {
                public int frame;
                public int type;
                public int size;
                public int x;
                public int y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", type = ").Append(this.type).Append(", size = ").Append(this.size).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserEditorSelectorArguments> UserEditorSelector;
            #region SyncFrameArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class SyncFrameArguments
            {
                public int frame;
                public int framerate;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", framerate = ").Append(this.framerate).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<SyncFrameArguments> SyncFrame;
            #region UserSyncFrameArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserSyncFrameArguments : WithUserArguments
            {
                public int frame;
                public int framerate;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", framerate = ").Append(this.framerate).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserSyncFrameArguments> UserSyncFrame;
            #region SyncFrameEchoArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class SyncFrameEchoArguments
            {
                public int frame;
                public int framerate;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", framerate = ").Append(this.framerate).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<SyncFrameEchoArguments> SyncFrameEcho;
            #region UserSyncFrameEchoArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserSyncFrameEchoArguments : WithUserArguments
            {
                public int frame;
                public int framerate;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", framerate = ").Append(this.framerate).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserSyncFrameEchoArguments> UserSyncFrameEcho;
            #region SetShakerEnabledArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class SetShakerEnabledArguments
            {
                public int frame;
                public int value;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", value = ").Append(this.value).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<SetShakerEnabledArguments> SetShakerEnabled;
            #region UserSetShakerEnabledArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserSetShakerEnabledArguments : WithUserArguments
            {
                public int frame;
                public int value;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", value = ").Append(this.value).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserSetShakerEnabledArguments> UserSetShakerEnabled;
            #region SetPausedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class SetPausedArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<SetPausedArguments> SetPaused;
            #region UserSetPausedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserSetPausedArguments : WithUserArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserSetPausedArguments> UserSetPaused;
            #region ClearPausedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class ClearPausedArguments
            {
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().ToString();
                }
            }
            #endregion
            public event Action<ClearPausedArguments> ClearPaused;
            #region UserClearPausedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserClearPausedArguments : WithUserArguments
            {
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserClearPausedArguments> UserClearPaused;
            #region LoadEmbeddedLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LoadEmbeddedLevelArguments
            {
                public int frame;
                public int level;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", level = ").Append(this.level).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<LoadEmbeddedLevelArguments> LoadEmbeddedLevel;
            #region UserLoadEmbeddedLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLoadEmbeddedLevelArguments : WithUserArguments
            {
                public int frame;
                public int level;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", level = ").Append(this.level).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLoadEmbeddedLevelArguments> UserLoadEmbeddedLevel;
            #region LoadCustomLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LoadCustomLevelArguments
            {
                public int frame;
                public string data;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", data = ").Append(this.data).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<LoadCustomLevelArguments> LoadCustomLevel;
            #region UserLoadCustomLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLoadCustomLevelArguments : WithUserArguments
            {
                public int frame;
                public string data;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", data = ").Append(this.data).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLoadCustomLevelArguments> UserLoadCustomLevel;
            public RemoteEvents()
            {
                DispatchTable = new Dictionary<Messages, Action<IDispatchHelper>>
                        {
                            { Messages.Server_Hello, e => { Server_Hello(new Server_HelloArguments { user = e.GetInt32(0), name = e.GetString(1), others = e.GetInt32(2) }); } },
                            { Messages.Server_UserJoined, e => { Server_UserJoined(new Server_UserJoinedArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Server_UserLeft, e => { Server_UserLeft(new Server_UserLeftArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Hello, e => { Hello(new HelloArguments { name = e.GetString(0), frame = e.GetInt32(1) }); } },
                            { Messages.UserHello, e => { UserHello(new UserHelloArguments { user = e.GetInt32(0), name = e.GetString(1), frame = e.GetInt32(2) }); } },
                            { Messages.KeyStateChanged, e => { KeyStateChanged(new KeyStateChangedArguments { local = e.GetInt32(0), frame = e.GetInt32(1), sequence = e.GetInt32(2), key = e.GetInt32(3), state = e.GetInt32(4) }); } },
                            { Messages.UserKeyStateChanged, e => { UserKeyStateChanged(new UserKeyStateChangedArguments { user = e.GetInt32(0), local = e.GetInt32(1), frame = e.GetInt32(2), sequence = e.GetInt32(3), key = e.GetInt32(4), state = e.GetInt32(5) }); } },
                            { Messages.TeleportTo, e => { TeleportTo(new TeleportToArguments { frame = e.GetInt32(0), local = e.GetInt32(1), port = e.GetInt32(2), x = e.GetDouble(3), y = e.GetDouble(4), vx = e.GetDouble(5), vy = e.GetDouble(6) }); } },
                            { Messages.UserTeleportTo, e => { UserTeleportTo(new UserTeleportToArguments { user = e.GetInt32(0), frame = e.GetInt32(1), local = e.GetInt32(2), port = e.GetInt32(3), x = e.GetDouble(4), y = e.GetDouble(5), vx = e.GetDouble(6), vy = e.GetDouble(7) }); } },
                            { Messages.Vehicle_TeleportTo, e => { Vehicle_TeleportTo(new Vehicle_TeleportToArguments { index = e.GetInt32(0), x = e.GetDouble(1), y = e.GetDouble(2), vx = e.GetDouble(3), vy = e.GetDouble(4) }); } },
                            { Messages.UserVehicle_TeleportTo, e => { UserVehicle_TeleportTo(new UserVehicle_TeleportToArguments { user = e.GetInt32(0), index = e.GetInt32(1), x = e.GetDouble(2), y = e.GetDouble(3), vx = e.GetDouble(4), vy = e.GetDouble(5) }); } },
                            { Messages.LocalPlayers_Increase, e => { LocalPlayers_Increase(new LocalPlayers_IncreaseArguments { frame = e.GetInt32(0) }); } },
                            { Messages.UserLocalPlayers_Increase, e => { UserLocalPlayers_Increase(new UserLocalPlayers_IncreaseArguments { user = e.GetInt32(0), frame = e.GetInt32(1) }); } },
                            { Messages.LocalPlayers_Decrease, e => { LocalPlayers_Decrease(new LocalPlayers_DecreaseArguments { frame = e.GetInt32(0) }); } },
                            { Messages.UserLocalPlayers_Decrease, e => { UserLocalPlayers_Decrease(new UserLocalPlayers_DecreaseArguments { user = e.GetInt32(0), frame = e.GetInt32(1) }); } },
                            { Messages.EditorSelector, e => { EditorSelector(new EditorSelectorArguments { frame = e.GetInt32(0), type = e.GetInt32(1), size = e.GetInt32(2), x = e.GetInt32(3), y = e.GetInt32(4) }); } },
                            { Messages.UserEditorSelector, e => { UserEditorSelector(new UserEditorSelectorArguments { user = e.GetInt32(0), frame = e.GetInt32(1), type = e.GetInt32(2), size = e.GetInt32(3), x = e.GetInt32(4), y = e.GetInt32(5) }); } },
                            { Messages.SyncFrame, e => { SyncFrame(new SyncFrameArguments { frame = e.GetInt32(0), framerate = e.GetInt32(1) }); } },
                            { Messages.UserSyncFrame, e => { UserSyncFrame(new UserSyncFrameArguments { user = e.GetInt32(0), frame = e.GetInt32(1), framerate = e.GetInt32(2) }); } },
                            { Messages.SyncFrameEcho, e => { SyncFrameEcho(new SyncFrameEchoArguments { frame = e.GetInt32(0), framerate = e.GetInt32(1) }); } },
                            { Messages.UserSyncFrameEcho, e => { UserSyncFrameEcho(new UserSyncFrameEchoArguments { user = e.GetInt32(0), frame = e.GetInt32(1), framerate = e.GetInt32(2) }); } },
                            { Messages.SetShakerEnabled, e => { SetShakerEnabled(new SetShakerEnabledArguments { frame = e.GetInt32(0), value = e.GetInt32(1) }); } },
                            { Messages.UserSetShakerEnabled, e => { UserSetShakerEnabled(new UserSetShakerEnabledArguments { user = e.GetInt32(0), frame = e.GetInt32(1), value = e.GetInt32(2) }); } },
                            { Messages.SetPaused, e => { SetPaused(new SetPausedArguments { frame = e.GetInt32(0) }); } },
                            { Messages.UserSetPaused, e => { UserSetPaused(new UserSetPausedArguments { user = e.GetInt32(0), frame = e.GetInt32(1) }); } },
                            { Messages.ClearPaused, e => { ClearPaused(new ClearPausedArguments {  }); } },
                            { Messages.UserClearPaused, e => { UserClearPaused(new UserClearPausedArguments { user = e.GetInt32(0) }); } },
                            { Messages.LoadEmbeddedLevel, e => { LoadEmbeddedLevel(new LoadEmbeddedLevelArguments { frame = e.GetInt32(0), level = e.GetInt32(1) }); } },
                            { Messages.UserLoadEmbeddedLevel, e => { UserLoadEmbeddedLevel(new UserLoadEmbeddedLevelArguments { user = e.GetInt32(0), frame = e.GetInt32(1), level = e.GetInt32(2) }); } },
                            { Messages.LoadCustomLevel, e => { LoadCustomLevel(new LoadCustomLevelArguments { frame = e.GetInt32(0), data = e.GetString(1) }); } },
                            { Messages.UserLoadCustomLevel, e => { UserLoadCustomLevel(new UserLoadCustomLevelArguments { user = e.GetInt32(0), frame = e.GetInt32(1), data = e.GetString(2) }); } },
                        }
                ;
                DispatchTableDelegates = new Dictionary<Messages, Converter<object, Delegate>>
                        {
                            { Messages.Server_Hello, e => Server_Hello },
                            { Messages.Server_UserJoined, e => Server_UserJoined },
                            { Messages.Server_UserLeft, e => Server_UserLeft },
                            { Messages.Hello, e => Hello },
                            { Messages.UserHello, e => UserHello },
                            { Messages.KeyStateChanged, e => KeyStateChanged },
                            { Messages.UserKeyStateChanged, e => UserKeyStateChanged },
                            { Messages.TeleportTo, e => TeleportTo },
                            { Messages.UserTeleportTo, e => UserTeleportTo },
                            { Messages.Vehicle_TeleportTo, e => Vehicle_TeleportTo },
                            { Messages.UserVehicle_TeleportTo, e => UserVehicle_TeleportTo },
                            { Messages.LocalPlayers_Increase, e => LocalPlayers_Increase },
                            { Messages.UserLocalPlayers_Increase, e => UserLocalPlayers_Increase },
                            { Messages.LocalPlayers_Decrease, e => LocalPlayers_Decrease },
                            { Messages.UserLocalPlayers_Decrease, e => UserLocalPlayers_Decrease },
                            { Messages.EditorSelector, e => EditorSelector },
                            { Messages.UserEditorSelector, e => UserEditorSelector },
                            { Messages.SyncFrame, e => SyncFrame },
                            { Messages.UserSyncFrame, e => UserSyncFrame },
                            { Messages.SyncFrameEcho, e => SyncFrameEcho },
                            { Messages.UserSyncFrameEcho, e => UserSyncFrameEcho },
                            { Messages.SetShakerEnabled, e => SetShakerEnabled },
                            { Messages.UserSetShakerEnabled, e => UserSetShakerEnabled },
                            { Messages.SetPaused, e => SetPaused },
                            { Messages.UserSetPaused, e => UserSetPaused },
                            { Messages.ClearPaused, e => ClearPaused },
                            { Messages.UserClearPaused, e => UserClearPaused },
                            { Messages.LoadEmbeddedLevel, e => LoadEmbeddedLevel },
                            { Messages.UserLoadEmbeddedLevel, e => UserLoadEmbeddedLevel },
                            { Messages.LoadCustomLevel, e => LoadCustomLevel },
                            { Messages.UserLoadCustomLevel, e => UserLoadCustomLevel },
                        }
                ;
            }
            public WithUserArgumentsRouter_Broadcast BroadcastRouter
            {
                [DebuggerNonUserCode]
                get
                {
                    return this._BroadcastRouter;
                }
                [DebuggerNonUserCode]
                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if(_BroadcastRouter != null)
                    {
                        _BroadcastRouter.RemoveDelegates(this);
                    }
                    _BroadcastRouter = value;
                    if(_BroadcastRouter != null)
                    {
                        _BroadcastRouter.CombineDelegates(this);
                    }
                }
            }
            public WithUserArgumentsRouter_Singlecast SinglecastRouter
            {
                [DebuggerNonUserCode]
                get
                {
                    return this._SinglecastRouter;
                }
                [DebuggerNonUserCode]
                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if(_SinglecastRouter != null)
                    {
                        _SinglecastRouter.RemoveDelegates(this);
                    }
                    _SinglecastRouter = value;
                    if(_SinglecastRouter != null)
                    {
                        _SinglecastRouter.CombineDelegates(this);
                    }
                }
            }
        }
        #endregion
        #region Bridge
        [Script]
        [CompilerGenerated]
        public partial class Bridge : IEvents, IMessages
        {
            public Action<Action> VirtualLatency;
            public Bridge()
            {
                this.VirtualLatency = VirtualLatencyDefaultImplemenetation;
            }
            public void VirtualLatencyDefaultImplemenetation(Action e)
            {
                e();
            }
            public event Action<RemoteEvents.Server_HelloArguments> Server_Hello;
            void IMessages.Server_Hello(int user, string name, int others)
            {
                if(Server_Hello == null) return;
                var v = new RemoteEvents.Server_HelloArguments { user = user, name = name, others = others };
                this.VirtualLatency(() => this.Server_Hello(v));
            }

            public event Action<RemoteEvents.Server_UserJoinedArguments> Server_UserJoined;
            void IMessages.Server_UserJoined(int user, string name)
            {
                if(Server_UserJoined == null) return;
                var v = new RemoteEvents.Server_UserJoinedArguments { user = user, name = name };
                this.VirtualLatency(() => this.Server_UserJoined(v));
            }

            public event Action<RemoteEvents.Server_UserLeftArguments> Server_UserLeft;
            void IMessages.Server_UserLeft(int user, string name)
            {
                if(Server_UserLeft == null) return;
                var v = new RemoteEvents.Server_UserLeftArguments { user = user, name = name };
                this.VirtualLatency(() => this.Server_UserLeft(v));
            }

            public event Action<RemoteEvents.HelloArguments> Hello;
            void IMessages.Hello(string name, int frame)
            {
                if(Hello == null) return;
                var v = new RemoteEvents.HelloArguments { name = name, frame = frame };
                this.VirtualLatency(() => this.Hello(v));
            }

            public event Action<RemoteEvents.UserHelloArguments> UserHello;
            void IMessages.UserHello(int user, string name, int frame)
            {
                if(UserHello == null) return;
                var v = new RemoteEvents.UserHelloArguments { user = user, name = name, frame = frame };
                this.VirtualLatency(() => this.UserHello(v));
            }

            public event Action<RemoteEvents.KeyStateChangedArguments> KeyStateChanged;
            void IMessages.KeyStateChanged(int local, int frame, int sequence, int key, int state)
            {
                if(KeyStateChanged == null) return;
                var v = new RemoteEvents.KeyStateChangedArguments { local = local, frame = frame, sequence = sequence, key = key, state = state };
                this.VirtualLatency(() => this.KeyStateChanged(v));
            }

            public event Action<RemoteEvents.UserKeyStateChangedArguments> UserKeyStateChanged;
            void IMessages.UserKeyStateChanged(int user, int local, int frame, int sequence, int key, int state)
            {
                if(UserKeyStateChanged == null) return;
                var v = new RemoteEvents.UserKeyStateChangedArguments { user = user, local = local, frame = frame, sequence = sequence, key = key, state = state };
                this.VirtualLatency(() => this.UserKeyStateChanged(v));
            }

            public event Action<RemoteEvents.TeleportToArguments> TeleportTo;
            void IMessages.TeleportTo(int frame, int local, int port, double x, double y, double vx, double vy)
            {
                if(TeleportTo == null) return;
                var v = new RemoteEvents.TeleportToArguments { frame = frame, local = local, port = port, x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.TeleportTo(v));
            }

            public event Action<RemoteEvents.UserTeleportToArguments> UserTeleportTo;
            void IMessages.UserTeleportTo(int user, int frame, int local, int port, double x, double y, double vx, double vy)
            {
                if(UserTeleportTo == null) return;
                var v = new RemoteEvents.UserTeleportToArguments { user = user, frame = frame, local = local, port = port, x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.UserTeleportTo(v));
            }

            public event Action<RemoteEvents.Vehicle_TeleportToArguments> Vehicle_TeleportTo;
            void IMessages.Vehicle_TeleportTo(int index, double x, double y, double vx, double vy)
            {
                if(Vehicle_TeleportTo == null) return;
                var v = new RemoteEvents.Vehicle_TeleportToArguments { index = index, x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.Vehicle_TeleportTo(v));
            }

            public event Action<RemoteEvents.UserVehicle_TeleportToArguments> UserVehicle_TeleportTo;
            void IMessages.UserVehicle_TeleportTo(int user, int index, double x, double y, double vx, double vy)
            {
                if(UserVehicle_TeleportTo == null) return;
                var v = new RemoteEvents.UserVehicle_TeleportToArguments { user = user, index = index, x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.UserVehicle_TeleportTo(v));
            }

            public event Action<RemoteEvents.LocalPlayers_IncreaseArguments> LocalPlayers_Increase;
            void IMessages.LocalPlayers_Increase(int frame)
            {
                if(LocalPlayers_Increase == null) return;
                var v = new RemoteEvents.LocalPlayers_IncreaseArguments { frame = frame };
                this.VirtualLatency(() => this.LocalPlayers_Increase(v));
            }

            public event Action<RemoteEvents.UserLocalPlayers_IncreaseArguments> UserLocalPlayers_Increase;
            void IMessages.UserLocalPlayers_Increase(int user, int frame)
            {
                if(UserLocalPlayers_Increase == null) return;
                var v = new RemoteEvents.UserLocalPlayers_IncreaseArguments { user = user, frame = frame };
                this.VirtualLatency(() => this.UserLocalPlayers_Increase(v));
            }

            public event Action<RemoteEvents.LocalPlayers_DecreaseArguments> LocalPlayers_Decrease;
            void IMessages.LocalPlayers_Decrease(int frame)
            {
                if(LocalPlayers_Decrease == null) return;
                var v = new RemoteEvents.LocalPlayers_DecreaseArguments { frame = frame };
                this.VirtualLatency(() => this.LocalPlayers_Decrease(v));
            }

            public event Action<RemoteEvents.UserLocalPlayers_DecreaseArguments> UserLocalPlayers_Decrease;
            void IMessages.UserLocalPlayers_Decrease(int user, int frame)
            {
                if(UserLocalPlayers_Decrease == null) return;
                var v = new RemoteEvents.UserLocalPlayers_DecreaseArguments { user = user, frame = frame };
                this.VirtualLatency(() => this.UserLocalPlayers_Decrease(v));
            }

            public event Action<RemoteEvents.EditorSelectorArguments> EditorSelector;
            void IMessages.EditorSelector(int frame, int type, int size, int x, int y)
            {
                if(EditorSelector == null) return;
                var v = new RemoteEvents.EditorSelectorArguments { frame = frame, type = type, size = size, x = x, y = y };
                this.VirtualLatency(() => this.EditorSelector(v));
            }

            public event Action<RemoteEvents.UserEditorSelectorArguments> UserEditorSelector;
            void IMessages.UserEditorSelector(int user, int frame, int type, int size, int x, int y)
            {
                if(UserEditorSelector == null) return;
                var v = new RemoteEvents.UserEditorSelectorArguments { user = user, frame = frame, type = type, size = size, x = x, y = y };
                this.VirtualLatency(() => this.UserEditorSelector(v));
            }

            public event Action<RemoteEvents.SyncFrameArguments> SyncFrame;
            void IMessages.SyncFrame(int frame, int framerate)
            {
                if(SyncFrame == null) return;
                var v = new RemoteEvents.SyncFrameArguments { frame = frame, framerate = framerate };
                this.VirtualLatency(() => this.SyncFrame(v));
            }

            public event Action<RemoteEvents.UserSyncFrameArguments> UserSyncFrame;
            void IMessages.UserSyncFrame(int user, int frame, int framerate)
            {
                if(UserSyncFrame == null) return;
                var v = new RemoteEvents.UserSyncFrameArguments { user = user, frame = frame, framerate = framerate };
                this.VirtualLatency(() => this.UserSyncFrame(v));
            }

            public event Action<RemoteEvents.SyncFrameEchoArguments> SyncFrameEcho;
            void IMessages.SyncFrameEcho(int frame, int framerate)
            {
                if(SyncFrameEcho == null) return;
                var v = new RemoteEvents.SyncFrameEchoArguments { frame = frame, framerate = framerate };
                this.VirtualLatency(() => this.SyncFrameEcho(v));
            }

            public event Action<RemoteEvents.UserSyncFrameEchoArguments> UserSyncFrameEcho;
            void IMessages.UserSyncFrameEcho(int user, int frame, int framerate)
            {
                if(UserSyncFrameEcho == null) return;
                var v = new RemoteEvents.UserSyncFrameEchoArguments { user = user, frame = frame, framerate = framerate };
                this.VirtualLatency(() => this.UserSyncFrameEcho(v));
            }

            public event Action<RemoteEvents.SetShakerEnabledArguments> SetShakerEnabled;
            void IMessages.SetShakerEnabled(int frame, int value)
            {
                if(SetShakerEnabled == null) return;
                var v = new RemoteEvents.SetShakerEnabledArguments { frame = frame, value = value };
                this.VirtualLatency(() => this.SetShakerEnabled(v));
            }

            public event Action<RemoteEvents.UserSetShakerEnabledArguments> UserSetShakerEnabled;
            void IMessages.UserSetShakerEnabled(int user, int frame, int value)
            {
                if(UserSetShakerEnabled == null) return;
                var v = new RemoteEvents.UserSetShakerEnabledArguments { user = user, frame = frame, value = value };
                this.VirtualLatency(() => this.UserSetShakerEnabled(v));
            }

            public event Action<RemoteEvents.SetPausedArguments> SetPaused;
            void IMessages.SetPaused(int frame)
            {
                if(SetPaused == null) return;
                var v = new RemoteEvents.SetPausedArguments { frame = frame };
                this.VirtualLatency(() => this.SetPaused(v));
            }

            public event Action<RemoteEvents.UserSetPausedArguments> UserSetPaused;
            void IMessages.UserSetPaused(int user, int frame)
            {
                if(UserSetPaused == null) return;
                var v = new RemoteEvents.UserSetPausedArguments { user = user, frame = frame };
                this.VirtualLatency(() => this.UserSetPaused(v));
            }

            public event Action<RemoteEvents.ClearPausedArguments> ClearPaused;
            void IMessages.ClearPaused()
            {
                if(ClearPaused == null) return;
                var v = new RemoteEvents.ClearPausedArguments {  };
                this.VirtualLatency(() => this.ClearPaused(v));
            }

            public event Action<RemoteEvents.UserClearPausedArguments> UserClearPaused;
            void IMessages.UserClearPaused(int user)
            {
                if(UserClearPaused == null) return;
                var v = new RemoteEvents.UserClearPausedArguments { user = user };
                this.VirtualLatency(() => this.UserClearPaused(v));
            }

            public event Action<RemoteEvents.LoadEmbeddedLevelArguments> LoadEmbeddedLevel;
            void IMessages.LoadEmbeddedLevel(int frame, int level)
            {
                if(LoadEmbeddedLevel == null) return;
                var v = new RemoteEvents.LoadEmbeddedLevelArguments { frame = frame, level = level };
                this.VirtualLatency(() => this.LoadEmbeddedLevel(v));
            }

            public event Action<RemoteEvents.UserLoadEmbeddedLevelArguments> UserLoadEmbeddedLevel;
            void IMessages.UserLoadEmbeddedLevel(int user, int frame, int level)
            {
                if(UserLoadEmbeddedLevel == null) return;
                var v = new RemoteEvents.UserLoadEmbeddedLevelArguments { user = user, frame = frame, level = level };
                this.VirtualLatency(() => this.UserLoadEmbeddedLevel(v));
            }

            public event Action<RemoteEvents.LoadCustomLevelArguments> LoadCustomLevel;
            void IMessages.LoadCustomLevel(int frame, string data)
            {
                if(LoadCustomLevel == null) return;
                var v = new RemoteEvents.LoadCustomLevelArguments { frame = frame, data = data };
                this.VirtualLatency(() => this.LoadCustomLevel(v));
            }

            public event Action<RemoteEvents.UserLoadCustomLevelArguments> UserLoadCustomLevel;
            void IMessages.UserLoadCustomLevel(int user, int frame, string data)
            {
                if(UserLoadCustomLevel == null) return;
                var v = new RemoteEvents.UserLoadCustomLevelArguments { user = user, frame = frame, data = data };
                this.VirtualLatency(() => this.UserLoadCustomLevel(v));
            }

        }
        #endregion
    }
    #endregion
}
// 4.01.2009 13:44:23
