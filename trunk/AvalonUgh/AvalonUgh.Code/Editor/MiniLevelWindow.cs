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
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class MiniLevelWindow : Window
	{
		readonly List<FrameworkElement> Items = new List<FrameworkElement>();

		public readonly Canvas ExtendedContentContainer;

		public MiniLevelWindow()
		{
			this.ExtendedContentContainer = new Canvas
			{
				Width = ClientWidth,
				Height = ClientHeight
			}.AttachTo(this.ContentContainer);

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

		[Script]
		public class SmallTileInfo
		{
			public const int Width = 8;
			public const int Height = 6;
		}

		void UpdateContent()
		{
			const int VisibleTilesX = 20;
			const int VisibleTilesY = 16;

			Items.ToArray().Orphanize();
			Items.Clear();

			var Size = this.LevelReference.Size;

			this.ClientWidth = VisibleTilesX * SmallTileInfo.Width;
			this.ClientHeight = VisibleTilesY * SmallTileInfo.Height;

			this.ExtendedContentContainer.SizeTo(
				Size.Width * SmallTileInfo.Width,
				Size.Height * SmallTileInfo.Height
			);

			this.ContentContainer.ClipToBounds = true;

			var Map = this.InternalLevelReference.Map;

			this.ContentContainer.Background = Brushes.Black;

			var Selectors = new KnownSelectors();

			var Background = this.InternalLevelReference.Background;

			if (!string.IsNullOrEmpty(Background))
			{
				new Image
				{
					Stretch = Stretch.Fill,
					Width = this.ClientWidth,
					Height = this.ClientHeight,
					Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/" + Background + ".png").ToSource()
				}.AttachTo(this.ContentContainer).AddTo(Items);
			}

			this.ExtendedContentContainer.BringToFront().MoveTo(
				(SmallTileInfo.Width * (VisibleTilesX - Size.Width) / 2),
				(SmallTileInfo.Height * (VisibleTilesY - Size.Height) / 2)
			);

			Map.ForEach(
				k =>
				{
					if (string.IsNullOrEmpty(k.Value))
						return;

					var TileSelector = Selectors.TileTypes.FirstOrDefault(i => i.GetIdentifier() == k.Value);
					if (TileSelector != null)
					{
						var Tile = new ASCIITileSizeInfo(k);


						var i = TileSelector.ToolbarImage.ToImage();

						i.Width = Tile.Width * SmallTileInfo.Width;
						i.Height = Tile.Height * SmallTileInfo.Height;

						i.MoveTo(
							(k.X) * SmallTileInfo.Width,
							(k.Y) * SmallTileInfo.Height
						).AttachTo(this.ExtendedContentContainer).AddTo(Items);

					}

				}
			);

			var h = this.InternalLevelReference.Water;
			var h2 = h * SmallTileInfo.Height / PrimitiveTile.Heigth;

			new Rectangle
			{
				Fill = Brushes.DarkCyan,
				Opacity = 0.4
			}.SizeTo(
				Size.Width * SmallTileInfo.Width,
				Size.Height * SmallTileInfo.Height
			).MoveTo(0, Size.Height * SmallTileInfo.Height - h2).AttachTo(this.ExtendedContentContainer).AddTo(Items);


		}
	}
}
