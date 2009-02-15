using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Labs.Shared;
using ScriptCoreLib.CSharp.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;


namespace AvalonUgh.Labs
{
	class Program
	{

		[STAThread]
		static public void Main(string[] args)
		{
			TimerEventTracer.Trace(
				delegate
				{
					var w = new LabsCanvas(false).ToWindow();

					w.ShowDialog();
				}
			);
			

			Console.WriteLine("press any key!");
			Console.ReadKey();
		}
	}
}
