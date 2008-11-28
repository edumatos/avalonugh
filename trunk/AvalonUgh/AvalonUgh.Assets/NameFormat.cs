using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Media;
using System.Windows.Controls;

namespace AvalonUgh.Assets.Shared
{
	[Script]
	public sealed class NameFormat
	{
		// platform0_2x1.png

		public string Name;
		
		public int Index = 0;
		public int IndexCount = 1;

		public int Width = 1;
		
		public int Height = 1;

		public int Zoom = 1;

		public NameFormat Clone()
		{
			return new NameFormat
			{
				Name = Name,
				Index = Index,
				IndexCount = IndexCount,
				Width = Width,
				Height = Height,
				AnimationFrame = AnimationFrame,
				AnimationFramePadding = AnimationFramePadding,
				AnimationFrameName = AnimationFrameName,
				ToSource = ToSource,
				Zoom = Zoom
			};
		}

		public string AnimationFrameName;
		public int AnimationFrame = -1;
		public int AnimationFramePadding = 2;

		public override string ToString()
		{
			var n = Name + Index;

			if (Width == 1)
				if (Height == 1)
					return n;

			if (AnimationFrame >= 0)
			{
				n += "_" + (AnimationFrame + "").PadLeft(AnimationFramePadding, '0');
			}

			if (!string.IsNullOrEmpty(AnimationFrameName))
			{
				n += "_" + AnimationFrameName;
			}

			return n + "_" + Width + "x" + Height;
		}

		public NameFormat ToAnimationFrame(string AnimationFrameName)
		{
			var c = this.Clone();

			c.AnimationFrameName = AnimationFrameName;

			return c;
		}


		public Func<NameFormat, ImageSource> ToSource;

		public static implicit operator Image(NameFormat e)
		{
			if (e.ToSource == null)
				throw new ArgumentNullException("e.ToSource");

			return new Image 
			{
				Source = e.ToSource(e),
				Stretch = Stretch.Fill,
				Width = e.Width * PrimitiveTile.Width * e.Zoom,
				Height = e.Width * PrimitiveTile.Heigth * e.Zoom,
			};
		}
	}
}
