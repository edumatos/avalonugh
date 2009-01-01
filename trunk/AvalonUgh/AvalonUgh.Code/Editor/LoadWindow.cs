﻿using System;
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
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class LoadWindow : Window
	{
		public readonly BindingList<LevelReference> Items = new BindingList<LevelReference>();

		const int ItemsPerRow = 8;

		public event Action<LevelReference> Click;

		public LoadWindow()
			: this(null)
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
			this.Height = Padding + 100 + Padding + (30 + Padding) * 5;

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

			var PreviewContainer_Height = (30 + Padding) * 5 - Padding;
			var PreviewContainer = new Canvas
			{
				//Background = Brushes.Blue,
				Width = (48 + Padding) * ItemsPerRow - Padding,
				Height = PreviewContainer_Height,
				ClipToBounds = true,
			}.AttachTo(this).MoveTo(
				Padding,
				Padding * 2 + 100

			);

			var PreviewArea = new Canvas
			{
				//Background = Brushes.Red,
				Width = (48 + Padding) * ItemsPerRow - Padding,
			}.AttachTo(PreviewContainer);

			var PreviewArea_Move = NumericEmitter.OfDouble(
				(x, y) =>
				{
					PreviewArea.MoveTo(x, y);
				}
			);
			PreviewArea_Move(0, 0);

			PreviewContainer.MouseMove +=
				(sender, args) =>
				{
					var p = args.GetPosition(PreviewContainer);
					var y = ((p.Y - 30) / (PreviewContainer_Height - 60)).Max(0).Min(1);

					PreviewArea_Move(0, (PreviewArea.Height - PreviewContainer_Height) * -y);
				};


			this.Items.ForEachNewOrExistingItem(
				(value, index) =>
				{
					value.Preview.MoveTo(Padding, Padding);

					var x = index % ItemsPerRow * (value.SmallPreview.Width + Padding);
					var y = Convert.ToInt32(index / ItemsPerRow) * (value.SmallPreview.Height + Padding);

					value.SmallPreview.AttachTo(PreviewArea).MoveTo(
						x, y
					);

					var TouchOverlay = new Rectangle
					{
						Width = value.SmallPreview.Width,
						Height = value.SmallPreview.Height,
						Fill = Brushes.Black,
						Opacity = 0,
						Cursor = Cursors.Hand
					}.AttachTo(PreviewArea).MoveTo(x, y);

					TouchOverlay.MouseEnter +=
						delegate
						{
							value.Preview.AttachTo(this);

							Info.Clear();
							Info.AppendTextLine("Level " + value.Location.Embedded.AnimationFrame);
							Info.AppendTextLine(value.Text);
							Info.AppendTextLine("Size: " + value.Size.Width + "x" + value.Size.Height);
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

					PreviewArea.Height = (30 + Padding) * (Convert.ToInt32(index / ItemsPerRow) + 1) - Padding;
				}
			);



			// list

			this.Update();
		}


	}
}