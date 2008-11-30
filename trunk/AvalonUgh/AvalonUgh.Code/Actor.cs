using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code
{
	[Script]
	public abstract partial class Actor :
		ISupportsContainer, ISupportsPhysics, ISupportsLocationChanged, ISupportsPlayerInput, IDisposable
	{
		// the woman does not have talk animation
		// the default waiting position is between the outer edges between
		// the sign and the cave on the same platform

		public bool CanBeHitByVehicle ;

		public Level Level { get; set; }

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

		public Image[] TalkFrames = new Image[0];
		public int TalkInterval = 100;

		public Image[] WalkLeftFrames = new Image[0];
		public int WalkLeftInterval = 100;

		public Image[] WalkRightFrames = new Image[0];
		public int WalkRightInterval = 100;

		public void StabilityReached()
		{
			if (Level == null)
				return;

			if (this.ToObstacle().Bottom < this.Level.WaterTop)
			{
				if (this.Animation == AnimationEnum.Panic)
					this.Animation = AnimationEnum.Idle;
			}
		}

		public Actor(int Zoom)
		{
			this.CanBeHitByVehicle = true;
			this.RespectPlatforms = true;

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

		public enum AnimationEnum
		{
			Hidden,
			Idle,
			Talk,
			Panic,
			WalkLeft,
			WalkRight,
			CaveEnter,
			CaveExit
		}


		AnimationEnum _Animation = AnimationEnum.Idle;
		public AnimationEnum Animation
		{
			get
			{
				return _Animation;
			}
			set
			{
				_Animation = value;

				Console.WriteLine(new { ActorAnimation = value }.ToString());

				if (value != AnimationEnum.Idle)
					this.IdleFrames.ForEach(k => k.Hide());
				if (value != AnimationEnum.Panic)
					this.PanicFrames.ForEach(k => k.Hide());
				if (value != AnimationEnum.Talk)
					this.TalkFrames.ForEach(k => k.Hide());
				if (value != AnimationEnum.WalkLeft)
					this.WalkLeftFrames.ForEach(k => k.Hide());
				if (value != AnimationEnum.WalkRight)
					this.WalkRightFrames.ForEach(k => k.Hide());
				if (value == AnimationEnum.Idle)
					this.IdleFrames.First().Show();

				if (value == AnimationEnum.Panic)
					this.PanicFrames.First().Show();

				if (value == AnimationEnum.Talk)
					this.TalkFrames.First().Show();

				if (value == AnimationEnum.WalkLeft)
					this.WalkLeftFrames.First().Show();
				if (value == AnimationEnum.WalkRight)
					this.WalkRightFrames.First().Show();

			}
		}


		public bool PhysicsDisabled
		{
			get
			{
				return false;

				//return Animation != AnimationEnum.Panic;
			}
		}

		protected void Initialize()
		{
			this.IdleInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.Idle)
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

			this.TalkInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.Talk)
						return;

					this.TalkFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.TalkFrames.Length)
								k.Show();
							else
								k.Hide();
						}
					);
				}
			);

			this.WalkLeftInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.WalkLeft)
						return;

					this.WalkLeftFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.WalkLeftFrames.Length)
								k.Show();
							else
								k.Hide();
						}
					);
				}
			);

			this.WalkRightInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.WalkRight)
						return;

					this.WalkRightFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.WalkRightFrames.Length)
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
					if (Animation != AnimationEnum.Panic)
					{
						if (Animation == AnimationEnum.WalkLeft)
							return;

						if (Animation == AnimationEnum.WalkRight)
							return;

						// yet if we are falling?
						if (VelocityY > 0.2)
							this.Animation = AnimationEnum.Panic;

						return;
					}

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


		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth / 3,
				Top = y,
				Right = x + HalfWidth / 3,
				Bottom = y + HalfHeight,
				//SupportsVelocity = this
			};
		}

		public bool RespectPlatforms { get; set; }

		#region ISupportsPlayerInput Members

		public void AddAcceleration(PlayerInput e)
		{
			if (e.Keyboard.IsPressedLeft)
			{
				if (this.Animation != AnimationEnum.WalkLeft)
					this.Animation = AnimationEnum.WalkLeft;

				// at some point we should not be able to accelerate
				this.VelocityX -= this.Zoom * 0.06;
			}
			else if (e.Keyboard.IsPressedRight)
			{
				if (this.Animation != AnimationEnum.WalkRight)
					this.Animation = AnimationEnum.WalkRight;


				this.VelocityX += this.Zoom * 0.06;
			}
			else
			{
				if (this.VelocityY == 0)
				{

					this.VelocityX *= 0.7;
				}
			}

			if (Math.Abs(this.VelocityX) < Zoom * 0.1)
			{
				if (this.Animation == AnimationEnum.WalkLeft)
					this.Animation = AnimationEnum.Idle;

				if (this.Animation == AnimationEnum.WalkRight)
					this.Animation = AnimationEnum.Idle;
			}

			if (e.Keyboard.IsPressedUp)
			{
				if (this.VelocityY == 0)
				{
					this.VelocityY -= this.Zoom * 2.5;

					this.VelocityX *= 1.2;
				}
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Animation = AnimationEnum.Hidden;
			this.Container.Orphanize();

		}

		#endregion
	}
}
