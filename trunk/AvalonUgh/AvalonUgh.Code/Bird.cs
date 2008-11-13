using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.Windows;

namespace AvalonUgh.Code
{
	[Script]
	public class Bird : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public readonly int Zoom;

		public readonly int Width;
		public readonly int Height;

		public void MoveTo(double x, double y)
		{
			this.Container.MoveTo(x - Width / 2, y - Height / 2);
		}


		public Bird(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = 16 * Zoom * 2;
			this.Height = 12 * Zoom * 3;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			var frames = Enumerable.Range(0, 13).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/bird0_" + ("" + index).PadLeft(2, '0') + "_2x3.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			);

			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Visibility = Visibility.Visible;

					(1000 / 30).AtDelay(
						delegate
						{
							value.Visibility = Visibility.Hidden;
							SignalNext();
						}
					);
				}
			);
		}
	}
}
