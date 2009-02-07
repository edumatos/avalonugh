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
	public class PassengerSelector : SelectorBase
	{
		public readonly SelectorSize_2x2 Size_2x2 = new SelectorSize_2x2();


		public PassengerSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Name = "man",

					AnimationFrame = 0,

					Width = 2,
					Height = 2,

					Extension = "png",

					Path = KnownAssets.Path.Sprites
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


				Func<Actor> Constructor = () => new Actor.man0(Level.Zoom);


				{
					// the stone next to a cave has a window
					var TriggerPosition = Position[0, 0];

					var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, this.PrimitiveTileCountX, this.PrimitiveTileCountY);

					var trigger = Level.KnownPassengers.FirstOrDefault(k => k.ToObstacle().Intersects(o_trigger));

					if (trigger != null)
					{
						if (trigger is Actor.man0)
							Constructor = () => new Actor.woman0(Level.Zoom);
						else if (trigger is Actor.woman0)
							Constructor = () => new Actor.man1(Level.Zoom);

						Level.KnownPassengers.Remove(trigger);
					}
				}

				var g = Constructor();

				g.MoveTo(x, y);
				g.Container.Opacity = 0.5;
				//g.Animation = Tryoperus.AnimationEnum.Left_Hit;


				var a = Constructor();

				a.AccelerationHandicap = 0.6;
				a.MaxVelocityX = 1;
				a.DefaultPlayerInput =
					new AvalonUgh.Code.Input.PlayerInput
					{
						Keyboard = new AvalonUgh.Code.Input.KeyboardInput(new AvalonUgh.Code.Input.KeyboardInput.Arguments.Arrows())
					};

				a.Animation = Actor.AnimationEnum.Idle;
				a.StartPosition = g;
				a.AddTo(Level.KnownPassengers).MoveTo(x, y);
			}

		}
	}
}
