using ScriptCoreLib;
using ScriptCoreLib.Shared;

using ScriptCoreLib.PHP;
using System;
using System.Text;
using System.IO;
using AvalonUgh.Comparision.Shared;
using AvalonUgh.Labs.Shared;

namespace AvalonUgh.Comparision.Server
{
	[Script]
	static class Application
	{
		public const string Filename = "index.php";

		// change: C:\util\xampplite\apache\conf\httpd.conf

		// http://localhost/jsc/AvalonUgh.Comparision

		//Alias /jsc/AvalonUgh.Comparision "C:\work\code.google\avalonugh\AvalonUgh\AvalonUgh.Comparision\bin\Release\web"
		//<Directory "C:\work\code.google\avalonugh\AvalonUgh\AvalonUgh.Comparision\bin\Release\web">
		//       Options Indexes FollowSymLinks ExecCGI
		//       AllowOverride All
		//       Order allow,deny
		//       Allow from all
		//</Directory>

		/// <summary>
		/// php script will invoke this method
		/// </summary>
		[Script(NoDecoration = true)]
		public static void Application_Entrypoint()
		{

			Console.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
			Console.WriteLine("<html>");
			Console.WriteLine("<head>");

			Console.WriteLine("<meta name='description' content='jsc can convert your C# Application to PHP, JavaScript, Actionscript and Java' />");
			Console.WriteLine("<meta name='keywords' content='c# decompiler, cross compiler, flash, actionscript, php, java, javascript, ajax, web2, dhtml, jsc' />");
			Console.WriteLine("<link rel='alternate' type='application/rss+xml' title='RSS 2.0' href='http://zproxy.wordpress.com/feed/' />");


			Console.WriteLine("<title>" + "Avalon Ugh!".WithBranding() + "</title>");

			Console.WriteLine("<link rel='stylesheet' type='text/css' href='assets/AvalonUgh.Comparision/WebPage.css' />");
			
			Console.WriteLine("</head>");
			Console.WriteLine("<body>");

			(AvalonUgh.Comparision.Shared.KnownAssets.Path.Assets + "/jsc.png").ToImageToConsole();

		

			Action<int, int, string> CreateIFrame =
				(w, h, src) => Console.WriteLine("<iframe  width='" + w + "' height='" + h + "' src='" + src + "' ></iframe>"); ;

			Action<string> CreateVersion =
				src =>
				{
					if (!File.Exists(src))
						return;

					CreateIFrame(LabsCanvas.DefaultWidth, LabsCanvas.DefaultHeight, src);
				};

			Console.WriteLine("<center>");

			CreateVersion("LabsDocument.htm");
			CreateVersion("LabsDocument.htm");
			CreateVersion("AvalonUgh.Labs.XBAP.xbap");

			Console.WriteLine("</center>");

			

			Console.WriteLine("</body>");
			Console.WriteLine("</html>");

		}
	}
}
