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
			void Server_Hello(int user, string name, int others);

			void Server_UserJoined(int user, string name);
			void Server_UserLeft(int user, string name);


			void Hello(string name, int frame);
			void UserHello(int user, string name, int frame);

			void KeyStateChanged(int local, int frame, int sequence, int key, int state);
			void UserKeyStateChanged(int user, int local, int frame, int sequence, int key, int state);

			void TeleportTo(int frame, int local, int port, double x, double y, double vx, double vy);
			void UserTeleportTo(int user, int frame, int local, int port, double x, double y, double vx, double vy);

			void Vehicle_TeleportTo(int index, double x, double y, double vx, double vy);
			void UserVehicle_TeleportTo(int user, int index, double x, double y, double vx, double vy);


			void LocalPlayers_Increase(int frame);
			void UserLocalPlayers_Increase(int user, int frame);

			void LocalPlayers_Decrease(int frame);
			void UserLocalPlayers_Decrease(int user, int frame);

			void EditorSelector(int frame, int type, int size, int x, int y);
			void UserEditorSelector(int user, int frame, int type, int size, int x, int y);

			void SyncFrame(int frame, int framerate);
			void UserSyncFrame(int user, int frame, int framerate);

			void SyncFrameEcho(int frame, int framerate);
			void UserSyncFrameEcho(int user, int frame, int framerate);


			void SetShakerEnabled(int frame, int value);
			void UserSetShakerEnabled(int user, int frame, int value);

			void SetPaused(int frame);
			void UserSetPaused(int user, int frame);

			void ClearPaused();
			void UserClearPaused(int user);

			void LoadEmbeddedLevel(int frame, int level);
			void UserLoadEmbeddedLevel(int user, int frame, int level);

			void LoadCustomLevel(int frame, string data);
			void UserLoadCustomLevel(int user, int frame, string data);

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
