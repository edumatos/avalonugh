﻿using System;
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
	public class Vehicle : ISupportsContainer, ISupportsVelocity, ISupportsPhysics, ISupportsLocationChanged
	{
		// a vehicle can carry a rock
		// and throw it at trees and animals
		public Rock CurrentWeapon { get; set; }


		public int Stability { get; set; }

		public void StabilityReached()
		{
			// we have landed!
			// if we are on a platform which has a cave
			// we would need to notify the passengers of our arrivel

			Console.WriteLine("ready for boarding!");
		}

		public bool PhysicsDisabled { get; set; }

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

		public event Action LocationChanged;


		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);

			if (LocationChanged != null)
				LocationChanged();
		}

		readonly Image ColorStripeRed;
		readonly Image ColorStripeBlue;
		readonly Image ColorStripeYellow;
		readonly Image ColorStripeGray;

		readonly Image UnmannedImage;

		bool _Unmanned;
		public bool IsUnmanned
		{

			get
			{
				return _Unmanned;
			}
			set
			{

				_Unmanned = value;

				if (value)
				{
					this.ColorStripe = Colors.Gray;
					IsAnimated = false;
					UnmannedImage.Show();
					frames.ForEach(k => k.Hide());
				}
				else
				{
					UnmannedImage.Hide();
					if (frames_SignalNext != null)
						frames_SignalNext();
				}
			}
		}

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

				if (value == Colors.Gray)
				{
					ColorStripeGray.Show();
					return;
				}
			}
		}

		readonly Image[] frames;
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

			this.frames = Enumerable.Range(0, 6).ToArray(
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

			this.UnmannedImage = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle1_04_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			Func<string, Image> CreateStripe =
				color =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_" + color + "_2x2.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container);

			this.ColorStripeRed = CreateStripe("red");
			this.ColorStripeBlue  = CreateStripe("blue");
			this.ColorStripeYellow = CreateStripe("yellow");
			this.ColorStripeGray = CreateStripe("gray");
			

		



			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Show();
					frames_SignalNext = SignalNext;

					(1000 / 30).AtIntervalWithTimer(
						t =>
						{
							if (IsUnmanned)
								return;

							if (IsAnimated)
							{
								value.Hide();
								frames_SignalNext = null;
								SignalNext();
								t.Stop();
							}
						}
					);
				}
			);
		}

		Action frames_SignalNext;

		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - this.HalfWidth + 6 * this.Zoom,
				Right = x + this.HalfWidth - 6 * this.Zoom,
				Top = y - this.HalfHeight + 4 * this.Zoom,
				Bottom = y + this.HalfHeight - this.Zoom,

				SupportsVelocity = this
			};
		}
	}

}