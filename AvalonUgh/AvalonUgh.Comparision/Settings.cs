using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using ScriptCoreLib;
using ScriptCoreLib.Shared;
using AvalonUgh.Comparision.Client.Java;
using System.IO;
using System.Linq;

namespace AvalonUgh.Comparision
{
	static class Settings
	{

		public static void DefineEntryPoint(IEntryPoint e)
		{
			CreatePHPIndexPage(e, Server.Application.Filename, Server.Application.Application_Entrypoint);



			

		}

		#region PHP Section
		private static void CreatePHPIndexPage(IEntryPoint e, string file_name, Action entryfunction)
		{

			var w = new ScriptCoreLib.Shared.TextWriter();

			w.WriteLine("<?");

			SharedHelper.PHPInclude(w, SharedHelper.LocalModulesOf(Assembly.GetExecutingAssembly(), ScriptType.PHP));

			w.WriteLine(entryfunction.Method.Name + "();");

			w.Write("?>");

			e[file_name] = w.Text;
		}
		#endregion


	}

}
