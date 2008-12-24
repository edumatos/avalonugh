﻿using System;
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

			this.StopDelay = 500;
			this.StartDelay = 500;
		}

		public double Volume { get; set; }

		public int StartDelay { get; set; }
		public string Start { get; set; }
		public int StopDelay { get; set; }
		public string Stop { get; set; }

	

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
				Update();
			}
		}

		bool InternalEnabled;
		public bool Enabled
		{
			get
			{
				return InternalEnabled;
			}
			set
			{
				InternalEnabled = value;
				Update();
			}
		}

		public event Action LoopStarted;
		public event Action LoopStopped;

		bool Update_LoopStarted;

		private void Update()
		{
			if (InternalEnabled)
			{
				Action ApplyLoop = delegate
				{
					InternalLoop.Apply(
						(Source, Retry) =>
						{
							var Music = Source.ToSound();

							Music.PlaybackComplete += Retry;
							Music.SetVolume(this.Volume);
							Music.Start();

							InternalLoopStop = Music.Stop;
						}
					);
				};

				if (LoopStarted != null)
					LoopStarted();
				Update_LoopStarted = true;

				// start
				if (!string.IsNullOrEmpty(this.Start))
				{
					var StartMusic = this.Start.ToSound();

					StartMusic.SetVolume(this.Volume);
					StartMusic.Start();

					if (!string.IsNullOrEmpty(InternalLoop))
					{
						if (StartDelay > 0)
							StartDelay.AtDelay(ApplyLoop);
						else
							ApplyLoop();
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(InternalLoop))
					{
						ApplyLoop();
					}
				}
			}
			else
			{
				if (Update_LoopStarted)
				{
					Action StopLoop = delegate
					{
						if (InternalLoopStop != null)
						{
							InternalLoopStop();
							InternalLoopStop = null;
						}
					};

					if (!string.IsNullOrEmpty(this.Stop))
					{
						var StopMusic = this.Stop.ToSound();

						StopMusic.SetVolume(this.Volume);
						StopMusic.Start();

						if (!string.IsNullOrEmpty(InternalLoop))
						{
							if (StopDelay > 0)
								StopDelay.AtDelay(StopLoop);
							else
								StopLoop();
						}

						StopMusic.PlaybackComplete +=
							delegate
							{
								if (LoopStopped != null)
									LoopStopped();

							};
					}
					else
					{
						StopLoop();

						if (LoopStopped != null)
							LoopStopped();

					}
				}
			}
		}
	}
}
