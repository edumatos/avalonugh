using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class CommonFileWindow : Window
	{
		[Script]
		public class Tab
		{
			public readonly BindingList<LevelReference> Items = new BindingList<LevelReference>();

			public readonly Window.Button Button;

			public readonly Canvas PreviewContainer;

			const int Padding = 4;

			public event Action<LevelReference> MouseEnter;
			public event Action<LevelReference> MouseLeave;
			public event Action<LevelReference, MiniLevelWindow> Click;

			public Tab()
			{
				this.Button = new Window.Button
				{
					Text = "Embedded levels",
					ClientWidth = 150,
				};

				var PreviewArguments =
					new MiniLevelWindow.ConstructorArgumentsInfo
					{
						Padding = 2,
						Width = 4,
						Height = 3
					};

				var PreviewContainer_Height = (PreviewArguments.ClientHeight + Padding) * VisibleRows;

				this.PreviewContainer = new Canvas
				{
					//Background = Brushes.Blue,
					Width = (PreviewArguments.ClientWidth + Padding) * VisibleColumns,
					Height = PreviewContainer_Height,
					ClipToBounds = true,
				};

				var PreviewArea = new Canvas
				{
					//Background = Brushes.Red,
					Width = (PreviewArguments.ClientWidth + Padding) * VisibleColumns,
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

						y = Math.Round(y * AllRows) / AllRows;

						PreviewArea_Move(0, (PreviewArea.Height - PreviewContainer_Height) * -y);
					};



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
						//Preview.BackgroundContainer.Hide();



						Preview.AttachContainerTo(PreviewArea).MoveContainerTo(
							Convert.ToInt32(x), Convert.ToInt32(y)
						);

						Preview.DraggableArea.MouseEnter +=
							delegate
							{
								if (MouseEnter != null)
									MouseEnter(value);


							};

						Preview.DraggableArea.MouseLeave +=
							delegate
							{
								if (MouseLeave != null)
									MouseLeave(value);


							};

						Preview.DraggableArea.MouseLeftButtonUp +=
							delegate
							{

								if (Click != null)
									Click(value, Preview);
							};

						AddTask(
							SignalNext =>
							{

								Preview.LevelReference = value;

								var LoadDelay = 300;

								//if (this.Visibility == System.Windows.Visibility.Visible)
								//    LoadDelay = 50;

								LoadDelay.AtDelay(SignalNext);
							}
						);


					}
				);
			}

			public static implicit operator Tab(string Text)
			{
				var t = new Tab();

				t.Button.Text = Text;

				return t;
			}
		}



		public readonly BindingList<Tab> Tabs = new BindingList<Tab>();

		public const int VisibleColumns = 6;
		public const int VisibleRows = 4;


		public Action CurrentLevelChanged;
		public LevelReference CurrentLevel;

		public CommonFileWindow()
		{
			//this.Tabs = new BindingList<Tab> { EmbeddedLevels, SavedLevels };



			var PreviewArguments =
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
					Padding = 2,
					Width = 4,
					Height = 3
				};


			this.Width = Padding + (PreviewArguments.ClientWidth + Padding) * VisibleColumns;


			var PreviewContainer_Height = (PreviewArguments.ClientHeight + Padding) * VisibleRows - Padding;




			this.Tabs.ForEachNewOrExistingItem(
				(value, index) =>
				{
					value.Button.MoveContainerTo((value.Button.Width + Padding) * index, 0);
					value.Button.AttachContainerTo(this.OverlayContainer);

					value.Button.Click +=
						delegate
						{
							this.Tabs.ForEach(k => k.PreviewContainer.Show(k == value));

						};

					value.PreviewContainer.AttachTo(this.OverlayContainer).MoveTo(
						0,
						Padding + value.Button.Height
					);

					value.PreviewContainer.Show(index == 0);


					value.Click +=
						(level, Preview) =>
						{
							Preview.BackgroundColor = Colors.Yellow;

							CurrentLevel = level;

							if (CurrentLevelChanged != null)
								CurrentLevelChanged();

							Action Revert = null;
							Revert =
								delegate
								{
									if (CurrentLevel != level)
										Preview.BackgroundColor = Colors.Green;

									CurrentLevelChanged -= Revert;
								};
							CurrentLevelChanged += Revert;



						};
				}
			);




			this.ClientHeight = 20 + 100 + (PreviewArguments.ClientHeight + Padding) * VisibleRows;

			#region buttons

			var XCancelButton = new Window.Button
			{
				Text = "Close"
			};

			XCancelButton.AttachContainerTo(this.OverlayContainer);
			XCancelButton.MoveContainerTo(this.ClientWidth - XCancelButton.Width, this.ClientHeight - XCancelButton.Height);


			XCancelButton.Click += this.Hide;
			#endregion



		}

	}
}
