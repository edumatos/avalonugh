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
			void Server_Message(string text);
			void Server_LoadLevel(int index, string data);
			void Server_Hello(int user, string name, int others, int levels, int framelimit);

			void Server_UserJoined(int user, string name);
			void Server_UserLeft(int user, string name);


			void UserHello(int user, string name, int frame);
			void UserSynced(int user, int frame);

			void KeyStateChanged(int local, int frame, int key, int state);
			void UserKeyStateChanged(int user, int local, int frame, int key, int state);

			void TeleportTo(int frame, int local, int port, double x, double y, double vx, double vy);
			void UserTeleportTo(int user, int frame, int local, int port, double x, double y, double vx, double vy);



			void RemoveLocalPlayer(int frame, int local);
			void UserRemoveLocalPlayer(int user, int frame, int local);


			void EditorSelector(int frame, int port, int type, int size, int x, int y);
			void UserEditorSelector(int user, int frame, int port, int type, int size, int x, int y);

			void SyncFrame(int frame, int framerate);
			void UserSyncFrame(int user, int frame, int framerate);

			void SyncFrameEcho(int frame, int framerate);
			void UserSyncFrameEcho(int user, int frame, int framerate);


		
			void SetPaused(int frame);
			void UserSetPaused(int user, int frame);

			void ClearPaused();
			void UserClearPaused(int user);

			void LoadLevel(int frame, int port, int level, string custom);
			void UserLoadLevel(int user, int port, int frame, int level, string custom);

			void LoadLevelHint(int port);
			void UserLoadLevelHint(int user, int port);

			void MouseMove(int port, double x, double y);
			void UserMouseMove(int user, int port, double x, double y);

			void MissionStartHint(int frame, int difficulty);
			void UserMissionStartHint(int user, int frame, int difficulty);

			void Vehicalize(int frame, int local);
			void UserVehicalize(int user, int frame, int local);
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
