using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class Vehicle : ISupportsContainer, ISupportsVelocity, ISupportsPhysics
	{
		public int Stability { get; set; }

		public bool Hidden { get; set; }

		public double Acceleration = 0.4;
		public double Density { get; set; }

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

		public bool IsAnimated { get; set; }

		public double VelocityX { get; set; }
		public double VelocityY { get; set; }

		public double X { get; set; }
		public double Y { get; set; }

	

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);
		}

		readonly Image ColorStripeRed;
		readonly Image ColorStripeBlue;
		readonly Image ColorStripeYellow;

		public Color ColorStripe
		{
			set
			{
				ColorStripeBlue.Hide();
				ColorStripeRed.Hide();
				ColorStripeYellow.Hide();

				if (value == Colors.Red)
				{
					ColorStripeRed.Show();
					return;
				}

				if (value == Colors.Blue)
				{
					ColorStripeBlue.Show();
					return;
				}

				if (value == Colors.Yellow)
				{
					ColorStripeYellow.Show();
					return;
				}
			}
		}

		public Vehicle(int Zoom)
		{
			this.Density = 0.4;
			this.Zoom = Zoom;

			this.Width = 16 * Zoom * 2;
			this.Height = 12 * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			this.IsAnimated = true;

			var frames = Enumerable.Range(0, 6).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_" + ("" + index).PadLeft(2, '0') + "_2x2.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			);

			this.ColorStripeRed = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_red_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			this.ColorStripeBlue = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_blue_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			this.ColorStripeYellow = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_yellow_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Visibility = Visibility.Visible;

					(1000 / 30).AtIntervalWithTimer(
						t =>
						{
							if (IsAnimated)
							{
								value.Visibility = Visibility.Hidden;
								SignalNext();
								t.Stop();
							}
						}
					);
				}
			);
		}

		
		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - this.HalfWidth + 6 * this.Zoom,
				Right = x + this.HalfWidth - 6 * this.Zoom,
				Top = y - this.HalfHeight + 4 * this.Zoom,
				Bottom = y + this.HalfHeight,

				SupportsVelocity = this
			};
		}
	}

}
