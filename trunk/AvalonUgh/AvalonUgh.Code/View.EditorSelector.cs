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
using AvalonUgh.Code.Editor;
using AvalonUgh.Assets.Shared;

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
				//Width = PrimitiveTile.Width * this.Level.Zoom,
				//Height = PrimitiveTile.Heigth * this.Level.Zoom,
				Width = 0,
				Height = 0,
				Fill = Brushes.Yellow,
				Opacity = 0.2,
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
					if (args == null)
						return;

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

					var map_width = PrimitiveTile.Width * Level.Map.Width;
					var map_height = PrimitiveTile.Heigth * Level.Map.Height;

					if (map_width < EditorSelector.Width)
						x = (map_width - EditorSelector.Width) / 2;
					else
						x = x.Max(0).Min(map_width - EditorSelector.Width);

					y = y.Max(0).Min(map_height - EditorSelector.Height);


					handler(Convert.ToInt32(x), Convert.ToInt32(y));
				};

			MouseEventArgs TouchOverlay_MouseMove_args = null;
			Action TouchOverlay_MouseMove =
				delegate
				{
					if (TouchOverlay_MouseMove_args == null)
						return;

					if (EditorSelector == null)
						return;

					if (this.EditorSelectorRectangle.Visibility != System.Windows.Visibility.Visible)
						return;

					this.EditorSelectorRectangle.SizeTo(
						EditorSelector.Width * this.Level.Zoom,
						EditorSelector.Height * this.Level.Zoom
					);

					ToContentPosition(TouchOverlay_MouseMove_args,
						(x, y) =>
							this.EditorSelectorRectangle.MoveTo(x * this.Level.Zoom, y * this.Level.Zoom)
					);
				};

			this.TouchOverlay.MouseMove +=
				(sender, args) =>
				{
					TouchOverlay_MouseMove_args = args;
					TouchOverlay_MouseMove();
				};

			this.TouchOverlay.MouseWheel +=
				(sender, args) =>
				{
					if (args.Delta > 0)
					{
						if (EditorSelectorNextSize != null)
							EditorSelectorNextSize();
					}
					else
					{
						if (EditorSelectorPreviousSize != null)
							EditorSelectorPreviousSize();
					}
				};

		

			this.TouchOverlay.MouseLeftButtonUp +=
				(sender, args) =>
				{
					if (EditorSelector == null)
						return;

					if (this.EditorSelectorRectangle.Visibility != System.Windows.Visibility.Visible)
						return;

					ToContentPosition(args,
						(x, y) =>
						{
							var p = new SelectorPosition
							{
								ContentX = x,
								ContentY = y,


							};


							this.EditorSelector.CreateTo(this.Level, p);

							if (EditorSelectorApplied != null)
								EditorSelectorApplied(this.EditorSelector, p);
						}
					);
				};

			this.EditorSelectorChanged +=
				delegate
				{
					TouchOverlay_MouseMove();
				};

			//this.EditorSelector = new SelectorInfo
			//{
			//    Width = PrimitiveTile.Width,
			//    Height = PrimitiveTile.Heigth,
			//    PercisionX = PrimitiveTile.Width,
			//    PercisionY = PrimitiveTile.Heigth,
			//};
		}

		// to be used for syncing
		public event Action<SelectorInfo, SelectorPosition> EditorSelectorApplied;
		public event Action EditorSelectorNextSize;
		public event Action EditorSelectorPreviousSize;

		[Script]
		public class SelectorInfo
		{
			public int Width { get; set; }
			public int Height { get; set; }

			public bool Equals(ASCIITileSizeInfo e)
			{
				if (e.Width != this.PrimitiveTileCountX)
					return false;

				if (e.Height != this.PrimitiveTileCountY)
					return false;

				return true;
			}

			public int PrimitiveTileCountX
			{
				get
				{
					return Width / PrimitiveTile.Width;
				}
				set
				{
					Width = value * PrimitiveTile.Width;
				}
			}

			public int PrimitiveTileCountY
			{
				get
				{
					return Height / PrimitiveTile.Heigth;
				}
				set
				{
					Height = value * PrimitiveTile.Heigth;
				}
			}


			public int HalfWidth { get { return Width / 2; } }
			public int HalfHeight { get { return Height / 2; } }

			public int PercisionX;
			public int PercisionY;

			//public Action<View, SelectorPosition> Invoke;

			public virtual void CreateTo(Level Level, SelectorPosition Position)
			{

			}

			public SelectorInfo()
			{
				PercisionX = 1;
				PercisionY = 1;
			}
		}

		[Script]
		public class SelectorPosition
		{
			public int ContentX;
			public int ContentY;

			public int TileX { get { return ContentX / PrimitiveTile.Width; } }
			public int TileY { get { return ContentY / PrimitiveTile.Heigth; } }

			public SelectorPosition this[int x, int y]
			{
				get
				{
					return 	
						new View.SelectorPosition
						{
							ContentX = this.ContentX + x * PrimitiveTile.Width,
							ContentY = this.ContentY + y * PrimitiveTile.Heigth
						};
				}
			}
		}

		public Rectangle EditorSelectorRectangle;


		SelectorInfo InternalEditorSelector;
		public SelectorInfo EditorSelector
		{
			get
			{
				return InternalEditorSelector;
			}
			set
			{
				InternalEditorSelector = value;

				if (EditorSelectorChanged != null)
					EditorSelectorChanged();
			}
		}

		public event Action EditorSelectorChanged;
	}
}
