using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

namespace AvalonUgh.Code
{
	[Script]
	public class GameSocialLinks : ISupportsContainer, IEnumerable<GameSocialLinks.Button>
	{
		public Canvas Container { get; set; }

		[Script]
		public class Button
		{
			public ImageSource Source;

			public int Width;
			public int Height;

			public Action Click;

			public Uri Hyperlink;

			internal Image Image;
			internal Rectangle Overlay;
		}

		public readonly BindingList<Button> Buttons = new BindingList<Button>();

		public int Margin = 8;

		public GameSocialLinks(Canvas Container)
		{
			this.Container = Container;

			Buttons.ForEachNewItem(
				e =>
				{
					var i = true;
					var x = Buttons.Where(k => i).Aggregate(Margin,
						(s, k) =>
						{
							if (k == e)
							{
								i = false;
								return s;
							}

							return s + k.Width + Margin;
						}
					);

					var y = this.Container.Height - Margin - e.Height;

					e.Image = new Image
					{
						Source = e.Source,
						Width = e.Width,
						Height = e.Height,
						Name = "GameSocialLinks_Button_Image",
						Opacity = 0.6
					}.AttachTo(this).MoveTo(x, y);

					e.Overlay = new Rectangle
					{
						Fill = Brushes.White,
						Width = e.Width,
						Height = e.Height,
						Opacity = 0,
						Cursor = Cursors.Hand,
						Name = "GameSocialLinks_Button_Overlay",
					}.AttachTo(this).MoveTo(x, y);

					var ImageOpacity = e.Image.ToAnimatedOpacity();
					ImageOpacity.Opacity = 0.4;

					e.Overlay.MouseEnter +=
						delegate
						{
							ImageOpacity.Opacity = 1;
						};

					e.Overlay.MouseLeave +=
						delegate
						{
							ImageOpacity.Opacity = 0.4;
						};

					e.Overlay.MouseLeftButtonUp +=
						delegate
						{
							if (e.Click != null)
								e.Click();

							if (e.Hyperlink != null)
								e.Hyperlink.NavigateTo();
						};
				}

			);
		}

		public void Add(Button e)
		{
			this.Buttons.Add(e);
		}

		#region IEnumerable<Button> Members

		public IEnumerator<Button> GetEnumerator()
		{
			return this.Buttons.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Buttons.GetEnumerator();
		}

		#endregion
	}
}
