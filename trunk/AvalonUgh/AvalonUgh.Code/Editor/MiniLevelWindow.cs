using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Code.Editor.Tiles;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Linq;
using AvalonUgh.Assets.Avalon;
using System.Windows.Controls;
using System.Windows;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class MiniLevelWindow : Window
	{
		readonly List<FrameworkElement> Items = new List<FrameworkElement>();

		public MiniLevelWindow()
		{
			this.DraggableArea.BringToFront();

		}

		LevelReference InternalLevelReference;
		public LevelReference LevelReference
		{
			get
			{
				return InternalLevelReference;
			}
			set
			{

				InternalLevelReference = value;
				UpdateContent();
			}
		}

		void UpdateContent()
		{
			Items.ToArray().Orphanize();
			Items.Clear();

			var Size = this.LevelReference.Size;

			this.ClientWidth = Size.Width * 4;
			this.ClientHeight = Size.Height * 3;

			var Map = this.InternalLevelReference.Map;

			this.ContentContainer.Background = Brushes.Black;

			var Selectors = new KnownSelectors();

			var Background = this.InternalLevelReference.Background;

			if (!string.IsNullOrEmpty(Background))
			{
				new Image
				{
					Stretch = Stretch.Fill,
					Width = Size.Width * 4,
					Height = Size.Height * 3,
					Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/" + Background + ".png").ToSource()
				}.AttachTo(this.ContentContainer).AddTo(Items);
			}

			Map.ForEach(
				k =>
				{
					if (string.IsNullOrEmpty(k.Value))
						return;

					var Tile = new ASCIITileSizeInfo(k);
					var TileColor = default(SolidColorBrush);

					var TileSelector = Selectors.TileTypes.FirstOrDefault(i => i.GetIdentifier() == k.Value);


					if (TileSelector != null)
					{
						var i = TileSelector.ToolbarImage.ToImage();

						i.Width = Tile.Width * 4;
						i.Height = Tile.Height * 3;

						i.MoveTo(k.X * 4, k.Y * 3).AttachTo(this.ContentContainer).AddTo(Items);

					}
					else if (TileColor != null)
						new Rectangle
						{
							Fill = TileColor,
							Width = Tile.Width * 4,
							Height = Tile.Height * 3,
						}.MoveTo(k.X * 4, k.Y * 3).AttachTo(this.ContentContainer).AddTo(Items);
				}
			);


		}
	}
}
