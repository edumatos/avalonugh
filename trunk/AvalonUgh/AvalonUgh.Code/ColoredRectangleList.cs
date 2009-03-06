using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.ComponentModel;
using ScriptCoreLib;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class ColoredRectangleList : BindingList<Rectangle>
	{
		public Rectangle Add(Obstacle o, Brush b)
		{
			return Add(o, b, 0.4);
		}

		public Rectangle Add(Obstacle o, Brush b, double Opacity)
		{
			var r = new Rectangle
			{
				Fill = b,
				Width = o.Width,
				Height = o.Height,
				Opacity = Opacity
			}.MoveTo(
				o.Left, // + this.ContentOffsetX,
				o.Top // + this.ContentOffsetY
			);

			r.AddTo(this);

			return r;
		}
	}
}
