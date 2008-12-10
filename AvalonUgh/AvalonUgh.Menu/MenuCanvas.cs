using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Menu.Shared
{
	[Script]
	public class MenuCanvas : Canvas
	{
		public const int Zoom = 2;

		public const int DefaultWidth = 320 * Zoom;
		public const int DefaultHeight = 200 * Zoom;

		public MenuCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			Background = Brushes.Black;

			var y = 0;

			#region WriteBitmapFont
			Action<string, string> WriteBitmapFont =
				(font, text) =>
				{
					var x = 0;

					foreach (char c in text.ToLower())
					{
						var s = Convert.ToString(c);

						if (s == "?")
							s = "_Question";

						if (s == ":")
							s = "_Colon";

						if (s == " ")
							s = "_Space";

						new Image
						{
							Source =
								(font + "/" + s + ".png").ToSource(),
							Stretch = Stretch.Fill,
							Width = PrimitiveFont.Width * Zoom,
							Height = PrimitiveFont.Heigth * Zoom
						}.AttachTo(this).MoveTo(x * PrimitiveFont.Width * Zoom, y * PrimitiveFont.Heigth * Zoom);

						x++;

					}

					y++;
				};
			#endregion

			var WriteBlue = WriteBitmapFont.FixFirstParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Blue);
			var WriteBrown = WriteBitmapFont.FixFirstParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Brown);

			WriteBrown("");
			WriteBrown("   congratulations");
			WriteBrown(" you are one of the");
			WriteBrown("  five best cabbies");
			WriteBrown("");
			WriteBrown("enter your name:");

			WriteBlue("zproxy?");
		

		}
	}
}
