using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class Stone : Tile
	{
		
		public Stone(Level Level, TileSelector Selector) : base(Level, Selector)
		{
		}

		public override string GetIdentifier()
		{
			return StoneSelector.Identifier;
		}
		
	}
}
