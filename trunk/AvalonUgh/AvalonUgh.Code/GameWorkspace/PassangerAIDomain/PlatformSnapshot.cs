using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Editor;
using System.Windows.Shapes;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.GameWorkspace.PassangerAIDomain
{
	[Script]
	public class PlatformSnapshot
	{
		public Cave Cave;
		public Sign Sign;


		public static PlatformSnapshot Of(View View, Cave Cave)
		{
			// show that we are thinking about that cave

			var o = Cave.ToObstacle();

			View.AddToContentInfoColoredShapes(o, Brushes.Cyan);

			var LandingTiles = View.Level.KnownLandingTiles.ToArray();
			var Signs = View.Level.KnownSigns.ToArray();
			var Obstacles = View.Level.ToObstacles().ToArray();

			var a = new Obstacle
			{
				Left = o.Left,
				Top = o.Bottom,
				Width = View.Level.Zoom * PrimitiveTile.Width,
				Height = View.Level.Zoom * PrimitiveTile.Heigth,
			};

			Func<int, int, Obstacle> f =
				(x, y) => a.WithOffset(
					View.Level.Zoom * PrimitiveTile.Width * x,
					View.Level.Zoom * PrimitiveTile.Heigth * y
				);

			Func<int, int> PassInteger = y => y;

			var af = f.With(x => x * -1, PassInteger);
			var bf = f.With(x => x + 1, PassInteger);

			var b = bf(0, 0);

			{
				// left direction
				var at = LandingTiles.FirstOrDefault(k => k.ToObstacle().Intersects(a));
				if (at != null)
				{
					View.AddToContentInfoColoredShapes(at.ToObstacle(), Brushes.Green);
				}


				// right direction
				var bt = LandingTiles.FirstOrDefault(k => k.ToObstacle().Intersects(b));
				if (bt != null)
				{
					View.AddToContentInfoColoredShapes(bt.ToObstacle(), Brushes.Green);
				}
			}

			Action<A> ReturnSignIfPathIsOk =
				e =>
				{
					var LandingTile = e.GetObstacleAtHeight(0);

					var LandingTileReference = LandingTiles.FirstOrDefault(k => k.ToObstacle().Intersects(LandingTile));

					if (LandingTileReference == null)
					{
						e.StopSearch();
						return;
					}

					// we got platform
					View.AddToContentInfoColoredShapes(LandingTileReference.ToObstacle(), Brushes.Green);

					var SpaceOnLandingTile = e.GetObstacleAtHeight(-1);

					var SpaceOnLandingTileObstacle = Obstacles.FirstOrDefault(k => k.Intersects(SpaceOnLandingTile));
					if (SpaceOnLandingTileObstacle != null)
					{
						// we got platform
						View.AddToContentInfoColoredShapes(SpaceOnLandingTileObstacle, Brushes.Red);
						e.StopSearch();
					}

					var TheSign = Signs.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (TheSign != null)
					{
						View.AddToContentInfoColoredShapes(TheSign.ToObstacle(), Brushes.Cyan);
						e.SignFound(TheSign);
					}
				};

			//var TheFoundSign = default(Sign);
			var PossibleDirections = new[] { af, bf };

		
			foreach (var k in
				PossibleDirections.SelectMany(Direction => Enumerable.Range(1, 10).Select(StepsTakenInThatDirection => Direction.FixFirstParam(StepsTakenInThatDirection)))

				//from Direction in PossibleDirections
				////let KeepGoingInThisDirection = new BooleanObject { Value = true }
				//from StepsTakenInThatDirection in Enumerable.Range(1, 10)
				////where KeepGoingInThisDirection.Value
				////where TheFoundSign == null
				//select new { ff = Direction.FixFirstParam(StepsTakenInThatDirection)
				//    //, KeepGoingInThisDirection 
				//}
				)
			{
				var z = new A
				{
					GetObstacleAtHeight = k,
					StopSearch =
						delegate
						{
						},
					SignFound = 
						delegate
						{
						}
				};

				

				ReturnSignIfPathIsOk(z);
			}


			return new PlatformSnapshot { Cave = Cave };
		}

		[Script]
		public class A
		{
			public Func<int, Obstacle> GetObstacleAtHeight;
			public Action<Sign> SignFound;
			public Action StopSearch;
		}
	}

}
