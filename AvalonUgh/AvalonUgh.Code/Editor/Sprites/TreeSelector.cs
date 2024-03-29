﻿using System;
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

			public override void CreateTo(LevelType Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;



				{
					var TriggerObstacle = Obstacle.Of(Position, Level.Zoom, this.PrimitiveTileCountX, this.PrimitiveTileCountY);

					Level.KnownTrees.Remove(k => k.ToObstacle().Intersects(TriggerObstacle));
				}

				new Tree(Level.Zoom)
				{
					Selector = this
				}.AddTo(Level.KnownTrees).MoveTo(x, y);
			}

		}

	}
}
