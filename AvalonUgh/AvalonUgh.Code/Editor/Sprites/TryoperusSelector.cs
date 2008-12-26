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
	public class TryoperusSelector : SelectorBase
	{
		public readonly SelectorSize_2x2 Size_2x2 = new SelectorSize_2x2();


		public TryoperusSelector()
		{
			this.ToolbarImage =
				new Tryoperus.SpecificNameFormat
				{
					AnimationFrame = Tryoperus.AnimationFrames.Left.HitOffset,
				};


			this.Sizes =
				new[]
				{
					Size_2x2
				};
		}

		[Script]
		public class SelectorSize_2x2 : SpriteSelector
		{
			public SelectorSize_2x2()
			{
				Width = PrimitiveTile.Width * 2;
				Height = PrimitiveTile.Heigth * 2;
				PercisionX = PrimitiveTile.Width / 2;
				PercisionY = PrimitiveTile.Heigth;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;


				RemoveEntities(this, Level, Position);

				var g = new Tryoperus(Level);

				g.MoveTo(x, y);
				g.Container.Opacity = 0.5;
				g.Animation = Tryoperus.AnimationEnum.Left_Hit;


				new Tryoperus(Level)
				{
					Selector = this,
					StartPosition = g
				}.AddTo(Level.KnownTryoperus).MoveTo(x, y);
			}

		}
	}
}
