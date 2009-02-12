using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Comparision.Server
{
	[Script]
	public class FlashContainer : CoolRoundedContainer
	{
		public readonly IHTMLEmbed EmbedContent;

		public FlashContainer()
		{
			this.EmbedContent = new IHTMLEmbed
			{
				Style = new IStyle
				{
					position = "absolute",
					left = "27px",
					top = "53px",
				}
			};

			// <embed src='http://nonoba.com/zproxy/avalon-ugh/embed' allowNetworking='all' allowScriptAccess='always' type='application/x-shockwave-flash' width='840' height='400'></embed>

			this.GetContent =
				delegate
				{
					var c = EmbedContent.ToString();
					return c;
				};

			this.ContentSizeChanged +=
				delegate
				{
					this.EmbedContent.Width = this.ContentWidth;
					this.EmbedContent.Height = this.ContentHeight;
				};

			this.ContentWidth = 400;
			this.ContentHeight = 300;
		}

		
	}
}
