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
            public void KeyStateChanged(int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.KeyStateChanged, args = new object[] { key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.KeyStateChanged(key, state);
                    }
                }
            }
            public void UserKeyStateChanged(int user, int key, int state)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserKeyStateChanged, args = new object[] { user, key, state } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserKeyStateChanged(user, key, state);
                    }
                }
            }
            public void TeleportTo(double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.TeleportTo, args = new object[] { x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.TeleportTo(x, y, vx, vy);
                    }
                }
            }
            public void UserTeleportTo(int user, double x, double y, double vx, double vy)
            {
                if (this.Send != null)
                {
                    Send(new SendArguments { i = Messages.UserTeleportTo, args = new object[] { user, x, y, vx, vy } });
                }
                if (this.VirtualTargets != null)
                {
                    foreach (var Target__ in this.VirtualTargets())
                    {
                        Target__.UserTeleportTo(user, x, y, vx, vy);
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
                }

                public void RemoveDelegates(IEvents value)
                {
                    value.Hello -= this.UserHello;
                    value.KeyStateChanged -= this.UserKeyStateChanged;
                    value.TeleportTo -= this.UserTeleportTo;
                }
                #endregion

                #region Routing
                public void UserHello(HelloArguments e)
                {
                    Target.UserHello(this.user, e.name);
                }
                public void UserKeyStateChanged(KeyStateChangedArguments e)
                {
                    Target.UserKeyStateChanged(this.user, e.key, e.state);
                }
                public void UserTeleportTo(TeleportToArguments e)
                {
                    Target.UserTeleportTo(this.user, e.x, e.y, e.vx, e.vy);
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
                public void UserKeyStateChanged(int key, int state)
                {
                    this.Target.UserKeyStateChanged(this.user, key, state);
                }
                public void UserKeyStateChanged(UserKeyStateChangedArguments e)
                {
                    this.Target.UserKeyStateChanged(this.user, e.key, e.state);
                }
                public void UserTeleportTo(double x, double y, double vx, double vy)
                {
                    this.Target.UserTeleportTo(this.user, x, y, vx, vy);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    this.Target.UserTeleportTo(this.user, e.x, e.y, e.vx, e.vy);
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
                }

                public void RemoveDelegates(IEvents value)
                {
                    value.UserHello -= this.UserHello;
                    value.UserKeyStateChanged -= this.UserKeyStateChanged;
                    value.UserTeleportTo -= this.UserTeleportTo;
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
                    _target.UserKeyStateChanged(this.user, e.key, e.state);
                }
                public void UserTeleportTo(UserTeleportToArguments e)
                {
                    var _target = this.Target(e.user);
                    if (_target == null) return;
                    _target.UserTeleportTo(this.user, e.x, e.y, e.vx, e.vy);
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
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<KeyStateChangedArguments> KeyStateChanged;
            #region UserKeyStateChangedArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserKeyStateChangedArguments : WithUserArguments
            {
                public int key;
                public int state;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", key = ").Append(this.key).Append(", state = ").Append(this.state).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserKeyStateChangedArguments> UserKeyStateChanged;
            #region TeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class TeleportToArguments
            {
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<TeleportToArguments> TeleportTo;
            #region UserTeleportToArguments
            [Script]
            [CompilerGenerated]
            public sealed partial class UserTeleportToArguments : WithUserArguments
            {
                public double x;
                public double y;
                public double vx;
                public double vy;
                [DebuggerHidden]
                public override string ToString()
                {
                    return new StringBuilder().Append("{ user = ").Append(this.user).Append(", x = ").Append(this.x).Append(", y = ").Append(this.y).Append(", vx = ").Append(this.vx).Append(", vy = ").Append(this.vy).Append(" }").ToString();
                }
            }
            #endregion
            public event Action<UserTeleportToArguments> UserTeleportTo;
            public RemoteEvents()
            {
                DispatchTable = new Dictionary<Messages, Action<IDispatchHelper>>
                        {
                            { Messages.Server_Hello, e => { Server_Hello(new Server_HelloArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Server_UserJoined, e => { Server_UserJoined(new Server_UserJoinedArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Server_UserLeft, e => { Server_UserLeft(new Server_UserLeftArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.Hello, e => { Hello(new HelloArguments { name = e.GetString(0) }); } },
                            { Messages.UserHello, e => { UserHello(new UserHelloArguments { user = e.GetInt32(0), name = e.GetString(1) }); } },
                            { Messages.KeyStateChanged, e => { KeyStateChanged(new KeyStateChangedArguments { key = e.GetInt32(0), state = e.GetInt32(1) }); } },
                            { Messages.UserKeyStateChanged, e => { UserKeyStateChanged(new UserKeyStateChangedArguments { user = e.GetInt32(0), key = e.GetInt32(1), state = e.GetInt32(2) }); } },
                            { Messages.TeleportTo, e => { TeleportTo(new TeleportToArguments { x = e.GetDouble(0), y = e.GetDouble(1), vx = e.GetDouble(2), vy = e.GetDouble(3) }); } },
                            { Messages.UserTeleportTo, e => { UserTeleportTo(new UserTeleportToArguments { user = e.GetInt32(0), x = e.GetDouble(1), y = e.GetDouble(2), vx = e.GetDouble(3), vy = e.GetDouble(4) }); } },
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
            void IMessages.KeyStateChanged(int key, int state)
            {
                if(KeyStateChanged == null) return;
                var v = new RemoteEvents.KeyStateChangedArguments { key = key, state = state };
                this.VirtualLatency(() => this.KeyStateChanged(v));
            }

            public event Action<RemoteEvents.UserKeyStateChangedArguments> UserKeyStateChanged;
            void IMessages.UserKeyStateChanged(int user, int key, int state)
            {
                if(UserKeyStateChanged == null) return;
                var v = new RemoteEvents.UserKeyStateChangedArguments { user = user, key = key, state = state };
                this.VirtualLatency(() => this.UserKeyStateChanged(v));
            }

            public event Action<RemoteEvents.TeleportToArguments> TeleportTo;
            void IMessages.TeleportTo(double x, double y, double vx, double vy)
            {
                if(TeleportTo == null) return;
                var v = new RemoteEvents.TeleportToArguments { x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.TeleportTo(v));
            }

            public event Action<RemoteEvents.UserTeleportToArguments> UserTeleportTo;
            void IMessages.UserTeleportTo(int user, double x, double y, double vx, double vy)
            {
                if(UserTeleportTo == null) return;
                var v = new RemoteEvents.UserTeleportToArguments { user = user, x = x, y = y, vx = vx, vy = vy };
                this.VirtualLatency(() => this.UserTeleportTo(v));
            }

        }
        #endregion
    }
    #endregion
}
// 19.12.2008 8:16:24
