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

		public int TotalValidSteps;

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


			Func<A, bool> CheckPath =
				e =>
				{
					var LandingTile = e.GetObstacleAtHeight(0);

					var LandingTileReference = LandingTiles.FirstOrDefault(k => k.ToObstacle().Intersects(LandingTile));

					if (LandingTileReference == null)
					{
						e.StopSearch();
						return false;
					}

					// we got platform
					//Level.AddToContentInfoColoredShapes(LandingTile, Brushes.Green);

					var SpaceOnLandingTile = e.GetObstacleAtHeight(-1);

					var SpaceOnLandingTileObstacle = Obstacles.FirstOrDefault(k => k.Intersects(SpaceOnLandingTile));
					if (SpaceOnLandingTileObstacle != null)
					{
						// we got platform
						Level.ContentInfoColoredShapes_PlatformSnapshots.Add(SpaceOnLandingTile, Brushes.Red);
						e.StopSearch();
						return false;
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
						Level.ContentInfoColoredShapes_PlatformSnapshots.Add(SpaceOnLandingTile, Brushes.White);
						return false;
					}

					var TheSign = Signs.FirstOrDefault(k => k.ToObstacle().Intersects(SpaceOnLandingTile));
					if (TheSign != null)
					{
						Level.ContentInfoColoredShapes_PlatformSnapshots.Add(SpaceOnLandingTile, Brushes.Cyan);
						TheSign.DistinctAddTo(p.CaveSigns);
					}


					return true;
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



				if (CheckPath(z))
				{
					p.TotalValidSteps++;

					// a step in current direction was valid.
				}
			}

			if (p.IncludedSpace != null)
				Level.ContentInfoColoredShapes_PlatformSnapshots.Add(p.IncludedSpace, Brushes.Green, 0.2);

			#region find WaitPosition
			p.CaveSigns.FirstOrDefault().Apply(
				TheFirstSign =>
				{
					var TheFirstSignAsObstacle = TheFirstSign.ToObstacle();

					var x = 0.0;

					if (TheFirstSign.WaitPositionPreference != Sign.WaitPositionPreferences.BeforeCave)
					{

						var DirectionModifier = Cave.X < TheFirstSignAsObstacle.X;

						if ((int)TheFirstSign.WaitPositionPreference < 0)
							DirectionModifier = !DirectionModifier;

						var DirectionMultiplier = (double)(Math.Abs((int)TheFirstSign.WaitPositionPreference) - 1) / ((double)Sign.WaitPositionPreferences._DivideBy - 1);

						if (DirectionModifier)
							x = o.Right + (TheFirstSignAsObstacle.Left - o.Right) * DirectionMultiplier;
						else
							x = o.Left + (TheFirstSignAsObstacle.Right - o.Left) * DirectionMultiplier;

					}
					else
					{
						x = o.X;
					}

					p.WaitPosition = new Obstacle
					{
						Left = x - Level.Zoom * 1,
						Right = x + Level.Zoom * 1,
						Top = TheFirstSignAsObstacle.Top,
						Bottom = TheFirstSignAsObstacle.Bottom
					};

					Level.ContentInfoColoredShapes_PlatformSnapshots.Add(p.WaitPosition, Brushes.GreenYellow);
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
