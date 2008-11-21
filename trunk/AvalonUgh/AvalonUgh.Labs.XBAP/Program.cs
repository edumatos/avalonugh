using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonUgh.Labs.XBAP
{
	using global::ScriptCoreLib.CSharp.Avalon.Extensions;
	using global::AvalonUgh.Labs.Shared;

	class Program
	{
		[STAThread]
		public static void Main()
		{
			AvalonExtensions.ToApplication<LabsCanvas>();
		}
	}
}
