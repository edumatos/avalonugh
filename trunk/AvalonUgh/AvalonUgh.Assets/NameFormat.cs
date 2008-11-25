using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

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

		public NameFormat Clone()
		{
			return new NameFormat
			{
				Name = Name,
				Index = Index,
				IndexCount = IndexCount,
				Width = Width,
				Height = Height
			};
		}

		public override string ToString()
		{
			var n = Name + Index;

			if (Width == 1)
				if (Height == 1)
					return n;


			return n + "_" + Width + "x" + Height;
		}
	}
}
