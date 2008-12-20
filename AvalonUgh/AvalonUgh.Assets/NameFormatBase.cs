using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;


namespace AvalonUgh.Assets.Shared
{
	[Script]
	public class NameFormatBase
	{
		// platform0_2x1.png

		public string Name;

		public int Index = 0;
		public int IndexCount = 1;

		public int Width = 1;

		public int Height = 1;

		public int Zoom = 1;



		public string AnimationFrameName;
		public int AnimationFrame = -1;
		public int AnimationFramePadding = 2;

		public string Path;
		public string Extension;

		public override string ToString()
		{
			var n = "";

			if (!string.IsNullOrEmpty(Path))
			{
				n += Path + "/";
			}

			n += Name + Index;


			if (AnimationFrame >= 0)
			{
				n += "_" + (AnimationFrame + "").PadLeft(AnimationFramePadding, '0');
			}

			if (!string.IsNullOrEmpty(AnimationFrameName))
			{
				n += "_" + AnimationFrameName;
			}

			if (!IsSingleTile)
				n += "_" + Width + "x" + Height;

			if (!string.IsNullOrEmpty(Extension))
			{
				n += "." + Extension;
			}

			return n;
		}

		public bool IsSingleTile
		{
			get
			{
				if (Width > 1)
					return false;

				if (Height > 1)
					return false;

				return true;
			}
		}



	}
}
