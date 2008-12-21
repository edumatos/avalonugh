using System;
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

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public partial class Vehicle :
		ISupportsContainer, ISupportsVelocity, ISupportsPhysics,
		ISupportsLocationChanged, IDisposable
	{
		public Vehicle StartPosition;

		public double LastCollisionVelocity { get; set; }


		public Level CurrentLevel { get; set; }

		// a vehicle can carry a rock
		// and throw it at trees and animals
		public Rock CurrentWeapon { get; set; }

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
					this.ColorStripe = Colors.Red;

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
			if (this.CurrentLevel == null)
				return;

			if (this.IsUnmanned)
				return;

			// we have landed!
			// if we are on a platform which has a cave
			// we would need to notify the passengers of our arrivel


			var scan = new PlatformScan(this, this.CurrentLevel);

			if (scan.LandingTiles.Any())
			{
				Console.WriteLine("ready for boarding!");

			}
			else
			{
				Console.WriteLine("try landing on a platform!");

			}
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
			if (this.StartPosition != null)
			{
				this.StartPosition.Dispose();
				this.StartPosition = null;
			}

			if (this.CurrentDriver != null)
			{
				this.CurrentDriver = null;
			}


			IsDisposed = true;

			this.IsAnimated = false;
			this.Container.Orphanize();
		}

		#endregion
	}

}
