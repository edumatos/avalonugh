using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code
{
	partial class Actor
	{
		[Script]
		public class man0 : Actor
		{
			public man0(int Zoom) : base(Zoom)
			{
				Func<int, Image> ToFrame =
					index =>
						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Sprites + "/man0_" + ("" + index).PadLeft(2, '0') + "_2x2.png").ToSource(),
							Stretch = Stretch.Fill,
							Width = this.Width,
							Height = this.Height,
							Visibility = Visibility.Hidden
						}.AttachTo(this.Container);


				this.IdleFrames =
					new[]
					{
						ToFrame(0),
						ToFrame(1)
					};

				this.PanicFrames =
					new[]
					{
						ToFrame(10),
						ToFrame(11)
					};


				Initialize();
			}
		}
	}
}
