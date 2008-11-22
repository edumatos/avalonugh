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
	public class FenceSelector : TileSelector
	{
		public FenceSelector()
		{
			PrimitiveTileCountX = 1;
			PrimitiveTileCountY = 1;
		
			Invoke =
				(View, Selector, Position) =>
				{
					// add a new fence tile

					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/fence0.png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,
						Width = Selector.Width * View.Level.Zoom,
						Height = Selector.Height * View.Level.Zoom,
					}.AttachTo(View.Platforms).MoveTo(
						Position.ContentX * View.Level.Zoom,
						Position.ContentY * View.Level.Zoom
					);
				};
		}
	}
}
