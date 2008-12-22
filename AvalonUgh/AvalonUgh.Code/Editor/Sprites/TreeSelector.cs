using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class TreeSelector : SelectorBase
	{

		public TreeSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "tree",
					Index = 0,
					Width = 2,
					Height = 2,
					Extension = "png"
				};


			this.Sizes =
				new[]
				{
					new Size_2x2()
				}; 
		}


		[Script]
		public class Size_2x2 : SpriteSelector
		{
			public Size_2x2()
			{
				Width = PrimitiveTile.Width * 2;
				Height = PrimitiveTile.Heigth * 2;
				PercisionX = PrimitiveTile.Width / 2;
				PercisionY = PrimitiveTile.Heigth;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				RemoveEntities(this, Level, Position);

				new Tree(Level.Zoom)
				{
					Selector = this
				}.AddTo(Level.KnownTrees).MoveTo(
					(Position.ContentX + this.HalfWidth) * Level.Zoom,
					(Position.ContentY + this.HalfHeight) * Level.Zoom
				);
			}

		}

	}
}
