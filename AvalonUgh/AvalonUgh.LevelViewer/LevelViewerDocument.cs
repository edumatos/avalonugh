using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.JavaScript.Extensions;
using ScriptCoreLib.JavaScript.DOM.HTML;

namespace AvalonUgh.LevelViewer.JavaScript
{
	using TargetCanvas = global::AvalonUgh.LevelViewer.Shared.LevelViewerCanvas;

	[Script, ScriptApplicationEntryPoint]
	public class LevelViewerDocument
	{
		public LevelViewerDocument(IHTMLElement e)
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

		static LevelViewerDocument()
		{
			typeof(LevelViewerDocument).SpawnTo(i => new LevelViewerDocument(i));
		}

	}
}
