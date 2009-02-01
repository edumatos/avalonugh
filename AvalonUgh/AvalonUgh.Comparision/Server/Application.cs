using System;
using System.IO;
using System.Text;
using AvalonUgh.Comparision.Shared;
using AvalonUgh.Labs.Shared;
using ScriptCoreLib;
using ScriptCoreLib.PHP;
using ScriptCoreLib.Shared;

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

			var Query = Native.SuperGlobals.Server[Native.SuperGlobals.ServerVariables.QUERY_STRING];

			if (Query == "multiplayer")
			{
				PromotionPage.Render();
				return;
			}

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

			Console.WriteLine("<a href='?'>");
			(AvalonUgh.Comparision.Shared.KnownAssets.Path.Assets + "/jsc.png").ToImageToConsole();
			Console.WriteLine("</a>");

			Native.Link("multiplayer", "?multiplayer");


			Action<int, int, string> CreateIFrame =
				(w, h, src) => Console.WriteLine("<iframe frameborder='0' width='" + w + "' height='" + h + "' src='" + src + "' ></iframe>"); ;

			Action<string, string> CreateVersion =
				(title, src) =>
				{
					if (!File.Exists(src))
					{
						("missing: " + src).ToCommentToConsole();

						return;
					}

					Console.WriteLine("<h2><a href='?" + title + "'>" + title + "</a></h2>");

					if (Query == title)
						CreateIFrame(LabsCanvas.DefaultWidth, LabsCanvas.DefaultHeight, src);
				};

			Console.WriteLine("<center>");

			CreateVersion("JavaScript", "LabsDocument.htm");
			CreateVersion("Flash", "LabsFlash.htm");
			CreateVersion("XBAP", "AvalonUgh.Labs.XBAP.xbap");

			Console.WriteLine("</center>");


			

			Console.WriteLine("</body>");
			Console.WriteLine("</html>");

		}
	}
}
