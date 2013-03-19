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
	public class BirdSelector : SelectorBase
	{
		public readonly SelectorSize_2x3 Size_2x3 = new SelectorSize_2x3();

		public BirdSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "bird",
					AnimationFrame = 0,
					Index = 0,
					Width = 2,
					Height = 3,
					Extension = "png"
				};


			this.Sizes =
				new[]
				{
					Size_2x3
				};
		}


		[Script]
		public class SelectorSize_2x3 : SpriteSelector
		{
			public SelectorSize_2x3()
			{
				Width = PrimitiveTile.Width * 2;
				Height = PrimitiveTile.Heigth * 3;
				PercisionX = PrimitiveTile.Width / 2;
				PercisionY = PrimitiveTile.Heigth;
			}

			public override void CreateTo(LevelType Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;



				//{
				//    var TriggerObstacle = Obstacle.Of(Position, Level.Zoom, this.PrimitiveTileCountX, this.PrimitiveTileCountY);

				//    Level.KnownTrees.Remove(k => k.ToObstacle().Intersects(TriggerObstacle));
				//}

				var g = new Bird(Level.Zoom)
				{
					//Selector = this
				};
				g.MoveTo(x, y);
				g.Container.Opacity = 0.5;

				new Bird(Level.Zoom)
				{
					StartPosition = g
					//Selector = this
				}.AddTo(Level.KnownBirds).MoveTo(x, y);
			}


			public static LevelType.Attribute.Int32_Array SerializeBird(Bird i, LevelType.ToStringMode Mode)
			{
				var StartPosition = i.StartPosition;

				LevelType.Attribute.Int32_Array a = "bird";

				if (Mode == LevelType.ToStringMode.ForSync)
				{
					a.Value[0] = 1;

					a[1] = i.X;
					a[2] = i.Y;
					a[3] = i.VelocityX;
					a[4] = i.VelocityY;
				}

				a[6] = StartPosition.X;
				a[7] = StartPosition.Y;

				return a;
			}

			public void CreateTo(LevelType level, LevelType.Attribute.Int32_Array source)
			{
				var ForSync = source.Value[0];

				var gx = source[6];
				var gy = source[7];

				var g = new Bird(level.Zoom);

				g.Container.Opacity = 0.5;
				g.MoveTo(gx, gy);


				var a = new Bird(level.Zoom);
			
				a.StartPosition = g;


				if (ForSync == 1)
				{

					var zx = source[1];
					var zy = source[2];
					var zvx = source[3];
					var zvy = source[4];

					a.VelocityX = zvx;
					a.VelocityY = zvy;
					a.MoveTo(zx, zy);

				}
				else
				{

					a.MoveTo(gx, gy);
				}

				a.AddTo(level.KnownBirds);
			}


		}

	}
}
