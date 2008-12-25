using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class DinoSelector : SelectorBase
	{
		public DinoSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "dino",
					Index = 0,
					AnimationFrame = 0,
					Extension = "png",
					Width = 3,
					Height = 2
				};

			var Zoom = 2.0 / 3.0;

			this.ImageWidth = Convert.ToInt32( this.ToolbarImage.Width * PrimitiveTile.Width * Zoom);
			this.ImageHeight = Convert.ToInt32( this.ToolbarImage.Height * PrimitiveTile.Heigth * Zoom);

			this.Sizes = new View.SelectorInfo[]
			{
				new Size_3x2()
			};
		}


		[Script]
		public class Size_3x2 : SpriteSelector
		{
			public Size_3x2()
			{
				base.PrimitiveTileCountX = 3;
				base.PrimitiveTileCountY = 2;

				PercisionX = PrimitiveTile.Width;
				PercisionY = PrimitiveTile.Heigth;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemoveEntities(this, Level, Position);

				new Dino(Level.Zoom)
				{
					Selector = this
				}.AddTo(Level.KnownDinos).MoveTo(
					(Position.ContentX + this.HalfWidth) * Level.Zoom,
					(Position.ContentY + this.HalfHeight) * Level.Zoom
				);
			}
		}
	}
}
