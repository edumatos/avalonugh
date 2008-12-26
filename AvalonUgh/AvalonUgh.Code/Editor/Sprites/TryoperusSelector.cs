using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class TryoperusSelector : SelectorBase
	{
		public TryoperusSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "tryo",
					Index = 0,
					AnimationFrame = 0,
					Extension = "png",
					Width = 2,
					Height = 2
				};
		}
	}
}
