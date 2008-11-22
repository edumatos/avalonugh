using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class EmptyTile : Tile
	{
		public EmptyTile()
		{
			Width = PrimitiveTile.Width;
			Height = PrimitiveTile.Heigth;
			PercisionX = PrimitiveTile.Width;
			PercisionY = PrimitiveTile.Heigth;

			Invoke =
				(View, Selector, Position) =>
				{

					
				};
		}
	}
}
