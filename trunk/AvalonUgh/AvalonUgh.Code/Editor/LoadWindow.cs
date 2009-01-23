using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class LoadWindow : Window
	{
		public readonly BindingList<LevelReference> Items = new BindingList<LevelReference>();

		const int VisibleColumns = 7;
		const int VisibleRows = 4;

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

			var PreviewArguments =
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
					Padding = 0,
					Width = 4,
					Height = 3
				};


			this.Width = Padding + (PreviewArguments.ClientWidth + Padding) * VisibleColumns;
			this.Height = Padding + 100 + Padding + (PreviewArguments.ClientHeight + Padding) * VisibleRows;

			const string DefaultText = "Select a level below!";

			var Info = new TextBox
			{
				IsReadOnly = true,
				AcceptsReturn = true,

				FontFamily = new FontFamily("Courier"),
				
				Foreground = Brushes.Yellow,

				Width = Width - 180 - Padding * 3,
				Height = 100,
				Background = Brushes.Transparent,
				BorderThickness = new System.Windows.Thickness(0),
				Text = DefaultText
			}.MoveTo(Padding * 2 + 180, Padding).AttachTo(this);

			this.DraggableArea.BringToFront();

			var PreviewContainer_Height = (PreviewArguments.ClientHeight + Padding) * VisibleRows - Padding;
			var PreviewContainer = new Canvas
			{
				//Background = Brushes.Blue,
				Width = (PreviewArguments.ClientWidth + Padding) * VisibleColumns - Padding,
				Height = PreviewContainer_Height,
				ClipToBounds = true,
			}.AttachTo(this).MoveTo(
				Padding,
				Padding * 2 + 100

			);

			var PreviewArea = new Canvas
			{
				//Background = Brushes.Red,
				Width = (PreviewArguments.ClientWidth + Padding) * VisibleColumns - Padding,
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

					var AllRows = Convert.ToInt32(this.Items.Count / VisibleColumns) + 1;

					y = Math.Round( y * AllRows) / AllRows;

					PreviewArea_Move(0, (PreviewArea.Height - PreviewContainer_Height) * -y);
				};

			var PreviewLarge = new MiniLevelWindow(
					new MiniLevelWindow.ConstructorArgumentsInfo
					{
						Width = 8,
						Height = 6
					}
				)
			{

			}.MoveContainerTo(Padding, Padding).AttachContainerTo(this);

			this.Items.ForEachNewOrExistingItem(
				(value, index, AddTask) =>
				{
					//value.Preview.MoveTo(Padding, Padding);

					var x = index % VisibleColumns * (PreviewArguments.ClientWidth + Padding);
					var y = Convert.ToInt32(index / VisibleColumns) * (PreviewArguments.ClientHeight + Padding);


					PreviewArea.Height = (PreviewArguments.ClientHeight + Padding) * (Convert.ToInt32(index / VisibleColumns) + 1) - Padding;

					var Preview = new MiniLevelWindow(PreviewArguments)
					{
					};



					Preview.DraggableArea.Cursor = Cursors.Hand;
					Preview.BackgroundContainer.Hide();



					Preview.AttachContainerTo(PreviewArea).MoveContainerTo(
						Convert.ToInt32(x), Convert.ToInt32(y)
					);

					Preview.DraggableArea.MouseEnter +=
						delegate
						{
							PreviewLarge.LevelReference = value;


							Info.Clear();
							Info.AppendTextLine("Level " + value.Location.Embedded.AnimationFrame);
							Info.AppendTextLine(value.Text);
							Info.AppendTextLine("Size: " + value.Size.Width + "x" + value.Size.Height);
						};

					Preview.DraggableArea.MouseLeave +=
						delegate
						{
							PreviewLarge.LevelReference = null;
							Info.Text = DefaultText;
						};

					Preview.DraggableArea.MouseLeftButtonUp +=
						delegate
						{
							if (Click != null)
								Click(value);
						};

					AddTask(
						SignalNext =>
						{

							Preview.LevelReference = value;

							var LoadDelay = 300;

							if (this.Visibility == System.Windows.Visibility.Visible)
								LoadDelay = 50;

							LoadDelay.AtDelay(SignalNext);
					    }
					);


				}
			);



			// list

			this.Update();
		}

	}
}
