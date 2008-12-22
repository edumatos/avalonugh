using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.Editor
{
	[Script]
	public class ArrowSelector : SelectorBase
	{
		public ArrowSelector()
		{
			this.ImageWidth = 32;
			this.ImageHeight = 32;

			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "Aero_Arrow",
					Index = -1,
					Extension = "png"
				};

			this.Sizes =
				new View.SelectorInfo[]
				{
					new View.SelectorInfo(),
				};
		}
	}
}
