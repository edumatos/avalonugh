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
	public class LoadWindow : CommonFileWindow
	{
		public readonly Tab EmbeddedLevels = "Embedded levels";
		public readonly Tab SavedLevels = "Saved levels";

		public event Action<LevelReference> Click;


		public LoadWindow()
		{
			this.Tabs.AddRange(EmbeddedLevels, SavedLevels);



			var PreviewArguments =
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
					Padding = 2,
					Width = 4,
					Height = 3
				};


			//this.Width = Padding + (PreviewArguments.ClientWidth + Padding) * VisibleColumns;

			const string DefaultText = "Select a level below!";

			var PreviewContainer_Height = (PreviewArguments.ClientHeight + Padding) * VisibleRows;

			var Info = new TextBox
			{
				IsReadOnly = true,
				AcceptsReturn = true,

				FontFamily = new FontFamily("Courier New"),

				Foreground = Brushes.Yellow,

				Width = Width - 180 - Padding * 3,
				Height = 100,
				Background = Brushes.Transparent,
				BorderThickness = new System.Windows.Thickness(0),
				Text = DefaultText
			}.MoveTo(Padding, Padding * 3 + PreviewContainer_Height + EmbeddedLevels.Button.Height).AttachTo(this);




			Action<LevelReference> ShowInfo =
				level =>
				{

					if (level != null)
						if (level.Data != null)
						{
							Info.Clear();
							
							if (level.Location.Embedded != null)
								Info.AppendTextLine("Level " + level.Location.Embedded.AnimationFrame);

							Info.AppendTextLine(level.Text);
							Info.AppendTextLine("Size: " + level.Size.Width + "x" + level.Size.Height);

							return;
						}

					Info.Text = DefaultText;
				};

			this.Tabs.ForEachNewOrExistingItem(
				(value, index) =>
				{
					value.MouseEnter += ShowInfo;

					value.MouseLeave +=
						level =>
						{
							ShowInfo(this.CurrentLevel);
						};


				}
			);


			#region buttons

			var ButtonLoad = new Window.Button(
				new Image
				{
					Width = 16,
					Height = 16,
					Stretch = Stretch.Fill,
					Source = new NameFormat
					{
						Path = Assets.Shared.KnownAssets.Path.Assets,
						Index = -1,
						Name = "btn_load",
						Extension = "png"
					}
				})
			{
				Text = "Load"
			};
			ButtonLoad.AttachContainerTo(this.OverlayContainer);
			ButtonLoad.MoveContainerTo(this.ClientWidth - ButtonLoad.Width, this.ClientHeight - ButtonLoad.Height * 2 - Padding);

			ButtonLoad.Click +=
				delegate
				{
					if (this.Click != null)
						this.Click(this.CurrentLevel);
				};
			#endregion



		}

	}
}
