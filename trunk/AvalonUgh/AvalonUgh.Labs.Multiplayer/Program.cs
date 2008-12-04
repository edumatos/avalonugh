using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Labs.Multiplayer.Shared;
using ScriptCoreLib.CSharp.Avalon.Extensions;

namespace AvalonUgh.Labs.Multiplayer
{
	class Program
	{
		// this project will build to
		// nonoba server dll
		// nonoba client flash
		// desktop inmemory multiplayer

		[STAThread]
		static public void Main(string[] args)
		{
			new OrcasAvalonApplicationCanvas().ToWindow().ShowDialog();
		}
	}
}
