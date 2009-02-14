using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using System.Windows.Media;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Assets.Avalon
{
	[Script]
	public class NameFormat : NameFormatBase
	{
		public NameFormat()
		{
			this.ToSource = f => f.ToString().ToSource();

		}

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
				Zoom = Zoom,
				ToSource = ToSource,
				Path = Path,
				Extension = Extension
			};
		}

		public NameFormat ToAnimationFrame(int AnimationFrame)
		{
			var c = this.Clone();

			c.AnimationFrame = AnimationFrame;

			return c;
		}

		public NameFormat ToIndex(int Index)
		{
			var c = this.Clone();

			c.Index = Index;

			return c;
		}

		public NameFormat ToName(string Name)
		{
			var c = this.Clone();

			c.Name = Name;

			return c;
		}

		public NameFormat ToAnimationFrame(string AnimationFrameName)
		{
			var c = this.Clone();

			c.AnimationFrameName = AnimationFrameName;

			return c;
		}

		public Func<NameFormat, ImageSource> ToSource;

		public static implicit operator ImageSource(NameFormat e)
		{
			return e.ToSource(e);
		}

		public Image ToImage(int AnimationFrame)
		{
			return this.ToAnimationFrame(AnimationFrame).ToImage();
		}

		public Image ToImage()
		{
			return ToImage(
				this.Width * PrimitiveTile.Width,
				this.Height * PrimitiveTile.Heigth
			);
		}

		public Image ToImage(double Width, double Height)
		{

			if (this.ToSource == null)
				throw new ArgumentNullException("e.ToSource");

			return new Image
			{
				Source = this.ToSource(this),
				Stretch = Stretch.Fill,
				Width = Width * this.Zoom,
				Height = Height * this.Zoom,
			};
		}

		public static implicit operator Image(NameFormat e)
		{
			return e.ToImage();
		}
	}
}
