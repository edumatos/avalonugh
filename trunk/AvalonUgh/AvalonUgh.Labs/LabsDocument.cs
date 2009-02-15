using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.JavaScript.Extensions;
using ScriptCoreLib.JavaScript.DOM.HTML;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Labs.JavaScript
{
	using TargetCanvas = global::AvalonUgh.Labs.Shared.LabsCanvas;
	using ScriptCoreLib.JavaScript;
	using ScriptCoreLib.Shared.Drawing;

	[Script, ScriptApplicationEntryPoint]
	public class LabsDocument
	{
		// http://localhost/jsc/AvalonUgh.Labs

		//Alias /jsc/AvalonUgh.Labs "C:\work\code.google\avalonugh\AvalonUgh\AvalonUgh.Labs\bin\Release\web"
		//<Directory "C:\work\code.google\avalonugh\AvalonUgh\AvalonUgh.Labs\bin\Release\web">
		//       Options Indexes FollowSymLinks ExecCGI
		//       AllowOverride All
		//       Order allow,deny
		//       Allow from all
		//</Directory>

		public LabsDocument(IHTMLElement e)
		{
			Native.Document.body.style.margin = "0";
			Native.Document.body.style.padding = "0";
			Native.Document.body.style.border = "0";
			Native.Document.body.style.backgroundColor = Color.Black;

			new IHTMLElement(IHTMLElement.HTMLElementEnum.center).Apply(
				center =>
				{
					var c = new IHTMLDiv().AttachTo(center);

					c.style.position = ScriptCoreLib.JavaScript.DOM.IStyle.PositionEnum.relative;
					c.style.SetSize(TargetCanvas.DefaultWidth, TargetCanvas.DefaultHeight);
					c.style.overflow = ScriptCoreLib.JavaScript.DOM.IStyle.OverflowEnum.hidden;

					//// wpf here
					new TargetCanvas(true).AttachToContainer(c);
				}
			).AttachToDocument();

		}

		static LabsDocument()
		{
			typeof(LabsDocument).SpawnTo(i => new LabsDocument(i));
		}

	}
}
