using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class BoxSelector : SelectorBase
	{
		public BoxSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "box",
					Index = 0,
					Extension = "png"
				};

			// clicking on the toolbar will shuffle between those sizes
			// also while loading tiles the map will tell us which size to use

			this.Sizes =
				new[]
				{
					new Size_1x1()
				};
		}

		[Script]
		public class Size_1x1 : SpriteSelector
		{
			public Size_1x1()
			{
				this.PrimitiveTileCountX = 1;
				this.PrimitiveTileCountY = 1;

				this.PercisionX = PrimitiveTile.Width / 2;
				this.PercisionY = PrimitiveTile.Heigth / 2;

		
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;


				var v = new Box(Level.Zoom);

				Level.KnownBoxes.Add(v);
				v.MoveTo(x, y);
			}
		}
	}
}
