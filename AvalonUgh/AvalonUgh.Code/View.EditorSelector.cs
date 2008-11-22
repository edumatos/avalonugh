using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Input;

namespace AvalonUgh.Code
{
	partial class View
	{
		public void AttachEditorSelector()
		{
			this.EditorSelectorRectangle = new Rectangle
			{
				//Width = 100,
				//Height = 100,
				Width = PrimitiveTile.Width * this.Level.Zoom,
				Height = PrimitiveTile.Heigth * this.Level.Zoom,
				Fill = Brushes.Yellow,
				Opacity = 0.2
			}.MoveTo(64, 64).AttachTo(this.PlatformsInfoOverlay);


			(1000 / 10).AtIntervalWithCounter(
				c =>
				{
					this.EditorSelectorRectangle.Opacity = (Math.Sin(c * 0.5) + 1.0) / 2.0 * 0.3 + 0.2;
				}
			);

			Action<MouseEventArgs, Action<int, int>> ToContentPosition =
				(args, handler) =>
				{
					var p = args.GetPosition(this.TouchOverlay);

					//TouchTileSelector.MoveTo(p.X, p.Y);

					var x = p.X - MaxShakeSize - (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2;
					var y = p.Y - MaxShakeSize - (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2;

					x /= this.Level.Zoom;
					y /= this.Level.Zoom;

					// lets center the selector
					x -= EditorSelector.Width / 2;
					y -= EditorSelector.Height / 2;



					var PercisionX = EditorSelector.PercisionX;
					var PercisionY = EditorSelector.PercisionY;

					x = Math.Round(x / PercisionX) * PercisionX;
					y = Math.Round(y / PercisionY) * PercisionY;

					handler(Convert.ToInt32(x), Convert.ToInt32(y));
				};

			this.TouchOverlay.MouseMove +=
				(sender, args) =>
				{
					if (this.EditorSelectorRectangle.Visibility != System.Windows.Visibility.Visible)
						return;

					this.EditorSelectorRectangle.SizeTo(
						EditorSelector.Width * this.Level.Zoom,
						EditorSelector.Height * this.Level.Zoom
					);

					ToContentPosition(args,
						(x, y) =>
							this.EditorSelectorRectangle.MoveTo(x * this.Level.Zoom, y * this.Level.Zoom)
					);
				};

			this.TouchOverlay.MouseLeftButtonUp +=
				(sender, args) =>
				{
					if (this.EditorSelectorRectangle.Visibility != System.Windows.Visibility.Visible)
						return;

					ToContentPosition(args,
						(x, y) =>
						{
							if (this.EditorSelector.Invoke != null)
							{
								this.EditorSelector.Invoke(this, this.EditorSelector,
									new SelectorPosition
									{
										ContentX = x,
										ContentY = y,

										TileX = x / PrimitiveTile.Width,
										TileY = y / PrimitiveTile.Heigth,
									}
								);
							}

						}
					);
				};

			this.EditorSelector = new SelectorInfo
			{
				Width = PrimitiveTile.Width * 2,
				Height = PrimitiveTile.Heigth * 3,
				PercisionX = PrimitiveTile.Width,
				PercisionY = PrimitiveTile.Heigth,
			};
		}

		[Script]
		public class SelectorInfo 
		{
			public int Width;
			public int Height;

			public int HalfWidth { get { return Width / 2; } }
			public int HalfHeight { get { return Height / 2; } }

			public int PercisionX;
			public int PercisionY;

			public Action<View, SelectorInfo, SelectorPosition> Invoke;

		}

		[Script]
		public class SelectorPosition
		{
			public int ContentX;
			public int ContentY;

			public int TileX;
			public int TileY;
		}

		public Rectangle EditorSelectorRectangle;

		public SelectorInfo EditorSelector;

	}
}
