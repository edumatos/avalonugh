using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class Fence : Tile
	{
		public Fence(LevelType Level, TileSelector Selector)
			: base(Level, Selector)
		{
		}

		public override string GetIdentifier()
		{
			return FenceSelector.Identifier;
		}
	}
}
