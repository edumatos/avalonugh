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

		public readonly Property PropertyText = new Property { KeyText = "Text", ValueText = "Look what I made!" };
		public readonly Property PropertyCode = new Property { KeyText = "Code", ValueText = "" };
		public readonly Property PropertyNextLevelCode = new Property { KeyText = "Next Level Code", ValueText = "" };


		public SaveWindow()
		{
			this.ClientWidth = 300 + Padding;
			this.ClientHeight = 200;

			Items.Add(PropertyText);
			Items.Add(PropertyCode);
			Items.Add(PropertyNextLevelCode);

			Items.ForEachNewOrExistingItem(
				(p, i) =>
				{
					p.Key.Orphanize().AttachTo(this.ContentContainer).MoveTo(0, i * (22 + Padding));
					p.Value.Orphanize().AttachTo(this.OverlayContainer).MoveTo(100 + Padding, i * (22 + Padding));
				}
			);

		

			var XSaveButton = new Window.Button(
				new Image
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
				})
			{
				Text = "Save"
			};

			XSaveButton.AttachContainerTo(this.OverlayContainer);
			XSaveButton.MoveContainerTo(this.ClientWidth - XSaveButton.Width, this.ClientHeight - XSaveButton.Height);


			// text
			// code
			// nextcode 
		}
	}
}
