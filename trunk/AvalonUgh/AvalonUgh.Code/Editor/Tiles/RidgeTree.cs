using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class RidgeTree : Tile
	{
		public RidgeTree(LevelType Level, TileSelector Selector)
			: base(Level, Selector)
		{
		}

		public override string GetIdentifier()
		{
			return RidgeTreeSelector.Identifier;
		}
		
	}
}
