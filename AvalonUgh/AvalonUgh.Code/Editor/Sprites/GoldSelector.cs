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
	public class GoldSelector : SelectorInfo
	{
		public GoldSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "gold",
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
				PrimitiveTileCountX = 1;
				PrimitiveTileCountY = 1;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;


				var v = new Gold(Level.Zoom);

				v.AddTo(Level.KnownGold);
				v.MoveTo(x, y);
			}
		}

	}
}
