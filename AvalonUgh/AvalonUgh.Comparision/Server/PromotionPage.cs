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

			Console.WriteLine("<table border='0' cellspacing='0' cellpadding='0'>");
			Console.WriteLine(@"
		<tr>
			<td style='width: 27px; height: 53px;'><img src='assets/AvalonUgh.Comparision/layout_top_left.png' style='width: 27px; height: 53px;' /></td>
			<td style='width: 840px; height: 53px;'><img src='assets/AvalonUgh.Comparision/layout_top_span.png' style='width: 840px; height: 53px;' /></td>
			<td style='width: 27px; height: 53px;'><img src='assets/AvalonUgh.Comparision/layout_top_right.png' style='width: 27px; height: 53px;' /></td>
		</tr>
			");

			Console.WriteLine("<tr><td><img src='assets/AvalonUgh.Comparision/layout_middle_left.png' style='width: 27px; height: 400px;' /></td><td>");

			
			Console.WriteLine("<object width='840' height='400'><param name='movie' value='http://nonoba.com/zproxy/avalon-ugh/embed'></param><param name='allowScriptAccess' value='always' ></param><param name='allowNetworking' value='all' ></param><embed src='http://nonoba.com/zproxy/avalon-ugh/embed' allowNetworking='all' allowScriptAccess='always' type='application/x-shockwave-flash' width='840' height='400'></embed></object>");

			Console.WriteLine("</td><td><img src='assets/AvalonUgh.Comparision/layout_middle_right.png' style='width: 27px; height: 400px;' /></td></tr>");

			Console.WriteLine(@"
		<tr>
			<td><img src='assets/AvalonUgh.Comparision/layout_bottom_left.png' style='width: 27px; height: 53px;' /></td>
			<td><img src='assets/AvalonUgh.Comparision/layout_bottom_span.png' style='width: 840px; height: 53px;' /></td>
			<td><img src='assets/AvalonUgh.Comparision/layout_bottom_right.png' style='width: 27px; height: 53px;' /></td>
		</tr>
			");
			
			Console.WriteLine("</table>");

			Console.WriteLine("</center>");


			Console.WriteLine("</body>");
			Console.WriteLine("</html>");
		}
	}
}
