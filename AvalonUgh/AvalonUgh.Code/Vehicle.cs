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
	public class Vehicle : ISupportsContainer
	{
		// mass = density / volume
		// length * width * height = volume
		// http://ca.youtube.com/watch?v=VDSYXmvjg6M
		// concrete density = 2.3g / cm^3
		// wood density = 0.4g / cm^3
		// water density = 1g / cm^3
		// air density = 0.001g / cm^3

		// http://www.engineeringtoolbox.com/density-specific-weight-gravity-d_290.html

		// wood density
		// http://www.engineeringtoolbox.com/wood-density-d_40.html
		// http://www.engineeringtoolbox.com/accelaration-gravity-d_340.html
		// http://www.phynet.de/mechanik/dynamik/hydrodynamik/die-auftriebskraft-in-flussigkeiten


		public double Density = 0.4;

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

		public bool IsAnimated { get; set; }

		public double VelocityX { get; set; }
		public double VelocityY { get; set; }

		public double X { get; set; }
		public double Y { get; set; }

		public void MoveTo()
		{
			MoveTo(X + VelocityX, Y + VelocityY);
		}

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - Width / 2, y - Height / 2);
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
	}

}
