using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Editor.Sprites;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class PlatformScan
	{
		public readonly ISupportsObstacle Target;
		public readonly Level Level;
		public readonly Tile[] LandingTiles;
		public readonly Vehicle[] Vehicles;
		public readonly Actor[] Actors;

		public PlatformScan(ISupportsObstacle Target, Level Level)
		{
			this.Target = Target;
			this.Level = Level;

			var KnownLandingTiles = Level.KnownLandingTiles.ToArray();

			var BeneathTarget = Target.ToObstacle(Target.X, Target.Y + Level.Zoom);

			var Platforms = new List<Tile>();
			var Platform = KnownLandingTiles.FirstOrDefault(k => k.ToObstacle().Intersects(BeneathTarget));

			Action<Tile, int> HorizontalScan = null;


			HorizontalScan =
				(p, x) =>
				{
					var t = p.ToObstacle(p.X + Level.Zoom * x, p.Y);
					var z = KnownLandingTiles.Where(k => k != p).FirstOrDefault(k => k.ToObstacle().Intersects(t));

					if (z != null)
					{
						Platforms.Add(z);
						HorizontalScan(z, x);
					}
				};

			if (Platform != null)
			{
				Platforms.Add(Platform);

				HorizontalScan(Platform, 1);
				HorizontalScan(Platform, -1);

				
			}

			

			this.LandingTiles = Platforms.ToArray();

			var Actors = new List<Actor>();
			var Vehicles = new List<Vehicle>();

			foreach (var p in this.LandingTiles)
			{
				var t = p.ToObstacle(p.X, p.Y - Level.Zoom);


				//Actors.AddRange(Level.knownac
			}

			this.Actors = Actors.ToArray();
			this.Vehicles = Vehicles.ToArray();
		}
	}
}
