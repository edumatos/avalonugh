using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;
using System.Collections.Generic;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		//public const int DefaultFramerate = 55;
		public const int DefaultFramerate = 55;

		public Window FrameStatusToolbar;



		void StartThinking()
		{
			var ClientWidth = 400;
			var ClientHeight = 24;

			var StatusText = new TextBox
			{
				Width = ClientWidth,
				Height = ClientHeight,
				Foreground = Brushes.Yellow,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0),
				FontFamily = new FontFamily("Courier New"),
				FontSize = 10,
				IsReadOnly = true
			};

			var Status = new Window
			{
				ClientWidth = ClientWidth,
				ClientHeight = ClientHeight,

				DragContainer = this.Container
			};
			FrameStatusToolbar = Status;
			Status.BackgroundContainer.Opacity = 0.4;

			this.Console.AnimatedTopChanged +=
				delegate
				{
					Status.BringContainerToFront();
				};

			StatusText.AttachTo(Status.ContentContainer).MoveTo(Status.Padding, Status.Padding);

			Status.DraggableArea.BringToFront();

			Status.AttachContainerTo(this);

			ThinkTimer = (1000 / DefaultFramerate).AtInterval(
				delegate
				{

					StatusText.Text = new
					{
						Frame = this.LocalIdentity.SyncFrame,
						Paused = this.LocalIdentity.SyncFramePaused,
						Limit = this.LocalIdentity.SyncFrameLimit,
						Checksum = Checksum
					}.ToString();

					Think();
				}
			);
		}

		DispatcherTimer ThinkTimer;

		void Think()
		{
			if (this.LocalIdentity.SyncFramePaused)
			{
				if (this.LocalIdentity.SyncFramePausedSkip)
				{
					this.LocalIdentity.SyncFramePausedSkip = false;
				}
				else
				{
					return;
				}
			}

			if (this.LocalIdentity.SyncFrameLimit > 0)
			{
				if (this.LocalIdentity.SyncFrameLimit <= this.LocalIdentity.SyncFrame)
				{
					return;
				}
			}

			if (this.Ports.Any(k => k.IsLoading))
				return;

			//// here we should check for desync
			//var CurrentFrame = this.LocalIdentity.SyncFrame;
			//var CurrentChecksum = this.Checksum;

			//// do we have an history item, eg the last frame?
			//var InternalLastChecksumHistoryItem = this.InternalChecksumHistory.LastOrDefault();
			//if (InternalLastChecksumHistoryItem != null)
			//{
			//    if (InternalLastChecksumHistoryItem.SyncFrame == CurrentFrame)
			//    {
			//        // we are goin to continue the next frame shortly

			//        // now we need to step backwards in time to see
			//        // if one of our peers had reported a wrong checksum

			//        foreach (var ExternalHistoryFrame in Enumerable.Range(0, this.LocalIdentity.SyncFrameWindow).SelectMany(k => this.ExternalChecksumHistory.Where(j => j.SyncFrame == CurrentFrame - k)))
			//        {
			//            var InternalHistoryFrame = this.InternalChecksumHistory.SingleOrDefault(k => k.SyncFrame == ExternalHistoryFrame.SyncFrame);

			//            if (InternalHistoryFrame != null)
			//                if (ExternalHistoryFrame.Checksum != InternalHistoryFrame.Checksum)
			//                {
			//                    // we have found a desync
			//                    this.LocalIdentity.SyncFramePaused = true;
			//                    this.Console.BringContainerToFront();
			//                    this.Console.AnimatedTop = 0;
			//                    this.Console.WriteLine("desync: " +
			//                        new
			//                        {
			//                            ExternalHistoryFrame.SyncFrame,
			//                            ExternalHistoryFrame.NetworkNumber,
			//                            ecrc = ExternalHistoryFrame.Checksum,
			//                            icrc = InternalHistoryFrame.Checksum
			//                        }
			//                    );

			//                }

			//        }
			//    }
			//    else
			//    {
			//        // there was a time warp?
			//    }
			//}

			if (this.LocalIdentity.SyncFramePaused)
			{
				if (this.LocalIdentity.SyncFramePausedSkip)
				{
					this.LocalIdentity.SyncFramePausedSkip = false;
				}
				else
				{
					return;
				}
			}

			//// we could pause the game here
			foreach (var p in Players)
			{
				p.AddAcceleration();
			}

			this.Audio_WaterRise.Enabled = this.Ports.Where(k => k.Level != null).Where(k => k.Level.AttributeWaterRise.BooleanValue).Any();

			foreach (var p in this.Ports)
			{
				if (p.Level != null)
				{
					this.ThinkForComputerPlayers(p.View);

					if (this.LocalIdentity.SyncFrame % 80 == 0)
						if (p.Level.AttributeWaterRise.BooleanValue)
							p.Level.AttributeWater.Value++;

					// some animations need to be synced by frame
					foreach (var dino in p.Level.KnownDinos)
					{
						dino.Animate(this.LocalIdentity.SyncFrame);
					}

					foreach (var t in p.Level.KnownTryoperus)
					{
						t.Think();
					}

					foreach (var t in p.Level.KnownActors)
					{
						if (t.NextAnimationFrame != null)
							t.NextAnimationFrame();
					}

					foreach (var t in p.Level.KnownPassengers)
					{
						t.AddAcceleration(t.DefaultPlayerInput);
					}

					if (this.LocalIdentity.SyncFrame % 10 == 0)
						if (p.Level.KnownBirds.Count > 0)
							SoundBoard.Default.wings1();

					p.Level.Physics.Apply();


				}
			}

			var NextFrame = this.LocalIdentity.SyncFrame + 1;

			//var NextChecksumItem = new ChecksumItem
			//{
			//    NetworkNumber = this.LocalIdentity.NetworkNumber,
			//    Checksum = Checksum,
			//    SyncFrame = NextFrame
			//};

			//this.InternalChecksumHistory.Enqueue(NextChecksumItem);

			//if (this.InternalChecksumHistory.Count > this.LocalIdentity.SyncFrameWindow)
			//    this.InternalChecksumHistory.Dequeue();

			this.LocalIdentity.SyncFrame = NextFrame;
		}

		//[Script]
		//public class ChecksumItem
		//{
		//    public int NetworkNumber;

		//    public int Checksum;

		//    /// <summary>
		//    /// This frame will begin with this checksum
		//    /// </summary>
		//    public int SyncFrame;
		//}

		//public readonly Queue<ChecksumItem> InternalChecksumHistory = new Queue<ChecksumItem>();
		//public readonly Queue<ChecksumItem> ExternalChecksumHistory = new Queue<ChecksumItem>();

		public int Checksum
		{
			get
			{
				var x = this.AllPlayers.Aggregate(0, (seed, value) => seed ^ value.Checksum.Checksum);

				return x;
			}
		}

	}
}
