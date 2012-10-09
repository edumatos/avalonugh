using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using AvalonUgh.Assets.Avalon;
using System.Windows.Media;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Threading;

namespace AvalonUgh.Code
{
	[Script]
	public class Snow : ISupportsContainer
	{
		public Canvas Container { get; set; }

		[Script]
		public class Flake
		{
			public int offsetx;
			public int offsety;

			public Image Image;

			public Flake(int x, int y, NameFormat Name, int size, Canvas Container)
			{
				offsetx = (x - 1) * size;
				offsety = (y - 1) * size;

				Image =
					new Image
					{
						Stretch = Stretch.Fill,
						Source = Name,
						Width = size,
						Height = size
					}.AttachTo(Container).MoveTo(offsetx, offsety);
			}
		}

		public readonly DispatcherTimer Timer;

		public Snow(View view)
		{


			this.Container = new Canvas
			{
				Width = view.ContentExtendedWidth,
				Height = view.ContentExtendedHeight
			};

			var SnowFlake_Size = 128 * view.Level.Zoom;

			var CountX = (view.ContentExtendedWidth / SnowFlake_Size) + 2;
			var CountY = (view.ContentExtendedHeight / SnowFlake_Size) + 2;

			var Name = new NameFormat
			{
				Path = Assets.Shared.KnownAssets.Path.Assets,
				Name = "snow",
				Extension = "png"
			};

			var Name0 = Name.ToIndex(0);
			var Name1 = Name.ToIndex(1);

			var Frames0 = Enumerable.ToArray(
				CountX.ToRange().SelectMany(x => CountY.ToRange().Select(y => new Flake(x, y, Name0, SnowFlake_Size, this.Container)))
			);

			var Frames1 = Enumerable.ToArray(
				CountX.ToRange().SelectMany(x => CountY.ToRange().Select(y => new Flake(x, y, Name1, SnowFlake_Size, this.Container)))
			);

			this.Timer = (1000 / 50).AtIntervalWithCounter(
				c =>
				{
					{
						var y = (c * 2) % SnowFlake_Size;
						var x = 0;

						if (view.Level.AttributeWind.Value < 0) x = SnowFlake_Size - y;
						else if (view.Level.AttributeWind.Value > 0) x = y;

						Frames0.ForEach(
							k => k.Image.MoveTo(x + k.offsetx, y + k.offsety)
						);
					}

					{
						var y = (c * 3) % SnowFlake_Size;
						var x = 0;

						if (view.Level.AttributeWind.Value < 0) x = SnowFlake_Size - y;
						else if (view.Level.AttributeWind.Value > 0) x = y;

						Frames1.ForEach(
							k => k.Image.MoveTo(x + k.offsetx, y + k.offsety)
						);
					}
				}
			);


		}
	}
}
