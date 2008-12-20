using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class VehicleSelector
	{
		public static readonly string ToolbarImage =
			(Assets.Shared.KnownAssets.Path.Sprites + "/" +
				new NameFormat { Name = "vehicle", Index = 1, AnimationFrame = 4, Width = 2, Height = 2 }
				 + ".png");

		// clicking on the toolbar will shuffle between those sizes
		// also while loading tiles the map will tell us which size to use
		public static readonly View.SelectorInfo[] Sizes =
			new[]
			{
				new Size_2x2()
			};

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

				var v = new Vehicle(Level.Zoom);

				v.CurrentDriver = null;

				v.MoveTo(
					(Position.ContentX + this.HalfWidth) * Level.Zoom,
					(Position.ContentY + this.HalfHeight) * Level.Zoom	
				);
				
				Level.KnownVehicles.Add(v);
				
				// we should add a ghost for the vehicle starting point
			}
		}
	}
}
