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

		/// <summary>
		/// A cave can be referenced by multiple signs giving the passanger to indicate 
		/// his wishes in multiple sign numbers. It only will create confusion for
		/// the driver tho.
		/// </summary>
		public readonly List<Sign> CaveSigns = new List<Sign>();

		/// <summary>
		/// These vehicles are ready for passengers. We will select the first in this list. The list should be 
		/// distance sorted.
		/// </summary>
		public readonly List<Vehicle> CaveVehicles = new List<Vehicle>();



		public static PlatformSnapshot Of(View View, Cave Cave)
		{
			// show that we are thinking about that cave

			var o = Cave.ToObstacle();

			//View.AddToContentInfoColoredShapes(o, Brushes.Cyan);

			var LandingTiles = View.Level.KnownLandingTiles.ToArray();
			var Vehicles = View.Level.KnownVehicles.Where(k => k.CurrentDriver != null).ToArray();
			var Signs = View.Level.KnownSigns.ToArray();
			var Obstacles = View.Level.ToObstacles().ToArray();
			var Caves = View.Level.KnownCaves.Where(k => k != Cave).ToArray();

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
					View.AddToContentInfoColoredShapes(LandingTile, Brushes.Green);

					var SpaceOnLandingTile = e.GetObstacleAtHeight(-1);

					var SpaceOnLandingTileObstacle = Obstacles.FirstOrDefault(k => k.Intersects(SpaceOnLandingTile));
					if (SpaceOnLandingTileObstacle != null)
					{
						// we got platform
						View.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.Red);
						e.StopSearch();
						return;
					}

					var AnotherCave = Caves.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (AnotherCave != null)
					{
						e.StopSearch();
						View.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.White);
						return;
					}

					var TheSign = Signs.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (TheSign != null)
					{
						View.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.Cyan);
						TheSign.DistinctAddTo(p.CaveSigns);
					}

					var TheVehicle = Vehicles.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (TheVehicle != null)
					{
						View.AddToContentInfoColoredShapes(SpaceOnLandingTile, Brushes.GreenYellow);
						TheVehicle.DistinctAddTo(p.CaveVehicles);
					}

					// there could be a cave in the path
					// there could be multiple signs
					// there could be a vehicle

				};

			//var TheFoundSign = default(Sign);
			var PossibleDirections = new[] { af, bf }.ToFlaggable();
			var PossibleSteps = Enumerable.Range(0, 10).ToFlaggable();


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
