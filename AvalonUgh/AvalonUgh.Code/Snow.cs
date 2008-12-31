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

		public Snow(int DefaultWidth, int DefaultHeight, int DefaultZoom)
		{
			this.Container = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			};

			var SnowFlake_Size = 128 * DefaultZoom;

			var CountX = Convert.ToInt32(DefaultWidth / SnowFlake_Size) + 2;
			var CountY = Convert.ToInt32(DefaultHeight / SnowFlake_Size) + 2;

		


			{
				var Name = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "snow",
					Extension = "png"
				};

				var Frames = Enumerable.ToArray(
					CountX.ToRange().SelectMany(x => CountY.ToRange().Select(y =>	new Flake(x, y, Name, SnowFlake_Size, this.Container)))
				);


				(1000 / 60).AtIntervalWithCounter(
					y =>
					{
						y *= 2;
						y %= SnowFlake_Size;
						var x = SnowFlake_Size - y;

						Frames.ForEach(
							k => k.Image.MoveTo(x + k.offsetx, y + k.offsety)
						);
					}
				);
			}

			{
				var Name = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "snow",
					Index = 1,
					Extension = "png"
				};

				var Frames = Enumerable.ToArray(
					CountX.ToRange().SelectMany(x => CountY.ToRange().Select(y => new Flake(x, y, Name, SnowFlake_Size, this.Container)))
				);


				(1000 / 60).AtIntervalWithCounter(
					y =>
					{
						y *= 3;
						y %= SnowFlake_Size;
						var x = SnowFlake_Size - y;
						

						Frames.ForEach(
							k => k.Image.MoveTo(x + k.offsetx, y + k.offsety)
						);
					}
				);
			}
		}
	}
}
