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
	public class SignSelector : SelectorBase
	{
		public SignSelector()
		{
			this.Sizes =
				new[]
				{
					new Size_1x1()
				};

			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "sign",
					Index = 0,
					Extension = "png"
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



				// we should increment the old sign actually
				var c = 0;

				{
					// the stone next to a cave has a window
					var TriggerPosition = Position[0, 0];

					var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 1);

					var trigger = Level.KnownSigns.FirstOrDefault(k => k.ToObstacle().Intersects(o_trigger));

					if (trigger != null)
					{
						c = trigger.Value + 1;

						Level.KnownSigns.Remove(trigger);
					}
				}


				var v = new Sign(Level.Zoom)
				{
					Value = c % 6,
					Selector = this
				};

				v.AddTo(Level.KnownSigns);
				v.MoveTo(x, y);
			}
		}

	}
}
