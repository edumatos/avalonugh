using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.ComponentModel;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class PlayerIdentity
	{
		//public int SyncFrameWindow = 5;

		// should be larger for laggy networks
		//public int SyncFrameWindow = 6;
		
		public int SyncFrameWindow = 5;


		public int Number;

		public string Name;

		public readonly BindingList<PlayerInfo> Locals = new BindingList<PlayerInfo>();

		public int SyncFrameLatency;
		public int SyncFrameRate;
		public bool SyncFramePaused;
		public bool SyncFramePausedSkip;


		public event Action SyncFrameLimitChanged;
		int InternalSyncFrameLimit;
		public int SyncFrameLimit
		{
			get
			{
				return InternalSyncFrameLimit;
			}
			set
			{
				if (value != 0)
					if (value < InternalSyncFrameLimit)
						throw new Exception("SyncFrameLimit can only be reset or increased");

				InternalSyncFrameLimit = value;

				if (SyncFrameLimitChanged != null)
					SyncFrameLimitChanged();
			}
		}

	

		#region SyncFrame
		public event Action SyncFrameChanged;
		int InternalSyncFrame;
		public int SyncFrame
		{
			get
			{
				return InternalSyncFrame;
			}
			set
			{
				InternalSyncFrame = value;
				if (SyncFrameChanged != null)
					SyncFrameChanged();
			}
		}
		#endregion

		public PlayerIdentity()
		{
			this.Locals.ForEachNewItem(
				i =>
				{
					i.IdentityLocal = this.Locals.Count;
				}
			);
		}

		public override string ToString()
		{
			return new { Number, Name, SyncFrame }.ToString();
		}

		public PlayerInfo this[int IdentityLocal]
		{
			get
			{
				return this.Locals.SingleOrDefault(k => k.IdentityLocal == IdentityLocal);
			}
		}



		public int HandleFutureFrame(Action handler)
		{
			return HandleFutureFrame(0, handler);
		}

		public int HandleFutureFrame(int offset, Action handler)
		{
			return HandleFutureFrame(offset, handler, null);
		}

		public int HandleFutureFrame(int offset, Action handler, Action desync)
		{
			int FutureFrame = this.SyncFrame + this.SyncFrameWindow + offset;

			HandleFrame(FutureFrame, handler, desync);

			return FutureFrame;
		}

		public void HandleFrame(int frame, Action handler)
		{
			HandleFrame(frame, handler, null);
		}

		public void HandleFrame(int frame, Action handler, Action desync)
		{
			if (this.SyncFrame == frame)
			{
				handler();
				return;
			}

			Action SyncFrameChanged = null;

			SyncFrameChanged = delegate
			{
				if (this.SyncFrame < frame)
				{
					// we need to wait. the event mus occur in the future
					return;
				}


				if (this.SyncFrame > frame)
				{
					// did we miss the correct frame?

					// we would miss the correct frame
					// if we would send input from the past

					// this event will cause desync!

					if (desync != null)
						desync();
				}

				handler();

				this.SyncFrameChanged -= SyncFrameChanged;
			};


			this.SyncFrameChanged += SyncFrameChanged;
		}
	}
}
