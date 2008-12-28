using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.ComponentModel;
using System.Windows.Media;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Shapes;
using System.Windows.Input;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class LoadWindow : Window
	{
		public readonly BindingList<LevelReference> Items = new BindingList<LevelReference>();

		const int ItemsPerRow = 8;

		public event Action<LevelReference> Click;

		public LoadWindow() :this(null)
		{
		}

		public LoadWindow(BindingList<LevelReference> Items)
		{
			if (Items != null)
			{
				this.Items = Items;
			}
			else
			{
				Enumerable.Range(1, 20).ForEach(
					LevelNumber =>
					{
						this.Items.Add(new LevelReference(LevelNumber));
					}
				);
			}


			this.Width = Padding + (48 + Padding) * ItemsPerRow;
			this.Height = Padding + 100 + Padding + (30 + Padding) * 3;

			const string DefaultText = "Select a level below!";

			var Info = new TextBox
			{
				IsReadOnly = true,
				AcceptsReturn = true,

				Width = Width - 160 - Padding * 3,
				Height = 100,
				Background = Brushes.Transparent,
				BorderThickness = new System.Windows.Thickness(0),
				Text = DefaultText
			}.MoveTo(Padding * 2 + 160, Padding).AttachTo(this);

			this.DraggableArea.BringToFront();



			this.Items.ForEachNewOrExistingItem(
				(value, index) =>
				{
					value.Preview.MoveTo(Padding, Padding);

					var x = index % ItemsPerRow * (value.SmallPreview.Width + Padding) + Padding;
					var y = value.Preview.Height + Padding * 2
						+ Convert.ToInt32(index / ItemsPerRow) * (value.SmallPreview.Height + Padding);

					value.SmallPreview.AttachTo(this).MoveTo(
						x, y
					);

					var TouchOverlay = new Rectangle
					{
						Width = value.SmallPreview.Width,
						Height = value.SmallPreview.Height,
						Fill = Brushes.Black,
						Opacity = 0,
						Cursor = Cursors.Hand
					}.AttachTo(this).MoveTo(x, y);

					TouchOverlay.MouseEnter +=
						delegate
						{
							value.Preview.AttachTo(this);
							Info.Text = value.Text;
						};

					TouchOverlay.MouseLeave +=
						delegate
						{
							value.Preview.Orphanize();
							Info.Text = DefaultText;
						};

					TouchOverlay.MouseLeftButtonUp +=
						delegate
						{
							if (Click != null)
								Click(value);
						};
				}
			);

		

			// list

			this.Update();
		}


	}
}
