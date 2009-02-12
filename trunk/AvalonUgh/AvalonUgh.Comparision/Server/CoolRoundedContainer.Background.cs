using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Comparision.Server
{
	public partial class CoolRoundedContainer
	{
		[Script]
		public class BackgroundContainerType
		{
			public IHTMLElement Container { get; set; }

			public IHTMLElement BackgroundContainer { get; set; }


			public IHTMLImage BorderTopLeft;
			public IHTMLImage BorderTopRight;
			public IHTMLImage BorderTopSpan;


			public IHTMLImage BorderMiddleLeft;
			public IHTMLImage BorderMiddleRight;

			public IHTMLImage BorderBottomLeft;
			public IHTMLImage BorderBottomRight;
			public IHTMLImage BorderBottomSpan;

			public IHTMLElement Fill;

			public int ContentWidth
			{
				set
				{
					this.Container.Style.width = (value + (27 * 2)) + "px";
					this.Fill.Style.width = (value) + "px";
					this.BackgroundContainer.Style.width = (value + (27 * 2)) + "px";

					this.BorderTopSpan.Style.width = (value) + "px";
					this.BorderTopRight.Style.left = (value + (27 * 1)) + "px";
					this.BorderBottomRight.Style.left = (value + (27 * 1)) + "px";
					this.BorderMiddleRight.Style.left = (value + (27 * 1)) + "px";
					this.BorderBottomSpan.Style.width = (value) + "px";


				}
			}

			public int ContentHeight
			{
				set
				{
					this.Container.Style.height = (value + (53 * 2)) + "px";
					this.Fill.Style.height = (value) + "px";
					this.BackgroundContainer.Style.height = (value + (53 * 2)) + "px";

					this.BorderMiddleLeft.Style.height = (value) + "px";
					this.BorderBottomLeft.Style.top = (value + (53 * 1)) + "px";
					this.BorderBottomRight.Style.top = (value + (53 * 1)) + "px";
					this.BorderMiddleRight.Style.height = (value) + "px";
					this.BorderBottomSpan.Style.top = (value + (53 * 1)) + "px";

				}
			}

			public BackgroundContainerType()
			{
				this.Container = new IHTMLElement
				{
					Style = new IStyle
					{
						position = "absolute"
					}
				};

				this.BackgroundContainer = new IHTMLElement
				{
					Style = new IStyle
					{
						position = "relative"
					}
				};


				this.BorderTopLeft = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_top_left.png",
					Style = new IStyle
					{
						top = "0px",
						left = "0px",
						position = "absolute",
						width = "27px",
						height = "53px"
					}
				};

				this.BorderTopSpan = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_top_span.png",
					Style = new IStyle
					{
						top = "0px",
						left = "27px",
						position = "absolute",
						height = "53px"
					}
				};

				this.BorderTopRight = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_top_right.png",
					Style = new IStyle
					{
						position = "absolute",
						width = "27px",
						height = "53px"
					}
				};

				this.BorderMiddleLeft = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_middle_left.png",
					Style = new IStyle
					{
						position = "absolute",
						top = "53px",
						left = "0px",
						width = "27px",
					}
				};

				this.BorderMiddleRight = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_middle_right.png",
					Style = new IStyle
					{
						position = "absolute",
						top = "53px",
						width = "27px",
					}
				};

				this.BorderBottomLeft = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_bottom_left.png",
					Style = new IStyle
					{
						left = "0px",
						position = "absolute",
						width = "27px",
						height = "53px"
					}
				};

				this.BorderBottomRight = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_bottom_right.png",
					Style = new IStyle
					{
						position = "absolute",
						width = "27px",
						height = "53px"
					}
				};

				this.BorderBottomSpan = new IHTMLImage
				{
					Source = "assets/AvalonUgh.Comparision/layout_bottom_span.png",
					Style = new IStyle
					{
						left = "27px",
						position = "absolute",
						height = "53px"
					}
				};

				this.Fill = new IHTMLElement
				{
					Style = new IStyle
					{
						backgroundColor = "white",
						position = "absolute",
						left = "27px",
						top = "53px",
					}
				};

				this.Container.GetContent =
					delegate
					{
						var BackgroundContainerContent = "";

						BackgroundContainerContent += this.Fill.ToString();
						BackgroundContainerContent += this.BorderTopLeft.ToString();
						BackgroundContainerContent += this.BorderTopSpan.ToString();
						BackgroundContainerContent += this.BorderTopRight.ToString();
						BackgroundContainerContent += this.BorderMiddleLeft.ToString();
						BackgroundContainerContent += this.BorderMiddleRight.ToString();
						BackgroundContainerContent += this.BorderBottomLeft.ToString();
						BackgroundContainerContent += this.BorderBottomRight.ToString();
						BackgroundContainerContent += this.BorderBottomSpan.ToString();

						this.BackgroundContainer.Content = BackgroundContainerContent;

						var Content = "";

						Content += this.BackgroundContainer.ToString();

						return Content;
					};

				this.ContentWidth = 840;
				this.ContentHeight = 400;
			}

		}

	}
}
