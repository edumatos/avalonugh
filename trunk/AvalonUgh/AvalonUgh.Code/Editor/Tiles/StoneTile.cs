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
	public class StoneTile : Tile
	{
		public StoneTile()
		{
			Width = PrimitiveTile.Width;
			Height = PrimitiveTile.Heigth;
			PercisionX = PrimitiveTile.Width;
			PercisionY = PrimitiveTile.Heigth;

			Invoke =
				(View, Selector, Position) =>
				{
					// add a new fence tile

					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/stone0.png").ToSource(),
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
