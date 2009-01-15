using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class SelectorBase
	{
		public int ImageWidth;
		public int ImageHeight;

		public NameFormat ToolbarImage;
		public View.SelectorInfo[] Sizes;

		public SelectorBase()
		{
			this.Sizes = new View.SelectorInfo[0];
		}

		public virtual string GetIdentifier()
		{
			return "";
		}
	}
}
