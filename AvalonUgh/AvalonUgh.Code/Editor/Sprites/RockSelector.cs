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
	public class RockSelector
	{
		[Script]
		public class Size_1x1 : SpriteSelector
		{
			public Size_1x1()
			{
				PrimitiveTileCountX = 1;
				PrimitiveTileCountY = 1;



				Invoke =
					(View, Position) =>
					{
						// add a new fence tile
						RemoveEntities(this, View.Level, Position);

						new Rock(View.Level.Zoom)
						{
							Selector = this
						}.AddTo(View.Level.KnownRocks).MoveTo(
							(Position.ContentX + this.HalfWidth) * View.Level.Zoom,
							(Position.ContentY + this.HalfHeight) * View.Level.Zoom
						);
					};
			}
		}

	}
}
