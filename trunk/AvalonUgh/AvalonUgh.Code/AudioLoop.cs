using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class AudioLoop
	{
		public AudioLoop()
		{
			this.Volume = 1;
		}

		public double Volume { get; set; }

		Action InternalLoopStop;
		string InternalLoop;
		public string Loop
		{
			get
			{
				return InternalLoop;
			}
			set
			{
				if (InternalLoopStop != null)
				{
					InternalLoopStop();
					InternalLoopStop = null;
				}

				InternalLoop = value;

				if (!string.IsNullOrEmpty(InternalLoop))
				{
					// start
					InternalLoop.Apply(
						(Source, Retry) =>
						{
							var Music = Source.PlaySound();

							Music.PlaybackComplete += Retry;
							Music.SetVolume(this.Volume);
							InternalLoopStop = Music.Stop;
						}
					);
				}
			}
		}
	}
}
