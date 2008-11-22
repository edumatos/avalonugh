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
	public class SignSelector : SpriteSelector
	{
		public SignSelector()
		{
			Width = PrimitiveTile.Width;
			Height = PrimitiveTile.Heigth;
			PercisionX = PrimitiveTile.Width / 2;
			PercisionY = PrimitiveTile.Heigth;

			Invoke =
				(View, Selector, Position) =>
				{
					// add a new fence tile

					new Sign(View.Level.Zoom)
					{

					}.AttachContainerTo(View.Entities).AddTo(View.Level.KnownSigns).MoveTo(
						(Position.ContentX + Selector.HalfWidth) * View.Level.Zoom,
						(Position.ContentY + Selector.HalfHeight) * View.Level.Zoom
					);
				};
		}
	}
}
