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

			var af = f.With(x => x * -1, y => y);
			var bf = f.With(x => x + 1, y => y);

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

			Action<Func<int, Obstacle>, Action<Sign>, Action> ReturnSignIfPathIsOk =
				(ff, SignFound, StopSearch) =>
				{
					var LandingTile = ff(0);

					var LandingTileReference = LandingTiles.FirstOrDefault(k => k.ToObstacle().Intersects(LandingTile));

					if (LandingTileReference == null)
					{
						StopSearch();
						return;
					}

					// we got platform
					View.AddToContentInfoColoredShapes(LandingTileReference.ToObstacle(), Brushes.Green);

					var SpaceOnLandingTile = ff(-1);

					var SpaceOnLandingTileObstacle = Obstacles.FirstOrDefault(k => k.Intersects(SpaceOnLandingTile));
					if (SpaceOnLandingTileObstacle != null)
					{
						// we got platform
						View.AddToContentInfoColoredShapes(SpaceOnLandingTileObstacle, Brushes.Red);
						StopSearch();
					}

					var TheSign = Signs.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (TheSign != null)
					{
						View.AddToContentInfoColoredShapes(TheSign.ToObstacle(), Brushes.Cyan);
						SignFound(TheSign);
					}
				};

			var TheFoundSign = default(Sign);

			foreach (var k in
				from Direction in new[] { af, bf }
				let KeepGoingInThisDirection = new BooleanObject { Value = true }
				from StepsTakenInThatDirection in Enumerable.Range(1, 10)
				where KeepGoingInThisDirection.Value
				where TheFoundSign == null
				select new { ff = Direction.FixFirstParam(StepsTakenInThatDirection), KeepGoingInThisDirection })
			{
				
				ReturnSignIfPathIsOk(k.ff,
					value => TheFoundSign = value,
					() => k.KeepGoingInThisDirection.Value = false
				);
			}


			return new PlatformSnapshot { Cave = Cave };
		}

		[Script]
		public class BooleanObject
		{
			public bool Value;
		}
	}

}
