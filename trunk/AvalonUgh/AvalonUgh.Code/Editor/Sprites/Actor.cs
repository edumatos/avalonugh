using System;
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
using System.Windows.Media;
using System.Windows.Threading;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code
{
	[Script]
	public abstract partial class Actor :
		ISupportsContainer, ISupportsPhysics, ISupportsLocationChanged, ISupportsPlayerInput, IDisposable
	{
		public LevelType LevelViaKnownActors;

		public int AvailableFare;

		public virtual int GetActorType()
		{
			return -1;
		}

		public static Actor CreateFromType(int Type, int Zoom)
		{
			if (Type == 0)
				return new Actor.man0(Zoom);

			if (Type == 1)
				return new Actor.woman0(Zoom);

			if (Type == 2)
				return new Actor.man1(Zoom);

			throw new NotSupportedException();
		}

		public PlayerInput DefaultPlayerInput;

		public Actor StartPosition;

		public PlayerInfo PlayerInfo;

		public Color GetColorStripeForVehicle(Vehicle v)
		{
            if (this.PlayerInfo == null)
            {
                var value = Colors.Transparent;
                return value;
            }

			return v.SupportedColorStripes.Keys.AtModulus(this.PlayerInfo.Identity.NetworkNumber);
		}

		public double MassCenterModifier { get; set; }

		public double LastVelocity { get; set; }
		public double LastCollisionVelocity { get; set; }
		public double LastWaterCollisionVelocity { get; set; }


		public bool ReadyForGoldPickup = true;

		public readonly BindingList<Gold> GoldStash = new BindingList<Gold>();
		public readonly BindingList<Fruit> FruitStash = new BindingList<Fruit>();

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
					this.Memory_CaveAction = false;

					this.MoveTo(v.X, v.Y - this.ToObstacle().Height / 2);

					if (this.CurrentLevel != null)
						this.BringContainerToFront();

					this.Show();

					if (v.CurrentDriver != null)
						v.CurrentDriver = null;
				}
				else
				{

					this.Memory_CaveAction = true;
					this.Hide();

					if (InternalCurrentVehicle.CurrentDriver != this)
						InternalCurrentVehicle.CurrentDriver = this;
				}

				if (CurrentVehicleChanged != null)
					CurrentVehicleChanged();
			}
		}

		public bool EnterVehicleBlocked;


		public Cave CurrentCave;



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

		public Image[] SwimLeftFrames = new Image[0];
		public int SwimLeftInterval = 100;

		public Image[] SwimRightFrames = new Image[0];
		public int SwimRightInterval = 100;

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

		}

		public Actor(int Zoom)
		{
			this.Memory_CanBeHitByVehicle = true;
			this.RespectPlatforms = true;

			this.MassCenterModifier = 1.4;
			this.Density = 0.4;
			//this.Density = 1;
			//this.Density = 0.3;
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			this.KnownBubbles.ForEachNewOrExistingItem(
				k => k.MoveContainerTo(this.X + k.OffsetX, this.Y + k.OffsetY)
			);

			this.LocationChanged +=
				delegate
				{
					this.KnownBubbles.ForEachNewOrExistingItem(
						k => k.MoveContainerTo(this.X + k.OffsetX, this.Y + k.OffsetY)
					);
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
			// the old man cannot swim!
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
				if (value != AnimationEnum.SwimLeft)
					this.SwimLeftFrames.ForEach(k => k.Hide());
				if (value != AnimationEnum.SwimRight)
					this.SwimRightFrames.ForEach(k => k.Hide());
				if (value == AnimationEnum.Idle)
					this.IdleFrames.First().Show();

				if (value == AnimationEnum.Panic)
					this.PanicFrames.First().Show();

				if (value == AnimationEnum.Talk)
					this.TalkFrames.FirstOrDefault().Apply(k => k.Show());

				if (value == AnimationEnum.WalkLeft)
					this.WalkLeftFrames.FirstOrDefault().Apply(k => k.Show());
				if (value == AnimationEnum.WalkRight)
					this.WalkRightFrames.FirstOrDefault().Apply(k => k.Show());

				if (value == AnimationEnum.SwimLeft)
					this.SwimLeftFrames.FirstOrDefault().Apply(k => k.Show());
				if (value == AnimationEnum.SwimRight)
					this.SwimRightFrames.FirstOrDefault().Apply(k => k.Show());

			}
		}

		public Action NextAnimationFrame;

		public void PlayAnimation(AnimationEnum value, Action done)
		{
			this.Animation = value;

			if (value == AnimationEnum.CaveEnter)
			{
				this.CaveEnterFrames.ForEach(
					(Frame, SignalNext) =>
					{
						Frame.Show();


						this.NextAnimationFrame =
							LambdaExtensions.WhereCounter(
								delegate
								{
									NextAnimationFrame = null;
									Frame.Hide();
									SignalNext();
								},
							3);
					}
				)(done);
			}

			if (value == AnimationEnum.CaveExit)
			{
				this.CaveExitFrames.ForEach(
					(Frame, SignalNext) =>
					{
						Frame.Show();

						this.NextAnimationFrame =
							LambdaExtensions.WhereCounter(
								delegate
								{
									NextAnimationFrame = null;
									Frame.Hide();
									SignalNext();
								},
							3);
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

				if (CurrentPassengerVehicle != null)
					return true;

				if (CurrentCave != null)
					return true;

				return false;

				//return Animation != AnimationEnum.Panic;
			}
		}

		DispatcherTimer AnimationTimer_Idle;
		DispatcherTimer AnimationTimer_Talk;
		DispatcherTimer AnimationTimer_SwimLeft;
		DispatcherTimer AnimationTimer_SwimRight;
		DispatcherTimer AnimationTimer_WalkLeft;
		DispatcherTimer AnimationTimer_WalkRight;
		DispatcherTimer AnimationTimer_Panic;

		protected void InitializeAnimation()
		{
			AnimationTimer_Idle = this.IdleInterval.AtIntervalWithCounter(
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

			AnimationTimer_Talk = this.TalkInterval.AtIntervalWithCounter(
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


			AnimationTimer_SwimLeft = this.SwimLeftInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.SwimLeft)
						return;

					this.SwimLeftFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.SwimLeftFrames.Length)
								k.Show();
							else
								k.Hide();
						}
					);
				}
			);

			AnimationTimer_SwimRight = this.SwimRightInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.SwimRight)
						return;

					this.SwimRightFrames.ForEach(
						(k, j) =>
						{
							if (j == i % this.SwimRightFrames.Length)
								k.Show();
							else
								k.Hide();
						}
					);
				}
			);

			AnimationTimer_WalkLeft = this.WalkLeftInterval.AtIntervalWithCounter(
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

			AnimationTimer_WalkRight = this.WalkRightInterval.AtIntervalWithCounter(
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

			AnimationTimer_Panic = this.PanicInterval.AtIntervalWithCounter(
				i =>
				{
					if (Animation != AnimationEnum.Panic)
					{
						if (Animation == AnimationEnum.SwimLeft)
							return;

						if (Animation == AnimationEnum.SwimRight)
							return;

						if (Animation == AnimationEnum.WalkLeft)
							return;

						if (Animation == AnimationEnum.WalkRight)
							return;

						// yet if we are falling?
						if (VelocityY > 0.2)
							this.Animation = AnimationEnum.Panic;

						return;
					}
					else
					{
						if (VelocityY == 0)
						{
							if (this.ToObstacle().Bottom > this.LevelViaKnownActors.WaterTop)
								this.Animation = AnimationEnum.Panic;
							else
								this.Animation = AnimationEnum.Idle;
							return;
						}
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

		public readonly RoundedDouble InternalVelocityX = new RoundedDouble();
		public readonly RoundedDouble InternalVelocityY = new RoundedDouble();
		public readonly RoundedDouble InternalX = new RoundedDouble();
		public readonly RoundedDouble InternalY = new RoundedDouble();


		public double VelocityX { get { return InternalVelocityX.Value; } set { InternalVelocityX.Value = value; } }
		public double VelocityY { get { return InternalVelocityY.Value; } set { InternalVelocityY.Value = value; } }

		public double X { get { return InternalX.Value; } set { InternalX.Value = value; } }
		public double Y { get { return InternalY.Value; } set { InternalY.Value = value; } }

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
				Top = y + Zoom,
				Right = x + HalfWidth / 3,
				Bottom = y + HalfHeight,
				//SupportsVelocity = this
			};
		}

		public bool RespectPlatforms { get; set; }

		#region ISupportsPlayerInput Members

		public const double DefaultAcceleraton = 0.06;


		public double MaxVelocityX = 3.0;
		public double AccelerationHandicap = 1.0;

		public bool IsMaxVelocityXReached
		{
			get
			{
				if (this.MaxVelocityX == 0)
					return false;

				if (Math.Abs(this.VelocityX) > this.MaxVelocityX)
					return true;

				return false;
			}
		}
		public void AddAcceleration(PlayerInput e)
		{
			// if the AI is controlling this actor
			// we cannot slow it down
			// just looking at the keyboard keys anymore


			if (Memory_CaveAction)
				return;

			// we are hidden inside a cave
			if (this.CurrentCave != null)
				return;

			if (e.Keyboard.IsPressedLeft)
			{


				// at some point we should not be able to accelerate
				if (!IsMaxVelocityXReached)
					this.VelocityX -= this.Zoom * DefaultAcceleraton * AccelerationHandicap;
			}
			else if (e.Keyboard.IsPressedRight)
			{


				if (!IsMaxVelocityXReached)
					this.VelocityX += this.Zoom * DefaultAcceleraton * AccelerationHandicap;
			}
			else
			{

				//if (this.VelocityY == 0)
				//{
				//    this.VelocityX *= 0.85;

				//    if (this.VelocityX <= 0.03)
				//        this.VelocityX = 0;
				//}
			}

			if (this.Animation != AnimationEnum.Hidden)
			{
				if (this.VelocityX == 0)
				{
					if (this.Animation == AnimationEnum.WalkLeft)
						this.Animation = AnimationEnum.Idle;

					if (this.Animation == AnimationEnum.WalkRight)
						this.Animation = AnimationEnum.Idle;
				}
				else
				{
					if (this.VelocityX < 0)
					{
						if (this.ToObstacle().Bottom > this.LevelViaKnownActors.WaterTop)
						{
							if (this.Animation != AnimationEnum.SwimLeft)
								this.Animation = AnimationEnum.SwimLeft;
						}
						else
						{
							if (this.Animation != AnimationEnum.WalkLeft)
								this.Animation = AnimationEnum.WalkLeft;
						}

					}
					else
					{
						if (this.ToObstacle().Bottom > this.LevelViaKnownActors.WaterTop)
						{
							if (this.Animation != AnimationEnum.SwimRight)
								this.Animation = AnimationEnum.SwimRight;
						}
						else
						{
							if (this.Animation != AnimationEnum.WalkRight)
								this.Animation = AnimationEnum.WalkRight;
						}
					}
				}
			}

			if (e.Keyboard.IsPressedUp)
			{
				if (this.VelocityY == 0)
				{
					this.VelocityY -= this.Zoom * 2.0;

					this.VelocityX *= 1.1;

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

			if (e.Keyboard.IsPressedDrop)
			{
				if (Drop != null)
					Drop();
			}

			if (!EnterVehicleBlocked)
				if (e.Keyboard.IsPressedEnter)
				{

					if (EnterVehicle != null)
						EnterVehicle();
				}

			//if (!this.Memory_LogicState_IsTalking)
			//    if (this.GetVelocity() > 0)
			//        this.KnownBubbles.RemoveAll();

		}


		public event Action Drop;
		public event Action Jumping;
		public event Action EnterCave;
		public event Action EnterVehicle;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.AnimationTimer_Idle.Stop();
			this.AnimationTimer_Panic.Stop();
			this.AnimationTimer_Talk.Stop();
			this.AnimationTimer_SwimLeft.Stop();
			this.AnimationTimer_SwimRight.Stop();
			this.AnimationTimer_WalkLeft.Stop();
			this.AnimationTimer_WalkRight.Stop();

			this.Container.Orphanize();

		}

		#endregion

		public void RaiseDrop()
		{
			if (this.Drop != null)
				this.Drop();
		}


		public event Action WaterCollision;

		public void RaiseWaterCollision()
		{
			if (WaterCollision != null)
				WaterCollision();
		}


		LevelType InternalCurrentLevel;
		public LevelType CurrentLevel
		{
			get
			{
				return InternalCurrentLevel;
			}
			set
			{
				if (this.InternalCurrentLevel != null)
				{
					this.InternalCurrentLevel.KnownActors.Remove(this);

					if (this.CurrentVehicle != null)
						this.CurrentVehicle = null;
				}

				this.InternalCurrentLevel = value;

				if (this.InternalCurrentLevel != null)
				{
					this.InternalCurrentLevel.KnownActors.Add(this);
				}

				if (this.CurrentLevelChanged != null)
					this.CurrentLevelChanged();

			}
		}
		public event Action CurrentLevelChanged;

		public Cave NearbyCave
		{
			get
			{
				var ManAsObstacle = this.ToObstacle();

				return this.CurrentLevel.KnownCaves.FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

			}
		}

		public int BaseY
		{
			get
			{
				return Convert.ToInt32((this.Y + HalfHeight) / (PrimitiveTile.Heigth * Zoom));
			}
		}

		public int UnscaledX
		{
			get
			{
				return Convert.ToInt32(X / Zoom);
			}
		}

		public event Action CurrentPassengerVehicleChanged;
		Vehicle InternalCurrentPassengerVehicle;
		public Vehicle CurrentPassengerVehicle
		{
			get
			{
				return InternalCurrentPassengerVehicle;
			}
			set
			{
				InternalCurrentPassengerVehicle = value;
				if (CurrentPassengerVehicleChanged != null)
					CurrentPassengerVehicleChanged();
			}
		}

		public void StartTalkingConfusion()
		{
			var i = new { Passenger = this };


			SoundBoard.Default.talk0_01();


			i.Passenger.Memory_LogicState = Actor.Memory_LogicState_ConfusedStart;

			if (i.Passenger.ToObstacle().Bottom > this.LevelViaKnownActors.WaterTop)
			{
				i.Passenger.Animation = Actor.AnimationEnum.Panic;
			}
			else
				i.Passenger.Animation = Actor.AnimationEnum.Talk;

			i.Passenger.VelocityX = 0;
			i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
			i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;

			i.Passenger.KnownBubbles.RemoveAll();
			i.Passenger.KnownBubbles.Add(
				new Actor.Bubble(this.LevelViaKnownActors.Zoom, -1)
			);
		}

		public void StartTalking()
		{
			var i = new { Passenger = this };

			if (i.Passenger is Actor.woman0)
			{
				// woman will sound a little different than the males
				SoundBoard.Default.talk0_02();
			}
			else
			{
				SoundBoard.Default.talk0_00();
			}

			i.Passenger.KnownBubbles.Add(
				new Actor.Bubble(i.Passenger.LevelViaKnownActors.Zoom,
					i.Passenger.LevelViaKnownActors.ToPlatformSnapshots().AtModulus(i.Passenger.Memory_Route_NextPlatformIndex).CaveSigns.First().Value
				)
			);

			i.Passenger.Memory_FirstWait = true;
			i.Passenger.Memory_LogicState = Actor.Memory_LogicState_TalkStart;

			if (i.Passenger.ToObstacle().Bottom > this.LevelViaKnownActors.WaterTop)
			{
				i.Passenger.Animation = Actor.AnimationEnum.Panic;
			}
			else
				i.Passenger.Animation = Actor.AnimationEnum.Talk;

			i.Passenger.VelocityX = 0;
			i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
			i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;
		}
	}
}
