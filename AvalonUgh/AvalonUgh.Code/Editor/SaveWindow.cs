using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class SaveWindow : CommonFileWindow
	{
		public readonly Tab SavedLevels = "Saved levels";

		public event Action<LevelReference> Click;



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

		public readonly Property PropertyText = new Property { KeyText = "Text", ValueText = "My new level" };
		public readonly Property PropertyCode = new Property { KeyText = "Code", ValueText = "" };
		public readonly Property PropertyNextLevelCode = new Property { KeyText = "Next Level Code", ValueText = "" };

		public readonly MiniLevelWindow Preview = new MiniLevelWindow();

		public SaveWindow()
		{
			this.Tabs.AddRange(SavedLevels);

			var PreviewArguments =
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
					Padding = 0,
					Width = 4,
					Height = 3
				};

			this.Preview = new MiniLevelWindow(PreviewArguments);

			var PreviewContainer_Height = (PreviewArguments.ClientHeight + Padding) * VisibleRows;
		

			Items.Add(PropertyText);
			Items.Add(PropertyCode);
			Items.Add(PropertyNextLevelCode);

			Items.ForEachNewOrExistingItem(
				(p, i) =>
				{
					p.Key.Orphanize().AttachTo(this.ContentContainer).MoveTo(
						Preview.Width,
						40 + PreviewContainer_Height + i * (22 + Padding)
					);
					p.Value.Orphanize().AttachTo(this.OverlayContainer).MoveTo(
						Preview.Width + 100 + Padding,
						40 + PreviewContainer_Height + i * (22 + Padding)
					);
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

			XSaveButton.Click +=
				delegate
				{
					if (Click != null)
						Click(CurrentLevel);
				};

			XSaveButton.AttachContainerTo(this.OverlayContainer);
			XSaveButton.MoveContainerTo(this.ClientWidth - XSaveButton.Width, this.ClientHeight - XSaveButton.Height - XSaveButton.Height - 4);



			

			Preview.ThreeD_Bottom.Fill = Brushes.LightGreen;
			Preview.ThreeD_Right.Fill = Brushes.LightGreen;

			Preview.ThreeD_Top.Fill = Brushes.DarkGreen;
			Preview.ThreeD_Left.Fill = Brushes.DarkGreen;

			Preview.
				AttachContainerTo(this.OverlayContainer).
				MoveContainerTo(0,  this.ClientHeight - Preview.Height);

			// text
			// code
			// nextcode 
		}
	}
}
