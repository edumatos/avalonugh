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
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class Flag : ISupportsContainer, ISupportsMoveTo, ISupportsObstacle, IDisposable
	{
		// originally this was a flag, now its just a bush

		public int UnscaledX
		{
			get
			{
				return Convert.ToInt32(X / Zoom);
			}
		}

		public int BaseY
		{
			get
			{
				return Convert.ToInt32((this.Y + HalfHeight) / (PrimitiveTile.Heigth * Zoom));
			}
		}

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


		public event Action LocationChanged;

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);

			if (LocationChanged != null)
				LocationChanged();
		}


		public void MoveToTile(double x, double y)
		{
			MoveTo(PrimitiveTile.Width * x * Zoom + HalfWidth, PrimitiveTile.Heigth * y * Zoom + HalfHeight);

		}

		public Flag(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom;
			this.Height = PrimitiveTile.Heigth * Zoom;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			
			new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/flag" + 0 + ".png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
			}.AttachTo(this.Container);
		
		}

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
		}
	}
}
