using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.IO;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Editor.Sprites;
using System.ComponentModel;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor
{
	partial class Level
	{
		public enum SpawnLocationTag
		{
			Unknown,
			Invalid,
			Valid
		}

		public void ToPlatformSnapshotsWithReset()
		{
			ToPlatformSnapshots_Cache = null;
			ToPlatformSnapshots();
		}
		

		Func<IEnumerable<PlatformSnapshot>> ToPlatformSnapshots_Cache;
		public IEnumerable<PlatformSnapshot> ToPlatformSnapshots()
		{
			// we will recalculate tile obstacles only when they actually change
			if (ToPlatformSnapshots_Cache == null)
				ToPlatformSnapshots_Cache =
					new IBindingList[]
					{
						KnownCaves,
						KnownSigns,
						KnownStones,
						KnownFences,
						KnownBridges,
						KnownPlatforms,
						KnownRidges,
						KnownRidgeTrees,
					}.WhereListChanged(
						delegate
						{
							return ToPlatformSnapshotImplementation();
						}
					);

			return ToPlatformSnapshots_Cache();
		}

		public event Action<string> WriteLineEvent;

		public void WriteLine(string e)
		{
			if (WriteLineEvent != null)
				WriteLineEvent(e);
		}

		public Array2D<SpawnLocationTag> SpawnPointLookup
		{
			get
			{
				// trigger update
				ToPlatformSnapshots();

				return ToPlatformSnapshotImplementation_lookup;
			}
		}

		Array2D<SpawnLocationTag>  ToPlatformSnapshotImplementation_lookup;

		private IEnumerable<PlatformSnapshot> ToPlatformSnapshotImplementation()
		{
			Console.WriteLine("Level.ToPlatformSnapshots");

			// this lookup will remember where we already have looked and
			// where we should not be looking at the next iteration
			var lookup = new Array2D<SpawnLocationTag>(
				this.Map.Width,
				this.Map.Height
			);

			ToPlatformSnapshotImplementation_lookup = lookup;

			// actionscript did not zero the values
			lookup.ForEach(
				(x, y) => lookup[x, y] = SpawnLocationTag.Unknown
			);

			// clear any thought shapes
			ContentInfoColoredShapes_PlatformSnapshots.RemoveAll();

			// regardless of us returning that snapshot
			// it may have already be rendered
			var platforms =
				Enumerable.ToArray(
					from k in this.KnownCaves
					let p = PlatformSnapshot.Of(this, k)
					where p.IncludedSpace != null
					where p.WaitPosition != null
					select p
				);

			WriteLine("platforms: " + platforms.Length);

			// each time the platforms change we will be able to recalculate
			// travel space
			// does the travel space have a platform beneath?
			// how high are we from the platform?
			// first for each platform we will scan its airspace

			var SpawnLocationsByPlatforms =
				from platform in platforms
				let y = Convert.ToInt32((platform.IncludedSpace.Top / (PrimitiveTile.Heigth * this.Zoom)) - 1)
				let x = Convert.ToInt32((platform.IncludedSpace.Left / (PrimitiveTile.Width * this.Zoom)))
				select new { platform, x, y };

			Func<int, int, Obstacle> GetObstacle =
				(x, y) =>
					new Obstacle
					{
						Left = PrimitiveTile.Width * this.Zoom * (x),
						Top = PrimitiveTile.Heigth * this.Zoom * y,
						Width = PrimitiveTile.Width * this.Zoom * 2,
						Height = PrimitiveTile.Heigth * this.Zoom * 2,
					};

			var StartupLocations =
				SpawnLocationsByPlatforms.SelectMany(
					k => Enumerable.Range(0, k.platform.TotalValidSteps - 1).Select(ax => new { k, ax })
				).ToArray();


			WriteLine("StartupLocations: " + StartupLocations.Length);


			var WhereToLookNext =
				LambdaExtensions.ToConcatStream(
					from j in StartupLocations
					// will this fail?
					let x = j.k.x + j.ax
					select new
					{
						o = GetObstacle(x, j.k.y),
						j.k.platform,
						x,
						j.k.y,
						lookup
					}
				);



			var CountValid = 0;
			var CountReentry = 0;

			foreach (var p in WhereToLookNext)
			{
				//Console.WriteLine(new { p.x, p.y });

				if (p.lookup[p.x, p.y] != SpawnLocationTag.Unknown)
				{
					CountReentry++;
					continue;
				}

				var h = this.ToObstacles().Any(k => k.Intersects(p.o));

				if (h)
				{
					p.lookup[p.x, p.y] = SpawnLocationTag.Invalid;

					// this space is invalid to spawn a vehicle in...
				}
				else
				{
					p.lookup[p.x, p.y] = SpawnLocationTag.Valid;
					CountValid++;

					// this space is ok to spawn a vehicle in it.
					ContentInfoColoredShapes_PlatformSnapshots.Add(p.o, Brushes.YellowGreen, 0.05);

					// continue search
					Action<int, int> DistinctAdd =
						(x, y) =>
						{
							if (!p.lookup.ContainsIndex(x, y))
								return;

							if (p.lookup[x, y] != SpawnLocationTag.Unknown)
								return;

							WhereToLookNext.Add(
								new
								{
									o = GetObstacle(x, y),
									p.platform,
									x,
									y,
									p.lookup
								}
							);
						};

					DistinctAdd(p.x - 1, p.y);
					DistinctAdd(p.x + 1, p.y);
					DistinctAdd(p.x, p.y - 1);
					DistinctAdd(p.x, p.y + 1);


				}
			}

			WriteLine("CountValid: " + CountValid);
			WriteLine("CountReentry: " + CountReentry);


			//ContentInfoColoredShapes_PlatformSnapshots.Add(
			//    new Obstacle
			//    {
			//        Left = 100,
			//        Top = 200,
			//        Width = 100,
			//        Height = 100
			//    }
			//    , Brushes.OrangeRed, 0.2
			//);

			return platforms.AsEnumerable();
		}



	}
}
