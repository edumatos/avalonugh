using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class KnownLevels
	{
		public readonly LevelReference[] Levels;

		public KnownLevels()
		{
			this.Levels = Enumerable.Range(0, 52).ToArray(i => new LevelReference(i));
		}
	}
}
