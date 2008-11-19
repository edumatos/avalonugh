using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;

namespace AvalonUgh.Code
{
	/// <summary>
	/// View enables to display maps that are larger than the provided
	/// client area by enabling autoscroll. In the editor mode the scroll
	/// should follow the mouse while in playmode it should follow
	/// the active player be it a vehicle or an actor
	/// </summary>
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
