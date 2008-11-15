using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using System.Windows.Media;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Shapes;
using System.Windows;

namespace AvalonUgh.Code
{
	[Script]
	public class Water : ISupportsContainer
	{
		public Canvas Container { get; set; }

		[Script]
		public class Info
		{
			public int DefaultWidth;

			public int WaterHeight;
			public int WaterTop;

			public int Zoom;

			public Color WaterColorTop = Colors.Cyan;
			public Color WaterColorBottom = Colors.Violet;
		}

		public Water(Info e)
		{

			#region water gradient
			// water with waves 
			// http://learnwpf.com/Posts/Post.aspx?postId=9b2c71c0-7136-4ee7-ab2a-f8eec62874af

			this.Container = new Canvas
			{
				Width = e.DefaultWidth,
				Height = e.WaterHeight
			}.MoveTo(0, e.WaterTop);

		
			e.WaterColorTop.A = 40;
			e.WaterColorBottom.A = 60;

			e.WaterColorTop.ToGradient(e.WaterColorBottom, e.WaterHeight / e.Zoom).Select(
				(c, i) =>
				{
					var Opacity = c.A / 255.0;

					c.A = 0xFF;

					return new Rectangle
					{
						Fill = new SolidColorBrush(c),
						Width = e.DefaultWidth,
						Height = e.Zoom,
						Opacity = Opacity
					}.MoveTo(0, i * e.Zoom).AttachTo(this.Container);
				}
			).ToArray();
			#endregion


			#region watersurface
			{

				var watersurfaces = Enumerable.Range(0, 3).ToArray(
					index =>
					{
						var frame = new Canvas
						{
							Width = e.DefaultWidth,
							Height = e.WaterHeight,
							//Visibility = Visibility.Hidden
						}.MoveTo(0, 0).AttachTo(this.Container);

						Action<double, int> CreateWater =
							(WaterOpacity, WaterIndex) =>
							{
								new Image
								{
									Source = (Assets.Shared.KnownAssets.Path.Assets + "/water" + index + ".png").ToSource(),
									Stretch = Stretch.Fill,
									Width = e.DefaultWidth,
									Height = 1 * e.Zoom,
									Opacity = WaterOpacity
								}.AttachTo(frame).MoveTo(0, WaterIndex * e.Zoom);
							};

						CreateWater.FixLastParamToIndex()(
							1,
							0.5,
							0.2
						);




						return frame;
					}
				);

				watersurfaces.AsCyclicEnumerable().ForEach(
					(value, SignalNext) =>
					{
						value.Visibility = Visibility.Visible;

						(300).AtDelay(
							delegate
							{
								value.Visibility = Visibility.Hidden;
								SignalNext();
							}
						);
					}
				);
			}
			#endregion
		}
	}
}
