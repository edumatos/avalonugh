using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public abstract partial class Actor : ISupportsContainer, ISupportsPhysics
	{
		public double Density { get; set; }
		public int Stability { get; set; }

		public Canvas Container { get; set; }

		public readonly int Zoom;

		public readonly int Width;
		public readonly int Height;

		public Image[] IdleFrames;
		public int IdleInterval = 500;

		public Image[] PanicFrames;
		public int PanicInterval = 100;


		public Actor(int Zoom)
		{
			this.Density = 0.3;
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};
		}

		public bool IsIdle = true;
		public bool IsPanicing = false;

		public bool PhysicsDisabled
		{
			get
			{
				return !IsPanicing;
			}
		}

		protected void Initialize()
		{
			this.IdleInterval.AtIntervalWithCounter(
				i =>
				{
					if (!IsIdle)
						return;

					this.IdleFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.IdleFrames.Length)
								k.Show();
							else
								k.Hide();
						}
					);
				}
			);

			this.PanicInterval.AtIntervalWithCounter(
				i =>
				{
					if (!IsPanicing)
						return;

					this.PanicFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.PanicFrames.Length)
								k.Show();
							else
								k.Hide();
						}
					);
				}
			);
		}



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


		public void MoveToTile(double x, double y)
		{
			MoveTo(PrimitiveTile.Width * x * Zoom + HalfWidth, PrimitiveTile.Heigth * y * Zoom + HalfHeight);

		}


		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth / 2,
				Top = y - HalfHeight / 2,
				Right = x + HalfWidth / 2,
				Bottom = y + HalfHeight,
				//SupportsVelocity = this
			};
		}
	}
}
