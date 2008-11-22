using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class TreeSprite : Sprite
	{
		public TreeSprite()
		{
			Width = PrimitiveTile.Width * 2;
			Height = PrimitiveTile.Heigth * 2;
			PercisionX = PrimitiveTile.Width / 2;
			PercisionY = PrimitiveTile.Heigth;

			Invoke =
				(View, Selector, Position) =>
				{
					// add a new fence tile

					new Tree(View.Level.Zoom)
					{

					}.AttachContainerTo(View.Entities).AddTo(View.Level.KnownTrees).MoveTo(
						(Position.ContentX + Selector.HalfWidth) * View.Level.Zoom,
						(Position.ContentY + Selector.HalfHeight) * View.Level.Zoom
					);
				};
		}
	}
}
