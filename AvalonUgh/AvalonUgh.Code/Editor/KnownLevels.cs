using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class KnownLevels
	{
		public readonly LevelReference[] Levels;

		public readonly LevelReference DefaultCaveLevel;

		public KnownLevels()
		{
			var a = new List<LevelReference>();

			a.AddRange(Enumerable.Range(0, 56).Select(i => new LevelReference(i)));

			this.DefaultCaveLevel =
				new LevelReference(
					new LevelReference.StorageLocation
					{
						Embedded = new NameFormat
						{
							Path = Assets.Shared.KnownAssets.Path.Levels,
							Extension = "txt",
							Name = "level",
							Index = 1,
							AnimationFrame = 9000
						}
					}
				);

			a.Add(this.DefaultCaveLevel);

			this.Levels = a.ToArray();
		}
	}
}
