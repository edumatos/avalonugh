using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class Bridge : Tile
	{

		public Bridge(Level Level, TileSelector Selector)
			: base(Level, Selector)
		{
			this.ObstaclePaddingBottom = 4;
		}

		public override string GetIdentifier()
		{
			return BridgeSelector.Identifier;
		}
		
	}
}
