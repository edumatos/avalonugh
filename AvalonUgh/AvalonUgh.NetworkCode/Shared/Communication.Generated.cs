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
            Server_Message,
            Server_LoadLevel,
            Server_Hello,
            Server_UserJoined,
            Server_UserLeft,
            UserHello,
            UserSynced,
            KeyStateChanged,
            UserKeyStateChanged,
            TeleportTo,
            UserTeleportTo,
            RemoveLocalPlayer,
            UserRemoveLocalPlayer,
            EditorSelector,
            UserEditorSelector,
            SyncFrame,
            UserSyncFrame,
            SyncFrameEcho,
            UserSyncFrameEcho,
            SetPaused,
            UserSetPaused,
            ClearPaused,
            UserClearPaused,
            LoadLevel,
            UserLoadLevel,
            LoadLevelHint,
            UserLoadLevelHint,
            MouseMove,
            UserMouseMove,
            MissionStartHint,
            UserMissionStartHint,
            Vehicalize,
            UserVehicalize,
            SyncCheck,
            UserSyncCheck,
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
            event Action<RemoteEvents.Server_MessageArguments> Server_Message;
            event Action<RemoteEvents.Server_LoadLevelArguments> Server_LoadLevel;
            event Action<RemoteEvents.Server_HelloArguments> Server_Hello;
            event Action<RemoteEvents.Server_UserJoinedArguments> Server_UserJoined;
            event Action<RemoteEvents.Server_UserLeftArguments> Server_UserLeft;
            event Action<RemoteEvents.UserHelloArguments> UserHello;
            event Action<RemoteEvents.UserSyncedArguments> UserSynced;
            event Action<RemoteEvents.KeyStateChangedArguments> KeyStateChanged;
            event Action<RemoteEvents.UserKeyStateChangedArguments> UserKeyStateChanged;
            event Action<RemoteEvents.TeleportToArguments> TeleportTo;
            event Action<RemoteEvents.UserTeleportToArguments> UserTeleportTo;
            event Action<RemoteEvents.RemoveLocalPlayerArguments> RemoveLocalPlayer;
            event Action<RemoteEvents.UserRemoveLocalPlayerArguments> UserRemoveLocalPlayer;
            event Action<RemoteEvents.EditorSelectorArguments> EditorSelector;
            event Action<RemoteEvents.UserEditorSelectorArguments> UserEditorSelector;
            event Action<RemoteEvents.SyncFrameArguments> SyncFrame;
            event Action<RemoteEvents.UserSyncFrameArguments> UserSyncFrame;
            event Action<RemoteEvents.SyncFrameEchoArguments> SyncFrameEcho;
            event Action<RemoteEvents.UserSyncFrameEchoArguments> UserSyncFrameEcho;
            event Action<RemoteEvents.SetPausedArguments> SetPaused;
            event Action<RemoteEvents.UserSetPausedArguments> UserSetPaused;
            event Action<RemoteEvents.ClearPausedArguments> ClearPaused;
            event Action<RemoteEvents.UserClearPausedArguments> UserClearPaused;
            event Action<RemoteEvents.LoadLevelArguments> LoadLevel;
            event Action<RemoteEvents.UserLoadLevelArguments> UserLoadLevel;
            event Action<RemoteEvents.LoadLevelHintArguments> LoadLevelHint;
            event Action<RemoteEvents.UserLoadLevelHintArguments> UserLoadLevelHint;
            event Action<RemoteEvents.MouseMoveArguments> MouseMove;
            event Action<RemoteEvents.UserMouseMoveArguments> UserMouseMove;
            event Action<RemoteEvents.MissionStartHintArguments> MissionStartHint;
            event Action<RemoteEvents.UserMissionStartHintArguments> UserMissionStartHint;
            event Action<RemoteEvents.VehicalizeArguments> Vehicalize;
            event Action<RemoteEvents.UserVehicalizeArguments> UserVehicalize;
            event Action<RemoteEvents.SyncCheckArguments> SyncCheck;
            event Action<RemoteEvents.UserSyncCheckArguments> UserSyncCheck;
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
            public void Server_Message(string text)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_Message, args = new object[] { text } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_Message(text);
                    }
                }
            }
            public void Server_LoadLevel(int index, string data)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_LoadLevel, args = new object[] { index, data } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_LoadLevel(index, data);
                    }
                }
            }
            public void Server_Hello(int user, string name, int others, int levels, int framelimit)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_Hello, args = new object[] { user, name, others, levels, framelimit } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_Hello(user, name, others, levels, framelimit);
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
            public void UserSynced(int user, int frame)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSynced, args = new object[] { user, frame } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSynced(user, frame);
                    }
                }
            }
            public void KeyStateChanged(int local, int frame, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.KeyStateChanged, args = new object[] { local, frame, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.KeyStateChanged(local, frame, key, state);
                    }
                }
            }
            public void UserKeyStateChanged(int user, int local, int frame, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserKeyStateChanged, args = new object[] { user, local, frame, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserKeyStateChanged(user, local, frame, key, state);
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
            public void RemoveLocalPlayer(int frame, int local)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.RemoveLocalPlayer, args = new object[] { frame, local } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.RemoveLocalPlayer(frame, local);
                    }
                }
            }
            public void UserRemoveLocalPlayer(int user, int frame, int local)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserRemoveLocalPlayer, args = new object[] { user, frame, local } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserRemoveLocalPlayer(user, frame, local);
                    }
                }
            }
            public void EditorSelector(int frame, int port, int type, int size, int x, int y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.EditorSelector, args = new object[] { frame, port, type, size, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.EditorSelector(frame, port, type, size, x, y);
                    }
                }
            }
            public void UserEditorSelector(int user, int frame, int port, int type, int size, int x, int y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserEditorSelector, args = new object[] { user, frame, port, type, size, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserEditorSelector(user, frame, port, type, size, x, y);
                    }
                }
            }
            public void SyncFrame(int frame, int framerate, int crc)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.SyncFrame, args = new object[] { frame, framerate, crc } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.SyncFrame(frame, framerate, crc);
                    }
                }
            }
            public void UserSyncFrame(int user, int frame, int framerate, int crc)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSyncFrame, args = new object[] { user, frame, framerate, crc } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSyncFrame(user, frame, framerate, crc);
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
            public void LoadLevel(int frame, int port, int level, string custom)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LoadLevel, args = new object[] { frame, port, level, custom } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LoadLevel(frame, port, level, custom);
                    }
                }
            }
            public void UserLoadLevel(int user, int port, int frame, int level, string custom)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLoadLevel, args = new object[] { user, port, frame, level, custom } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLoadLevel(user, port, frame, level, custom);
                    }
                }
            }
            public void LoadLevelHint(int port)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LoadLevelHint, args = new object[] { port } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LoadLevelHint(port);
                    }
                }
            }
            public void UserLoadLevelHint(int user, int port)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLoadLevelHint, args = new object[] { user, port } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLoadLevelHint(user, port);
                    }
                }
            }
            public void MouseMove(int port, double x, double y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.MouseMove, args = new object[] { port, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.MouseMove(port, x, y);
                    }
                }
            }
            public void UserMouseMove(int user, int port, double x, double y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserMouseMove, args = new object[] { user, port, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserMouseMove(user, port, x, y);
                    }
                }
            }
            public void MissionStartHint(int frame, int difficulty)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.MissionStartHint, args = new object[] { frame, difficulty } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.MissionStartHint(frame, difficulty);
                    }
                }
            }
            public void UserMissionStartHint(int user, int frame, int difficulty)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserMissionStartHint, args = new object[] { user, frame, difficulty } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserMissionStartHint(user, frame, difficulty);
                    }
                }
            }
            public void Vehicalize(int frame, int local)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Vehicalize, args = new object[] { frame, local } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Vehicalize(frame, local);
                    }
                }
            }
            public void UserVehicalize(int user, int frame, int local)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserVehicalize, args = new object[] { user, frame, local } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserVehicalize(user, frame, local);
                    }
                }
            }
            public void SyncCheck(int frame, int value)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.SyncCheck, args = new object[] { frame, value } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.SyncCheck(frame, value);
                    }
                }
            }
            public void UserSyncCheck(int user, int frame, int value)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserSyncCheck, args = new object[] { user, frame, value } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserSyncCheck(user, frame, value);
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
                    value.KeyStateChanged += this.UserKeyStateChanged;
                    value.TeleportTo += this.UserTeleportTo;
                    value.RemoveLocalPlayer += this.UserRemoveLocalPlayer;
                    value.EditorSelector += this.UserEditorSelector;
                    value.SyncFrame += this.UserSyncFrame;
                    value.SyncFrameEcho += this.UserSyncFrameEcho;
                    value.SetPaused += this.UserSetPaused;
                    value.ClearPaused += this.UserClearPaused;
                    value.LoadLevel += this.UserLoadLevel;
                    value.LoadLevelHint += this.UserLoadLevelHint;
                    value.MouseMove += this.UserMouseMove;
                    value.MissionStartHint += this.UserMissionStartHint;
                    value.Vehicalize += this.UserVehicalize;
                    value.SyncCheck += this.UserSyncCheck;
                }

                public void RemoveDelegates(IEvents value)
                {
                    value.KeyStateChanged -= this.UserKeyStateChanged;
                    value.TeleportTo -= this.UserTeleportTo;
                    value.RemoveLocalPlayer -= this.UserRemoveLocalPlayer;
                    value.EditorSelector -= this.UserEditorSelector;
                    value.SyncFrame -= this.UserSyncFrame;
                    value.SyncFrameEcho -= this.UserSyncFrameEcho;
                    value.SetPaused -= this.UserSetPaused;
                    value.ClearPaused -= this.UserClearPaused;
                    value.LoadLevel -= this.UserLoadLevel;
                    value.LoadLevelHint -= this.UserLoadLevelHint;
                    value.MouseMove -= this.UserMouseMove;
                    value.MissionStartHint -= this.UserMissionStartHint;
                    value.Vehicalize -= this.UserVehicalize;
                    value.SyncCheck -= this.UserSyncCheck;
                }
                #endregion

                #region Routing
                public void UserKeyStateChanged(KeyStateChangedArguments e)
                {
                    Target.UserKeyStateChanged(this.user, e.local, e.frame, e.key, e.state);
                }
                public void UserTeleportTo(TeleportToArguments e)
                {
                    Target.UserTeleportTo(this.user, e.frame, e.local, e.port, e.x, e.y, e.vx, e.vy);
                }
                public void UserRemoveLocalPlayer(RemoveLocalPlayerArguments e)
                {
                    Target.UserRemoveLocalPlayer(this.user, e.frame, e.local);
                }
                public void UserEditorSelector(EditorSelectorArguments e)
                {
                    Target.UserEditorSelector(this.user, e.frame, e.port, e.type, e.size, e.x, e.y);
                }
                public void UserSyncFrame(SyncFrameArguments e)
                {
                    Target.UserSyncFrame(this.user, e.frame, e.framerate, e.crc);
                }
                public void UserSyncFrameEcho(SyncFrameEchoArguments e)
                {
                    Target.UserSyncFrameEcho(this.user, e.frame, e.framerate);
                }
                public void UserSetPaused(SetPausedArguments e)
                {
                    Target.UserSetPaused(this.user, e.frame);
                }
                public void UserClearPaused(ClearPausedArguments e)
                {
                    Target.UserClearPaused(this.user);
                }
                public void UserLoadLevel(LoadLevelArguments e)
                {
                    Target.UserLoadLevel(this.user, e.port, e.frame, e.level, e.custom);
                }
                public void UserLoadLevelHint(LoadLevelHintArguments e)
                {
                    Target.UserLoadLevelHint(this.user, e.port);
                }
                public void UserMouseMove(MouseMoveArguments e)
                {
                    Target.UserMouseMove(this.user, e.port, e.x, e.y);
                }
                public void UserMissionStartHint(MissionStartHintArguments e)
                {
                    Target.UserMissionStartHint(this.user, e.frame, e.difficulty);
                }
                public void UserVehicalize(VehicalizeArguments e)
                {
                    Target.UserVehicalize(this.user, e.frame, e.local);
                }
                public void UserSyncCheck(SyncCheckArguments e)
                {
                    Target.UserSyncCheck(this.user, e.frame, e.value);
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
                public void UserSynced(int frame)
                {
                    this.Target.UserSynced(this.user, frame);
                }
                public void UserSynced(UserSyncedArguments e)
                {
                    this.Target.UserSynced(this.user, e.frame);
                }
                public void UserKeyStateChanged(int local, int frame, int key, int state)
                {
                    this.Target.UserKeyStateChanged(this.user, local, frame, key, state);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    this.Target.UserKeyStateChanged(this.user, e.local, e.frame, e.key, e.state);
                }
                public void UserTeleportTo(int frame, int local, int port, double x, double y, double vx, double vy)
                {
                    this.Target.UserTeleportTo(this.user, frame, local, port, x, y, vx, vy);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    this.Target.UserTeleportTo(this.user, e.frame, e.local, e.port, e.x, e.y, e.vx, e.vy);
                }
                public void UserRemoveLocalPlayer(int frame, int local)
                {
                    this.Target.UserRemoveLocalPlayer(this.user, frame, local);
                }
                public void UserRemoveLocalPlayer(UserRemoveLocalPlayerArguments e)
                {
                    this.Target.UserRemoveLocalPlayer(this.user, e.frame, e.local);
                }
                public void UserEditorSelector(int frame, int port, int type, int size, int x, int y)
                {
                    this.Target.UserEditorSelector(this.user, frame, port, type, size, x, y);
                }
                public void UserEditorSelector(UserEditorSelectorArguments e)
                {
                    this.Target.UserEditorSelector(this.user, e.frame, e.port, e.type, e.size, e.x, e.y);
                }
                public void UserSyncFrame(int frame, int framerate, int crc)
                {
                    this.Target.UserSyncFrame(this.user, frame, framerate, crc);
                }
                public void UserSyncFrame(UserSyncFrameArguments e)
                {
                    this.Target.UserSyncFrame(this.user, e.frame, e.framerate, e.crc);
                }
                public void UserSyncFrameEcho(int frame, int framerate)
                {
                    this.Target.UserSyncFrameEcho(this.user, frame, framerate);
                }
                public void UserSyncFrameEcho(UserSyncFrameEchoArguments e)
                {
                    this.Target.UserSyncFrameEcho(this.user, e.frame, e.framerate);
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
                public void UserLoadLevel(int port, int frame, int level, string custom)
                {
                    this.Target.UserLoadLevel(this.user, port, frame, level, custom);
                }
                public void UserLoadLevel(UserLoadLevelArguments e)
                {
                    this.Target.UserLoadLevel(this.user, e.port, e.frame, e.level, e.custom);
                }
                public void UserLoadLevelHint(int port)
                {
                    this.Target.UserLoadLevelHint(this.user, port);
                }
                public void UserLoadLevelHint(UserLoadLevelHintArguments e)
                {
                    this.Target.UserLoadLevelHint(this.user, e.port);
                }
                public void UserMouseMove(int port, double x, double y)
                {
                    this.Target.UserMouseMove(this.user, port, x, y);
                }
                public void UserMouseMove(UserMouseMoveArguments e)
                {
                    this.Target.UserMouseMove(this.user, e.port, e.x, e.y);
                }
                public void UserMissionStartHint(int frame, int difficulty)
                {
                    this.Target.UserMissionStartHint(this.user, frame, difficulty);
                }
                public void UserMissionStartHint(UserMissionStartHintArguments e)
                {
                    this.Target.UserMissionStartHint(this.user, e.frame, e.difficulty);
                }
                public void UserVehicalize(int frame, int local)
                {
                    this.Target.UserVehicalize(this.user, frame, local);
                }
                public void UserVehicalize(UserVehicalizeArguments e)
                {
                    this.Target.UserVehicalize(this.user, e.frame, e.local);
                }
                public void UserSyncCheck(int frame, int value)
                {
                    this.Target.UserSyncCheck(this.user, frame, value);
                }
                public void UserSyncCheck(UserSyncCheckArguments e)
                {
                    this.Target.UserSyncCheck(this.user, e.frame, e.value);
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
                    value.UserSynced += this.UserSynced;
                    value.UserKeyStateChanged += this.UserKeyStateChanged;
                    value.UserTeleportTo += this.UserTeleportTo;
                    value.UserRemoveLocalPlayer += this.UserRemoveLocalPlayer;
                    value.UserEditorSelector += this.UserEditorSelector;
                    value.UserSyncFrame += this.UserSyncFrame;
                    value.UserSyncFrameEcho += this.UserSyncFrameEcho;
                    value.UserSetPaused += this.UserSetPaused;
                    value.UserClearPaused += this.UserClearPaused;
                    value.UserLoadLevel += this.UserLoadLevel;
                    value.UserLoadLevelHint += this.UserLoadLevelHint;
                    value.UserMouseMove += this.UserMouseMove;
                    value.UserMissionStartHint += this.UserMissionStartHint;
                    value.UserVehicalize += this.UserVehicalize;
                    value.UserSyncCheck += this.UserSyncCheck;
                }

                public void RemoveDelegates(IEvents value)
                {
                    value.UserHello -= this.UserHello;
                    value.UserSynced -= this.UserSynced;
                    value.UserKeyStateChanged -= this.UserKeyStateChanged;
                    value.UserTeleportTo -= this.UserTeleportTo;
                    value.UserRemoveLocalPlayer -= this.UserRemoveLocalPlayer;
                    value.UserEditorSelector -= this.UserEditorSelector;
                    value.UserSyncFrame -= this.UserSyncFrame;
                    value.UserSyncFrameEcho -= this.UserSyncFrameEcho;
                    value.UserSetPaused -= this.UserSetPaused;
                    value.UserClearPaused -= this.UserClearPaused;
                    value.UserLoadLevel -= this.UserLoadLevel;
                    value.UserLoadLevelHint -= this.UserLoadLevelHint;
                    value.UserMouseMove -= this.UserMouseMove;
                    value.UserMissionStartHint -= this.UserMissionStartHint;
                    value.UserVehicalize -= this.UserVehicalize;
                    value.UserSyncCheck -= this.UserSyncCheck;
                }
                #endregion

                #region Routing
                public void UserHello(UserHelloArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserHello(this.user, e.name, e.frame);
                }
                public void UserSynced(UserSyncedArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSynced(this.user, e.frame);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserKeyStateChanged(this.user, e.local, e.frame, e.key, e.state);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserTeleportTo(this.user, e.frame, e.local, e.port, e.x, e.y, e.vx, e.vy);
                }
                public void UserRemoveLocalPlayer(UserRemoveLocalPlayerArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserRemoveLocalPlayer(this.user, e.frame, e.local);
                }
                public void UserEditorSelector(UserEditorSelectorArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserEditorSelector(this.user, e.frame, e.port, e.type, e.size, e.x, e.y);
                }
                public void UserSyncFrame(UserSyncFrameArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSyncFrame(this.user, e.frame, e.framerate, e.crc);
                }
                public void UserSyncFrameEcho(UserSyncFrameEchoArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSyncFrameEcho(this.user, e.frame, e.framerate);
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
                public void UserLoadLevel(UserLoadLevelArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLoadLevel(this.user, e.port, e.frame, e.level, e.custom);
                }
                public void UserLoadLevelHint(UserLoadLevelHintArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLoadLevelHint(this.user, e.port);
                }
                public void UserMouseMove(UserMouseMoveArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserMouseMove(this.user, e.port, e.x, e.y);
                }
                public void UserMissionStartHint(UserMissionStartHintArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserMissionStartHint(this.user, e.frame, e.difficulty);
                }
                public void UserVehicalize(UserVehicalizeArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserVehicalize(this.user, e.frame, e.local);
                }
                public void UserSyncCheck(UserSyncCheckArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserSyncCheck(this.user, e.frame, e.value);
                }
                #endregion
            }
            #endregion
            #region Server_MessageArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Server_MessageArguments
            {
                public string text;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ text = ").Append(this.text).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<Server_MessageArguments> Server_Message;
            #region Server_LoadLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Server_LoadLevelArguments
            {
                public int index;
                public string data;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ index = ").Append(this.index).Append(", data = ").Append(this.data).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<Server_LoadLevelArguments> Server_LoadLevel;
            #region Server_HelloArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class Server_HelloArguments
            {
                public int user;
                public string name;
                public int others;
                public int levels;
                public int framelimit;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(", others = ").Append(this.others).Append(", levels = ").Append(this.levels).Append(", framelimit = ").Append(this.framelimit).Append(" }").ToString();
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
            #region UserSyncedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserSyncedArguments : WithUserArguments
            {
                public int frame;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserSyncedArguments> UserSynced;
            #region KeyStateChangedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class KeyStateChangedArguments
            {
                public int local;
                public int frame;
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ local = ").Append(this.local).Append(", frame = ").Append(this.frame).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
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
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", local = ").Append(this.local).Append(", frame = ").Append(this.frame).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
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
            #region RemoveLocalPlayerArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class RemoveLocalPlayerArguments
            {
                public int frame;
                public int local;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", local = ").Append(this.local).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<RemoveLocalPlayerArguments> RemoveLocalPlayer;
            #region UserRemoveLocalPlayerArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserRemoveLocalPlayerArguments : WithUserArguments
            {
                public int frame;
                public int local;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", local = ").Append(this.local).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserRemoveLocalPlayerArguments> UserRemoveLocalPlayer;
            #region EditorSelectorArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class EditorSelectorArguments
            {
                public int frame;
                public int port;
                public int type;
                public int size;
                public int x;
                public int y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", port = ").Append(this.port).Append(", type = ").Append(this.type).Append(", size = ").Append(this.size).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
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
                public int port;
                public int type;
                public int size;
                public int x;
                public int y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", port = ").Append(this.port).Append(", type = ").Append(this.type).Append(", size = ").Append(this.size).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
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
                public int crc;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", framerate = ").Append(this.framerate).Append(", crc = ").Append(this.crc).Append(" }").ToString();
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
                public int crc;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", framerate = ").Append(this.framerate).Append(", crc = ").Append(this.crc).Append(" }").ToString();
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
            #region LoadLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LoadLevelArguments
            {
                public int frame;
                public int port;
                public int level;
                public string custom;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", port = ").Append(this.port).Append(", level = ").Append(this.level).Append(", custom = ").Append(this.custom).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<LoadLevelArguments> LoadLevel;
            #region UserLoadLevelArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLoadLevelArguments : WithUserArguments
            {
                public int port;
                public int frame;
                public int level;
                public string custom;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", port = ").Append(this.port).Append(", frame = ").Append(this.frame).Append(", level = ").Append(this.level).Append(", custom = ").Append(this.custom).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLoadLevelArguments> UserLoadLevel;
            #region LoadLevelHintArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LoadLevelHintArguments
            {
                public int port;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ port = ").Append(this.port).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<LoadLevelHintArguments> LoadLevelHint;
            #region UserLoadLevelHintArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLoadLevelHintArguments : WithUserArguments
            {
                public int port;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", port = ").Append(this.port).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLoadLevelHintArguments> UserLoadLevelHint;
            #region MouseMoveArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class MouseMoveArguments
            {
                public int port;
                public double x;
                public double y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ port = ").Append(this.port).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<MouseMoveArguments> MouseMove;
            #region UserMouseMoveArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserMouseMoveArguments : WithUserArguments
            {
                public int port;
                public double x;
                public double y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", port = ").Append(this.port).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserMouseMoveArguments> UserMouseMove;
            #region MissionStartHintArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class MissionStartHintArguments
            {
                public int frame;
                public int difficulty;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", difficulty = ").Append(this.difficulty).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<MissionStartHintArguments> MissionStartHint;
            #region UserMissionStartHintArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserMissionStartHintArguments : WithUserArguments
            {
                public int frame;
                public int difficulty;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", difficulty = ").Append(this.difficulty).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserMissionStartHintArguments> UserMissionStartHint;
            #region VehicalizeArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class VehicalizeArguments
            {
                public int frame;
                public int local;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ frame = ").Append(this.frame).Append(", local = ").Append(this.local).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<VehicalizeArguments> Vehicalize;
            #region UserVehicalizeArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserVehicalizeArguments : WithUserArguments
            {
                public int frame;
                public int local;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", frame = ").Append(this.frame).Append(", local = ").Append(this.local).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserVehicalizeArguments> UserVehicalize;
            #region SyncCheckArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class SyncCheckArguments
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
            public event Action<SyncCheckArguments> SyncCheck;
            #region UserSyncCheckArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserSyncCheckArguments : WithUserArguments
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
            public event Action<UserSyncCheckArguments> UserSyncCheck;
            public RemoteEvents()
            {
                DispatchTable = new Dictionary<Messages, Action<IDispatchHelper>>
                        {
                            { Messages.Server_Message, e => { Server_Message(new Server_MessageArguments { text = e.GetString(0) }); } },
                            { Messages.Server_LoadLevel, e => { Server_LoadLevel(new Server_LoadLevelArguments { index = e.GetInt32(0), data = e.GetString(1) }); } },
                            { Messages.Server_Hello, e => { Server_Hello(new Server_HelloArguments { user = e.GetInt32(0), name = e.GetString(1), others = e.GetInt32(2), levels = e.GetInt32(3), framelimit = e.GetInt32(4) }); } },
                            { Messages.Server_UserJoined, e => { Server_UserJoined(new Server_UserJoinedArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Server_UserLeft, e => { Server_UserLeft(new Server_UserLeftArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.UserHello, e => { UserHello(new UserHelloArguments { user = e.GetInt32(0), name = e.GetString(1), frame = e.GetInt32(2) }); } },
                            { Messages.UserSynced, e => { UserSynced(new UserSyncedArguments { user = e.GetInt32(0), frame = e.GetInt32(1) }); } },
                            { Messages.KeyStateChanged, e => { KeyStateChanged(new KeyStateChangedArguments { local = e.GetInt32(0), frame = e.GetInt32(1), key = e.GetInt32(2), state = e.GetInt32(3) }); } },
                            { Messages.UserKeyStateChanged, e => { UserKeyStateChanged(new UserKeyStateChangedArguments { user = e.GetInt32(0), local = e.GetInt32(1), frame = e.GetInt32(2), key = e.GetInt32(3), state = e.GetInt32(4) }); } },
                            { Messages.TeleportTo, e => { TeleportTo(new TeleportToArguments { frame = e.GetInt32(0), local = e.GetInt32(1), port = e.GetInt32(2), x = e.GetDouble(3), y = e.GetDouble(4), vx = e.GetDouble(5), vy = e.GetDouble(6) }); } },
                            { Messages.UserTeleportTo, e => { UserTeleportTo(new UserTeleportToArguments { user = e.GetInt32(0), frame = e.GetInt32(1), local = e.GetInt32(2), port = e.GetInt32(3), x = e.GetDouble(4), y = e.GetDouble(5), vx = e.GetDouble(6), vy = e.GetDouble(7) }); } },
                            { Messages.RemoveLocalPlayer, e => { RemoveLocalPlayer(new RemoveLocalPlayerArguments { frame = e.GetInt32(0), local = e.GetInt32(1) }); } },
                            { Messages.UserRemoveLocalPlayer, e => { UserRemoveLocalPlayer(new UserRemoveLocalPlayerArguments { user = e.GetInt32(0), frame = e.GetInt32(1), local = e.GetInt32(2) }); } },
                            { Messages.EditorSelector, e => { EditorSelector(new EditorSelectorArguments { frame = e.GetInt32(0), port = e.GetInt32(1), type = e.GetInt32(2), size = e.GetInt32(3), x = e.GetInt32(4), y = e.GetInt32(5) }); } },
                            { Messages.UserEditorSelector, e => { UserEditorSelector(new UserEditorSelectorArguments { user = e.GetInt32(0), frame = e.GetInt32(1), port = e.GetInt32(2), type = e.GetInt32(3), size = e.GetInt32(4), x = e.GetInt32(5), y = e.GetInt32(6) }); } },
                            { Messages.SyncFrame, e => { SyncFrame(new SyncFrameArguments { frame = e.GetInt32(0), framerate = e.GetInt32(1), crc = e.GetInt32(2) }); } },
                            { Messages.UserSyncFrame, e => { UserSyncFrame(new UserSyncFrameArguments { user = e.GetInt32(0), frame = e.GetInt32(1), framerate = e.GetInt32(2), crc = e.GetInt32(3) }); } },
                            { Messages.SyncFrameEcho, e => { SyncFrameEcho(new SyncFrameEchoArguments { frame = e.GetInt32(0), framerate = e.GetInt32(1) }); } },
                            { Messages.UserSyncFrameEcho, e => { UserSyncFrameEcho(new UserSyncFrameEchoArguments { user = e.GetInt32(0), frame = e.GetInt32(1), framerate = e.GetInt32(2) }); } },
                            { Messages.SetPaused, e => { SetPaused(new SetPausedArguments { frame = e.GetInt32(0) }); } },
                            { Messages.UserSetPaused, e => { UserSetPaused(new UserSetPausedArguments { user = e.GetInt32(0), frame = e.GetInt32(1) }); } },
                            { Messages.ClearPaused, e => { ClearPaused(new ClearPausedArguments {  }); } },
                            { Messages.UserClearPaused, e => { UserClearPaused(new UserClearPausedArguments { user = e.GetInt32(0) }); } },
                            { Messages.LoadLevel, e => { LoadLevel(new LoadLevelArguments { frame = e.GetInt32(0), port = e.GetInt32(1), level = e.GetInt32(2), custom = e.GetString(3) }); } },
                            { Messages.UserLoadLevel, e => { UserLoadLevel(new UserLoadLevelArguments { user = e.GetInt32(0), port = e.GetInt32(1), frame = e.GetInt32(2), level = e.GetInt32(3), custom = e.GetString(4) }); } },
                            { Messages.LoadLevelHint, e => { LoadLevelHint(new LoadLevelHintArguments { port = e.GetInt32(0) }); } },
                            { Messages.UserLoadLevelHint, e => { UserLoadLevelHint(new UserLoadLevelHintArguments { user = e.GetInt32(0), port = e.GetInt32(1) }); } },
                            { Messages.MouseMove, e => { MouseMove(new MouseMoveArguments { port = e.GetInt32(0), x = e.GetDouble(1), y = e.GetDouble(2) }); } },
                            { Messages.UserMouseMove, e => { UserMouseMove(new UserMouseMoveArguments { user = e.GetInt32(0), port = e.GetInt32(1), x = e.GetDouble(2), y = e.GetDouble(3) }); } },
                            { Messages.MissionStartHint, e => { MissionStartHint(new MissionStartHintArguments { frame = e.GetInt32(0), difficulty = e.GetInt32(1) }); } },
                            { Messages.UserMissionStartHint, e => { UserMissionStartHint(new UserMissionStartHintArguments { user = e.GetInt32(0), frame = e.GetInt32(1), difficulty = e.GetInt32(2) }); } },
                            { Messages.Vehicalize, e => { Vehicalize(new VehicalizeArguments { frame = e.GetInt32(0), local = e.GetInt32(1) }); } },
                            { Messages.UserVehicalize, e => { UserVehicalize(new UserVehicalizeArguments { user = e.GetInt32(0), frame = e.GetInt32(1), local = e.GetInt32(2) }); } },
                            { Messages.SyncCheck, e => { SyncCheck(new SyncCheckArguments { frame = e.GetInt32(0), value = e.GetInt32(1) }); } },
                            { Messages.UserSyncCheck, e => { UserSyncCheck(new UserSyncCheckArguments { user = e.GetInt32(0), frame = e.GetInt32(1), value = e.GetInt32(2) }); } },
                        }
                ;
                DispatchTableDelegates = new Dictionary<Messages, Converter<object, Delegate>>
                        {
                            { Messages.Server_Message, e => Server_Message },
                            { Messages.Server_LoadLevel, e => Server_LoadLevel },
                            { Messages.Server_Hello, e => Server_Hello },
                            { Messages.Server_UserJoined, e => Server_UserJoined },
                            { Messages.Server_UserLeft, e => Server_UserLeft },
                            { Messages.UserHello, e => UserHello },
                            { Messages.UserSynced, e => UserSynced },
                            { Messages.KeyStateChanged, e => KeyStateChanged },
                            { Messages.UserKeyStateChanged, e => UserKeyStateChanged },
                            { Messages.TeleportTo, e => TeleportTo },
                            { Messages.UserTeleportTo, e => UserTeleportTo },
                            { Messages.RemoveLocalPlayer, e => RemoveLocalPlayer },
                            { Messages.UserRemoveLocalPlayer, e => UserRemoveLocalPlayer },
                            { Messages.EditorSelector, e => EditorSelector },
                            { Messages.UserEditorSelector, e => UserEditorSelector },
                            { Messages.SyncFrame, e => SyncFrame },
                            { Messages.UserSyncFrame, e => UserSyncFrame },
                            { Messages.SyncFrameEcho, e => SyncFrameEcho },
                            { Messages.UserSyncFrameEcho, e => UserSyncFrameEcho },
                            { Messages.SetPaused, e => SetPaused },
                            { Messages.UserSetPaused, e => UserSetPaused },
                            { Messages.ClearPaused, e => ClearPaused },
                            { Messages.UserClearPaused, e => UserClearPaused },
                            { Messages.LoadLevel, e => LoadLevel },
                            { Messages.UserLoadLevel, e => UserLoadLevel },
                            { Messages.LoadLevelHint, e => LoadLevelHint },
                            { Messages.UserLoadLevelHint, e => UserLoadLevelHint },
                            { Messages.MouseMove, e => MouseMove },
                            { Messages.UserMouseMove, e => UserMouseMove },
                            { Messages.MissionStartHint, e => MissionStartHint },
                            { Messages.UserMissionStartHint, e => UserMissionStartHint },
                            { Messages.Vehicalize, e => Vehicalize },
                            { Messages.UserVehicalize, e => UserVehicalize },
                            { Messages.SyncCheck, e => SyncCheck },
                            { Messages.UserSyncCheck, e => UserSyncCheck },
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
            public event Action<RemoteEvents.Server_MessageArguments> Server_Message;
            void IMessages.Server_Message(string text)
            {
                if(Server_Message == null) return;
                var v = new RemoteEvents.Server_MessageArguments { text = text };
                this.VirtualLatency(() => this.Server_Message(v));
            }

            public event Action<RemoteEvents.Server_LoadLevelArguments> Server_LoadLevel;
            void IMessages.Server_LoadLevel(int index, string data)
            {
                if(Server_LoadLevel == null) return;
                var v = new RemoteEvents.Server_LoadLevelArguments { index = index, data = data };
                this.VirtualLatency(() => this.Server_LoadLevel(v));
            }

            public event Action<RemoteEvents.Server_HelloArguments> Server_Hello;
            void IMessages.Server_Hello(int user, string name, int others, int levels, int framelimit)
            {
                if(Server_Hello == null) return;
                var v = new RemoteEvents.Server_HelloArguments { user = user, name = name, others = others, levels = levels, framelimit = framelimit };
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

            public event Action<RemoteEvents.UserHelloArguments> UserHello;
            void IMessages.UserHello(int user, string name, int frame)
            {
                if(UserHello == null) return;
                var v = new RemoteEvents.UserHelloArguments { user = user, name = name, frame = frame };
                this.VirtualLatency(() => this.UserHello(v));
            }

            public event Action<RemoteEvents.UserSyncedArguments> UserSynced;
            void IMessages.UserSynced(int user, int frame)
            {
                if(UserSynced == null) return;
                var v = new RemoteEvents.UserSyncedArguments { user = user, frame = frame };
                this.VirtualLatency(() => this.UserSynced(v));
            }

            public event Action<RemoteEvents.KeyStateChangedArguments> KeyStateChanged;
            void IMessages.KeyStateChanged(int local, int frame, int key, int state)
            {
                if(KeyStateChanged == null) return;
                var v = new RemoteEvents.KeyStateChangedArguments { local = local, frame = frame, key = key, state = state };
                this.VirtualLatency(() => this.KeyStateChanged(v));
            }

            public event Action<RemoteEvents.UserKeyStateChangedArguments> UserKeyStateChanged;
            void IMessages.UserKeyStateChanged(int user, int local, int frame, int key, int state)
            {
                if(UserKeyStateChanged == null) return;
                var v = new RemoteEvents.UserKeyStateChangedArguments { user = user, local = local, frame = frame, key = key, state = state };
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

            public event Action<RemoteEvents.RemoveLocalPlayerArguments> RemoveLocalPlayer;
            void IMessages.RemoveLocalPlayer(int frame, int local)
            {
                if(RemoveLocalPlayer == null) return;
                var v = new RemoteEvents.RemoveLocalPlayerArguments { frame = frame, local = local };
                this.VirtualLatency(() => this.RemoveLocalPlayer(v));
            }

            public event Action<RemoteEvents.UserRemoveLocalPlayerArguments> UserRemoveLocalPlayer;
            void IMessages.UserRemoveLocalPlayer(int user, int frame, int local)
            {
                if(UserRemoveLocalPlayer == null) return;
                var v = new RemoteEvents.UserRemoveLocalPlayerArguments { user = user, frame = frame, local = local };
                this.VirtualLatency(() => this.UserRemoveLocalPlayer(v));
            }

            public event Action<RemoteEvents.EditorSelectorArguments> EditorSelector;
            void IMessages.EditorSelector(int frame, int port, int type, int size, int x, int y)
            {
                if(EditorSelector == null) return;
                var v = new RemoteEvents.EditorSelectorArguments { frame = frame, port = port, type = type, size = size, x = x, y = y };
                this.VirtualLatency(() => this.EditorSelector(v));
            }

            public event Action<RemoteEvents.UserEditorSelectorArguments> UserEditorSelector;
            void IMessages.UserEditorSelector(int user, int frame, int port, int type, int size, int x, int y)
            {
                if(UserEditorSelector == null) return;
                var v = new RemoteEvents.UserEditorSelectorArguments { user = user, frame = frame, port = port, type = type, size = size, x = x, y = y };
                this.VirtualLatency(() => this.UserEditorSelector(v));
            }

            public event Action<RemoteEvents.SyncFrameArguments> SyncFrame;
            void IMessages.SyncFrame(int frame, int framerate, int crc)
            {
                if(SyncFrame == null) return;
                var v = new RemoteEvents.SyncFrameArguments { frame = frame, framerate = framerate, crc = crc };
                this.VirtualLatency(() => this.SyncFrame(v));
            }

            public event Action<RemoteEvents.UserSyncFrameArguments> UserSyncFrame;
            void IMessages.UserSyncFrame(int user, int frame, int framerate, int crc)
            {
                if(UserSyncFrame == null) return;
                var v = new RemoteEvents.UserSyncFrameArguments { user = user, frame = frame, framerate = framerate, crc = crc };
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

            public event Action<RemoteEvents.LoadLevelArguments> LoadLevel;
            void IMessages.LoadLevel(int frame, int port, int level, string custom)
            {
                if(LoadLevel == null) return;
                var v = new RemoteEvents.LoadLevelArguments { frame = frame, port = port, level = level, custom = custom };
                this.VirtualLatency(() => this.LoadLevel(v));
            }

            public event Action<RemoteEvents.UserLoadLevelArguments> UserLoadLevel;
            void IMessages.UserLoadLevel(int user, int port, int frame, int level, string custom)
            {
                if(UserLoadLevel == null) return;
                var v = new RemoteEvents.UserLoadLevelArguments { user = user, port = port, frame = frame, level = level, custom = custom };
                this.VirtualLatency(() => this.UserLoadLevel(v));
            }

            public event Action<RemoteEvents.LoadLevelHintArguments> LoadLevelHint;
            void IMessages.LoadLevelHint(int port)
            {
                if(LoadLevelHint == null) return;
                var v = new RemoteEvents.LoadLevelHintArguments { port = port };
                this.VirtualLatency(() => this.LoadLevelHint(v));
            }

            public event Action<RemoteEvents.UserLoadLevelHintArguments> UserLoadLevelHint;
            void IMessages.UserLoadLevelHint(int user, int port)
            {
                if(UserLoadLevelHint == null) return;
                var v = new RemoteEvents.UserLoadLevelHintArguments { user = user, port = port };
                this.VirtualLatency(() => this.UserLoadLevelHint(v));
            }

            public event Action<RemoteEvents.MouseMoveArguments> MouseMove;
            void IMessages.MouseMove(int port, double x, double y)
            {
                if(MouseMove == null) return;
                var v = new RemoteEvents.MouseMoveArguments { port = port, x = x, y = y };
                this.VirtualLatency(() => this.MouseMove(v));
            }

            public event Action<RemoteEvents.UserMouseMoveArguments> UserMouseMove;
            void IMessages.UserMouseMove(int user, int port, double x, double y)
            {
                if(UserMouseMove == null) return;
                var v = new RemoteEvents.UserMouseMoveArguments { user = user, port = port, x = x, y = y };
                this.VirtualLatency(() => this.UserMouseMove(v));
            }

            public event Action<RemoteEvents.MissionStartHintArguments> MissionStartHint;
            void IMessages.MissionStartHint(int frame, int difficulty)
            {
                if(MissionStartHint == null) return;
                var v = new RemoteEvents.MissionStartHintArguments { frame = frame, difficulty = difficulty };
                this.VirtualLatency(() => this.MissionStartHint(v));
            }

            public event Action<RemoteEvents.UserMissionStartHintArguments> UserMissionStartHint;
            void IMessages.UserMissionStartHint(int user, int frame, int difficulty)
            {
                if(UserMissionStartHint == null) return;
                var v = new RemoteEvents.UserMissionStartHintArguments { user = user, frame = frame, difficulty = difficulty };
                this.VirtualLatency(() => this.UserMissionStartHint(v));
            }

            public event Action<RemoteEvents.VehicalizeArguments> Vehicalize;
            void IMessages.Vehicalize(int frame, int local)
            {
                if(Vehicalize == null) return;
                var v = new RemoteEvents.VehicalizeArguments { frame = frame, local = local };
                this.VirtualLatency(() => this.Vehicalize(v));
            }

            public event Action<RemoteEvents.UserVehicalizeArguments> UserVehicalize;
            void IMessages.UserVehicalize(int user, int frame, int local)
            {
                if(UserVehicalize == null) return;
                var v = new RemoteEvents.UserVehicalizeArguments { user = user, frame = frame, local = local };
                this.VirtualLatency(() => this.UserVehicalize(v));
            }

            public event Action<RemoteEvents.SyncCheckArguments> SyncCheck;
            void IMessages.SyncCheck(int frame, int value)
            {
                if(SyncCheck == null) return;
                var v = new RemoteEvents.SyncCheckArguments { frame = frame, value = value };
                this.VirtualLatency(() => this.SyncCheck(v));
            }

            public event Action<RemoteEvents.UserSyncCheckArguments> UserSyncCheck;
            void IMessages.UserSyncCheck(int user, int frame, int value)
            {
                if(UserSyncCheck == null) return;
                var v = new RemoteEvents.UserSyncCheckArguments { user = user, frame = frame, value = value };
                this.VirtualLatency(() => this.UserSyncCheck(v));
            }

        }
        #endregion
    }
    #endregion
}
// 8.02.2009 17:37:53
