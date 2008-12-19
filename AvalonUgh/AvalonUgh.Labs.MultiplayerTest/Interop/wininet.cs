using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AvalonUgh.Labs.MultiplayerTest.Interop
{
	public static class wininet
	{
		// http://msmvps.com/blogs/siva/archive/2006/10/04/Switch-Internet-Explorer-to-Offline-Mode.aspx

		[DllImport("wininet.dll")]
		private extern static bool InternetSetOption(int hInternet,
		int dwOption, ref INTERNET_CONNECTED_INFO lpBuffer, int dwBufferLength);

		[StructLayout(LayoutKind.Sequential)]
		struct INTERNET_CONNECTED_INFO
		{
			public int dwConnectedState;
			public int dwFlags;
		};
		private const int INTERNET_STATE_DISCONNECTED = 16;
		private const int INTERNET_STATE_CONNECTED = 1;
		private const int ISO_FORCE_DISCONNECTED = 1;
		private const int INTERNET_OPTION_CONNECTED_STATE = 50;

		public static void SetIEConnectionMode(bool offline)
		{
			INTERNET_CONNECTED_INFO ici = new INTERNET_CONNECTED_INFO();

			if (offline)
			{
				ici.dwConnectedState = INTERNET_STATE_DISCONNECTED;
				ici.dwFlags = ISO_FORCE_DISCONNECTED;
			}
			else
			{
				ici.dwConnectedState = INTERNET_STATE_CONNECTED;
			}

			InternetSetOption(0, INTERNET_OPTION_CONNECTED_STATE, ref ici, Marshal.SizeOf(ici));
		}

	}
}
