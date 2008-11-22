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
	public class SignSelector
	{
		[Script]
		public class Size_1x1 : SpriteSelector
		{
			public Size_1x1()
			{
				PrimitiveTileCountX = 1;
				PrimitiveTileCountY = 1;

				var c = 0;

				Invoke =
					(View, Selector, Position) =>
					{

						DemolishSelector.InternalInvoke(View, Selector, Position);

						c++;

						new Sign(View.Level.Zoom)
						{
							Value = c % 6,
							Selector = this
						}.AttachContainerTo(View.Entities).AddTo(View.Level.KnownSigns).MoveTo(
							(Position.ContentX + Selector.HalfWidth) * View.Level.Zoom,
							(Position.ContentY + Selector.HalfHeight) * View.Level.Zoom
						);
					};
			}
		}

	}
}
