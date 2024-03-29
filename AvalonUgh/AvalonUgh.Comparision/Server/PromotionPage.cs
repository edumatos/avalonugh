﻿using System;
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
	public class PromotionPage
	{
		// http://localhost/jsc/AvalonUgh.Comparision/?multiplayer
		public static void Render()
		{
			//var ReferenceToAssets = new AvalonUgh.Assets.Server.__AssetsImplementationDetails();
			var ReferenceToAssets = AvalonUgh.Assets.Shared.KnownAssets.Default;

			Console.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
			Console.WriteLine("<html>");
			Console.WriteLine("<head>");

			Console.WriteLine("<title>" + "Avalon Ugh!".WithBranding() + "</title>");
			Console.WriteLine("</head>");
			Console.WriteLine("<body>");
			Console.WriteLine("<style>");

			Console.WriteLine(@"
				body
				{
					background: url(assets/AvalonUgh.Comparision/TiledBackground.png);
				}
			");

			Console.WriteLine("</style>");

			Console.WriteLine("<p>");

			#region WriteBitmapFont
			Action<string, string> WriteBitmapFont =
				(font, text) =>
				{
					foreach (char c in text.ToLower())
					{
						var s = Convert.ToString(c);

						if (s == ":")
							s = "_Colon";

						if (s == " ")
							s = "_Space";

						(font + "/" + s + ".png").ToImageToConsoleWithStyle("width: 15px; height: 16px;");

					}
				};
			#endregion

			WriteBitmapFont(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Brown, "Avalon Ugh: Multiplayer");
			
			Console.WriteLine("<br />");

			WriteBitmapFont(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Blue, "Powered By JSC");
		


			Console.WriteLine("</p>");


		
			Console.WriteLine("<center>");

			var i = new FlashContainer();

			i.EmbedContent.URL = "http://nonoba.com/zproxy/avalon-ugh/embed";
			i.ContentWidth = 840;
			i.ContentHeight = 400;

			//i.EmbedContent.URL = "file:///C:/work/code.google/avalonugh/AvalonUgh/Public/LabsFlash.swf";
		
			Console.WriteLine(i.Container.ToString());




			Console.WriteLine("</center>");


			Console.WriteLine("</body>");
			Console.WriteLine("</html>");
		}
	}
}
