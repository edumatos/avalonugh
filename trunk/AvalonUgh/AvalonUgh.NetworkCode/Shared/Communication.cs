using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib.Shared.Nonoba;

namespace AvalonUgh.NetworkCode.Shared
{
	public static partial class Communication
	{
		public partial interface IMessages
		{
			void Server_Hello(int user, string name);

			void Server_UserJoined(int user, string name);
			void Server_UserLeft(int user, string name);


			void Hello(string name);
			void UserHello(int user, string name);

			void KeyStateChanged(int local, int key, int state);
			void UserKeyStateChanged(int user, int local, int key, int state);

			void TeleportTo(int local, double x, double y, double vx, double vy);
			void UserTeleportTo(int user, int local, double x, double y, double vx, double vy);

			void LocalPlayers_Increase();
			void UserLocalPlayers_Increase(int user);

			void LocalPlayers_Decrease();
			void UserLocalPlayers_Decrease(int user);

		}


		partial class RemoteEvents : IEventsDispatch
		{
			public void EmptyHandler<T>(T Arguments)
			{
			}

			bool IEventsDispatch.DispatchInt32(int e, IDispatchHelper h)
			{
				return Dispatch((Messages)e, h);
			}

			partial class DispatchHelper : IDispatchHelper
			{
				public Converter<object, int> GetLength { get; set; }

				public DispatchHelper()
				{
					new DefaultImplementationForIDispatchHelper(this);
				}
			}
		}

	}
}
