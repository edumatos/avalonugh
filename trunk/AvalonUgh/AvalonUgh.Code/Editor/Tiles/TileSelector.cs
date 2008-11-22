using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public abstract class TileSelector : View.SelectorInfo
	{
		public TileSelector()
		{
			PercisionX = PrimitiveTile.Width;
			PercisionY = PrimitiveTile.Heigth;
		}
	}
}
