using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using System.Windows.Media;
using System.Windows.Controls;

namespace AvalonUgh.Assets.Avalon
{
	[Script]
	public sealed class NameFormat : NameFormatBase
	{
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
				Zoom = Zoom
			};
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
