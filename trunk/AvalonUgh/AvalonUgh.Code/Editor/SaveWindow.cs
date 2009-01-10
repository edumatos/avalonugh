using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;
using System.Windows.Input;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class SaveWindow : Window
	{
		[Script]
		public class Property
		{
			public readonly TextBox Key;
			public readonly TextBox Value;

			public string KeyText
			{
				get
				{
					return Key.Text;
				}
				set
				{
					Key.Text = value;
				}
			}

			public string ValueText
			{
				get
				{
					return Value.Text;
				}
				set
				{
					Value.Text = value;
				}
			}


			public Property()
			{
				this.Key = new TextBox
				{
					Width = 100,
					Height = 22,
					BorderThickness = new Thickness(0),
					TextAlignment = TextAlignment.Right,
					Background = Brushes.Transparent,
					Text = "Text",
					IsReadOnly = true,
				};

				this.Value = new TextBox
				{
					Width = 200,
					Height = 22,
					BorderThickness = new Thickness(0),
					TextAlignment = TextAlignment.Left,
					Background = Brushes.LightGreen,
					Text = "New Level",
				};
			}
		}

		public readonly BindingList<Property> Items = new BindingList<Property>();

		public SaveWindow()
		{
			this.ClientWidth = 300 + Padding;
			this.ClientHeight = 200;

			Items.Add(new Property { KeyText = "Text", ValueText = "Look what I made!" });
			Items.Add(new Property { KeyText = "Code", ValueText = "" });
			Items.Add(new Property { KeyText = "Next Level Code", ValueText = "" });

			Items.ForEachNewOrExistingItem(
				(p, i) =>
				{
					p.Key.Orphanize().AttachTo(this.ContentContainer).MoveTo(0, i * (22 + Padding));
					p.Value.Orphanize().AttachTo(this.OverlayContainer).MoveTo(100 + Padding, i * (22 + Padding));
				}
			);

			var SaveButton = new Window
			{
				Padding = 1,
				ClientWidth = 100,
				ClientHeight = 24
			};

			var SaveButtonImage = new Image
			{
				Width = 16,
				Height = 16,
				Stretch = Stretch.Fill,
				Source = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Index = -1,
					Name = "btn_save",
					Extension = "png"
				}
			};

			SaveButtonImage.MoveTo(4, 4).AttachTo(SaveButton.ContentContainer);

			var SaveButtonText = new TextBox
			{
				BorderThickness = new Thickness(0),
				Background = Brushes.Transparent,
				Width = SaveButton.ClientWidth - SaveButton.ClientHeight - 1,
				Height = SaveButton.ClientHeight - 4,
				Text = "Save"
			};

			SaveButtonText.MoveTo(24, 2).AttachTo(SaveButton.ContentContainer);

			SaveButton.AttachContainerTo(this.OverlayContainer);
			SaveButton.MoveContainerTo(this.ClientWidth - SaveButton.Width, this.ClientHeight - SaveButton.Height);

			SaveButton.ColorOverlay.Element.Show();
			SaveButton.ColorOverlay.Element.Cursor = Cursors.Hand;

			SaveButton.ColorOverlay.Element.MouseLeave +=
				delegate
				{
					SaveButtonText.MoveTo(24, 2);
					SaveButtonImage.MoveTo(4, 4);

					SaveButton.ThreeD_Left.Fill = Brushes.LightGreen;
					SaveButton.ThreeD_Top.Fill = Brushes.LightGreen;

					SaveButtonText.Foreground = Brushes.Black;
				};

			SaveButton.ColorOverlay.Element.MouseEnter +=
				delegate
				{

					SaveButtonText.Foreground = Brushes.Yellow;
				};

			SaveButton.ColorOverlay.Element.MouseLeftButtonDown +=
				delegate
				{
					SaveButtonText.MoveTo(24 + 1, 2 + 1);
					SaveButtonImage.MoveTo(5, 5);
					SaveButton.ThreeD_Left.Fill = Brushes.DarkGreen;
					SaveButton.ThreeD_Top.Fill = Brushes.DarkGreen;
				};

			SaveButton.ColorOverlay.Element.MouseLeftButtonUp +=
				delegate
				{
					SaveButtonText.MoveTo(24, 2);
					SaveButtonImage.MoveTo(4, 4);

					SaveButton.ThreeD_Left.Fill = Brushes.LightGreen;
					SaveButton.ThreeD_Top.Fill = Brushes.LightGreen;
				};

			// text
			// code
			// nextcode 
		}
	}
}
