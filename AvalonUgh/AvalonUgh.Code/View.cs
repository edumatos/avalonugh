using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;

namespace AvalonUgh.Code
{
	[Script]
	public class View : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public Canvas Background { get; set; }

		public Canvas Platforms { get; set; }

		public Canvas Entities { get; set; }

		public Canvas Water { get; set; }
		
		public Canvas Spotlight { get; set; }

		public Canvas Overlay { get; set; }
	}
}
