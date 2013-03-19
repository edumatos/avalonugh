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
using System;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class MiniLevelWindow : Window
	{
		readonly List<FrameworkElement> Items = new List<FrameworkElement>();

		public readonly Canvas ExtendedContentContainer;

		[Script]
		public class ConstructorArgumentsInfo
		{
			public int VisibleTilesX = 20;
			public int VisibleTilesY = 16;

			public int Padding = 4;
			public int Width = 4;
			public int Height = 3;

			public int ClientWidth
			{
				get
				{
					return this.Width * VisibleTilesX;
				}
			}


			public int ClientHeight
			{
				get
				{
					return this.Height * VisibleTilesY;
				}
			}
		}


		public MiniLevelWindow()
			: this(null)
		{

		}

		public readonly ConstructorArgumentsInfo SmallTileInfo;


		public MiniLevelWindow(ConstructorArgumentsInfo args)
		{
			if (args == null)
				args = new ConstructorArgumentsInfo();

			this.ContentContainer.Background = Brushes.Black;

			this.SmallTileInfo = args;
			this.Padding = args.Padding;

			this.ExtendedContentContainer = new Canvas
			{
				Width = ClientWidth,
				Height = ClientHeight
			}.AttachTo(this.ContentContainer);

			this.DraggableArea.BringToFront();

			this.XLevelReference = null;
		}

		LevelReference InternalLevelReference;
		public LevelReference XLevelReference
		{
			get
			{
				return InternalLevelReference;
			}
			set
			{
				if (value != null)
					if (InternalLevelReference == value)
						return;

				ClearContent();

				InternalLevelReference = value;

				if (InternalLevelReference != null)
					InternalLevelReference.DataFuture.Continue(
						delegate
						{
							// javascript takes more time to load
							// we should check if really need to continue

							if (InternalLevelReference == value)
								UpdateContent();
						}
				);

			}
		}

		void ClearContent()
		{
			Items.ToArray().Orphanize();
			Items.Clear();


			this.ClientWidth = this.SmallTileInfo.VisibleTilesX * SmallTileInfo.Width;
			this.ClientHeight = this.SmallTileInfo.VisibleTilesY * SmallTileInfo.Height;
		}

		void UpdateContent()
		{



			if (this.XLevelReference == null)
				return;


			if (this.XLevelReference.Map == null)
				return;

			var Size = this.XLevelReference.Size;

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
				if (Background == "null")
					throw new Exception("Background null trap 1");

				if (Background == null)
					throw new Exception("Background null trap 2");

				new Image
				{
					Stretch = Stretch.Fill,
					Width = this.ClientWidth,
					Height = this.ClientHeight,
					Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/" + Background + ".png").ToSource()
				}.AttachTo(this.ContentContainer).AddTo(Items);
			}

			this.ExtendedContentContainer.BringToFront().MoveTo(
				(SmallTileInfo.Width * (this.SmallTileInfo.VisibleTilesX - Size.Width) / 2),
				(SmallTileInfo.Height * (this.SmallTileInfo.VisibleTilesY - Size.Height) / 2)
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
