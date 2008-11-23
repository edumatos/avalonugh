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
			: this(1, 1)
		{

		}

		public TileSelector(int PrimitiveTileCountX, int PrimitiveTileCountY)
		{
			this.PrimitiveTileCountX = PrimitiveTileCountX;
			this.PrimitiveTileCountY = PrimitiveTileCountY;

			PercisionX = PrimitiveTile.Width;
			PercisionY = PrimitiveTile.Heigth;
		}
	}
}
