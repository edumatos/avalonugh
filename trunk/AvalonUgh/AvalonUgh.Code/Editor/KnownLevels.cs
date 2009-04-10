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

		public readonly LevelReference DefaultLobbyLevel;
		public readonly LevelReference DefaultMissionLevel;
		public readonly LevelReference DefaultCaveLevel;
		//public const int DefaultMissionLevelNumber = 1;
		public const int DefaultMissionLevelNumber = 18;

		public readonly int DefaultEditorLevel = DefaultMissionLevelNumber;

		public KnownLevels()
		{
			var a = new List<LevelReference>();

			//this.DefaultLobbyLevel = new LevelReference(0);
			//this.DefaultMissionLevel = new LevelReference(1);

			//a.Add(this.DefaultLobbyLevel);
			//a.Add(this.DefaultMissionLevel);
			a.AddRange(Enumerable.Range(0, 70).Select(i => new LevelReference(i)));

			this.DefaultLobbyLevel = a[0];
			this.DefaultMissionLevel = a[DefaultMissionLevelNumber];

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
