using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class FlagSelector : SelectorBase
	{
		public FlagSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "flag",
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
				base.PercisionX = PrimitiveTile.Width / 2;

				PrimitiveTileCountX = 1;
				PrimitiveTileCountY = 1;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;


				
				{
					var TriggerObstacle = Obstacle.Of(Position, Level.Zoom, this.PrimitiveTileCountX, this.PrimitiveTileCountY);

					// remove all flags we hit directly
					Level.KnownFlags.Remove(k => k.ToObstacle().Intersects(TriggerObstacle));

					Level.ToPlatformSnapshots().FirstOrDefault(k => k.IncludedSpace.Intersects(TriggerObstacle)).Apply(
						Platform =>
						{
							Level.KnownFlags.Remove(k => k.ToObstacle().Intersects(Platform.IncludedSpace));
						}
					);

				}

				var v = new Flag(Level.Zoom);

				v.AddTo(Level.KnownFlags);
				v.MoveTo(x, y);
			}
		}

	}
}
