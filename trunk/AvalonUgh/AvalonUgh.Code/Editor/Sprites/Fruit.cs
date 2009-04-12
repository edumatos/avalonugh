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
using System.Windows.Threading;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class Fruit : ISupportsContainer, ISupportsPhysics, IDisposable
	{
		public double MassCenterModifier { get; set; }

		[Script]
		public class SpecificNameFormat : NameFormat
		{
			// this will be used to find the embedded resource files
			// and within the map loader
			public const string Alias = "fruit";

			public SpecificNameFormat()
			{
				Path = Assets.Shared.KnownAssets.Path.Sprites;
				Name = Alias;
				Index = 0;
				Extension = "png";
				Width = 2;
				Height = 2;
			}
		}

		public Rock StartPosition;

		public double LastVelocity { get; set; }
		public double LastCollisionVelocity { get; set; }
		public double LastWaterCollisionVelocity { get; set; }

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

		public int Stability { get; set; }

		public void StabilityReached()
		{

		}


		public bool PhysicsDisabled { get; set; }

		public Canvas Container { get; set; }

		public readonly int Zoom;

		public double Density { get; set; }

		public readonly RoundedDouble InternalVelocityX = new RoundedDouble();
		public readonly RoundedDouble InternalVelocityY = new RoundedDouble();
		public readonly RoundedDouble InternalX = new RoundedDouble();
		public readonly RoundedDouble InternalY = new RoundedDouble();


		public double VelocityX { get { return InternalVelocityX.Value; } set { InternalVelocityX.Value = value; } }
		public double VelocityY { get { return InternalVelocityY.Value; } set { InternalVelocityY.Value = value; } }

		public double X { get { return InternalX.Value; } set { InternalX.Value = value; } }
		public double Y { get { return InternalY.Value; } set { InternalY.Value = value; } }



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

		public bool IsSleeping { get; set; }

		public Fruit(int Zoom)
		{
			this.Density = 2.3;
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};



			new Image
			{
				Source = new SpecificNameFormat().ToRandomIndex(8).ToString().ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
			}.AttachTo(this.Container);
			
		}

		public readonly Action UpdateFrame;


		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth / 3,
				Top = y,
				Right = x + HalfWidth / 3,
				Bottom = y + HalfHeight,
				SupportsVelocity = this
			};
		}


	


		public void Dispose()
		{
		}
	}
}
