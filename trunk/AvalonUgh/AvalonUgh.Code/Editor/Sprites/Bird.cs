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
using AvalonUgh.Code.Editor;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class Bird : ISupportsContainer, ISupportsObstacle, ISupportsPhysics, IDisposable
	{
		// PTERODACTYL
		//-----------
		//HE IS THE MOST DANGEROUS OF ALL OPPONENTS. IF YOU TOUCH HIM YOUR HELICOPTER
		//WILL CRASH AT ONCE. JUST BEFORE PTERODACTYL APPEARS YOU WILL HEAR A CRY
		//AS SOON AS YOU HEAR THIS UNUSUAL NOISE, GET READY TO TAKE EVASIVE ACTION.

		public Bird StartPosition;

		public Canvas Container { get; set; }

		public readonly int Zoom;

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

		public Bird(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 3;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			var frames = Enumerable.Range(0, 13).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/bird0_" + ("" + index).PadLeft(2, '0') + "_2x3.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			);

			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Visibility = Visibility.Visible;

					(1000 / 30).AtDelay(
						delegate
						{
							value.Visibility = Visibility.Hidden;
							SignalNext();
						}
					);
				}
			);
		}

		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth,
				Top = y - HalfHeight / 2,
				Right = x + HalfWidth,
				Bottom = y + HalfHeight / 2,
				//SupportsVelocity = this
			};
		}

		#region ISupportsPhysics Members

		public int Stability { get; set; }


		public void StabilityReached()
		{
		}

		public bool PhysicsDisabled { get; set; }

		public double MassCenterModifier { get; set; }


		public double Density { get; set; }

		public double LastVelocity { get; set; }
		public double LastCollisionVelocity { get; set; }
		public double LastWaterCollisionVelocity { get; set; }

		#endregion

		public readonly RoundedDouble InternalVelocityX = new RoundedDouble();
		public readonly RoundedDouble InternalVelocityY = new RoundedDouble();
		public readonly RoundedDouble InternalX = new RoundedDouble();
		public readonly RoundedDouble InternalY = new RoundedDouble();


		public double VelocityX { get { return InternalVelocityX.Value; } set { InternalVelocityX.Value = value; } }
		public double VelocityY { get { return InternalVelocityY.Value; } set { InternalVelocityY.Value = value; } }

		public double X { get { return InternalX.Value; } set { InternalX.Value = value; } }
		public double Y { get { return InternalY.Value; } set { InternalY.Value = value; } }


		#region ISupportsMoveTo Members


		public event Action LocationChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
