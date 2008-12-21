﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Editor.Sprites;
using System.ComponentModel;

namespace AvalonUgh.Code
{
	[Script]
	public abstract partial class Actor :
		ISupportsContainer, ISupportsPhysics, ISupportsLocationChanged, ISupportsPlayerInput, IDisposable
	{
		public double LastCollisionVelocity { get; set; }


		public bool ReadyForGoldPickup = true;

		public readonly BindingList<Gold> GoldStash = new BindingList<Gold>();

		// the woman does not have talk animation
		// the default waiting position is between the outer edges between
		// the sign and the cave on the same platform

		public event Action CurrentVehicleChanged;
		Vehicle InternalCurrentVehicle;
		public Vehicle CurrentVehicle
		{
			get
			{
				return InternalCurrentVehicle;
			}
			set
			{
				if (value == InternalCurrentVehicle)
					return;

				EnterVehicleBlocked = true;
				500.AtDelay(() => EnterVehicleBlocked = false);

				var v = InternalCurrentVehicle;

				InternalCurrentVehicle = value;

				if (InternalCurrentVehicle == null)
				{
					this.AIInputEnabled = false;
					this.Animation = Actor.AnimationEnum.Idle;

					this.MoveTo(v.X, v.Y - this.ToObstacle().Height / 2);

					this.BringContainerToFront();

					if (v.CurrentDriver != null)
						v.CurrentDriver = null;
				}
				else
				{

					this.AIInputEnabled = true;
					this.Animation = Actor.AnimationEnum.Hidden;

					if (InternalCurrentVehicle.CurrentDriver != this)
						InternalCurrentVehicle.CurrentDriver = this;
				}

				if (CurrentVehicleChanged != null)
					CurrentVehicleChanged();
			}
		}

		public bool EnterVehicleBlocked;

		public Cave CurrentCave;

		public bool CanBeHitByVehicle;

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

		public Image[] CaveEnterFrames = new Image[0];
		public int CaveEnterInterval = 100;

		public Image[] CaveExitFrames = new Image[0];
		public int CaveExitInterval = 100;

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
			CaveExit,
			// only the woman seems to be able to swim!
			SwimLeft,
			SwimRight
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

		public void PlayAnimation(AnimationEnum value, Action done)
		{
			this.Animation = value;

			if (value == AnimationEnum.CaveEnter)
			{
				this.CaveEnterFrames.ForEach(
					(Frame, SignalNext) =>
					{
						Frame.Show();

						this.CaveEnterInterval.AtDelay(
							delegate
							{
								Frame.Hide();
								SignalNext();
							}
						);
					}
				)(done);
			}

			if (value == AnimationEnum.CaveExit)
			{
				this.CaveExitFrames.ForEach(
					(Frame, SignalNext) =>
					{
						Frame.Show();

						this.CaveEnterInterval.AtDelay(
							delegate
							{
								Frame.Hide();
								SignalNext();
							}
						);
					}
				)(done);
			}
		}

		public bool PhysicsDisabled
		{
			get
			{
				if (CurrentVehicle != null)
					return true;

				if (CurrentCave != null)
					return true;

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

		public Point Location
		{
			get
			{
				return new Point { X = this.X, Y = this.Y };
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

		public const double DefaultAcceleraton = 0.06;

		public bool AIInputEnabled;

		public void AddAcceleration(PlayerInput e)
		{
			// if the AI is controlling this actor
			// we cannot slow it down
			// just looking at the keyboard keys anymore


			if (AIInputEnabled)
				return;

			// we are hidden inside a cave
			if (this.CurrentCave != null)
				return;

			if (e.Keyboard.IsPressedLeft)
			{
				if (this.Animation != AnimationEnum.WalkLeft)
					this.Animation = AnimationEnum.WalkLeft;

				// at some point we should not be able to accelerate
				this.VelocityX -= this.Zoom * DefaultAcceleraton;
			}
			else if (e.Keyboard.IsPressedRight)
			{
				if (this.Animation != AnimationEnum.WalkRight)
					this.Animation = AnimationEnum.WalkRight;


				this.VelocityX += this.Zoom * DefaultAcceleraton;
			}
			else
			{

				if (this.VelocityY == 0)
				{

					this.VelocityX *= 0.85;
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

					if (Jumping != null)
						Jumping();
				}
			}

			if (e.Keyboard.IsPressedDown)
			{
				if (this.VelocityY == 0)
				{
					if (EnterCave != null)
						EnterCave();
				}
			}

			if (!EnterVehicleBlocked)
				if (e.Keyboard.IsPressedEnter)
				{
					if (this.VelocityY == 0)
					{
						if (EnterVehicle != null)
							EnterVehicle();
					}
				}
		}


		public event Action Jumping;
		public event Action EnterCave;
		public event Action EnterVehicle;

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
