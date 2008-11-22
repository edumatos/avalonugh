using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.Windows;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class Sign : ISupportsContainer, ISupportsMoveTo, ISupportsObstacle, IDisposable
	{
		public View.SelectorInfo Selector { get; set; }

		public Canvas Container { get; set; }

		public readonly int Zoom;

		public double X { get; set; }
		public double Y { get; set; }

		public readonly int Width;
		public readonly int Height;


		public int HalfHeight
		{
			get
			{
				return Height / 2;
			}
		}

		public int HalfWidth
		{
			get
			{
				return Width / 2;
			}
		}


		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);
		}

		public void MoveToTile(double x, double y)
		{
			MoveTo(PrimitiveTile.Width * x * Zoom + HalfWidth, PrimitiveTile.Heigth * y * Zoom + HalfHeight);

		}

		public Sign(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom;
			this.Height = PrimitiveTile.Heigth * Zoom;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			var ShowFrame = Enumerable.Range(0, 6).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/sign" + index + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			).ToShowFrame();


			UpdateFrame = () => ShowFrame(Value);
			UpdateFrame();
		}

		int _Value;
		public int Value { get { return _Value; } set { _Value = value; UpdateFrame(); } }

		readonly Action UpdateFrame;

		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth,
				Top = y - HalfHeight,
				Right = x + HalfWidth,
				Bottom = y + HalfHeight,
				//SupportsVelocity = this
			};
		}

		public void Dispose()
		{
			this.OrphanizeContainer();
		}
	}
}
