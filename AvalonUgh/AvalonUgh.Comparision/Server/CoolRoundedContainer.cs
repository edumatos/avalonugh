using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.PHP;

namespace AvalonUgh.Comparision.Server
{
	[Script]
	public partial class CoolRoundedContainer
	{
		public IHTMLElement Container { get; set; }
		public IHTMLElement ContentContainer { get; set; }

		public BackgroundContainerType Background { get; set; }

		public IHTMLElement Fill { get; set; }

		public Func<string> GetContent;

		public event Action ContentSizeChanged;

		int InternalContentWidth;
		public int ContentWidth
		{
			get
			{
				return InternalContentWidth;
			}
			set
			{
				InternalContentWidth = value;
				this.Container.Style.width = (value + (27 * 2)) + "px";
				this.ContentContainer.Style.width = (value + (27 * 2)) + "px";
				this.Background.ContentWidth = value;
				if (ContentSizeChanged != null)
					ContentSizeChanged();
			}
		}

		int InternalContentHeight;
		public int ContentHeight
		{
			get
			{
				return InternalContentHeight;
			}
			set
			{
				InternalContentHeight = value;
				this.Container.Style.height = (value + (53 * 2)) + "px";
				this.ContentContainer.Style.height = (value + (53 * 2)) + "px";
				this.Background.ContentHeight = value;
				if (ContentSizeChanged != null)
					ContentSizeChanged();
			}
		}

		public CoolRoundedContainer()
		{
			this.Container = new IHTMLElement
			{
				Style = new IStyle
				{
				}
			};

			this.ContentContainer = new IHTMLElement
			{
				Style = new IStyle
				{
					position = "relative"
				}
			};

			this.ContentContainer = new IHTMLElement
			{
				Style = new IStyle
				{
					position = "relative"
				}
			};

			this.Background = new BackgroundContainerType();



			this.Container.GetContent =
				delegate
				{
					var ContentContainerContent = "";

					ContentContainerContent += this.Background.Container.ToString();

					if (this.GetContent != null)
						ContentContainerContent += this.GetContent();

					this.ContentContainer.Content = ContentContainerContent;

					var Content = "";

					Content += this.ContentContainer.ToString();

					return Content;
				};

			this.ContentWidth = 840;
			this.ContentHeight = 400;
		}
	}
}
