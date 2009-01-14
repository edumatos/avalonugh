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

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class MiniLevelWindow : Window
	{
		readonly List<Rectangle> Items = new List<Rectangle>();

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
				}.AttachTo(this.ContentContainer);
			}

			Map.ForEach(
				k =>
				{
					var Tile = new ASCIITileSizeInfo(k);
					var TileColor = default(SolidColorBrush);
					var TileImage = default(NameFormat);

					if (Tile.Value == RidgeSelector.Identifier)
					{
						TileImage = Selectors.Ridge.ToolbarImage;
					}

					if (Tile.Value == StoneSelector.Identifier)
					{
						TileImage = Selectors.Stone.ToolbarImage;
					}

					if (Tile.Value == BridgeSelector.Identifier)
					{
						TileImage = Selectors.Bridge.ToolbarImage;
					}

					if (Tile.Value == PlatformSelector.Identifier)
					{
						TileImage = Selectors.Platform.ToolbarImage;

					}

					if (TileImage != null)
					{
						var i = TileImage.ToImage();

						i.Width = Tile.Width * 4;
						i.Height = Tile.Height * 3;

						i.MoveTo(k.X * 4, k.Y * 3).AttachTo(this.ContentContainer);

					}
					else if (TileColor != null)
						new Rectangle
						{
							Fill = TileColor,
							Width = Tile.Width * 4,
							Height = Tile.Height * 3,
						}.MoveTo(k.X * 4, k.Y * 3).AttachTo(this.ContentContainer);
				}
			);


		}
	}
}
