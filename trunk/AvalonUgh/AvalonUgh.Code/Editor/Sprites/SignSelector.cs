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
	public class SignSelector : SelectorBase
	{
		public readonly SelectorSize_1x1 Size_1x1 = new SelectorSize_1x1();

		public SignSelector()
		{
			this.Sizes =
				new[]
				{
					Size_1x1
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
		public class SelectorSize_1x1 : SpriteSelector
		{
			public SelectorSize_1x1()
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

			public void CreateTo(Level level, Level.Attribute.Int32_Array SyncAttributeSign, int TileRowsProcessed)
			{
				var x = SyncAttributeSign.Value[0] * level.Zoom;
				var y = TileRowsProcessed * PrimitiveTile.Heigth * level.Zoom;

				new Sign(level.Zoom)
				{
					Value = SyncAttributeSign.Value[1],
					WaitPositionPreference = (Sign.WaitPositionPreferences) SyncAttributeSign.Value[2]
				}.AddTo(level.KnownSigns).MoveBaseTo(x, y);
			}
		}

		public static Level.Attribute.Int32_Array SerializeSign(Sign value)
		{
			var a = new Level.Attribute.Int32_Array { Key = "sign" };

			a.Value[0] = value.UnscaledX;
			a.Value[1] = value.Value;
			a.Value[2] = (int)value.WaitPositionPreference;

			return a;

		}
	}
}
