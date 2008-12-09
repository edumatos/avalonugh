using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvalonUgh.Menu.Shared
{
	[Script]
	public class MenuCanvas : Canvas
	{
		public const int DefaultWidth = 480;
		public const int DefaultHeight = 320;

		public MenuCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			new Image
			{
				Source = 
					(Assets.Shared.KnownAssets.Path.Fonts.Blue + "/x.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = 15,
				Height = 16
			}.AttachTo(this);

		}
	}
}
