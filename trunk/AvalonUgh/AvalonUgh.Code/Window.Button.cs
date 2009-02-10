using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Controls;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Input;

namespace AvalonUgh.Code
{
	partial class Window
	{
		[Script]
		public class Button : Window
		{
			public readonly Image ButtonImage;
			public readonly TextBox ButtonText;

			public string Text
			{
				get
				{
					return this.ButtonText.Text;
				}
				set
				{
					this.ButtonText.Text = value;
				}
			}

			public Button()
				: this(null)
			{
			}

			public event Action Click;

			public Button(Image ButtonImage)
				: this(ButtonImage, 92, 16)
			{
			}

			public Button(Image ButtonImage, int ImageWidth, int ImageSize)
			{
				this.ButtonImage = ButtonImage;

				this.Padding = 1;
				this.ClientWidth = ImageWidth + 8;
				this.ClientHeight = ImageSize + 8; 



				if (ButtonImage != null)
					ButtonImage.MoveTo(4, 4).AttachTo(this.ContentContainer);

				this.ButtonText = new TextBox
				{
					BorderThickness = new Thickness(0),
					Background = Brushes.Transparent,
					Width = (this.ClientWidth - this.ClientHeight - 1).Max(0),
					Height = this.ClientHeight - 4,
					Text = "Button",
					IsReadOnly = true,
				};

				this.AfterUpdate +=
					delegate
					{
						this.ButtonText.Width = (this.ClientWidth - this.ClientHeight - 1).Max(0);
						this.ButtonText.Height = this.ClientHeight - 4;
					};

				this.ButtonText.MoveTo(ImageSize + 4, 2).AttachTo(this.ContentContainer);


				this.ColorOverlay.Element.Show();
				this.ColorOverlay.Element.Cursor = Cursors.Hand;

				this.ColorOverlay.Element.MouseLeave +=
					delegate
					{
						this.ButtonText.MoveTo(ImageSize + 4, 2);

						if (this.ButtonImage != null)
							this.ButtonImage.MoveTo(4, 4);

						this.ThreeD_Left.Fill = Brushes.LightGreen;
						this.ThreeD_Top.Fill = Brushes.LightGreen;

						this.ButtonText.Foreground = Brushes.Black;
					};

				this.ColorOverlay.Element.MouseEnter +=
					delegate
					{

						this.ButtonText.Foreground = Brushes.Yellow;
					};

				this.ColorOverlay.Element.MouseLeftButtonDown +=
					delegate
					{
						this.ButtonText.MoveTo(ImageSize + 4 + 1, 2 + 1);

						if (this.ButtonImage != null)
							this.ButtonImage.MoveTo(5, 5);

						this.ThreeD_Left.Fill = Brushes.DarkGreen;
						this.ThreeD_Top.Fill = Brushes.DarkGreen;
					};

				this.ColorOverlay.Element.MouseLeftButtonUp +=
					delegate
					{
						this.ButtonText.MoveTo(ImageSize + 4, 2);

						if (this.ButtonImage != null)
							this.ButtonImage.MoveTo(4, 4);

						this.ThreeD_Left.Fill = Brushes.LightGreen;
						this.ThreeD_Top.Fill = Brushes.LightGreen;

						if (this.Click != null)
							this.Click();
					};
			}
		}
	}
}
