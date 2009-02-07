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

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class PlatformSnapshot
	{
		public Cave Cave;

		/// <summary>
		/// A cave can be referenced by multiple signs giving the passanger to indicate 
		/// his wishes in multiple sign numbers. It only will create confusion for
		/// the driver tho.
		/// </summary>
		public readonly List<Sign> CaveSigns = new List<Sign>();

		public Obstacle WaitPosition;

		public Obstacle IncludedSpace;

		public static PlatformSnapshot Of(Level Level, Cave Cave)
		{
			// show that we are thinking about that cave

			var o = Cave.ToObstacle();

			//View.AddToContentInfoColoredShapes(o, Brushes.Cyan);

			var LandingTiles = Level.KnownLandingTiles.ToArray();

			var TaxiVehicles = Level.KnownVehicles.Where(k => k.CurrentDriver != null).Where(k => k.GetVelocity() == 0).ToArray();
			var Signs = Level.KnownSigns.ToArray();
			var Obstacles = Level.ToObstacles().ToArray();
			var Caves = Level.KnownCaves.Where(k => k != Cave).ToArray();

			var a = new Obstacle
			{
				Left = o.Left,
				Top = o.Bottom,
				Width = Level.Zoom * PrimitiveTile.Width,
				Height = Level.Zoom * PrimitiveTile.Heigth,
			};

			Func<int, int, Obstacle> f =
				(x, y) => a.WithOffset(
					Level.Zoom * PrimitiveTile.Width * x,
					Level.Zoom * PrimitiveTile.Heigth * y
				);

			Func<int, int> PassInteger = y => y;

			var af = f.With(x => x * -1, PassInteger);
			var bf = f.With(x => x + 1, PassInteger);

			var b = bf(0, 0);

			var p = new PlatformSnapshot { Cave = Cave };


			Action<A> CheckPath =
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
					//Level.AddToContentInfoColoredShapes(LandingTile, Brushes.Green);

					var SpaceOnLandingTile = e.GetObstacleAtHeight(-1);

					var SpaceOnLandingTileObstacle = Obstacles.FirstOrDefault(k => k.Intersects(SpaceOnLandingTile));
					if (SpaceOnLandingTileObstacle != null)
					{
						// we got platform
						Level.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.Red);
						e.StopSearch();
						return;
					}

					if (p.IncludedSpace == null)
					{
						p.IncludedSpace = SpaceOnLandingTile;
					}
					else
					{
						p.IncludedSpace = p.IncludedSpace.GrowTo(SpaceOnLandingTile);
					}


					var AnotherCave = Caves.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (AnotherCave != null)
					{
						e.StopSearch();
						Level.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.White);
						return;
					}

					var TheSign = Signs.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (TheSign != null)
					{
						Level.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.Cyan);
						TheSign.DistinctAddTo(p.CaveSigns);
					}



				};

			//var TheFoundSign = default(Sign);
			var PossibleDirections = new[] { af, bf }.ToFlaggable();
			var PossibleSteps = Enumerable.Range(0, 20).ToFlaggable();


			foreach (var k in
				PossibleSteps.SelectMany(
					Step =>
						PossibleDirections.Select(
							Direction =>
								new { Step, Direction }
						)
				)
			)
			{

				var z = new A
				{

					GetObstacleAtHeight = k.Direction.Current.FixFirstParam(k.Step.Current),
					StopSearch =
						delegate
						{
							k.Direction.SkipAtNextIteration = true;
						},

				};



				CheckPath(z);
			}

			if (p.IncludedSpace != null)
				Level.AddToContentInfoColoredShapes(p.IncludedSpace, Brushes.Green, 0.2);

			#region find WaitPosition
			p.CaveSigns.FirstOrDefault().Apply(
				TheFirstSign =>
				{
					var TheFirstSignAsObstacle = TheFirstSign.ToObstacle();

					var x = 0.0;

					if (Cave.X < TheFirstSignAsObstacle.X)
						x = o.Right + (TheFirstSignAsObstacle.Left - o.Right) / 2;
					else
						x = o.Left + (TheFirstSignAsObstacle.Right - o.Left) / 2;


					p.WaitPosition = new Obstacle
					{
						Left = x - Level.Zoom,
						Right = x + Level.Zoom,
						Top = TheFirstSignAsObstacle.Top,
						Bottom = TheFirstSignAsObstacle.Bottom
					};

					Level.AddToContentInfoColoredShapes(p.WaitPosition, Brushes.GreenYellow);
				}
			);
			#endregion


			return p;
		}

		[Script]
		public class A
		{
			public Func<int, Obstacle> GetObstacleAtHeight;
			public Action StopSearch;
		}
	}

}
