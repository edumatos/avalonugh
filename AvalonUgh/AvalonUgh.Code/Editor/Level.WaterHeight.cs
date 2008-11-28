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

namespace AvalonUgh.Code.Editor
{
	partial class Level
	{
		public int WaterTop
		{
			get
			{
				return this.ActualHeight - this.WaterHeight;
			}
		}

		public int WaterHeight
		{
			get
			{
				return this.AttributeWater.Value * Zoom;
			}
		}
	}
}
