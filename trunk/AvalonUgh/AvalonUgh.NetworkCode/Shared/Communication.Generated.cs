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
            public void Server_Hello(int user, string name)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Server_Hello, args = new object[] { user, name } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Server_Hello(user, name);
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
            public void Hello(string name)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.Hello, args = new object[] { name } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.Hello(name);
                    }
                }
            }
            public void UserHello(int user, string name)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserHello, args = new object[] { user, name } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserHello(user, name);
                    }
                }
            }
            public void KeyStateChanged(int local, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.KeyStateChanged, args = new object[] { local, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.KeyStateChanged(local, key, state);
                    }
                }
            }
            public void UserKeyStateChanged(int user, int local, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserKeyStateChanged, args = new object[] { user, local, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserKeyStateChanged(user, local, key, state);
                    }
                }
            }
            public void TeleportTo(int local, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.TeleportTo, args = new object[] { local, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.TeleportTo(local, x, y, vx, vy);
                    }
                }
            }
            public void UserTeleportTo(int user, int local, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserTeleportTo, args = new object[] { user, local, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserTeleportTo(user, local, x, y, vx, vy);
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
            public void LocalPlayers_Increase()
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LocalPlayers_Increase, args = new object[] {  } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LocalPlayers_Increase();
                    }
                }
            }
            public void UserLocalPlayers_Increase(int user)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLocalPlayers_Increase, args = new object[] { user } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLocalPlayers_Increase(user);
                    }
                }
            }
            public void LocalPlayers_Decrease()
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.LocalPlayers_Decrease, args = new object[] {  } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.LocalPlayers_Decrease();
                    }
                }
            }
            public void UserLocalPlayers_Decrease(int user)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserLocalPlayers_Decrease, args = new object[] { user } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserLocalPlayers_Decrease(user);
                    }
                }
            }
            public void EditorSelector(int type, int size, int x, int y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.EditorSelector, args = new object[] { type, size, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.EditorSelector(type, size, x, y);
                    }
                }
            }
            public void UserEditorSelector(int user, int type, int size, int x, int y)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserEditorSelector, args = new object[] { user, type, size, x, y } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserEditorSelector(user, type, size, x, y);
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
                }
                #endregion

                #region Routing
                public void UserHello(HelloArguments e)
                {
                    Target.UserHello(this.user, e.name);
                }
                public void UserKeyStateChanged(KeyStateChangedArguments e)
                {
                    Target.UserKeyStateChanged(this.user, e.local, e.key, e.state);
                }
                public void UserTeleportTo(TeleportToArguments e)
                {
                    Target.UserTeleportTo(this.user, e.local, e.x, e.y, e.vx, e.vy);
                }
                public void UserVehicle_TeleportTo(Vehicle_TeleportToArguments e)
                {
                    Target.UserVehicle_TeleportTo(this.user, e.index, e.x, e.y, e.vx, e.vy);
                }
                public void UserLocalPlayers_Increase(LocalPlayers_IncreaseArguments e)
                {
                    Target.UserLocalPlayers_Increase(this.user);
                }
                public void UserLocalPlayers_Decrease(LocalPlayers_DecreaseArguments e)
                {
                    Target.UserLocalPlayers_Decrease(this.user);
                }
                public void UserEditorSelector(EditorSelectorArguments e)
                {
                    Target.UserEditorSelector(this.user, e.type, e.size, e.x, e.y);
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
                public void UserHello(string name)
                {
                    this.Target.UserHello(this.user, name);
                }
                public void UserHello(UserHelloArguments e)
                {
                    this.Target.UserHello(this.user, e.name);
                }
                public void UserKeyStateChanged(int local, int key, int state)
                {
                    this.Target.UserKeyStateChanged(this.user, local, key, state);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    this.Target.UserKeyStateChanged(this.user, e.local, e.key, e.state);
                }
                public void UserTeleportTo(int local, double x, double y, double vx, double vy)
                {
                    this.Target.UserTeleportTo(this.user, local, x, y, vx, vy);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    this.Target.UserTeleportTo(this.user, e.local, e.x, e.y, e.vx, e.vy);
                }
                public void UserVehicle_TeleportTo(int index, double x, double y, double vx, double vy)
                {
                    this.Target.UserVehicle_TeleportTo(this.user, index, x, y, vx, vy);
                }
                public void UserVehicle_TeleportTo(UserVehicle_TeleportToArguments e)
                {
                    this.Target.UserVehicle_TeleportTo(this.user, e.index, e.x, e.y, e.vx, e.vy);
                }
                public void UserLocalPlayers_Increase()
                {
                    this.Target.UserLocalPlayers_Increase(this.user);
                }
                public void UserLocalPlayers_Increase(UserLocalPlayers_IncreaseArguments e)
                {
                    this.Target.UserLocalPlayers_Increase(this.user);
                }
                public void UserLocalPlayers_Decrease()
                {
                    this.Target.UserLocalPlayers_Decrease(this.user);
                }
                public void UserLocalPlayers_Decrease(UserLocalPlayers_DecreaseArguments e)
                {
                    this.Target.UserLocalPlayers_Decrease(this.user);
                }
                public void UserEditorSelector(int type, int size, int x, int y)
                {
                    this.Target.UserEditorSelector(this.user, type, size, x, y);
                }
                public void UserEditorSelector(UserEditorSelectorArguments e)
                {
                    this.Target.UserEditorSelector(this.user, e.type, e.size, e.x, e.y);
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
                }
                #endregion

                #region Routing
                public void UserHello(UserHelloArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserHello(this.user, e.name);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserKeyStateChanged(this.user, e.local, e.key, e.state);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserTeleportTo(this.user, e.local, e.x, e.y, e.vx, e.vy);
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
                    _target.UserLocalPlayers_Increase(this.user);
                }
                public void UserLocalPlayers_Decrease(UserLocalPlayers_DecreaseArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserLocalPlayers_Decrease(this.user);
                }
                public void UserEditorSelector(UserEditorSelectorArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserEditorSelector(this.user, e.type, e.size, e.x, e.y);
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
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(" }").ToString();
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
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ name = ").Append(this.name).Append(" }").ToString();
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
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", name = ").Append(this.name).Append(" }").ToString();
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
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ local = ").Append(this.local).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
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
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", local = ").Append(this.local).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserKeyStateChangedArguments> UserKeyStateChanged;
            #region TeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class TeleportToArguments
            {
                public int local;
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ local = ").Append(this.local).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<TeleportToArguments> TeleportTo;
            #region UserTeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserTeleportToArguments : WithUserArguments
            {
                public int local;
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", local = ").Append(this.local).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
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
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().ToString();
                }
            }
            #endregion
            public event Action<LocalPlayers_IncreaseArguments> LocalPlayers_Increase;
            #region UserLocalPlayers_IncreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLocalPlayers_IncreaseArguments : WithUserArguments
            {
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLocalPlayers_IncreaseArguments> UserLocalPlayers_Increase;
            #region LocalPlayers_DecreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class LocalPlayers_DecreaseArguments
            {
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().ToString();
                }
            }
            #endregion
            public event Action<LocalPlayers_DecreaseArguments> LocalPlayers_Decrease;
            #region UserLocalPlayers_DecreaseArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserLocalPlayers_DecreaseArguments : WithUserArguments
            {
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserLocalPlayers_DecreaseArguments> UserLocalPlayers_Decrease;
            #region EditorSelectorArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class EditorSelectorArguments
            {
                public int type;
                public int size;
                public int x;
                public int y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ type = ").Append(this.type).Append(", size = ").Append(this.size).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<EditorSelectorArguments> EditorSelector;
            #region UserEditorSelectorArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserEditorSelectorArguments : WithUserArguments
            {
                public int type;
                public int size;
                public int x;
                public int y;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", type = ").Append(this.type).Append(", size = ").Append(this.size).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserEditorSelectorArguments> UserEditorSelector;
            public RemoteEvents()
            {
                DispatchTable = new Dictionary<Messages, Action<IDispatchHelper>>
                        {
                            { Messages.Server_Hello, e => { Server_Hello(new Server_HelloArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Server_UserJoined, e => { Server_UserJoined(new Server_UserJoinedArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Server_UserLeft, e => { Server_UserLeft(new Server_UserLeftArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Hello, e => { Hello(new HelloArguments { name = e.GetString(0) }); } },
                            { Messages.UserHello, e => { UserHello(new UserHelloArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.KeyStateChanged, e => { KeyStateChanged(new KeyStateChangedArguments { local = e.GetInt32(0), key = e.GetInt32(1), state = e.GetInt32(2) }); } },
                            { Messages.UserKeyStateChanged, e => { UserKeyStateChanged(new UserKeyStateChangedArguments { user = e.GetInt32(0), local = e.GetInt32(1), key = e.GetInt32(2), state = e.GetInt32(3) }); } },
                            { Messages.TeleportTo, e => { TeleportTo(new TeleportToArguments { local = e.GetInt32(0), x = e.GetDouble(1), y = e.GetDouble(2), vx = e.GetDouble(3), vy = e.GetDouble(4) }); } },
                            { Messages.UserTeleportTo, e => { UserTeleportTo(new UserTeleportToArguments { user = e.GetInt32(0), local = e.GetInt32(1), x = e.GetDouble(2), y = e.GetDouble(3), vx = e.GetDouble(4), vy = e.GetDouble(5) }); } },
                            { Messages.Vehicle_TeleportTo, e => { Vehicle_TeleportTo(new Vehicle_TeleportToArguments { index = e.GetInt32(0), x = e.GetDouble(1), y = e.GetDouble(2), vx = e.GetDouble(3), vy = e.GetDouble(4) }); } },
                            { Messages.UserVehicle_TeleportTo, e => { UserVehicle_TeleportTo(new UserVehicle_TeleportToArguments { user = e.GetInt32(0), index = e.GetInt32(1), x = e.GetDouble(2), y = e.GetDouble(3), vx = e.GetDouble(4), vy = e.GetDouble(5) }); } },
                            { Messages.LocalPlayers_Increase, e => { LocalPlayers_Increase(new LocalPlayers_IncreaseArguments {  }); } },
                            { Messages.UserLocalPlayers_Increase, e => { UserLocalPlayers_Increase(new UserLocalPlayers_IncreaseArguments { user = e.GetInt32(0) }); } },
                            { Messages.LocalPlayers_Decrease, e => { LocalPlayers_Decrease(new LocalPlayers_DecreaseArguments {  }); } },
                            { Messages.UserLocalPlayers_Decrease, e => { UserLocalPlayers_Decrease(new UserLocalPlayers_DecreaseArguments { user = e.GetInt32(0) }); } },
                            { Messages.EditorSelector, e => { EditorSelector(new EditorSelectorArguments { type = e.GetInt32(0), size = e.GetInt32(1), x = e.GetInt32(2), y = e.GetInt32(3) }); } },
                            { Messages.UserEditorSelector, e => { UserEditorSelector(new UserEditorSelectorArguments { user = e.GetInt32(0), type = e.GetInt32(1), size = e.GetInt32(2), x = e.GetInt32(3), y = e.GetInt32(4) }); } },
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
            void IMessages.Server_Hello(int user, string name)
            {
                if(Server_Hello == null) return;
                var v = new RemoteEvents.Server_HelloArguments { user = user, name = name };
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
            void IMessages.Hello(string name)
            {
                if(Hello == null) return;
                var v = new RemoteEvents.HelloArguments { name = name };
                this.VirtualLatency(() => this.Hello(v));
            }

            public event Action<RemoteEvents.UserHelloArguments> UserHello;
            void IMessages.UserHello(int user, string name)
            {
                if(UserHello == null) return;
                var v = new RemoteEvents.UserHelloArguments { user = user, name = name };
                this.VirtualLatency(() => this.UserHello(v));
            }

            public event Action<RemoteEvents.KeyStateChangedArguments> KeyStateChanged;
            void IMessages.KeyStateChanged(int local, int key, int state)
            {
                if(KeyStateChanged == null) return;
                var v = new RemoteEvents.KeyStateChangedArguments { local = local, key = key, state = state };
                this.VirtualLatency(() => this.KeyStateChanged(v));
            }

            public event Action<RemoteEvents.UserKeyStateChangedArguments> UserKeyStateChanged;
            void IMessages.UserKeyStateChanged(int user, int local, int key, int state)
            {
                if(UserKeyStateChanged == null) return;
                var v = new RemoteEvents.UserKeyStateChangedArguments { user = user, local = local, key = key, state = state };
                this.VirtualLatency(() => this.UserKeyStateChanged(v));
            }

            public event Action<RemoteEvents.TeleportToArguments> TeleportTo;
            void IMessages.TeleportTo(int local, double x, double y, double vx, double vy)
            {
                if(TeleportTo == null) return;
                var v = new RemoteEvents.TeleportToArguments { local = local, x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.TeleportTo(v));
            }

            public event Action<RemoteEvents.UserTeleportToArguments> UserTeleportTo;
            void IMessages.UserTeleportTo(int user, int local, double x, double y, double vx, double vy)
            {
                if(UserTeleportTo == null) return;
                var v = new RemoteEvents.UserTeleportToArguments { user = user, local = local, x = x, y = y, vx = vx, vy = vy };
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
            void IMessages.LocalPlayers_Increase()
            {
                if(LocalPlayers_Increase == null) return;
                var v = new RemoteEvents.LocalPlayers_IncreaseArguments {  };
                this.VirtualLatency(() => this.LocalPlayers_Increase(v));
            }

            public event Action<RemoteEvents.UserLocalPlayers_IncreaseArguments> UserLocalPlayers_Increase;
            void IMessages.UserLocalPlayers_Increase(int user)
            {
                if(UserLocalPlayers_Increase == null) return;
                var v = new RemoteEvents.UserLocalPlayers_IncreaseArguments { user = user };
                this.VirtualLatency(() => this.UserLocalPlayers_Increase(v));
            }

            public event Action<RemoteEvents.LocalPlayers_DecreaseArguments> LocalPlayers_Decrease;
            void IMessages.LocalPlayers_Decrease()
            {
                if(LocalPlayers_Decrease == null) return;
                var v = new RemoteEvents.LocalPlayers_DecreaseArguments {  };
                this.VirtualLatency(() => this.LocalPlayers_Decrease(v));
            }

            public event Action<RemoteEvents.UserLocalPlayers_DecreaseArguments> UserLocalPlayers_Decrease;
            void IMessages.UserLocalPlayers_Decrease(int user)
            {
                if(UserLocalPlayers_Decrease == null) return;
                var v = new RemoteEvents.UserLocalPlayers_DecreaseArguments { user = user };
                this.VirtualLatency(() => this.UserLocalPlayers_Decrease(v));
            }

            public event Action<RemoteEvents.EditorSelectorArguments> EditorSelector;
            void IMessages.EditorSelector(int type, int size, int x, int y)
            {
                if(EditorSelector == null) return;
                var v = new RemoteEvents.EditorSelectorArguments { type = type, size = size, x = x, y = y };
                this.VirtualLatency(() => this.EditorSelector(v));
            }

            public event Action<RemoteEvents.UserEditorSelectorArguments> UserEditorSelector;
            void IMessages.UserEditorSelector(int user, int type, int size, int x, int y)
            {
                if(UserEditorSelector == null) return;
                var v = new RemoteEvents.UserEditorSelectorArguments { user = user, type = type, size = size, x = x, y = y };
                this.VirtualLatency(() => this.UserEditorSelector(v));
            }

        }
        #endregion
    }
    #endregion
}
// 21.12.2008 22:41:08
