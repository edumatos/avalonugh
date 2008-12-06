using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonUgh.Labs.MultiplayerTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("running Nonoba DevelopmentServer...");

			Nonoba.DevelopmentServer.Server.StartWithDebugging(100);
		}
	}
}
