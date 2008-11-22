using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class RockSelector : SpriteSelector
	{
		public RockSelector()
		{
			Width = PrimitiveTile.Width;
			Height = PrimitiveTile.Heigth;
			PercisionX = PrimitiveTile.Width / 2;
			PercisionY = PrimitiveTile.Heigth;

			Invoke =
				(View, Selector, Position) =>
				{
					// add a new fence tile

					new Rock(View.Level.Zoom)
					{
						Selector = this
					}.AttachContainerTo(View.Entities).AddTo(View.Level.KnownRocks).MoveTo(
						(Position.ContentX + Selector.HalfWidth) * View.Level.Zoom,
						(Position.ContentY + Selector.HalfHeight) * View.Level.Zoom
					);
				};
		}
	}
}
