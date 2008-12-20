using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class GoldSelector
	{
		public static NameFormat ToolbarImage
		{
			get
			{
				return
					new NameFormat
					{
						Path = Assets.Shared.KnownAssets.Path.Sprites,
						Name = "gold",
						Index = 0,
						Extension = "png"
					};
			}
		}

		// clicking on the toolbar will shuffle between those sizes
		// also while loading tiles the map will tell us which size to use
		public static readonly View.SelectorInfo[] Sizes =
			new[]
			{
				new Size_1x1()
			};

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
						//RemoveEntities(this, View.Level, Position);

						new Gold(View.Level.Zoom)
						{
							Selector = this
						}.AddTo(View.Level.KnownGold).MoveTo(
							(Position.ContentX + this.HalfWidth) * View.Level.Zoom,
							(Position.ContentY + this.HalfHeight) * View.Level.Zoom
						);
					};
			}
		}

	}
}
