using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	partial class Actor
	{
		[Script]
		public class woman0 : Actor
		{
			public override int GetActorType()
			{
				return 1;
			}

			public woman0(int Zoom): base(Zoom)
			{
				this.AvailableFare = 1599;


				Func<int, Image> ToFrame =
									index =>
										new Image
										{
											Source = (Assets.Shared.KnownAssets.Path.Sprites + "/woman0_" + ("" + index).PadLeft(2, '0') + "_2x2.png").ToSource(),
											Stretch = Stretch.Fill,
											Width = this.Width,
											Height = this.Height,
											Visibility = Visibility.Hidden
										}.AttachTo(this.Container);


				this.IdleFrames =
					new[]
					{
						ToFrame(0),
						ToFrame(1),
						ToFrame(2),
						ToFrame(1),
					};

				this.PanicFrames =
					new[]
					{
						ToFrame(10),
						ToFrame(11),
						ToFrame(12),
					};


				this.TalkFrames =
					new[]
					{
						ToFrame(1),
					};


				this.WalkLeftFrames = Enumerable.Range(30, 9).ToArray(ToFrame);
				this.WalkRightFrames = Enumerable.Range(40, 9).ToArray(ToFrame);
				this.CaveEnterFrames = Enumerable.Range(60, 8).ToArray(ToFrame);
				this.CaveExitFrames = Enumerable.Range(50, 9).ToArray(ToFrame);

				InitializeAnimation();
			}
		}
	}
}
