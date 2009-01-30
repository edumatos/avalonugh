using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		//public const int DefaultFramerate = 55;
		public const int DefaultFramerate = 60;

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
					StatusText.Text = new { 
						Frame = this.LocalIdentity.SyncFrame, 
						Paused = this.LocalIdentity.SyncFramePaused,
						Limit = this.LocalIdentity.SyncFrameLimit,
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


			//// we could pause the game here
			foreach (var p in Players)
			{
				p.AddAcceleration();
			}

			foreach (var p in this.Ports)
			{
				if (p.Level != null)
				{
					if (this.LocalIdentity.SyncFrame % 30 == 0)
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

					p.Level.Physics.Apply();
				}
			}


			this.LocalIdentity.SyncFrame++;
		}


	}
}
