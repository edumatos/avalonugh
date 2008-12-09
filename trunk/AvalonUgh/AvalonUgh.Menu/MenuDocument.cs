using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.JavaScript.Extensions;
using ScriptCoreLib.JavaScript.DOM.HTML;

namespace AvalonUgh.Menu.JavaScript
{
	using TargetCanvas = global::AvalonUgh.Menu.Shared.MenuCanvas;

	[Script, ScriptApplicationEntryPoint]
	public class MenuDocument
	{
		public MenuDocument(IHTMLElement e)
		{
			// wpf here
			var clip = new IHTMLDiv();

			clip.style.position = ScriptCoreLib.JavaScript.DOM.IStyle.PositionEnum.relative;
			clip.style.SetSize(TargetCanvas.DefaultWidth, TargetCanvas.DefaultHeight);
			clip.style.overflow = ScriptCoreLib.JavaScript.DOM.IStyle.OverflowEnum.hidden;

			if (e == null)
				clip.AttachToDocument();
			else
				e.insertPreviousSibling(clip);

			AvalonExtensions.AttachToContainer(new TargetCanvas(), clip);

		}

		static MenuDocument()
		{
			typeof(MenuDocument).SpawnTo(i => new MenuDocument(i));
		}

	}
}
