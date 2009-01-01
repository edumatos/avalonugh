using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Dialogs;

namespace AvalonUgh.Labs.Shared
{

	[Script]
	public class LabsCanvas : Canvas
	{
		public const int PortWidth = 640;
		public const int PortHeight = 400;

#if DEBUG
		public const int WindowPadding = 4;
		public const int DefaultWidth = 800;
		public const int DefaultHeight = 600;
#else
		public const int WindowPadding = 0;
		public const int DefaultWidth = PortWidth;
		public const int DefaultHeight = PortHeight;
#endif


		public LabsCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;


			var w = new Workspace(
				new Workspace.ConstructorArguments
				{
					WindowPadding = WindowPadding,

					PortWidth = PortWidth,
					PortHeight = PortHeight,

					DefaultWidth = DefaultWidth,
					DefaultHeight = DefaultHeight
				}
			);

			
			w.AttachContainerTo(this);
		}
	}


}
