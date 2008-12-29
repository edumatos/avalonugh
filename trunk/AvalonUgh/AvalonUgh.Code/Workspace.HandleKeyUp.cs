using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Editor;
using System.ComponentModel;
using AvalonUgh.Code.Dialogs;
using System.Windows.Input;
using ScriptCoreLib.Shared.Avalon.Tween;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using System.Windows.Shapes;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Code
{
	partial class Workspace 
	{
		void HandleKeyUp(object sender, KeyEventArgs args)
		{
			// oem7 will trigger the console
			if (args.Key == Key.Oem7)
			{
				args.Handled = true;

				if (Console.AnimatedTop == 0)
				{
					Console.AnimatedTop = -Console.Height;
				}
				else
				{
					Console.AnimatedTop = 0;
				}

				// the console is on top
				// of the game view
				// and under the transparent touch overlay
				// when the view is in editor mode
			}
		}
	}
}
