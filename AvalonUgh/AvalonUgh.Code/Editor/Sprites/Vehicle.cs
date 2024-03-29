﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;
using System.ComponentModel;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public partial class Vehicle :
		ISupportsContainer, ISupportsVelocity, ISupportsPhysics,
		ISupportsLocationChanged, IDisposable
	{
		public double MassCenterModifier { get; set; }
		public Vehicle StartPosition;

		public double LastVelocity { get; set; }
		public double LastCollisionVelocity { get; set; }
		public double LastWaterCollisionVelocity { get; set; }


		public LevelType CurrentLevel { get; set; }

		// a vehicle can carry a rock
		// and throw it at trees and animals
		public event Action CurrentWeaponChanged;
		Rock InternalWeapon;
		public Rock CurrentWeapon
		{
			get
			{
				return InternalWeapon;
			}
			set
			{
				if (InternalWeapon == value)
					return;

				if (InternalWeapon != null)
				{
					InternalWeapon.MoveTo(this.X, this.Y);
					InternalWeapon.PhysicsDisabled = false;
					InternalWeapon.Stability = 0;
					InternalWeapon.Show();
				}

				InternalWeapon = value;

				if (InternalWeapon != null)
				{
					InternalWeapon.PhysicsDisabled = true;
					InternalWeapon.Hide();
				}

				if (CurrentWeaponChanged != null)
					CurrentWeaponChanged();
			}
		}

		Actor _CurrentDriver;
		public Actor CurrentDriver
		{
			get
			{
				return _CurrentDriver;
			}
			set
			{
				var v = _CurrentDriver;

				_CurrentDriver = value;

				IsUnmanned = value == null;

				if (_CurrentDriver != null)
				{
					this.ExitIsBlocked = true;
                    Console.WriteLine("this.ColorStripe = value.GetColorStripeForVehicle(this);");
                    this.ColorStripe = value.GetColorStripeForVehicle(this);

					if (_CurrentDriver.CurrentVehicle != this)
						_CurrentDriver.CurrentVehicle = this;

					500.AtDelay(
						() => ExitIsBlocked = false
					);


				}
				else
				{
					if (v != null)
					{
						v.CurrentVehicle = null;
					}
				}
			}
		}


		public readonly BindingList<Actor> CurrentPassengers = new BindingList<Actor>();


		public bool ExitIsBlocked;

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

		public int Stability { get; set; }

		public void StabilityReached()
		{
		
		}

		public bool PhysicsDisabled { get; set; }

		public double Acceleration = 0.2;
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

		public readonly RoundedDouble InternalVelocityX = new RoundedDouble();
		public readonly RoundedDouble InternalVelocityY = new RoundedDouble();
		public readonly RoundedDouble InternalX = new RoundedDouble();
		public readonly RoundedDouble InternalY = new RoundedDouble();


		public double VelocityX { get { return InternalVelocityX.Value; } set { InternalVelocityX.Value = value; } }
		public double VelocityY { get { return InternalVelocityY.Value; } set { InternalVelocityY.Value = value; } }

		public double X { get { return InternalX.Value; } set { InternalX.Value = value; } }
		public double Y { get { return InternalY.Value; } set { InternalY.Value = value; } }


		public event Action LocationChanged;


		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);

			if (LocationChanged != null)
				LocationChanged();
		}



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
                    Console.WriteLine("this.ColorStripe = Colors.Transparent;");
					this.ColorStripe = Colors.Transparent;
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


		readonly Image[] frames;
		public Vehicle(int Zoom)
		{
			this.MassCenterModifier = 1.8;
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
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/" +
				new NameFormat { Name = "vehicle", Index = 1, AnimationFrame = 4, Width = 2, Height = 2 }
				 + ".png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			InitializeColorStripe();






			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Show();
					frames_SignalNext = SignalNext;

					(1000 / 30).AtIntervalWithTimer(
						t =>
						{
							if (IsDisposed)
							{
								t.Stop();
								return;
							}

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



		#region IDisposable Members

		bool IsDisposed;

		public void Dispose()
		{
			// finish some unfinished business

			if (this.CurrentWeapon != null)
			{
				this.CurrentWeapon = null;
			}

			if (this.CurrentDriver != null)
			{
				this.CurrentDriver = null;
			}


			IsDisposed = true;

			this.IsAnimated = false;
		}

		#endregion
	}

}
