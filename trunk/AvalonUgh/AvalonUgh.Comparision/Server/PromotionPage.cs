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
		public static void Render()
		{
			var ReferenceToAssets = new AvalonUgh.Assets.Server.__AssetsImplementationDetails();
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

			(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Brown + "/m.png").ToImageToConsole();

			Console.WriteLine("</p>");

			Console.WriteLine("<center>");
			Console.WriteLine("<object width='840' height='400'><param name='movie' value='http://nonoba.com/zproxy/avalon-ugh/embed'></param><param name='allowScriptAccess' value='always' ></param><param name='allowNetworking' value='all' ></param><embed src='http://nonoba.com/zproxy/avalon-ugh/embed' allowNetworking='all' allowScriptAccess='always' type='application/x-shockwave-flash' width='840' height='400'></embed></object>");
			Console.WriteLine("</center>");


			Console.WriteLine("</body>");
			Console.WriteLine("</html>");
		}
	}
}
