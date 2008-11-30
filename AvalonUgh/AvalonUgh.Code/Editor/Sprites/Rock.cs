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

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class Rock : ISupportsContainer, ISupportsPhysics , IDisposable
	{
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
			if (this.IsSleeping)
				5000.AtDelay(
					delegate
					{
						this.IsSleeping = false;
						this.UpdateFrame();
					}
				);
		}

		public bool ReadyForPickup
		{
			get
			{
				return Stability > 10;
			}
		}

		public bool PhysicsDisabled { get; set; }

		public Canvas Container { get; set; }

		public readonly int Zoom;

		public double Density { get; set; }

		public double VelocityX { get; set; }
		public double VelocityY { get; set; }

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

		public bool IsSleeping { get; set; }

		public Rock(int Zoom)
		{
			this.Density = 2.3;
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom;
			this.Height = PrimitiveTile.Heigth * Zoom;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			var ShowFrame = Enumerable.Range(0, 3).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/rock" + index + ".png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			).ToShowFrame(
				k =>
					new
					{
						up = k.FixParam(0),
						down = k.FixParam(1),
						sleep = k.FixParam(2),
					}
			);

			UpdateFrame =
				delegate
				{
					if (IsSleeping)
					{
						ShowFrame.sleep();
					}
					else
					{
						ShowFrame.up();

						5000.AtDelay(
							delegate
							{
								if (IsSleeping)
									return;

								ShowFrame.down();
							}
						);
					};
				};

			ShowFrame.up();
			AnimationTimer = 10000.AtInterval(UpdateFrame);
		}

		public readonly Action UpdateFrame;


		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth,
				Top = y - HalfHeight,
				Right = x + HalfWidth,
				Bottom = y + HalfHeight,
				SupportsVelocity = this
			};
		}


		public void GoToSleep()
		{
			if (this.IsSleeping)
				return;

			this.IsSleeping = true;
			this.UpdateFrame();



		}

		DispatcherTimer AnimationTimer;

		public void Dispose()
		{
			this.Container.Orphanize();
			AnimationTimer.Stop();
		}
	}
}
