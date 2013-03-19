using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public partial class Tryoperus : ISupportsContainer, ISupportsPhysics, IDisposable, ISupportsLocationChanged
	{
		public double MassCenterModifier { get; set; }

		//TRYOPERUS
		//---------
		//AS LONG AS YOU DONT DISTURB HIM ON HIS PLATFORM HE IS A HARMLESS FELLOW.
		//BUT IF YOU LAND ON HIS PLATFORM HE WILL THINK YOU ARE AN INTRUDER AND WILL
		//ATTACK YOU WITH HIS HORN. IF THERE IS NO OTHER WAY OUT YOU CAN PUT HIM OUT
		//OF ACTION FOR A FEW SECONDS WITH THE STONE.

		public View.SelectorInfo Selector { get; set; }
		public Tryoperus StartPosition;

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

		public double X { get; set; }
		public double Y { get; set; }

		public int UnscaledX
		{
			get
			{
				return Convert.ToInt32(X / Zoom);
			}
		}

		public int UnscaledY
		{
			get
			{
				return Convert.ToInt32(Y / Zoom);
			}
		}

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);


			if (LocationChanged != null)
				LocationChanged();
		}

		public bool IsSleeping
		{
			get;
			set;
		}


		readonly AnimationDictionary InternalAnimation;

		readonly LevelType Level;

		public Tryoperus(LevelType Level)
		{
			this.Level = Level;
			this.Zoom = Level.Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			this.InternalAnimation = new AnimationDictionary(this, new SpecificNameFormat { Zoom = Level.Zoom }.ToImage)
			{
				{ (int)AnimationEnum.Left_Hit, Tryoperus.AnimationFrames.Left.HitOffset},
				{ (int)AnimationEnum.Left_Stun, 1000 / 24,  
					Tryoperus.AnimationFrames.Left.StunOffset, 
					Tryoperus.AnimationFrames.Left.StunCount },
				{ (int)AnimationEnum.Left_Run, 1000 / 24,  
					Tryoperus.AnimationFrames.Left.RunOffset, 
					Tryoperus.AnimationFrames.Left.RunCount },
				{ (int)AnimationEnum.Left_Stare, 1000 / 15,  
					Tryoperus.AnimationFrames.Left.StareOffset, 
					Tryoperus.AnimationFrames.Left.StareCount },
				{ (int)AnimationEnum.Left_Walk, 1000 / 10,  
					Tryoperus.AnimationFrames.Left.WalkOffset, 
					Tryoperus.AnimationFrames.Left.WalkCount },
				{ (int)AnimationEnum.Left_Stand,
					Tryoperus.AnimationFrames.Left.WalkOffset},


				{ (int)AnimationEnum.Right_Walk, 1000 / 10,  
					Tryoperus.AnimationFrames.Right.WalkOffset, 
					Tryoperus.AnimationFrames.Right.WalkCount },
				{ (int)AnimationEnum.Right_Stand,
					Tryoperus.AnimationFrames.Right.WalkOffset}
			};

			this.Animation = AnimationEnum.Left_Walk;
		}


		public int BaseY
		{
			get
			{
				return Convert.ToInt32((this.Y + HalfHeight) / (PrimitiveTile.Heigth * Zoom));
			}
		}


		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth / 2,
				Top = y - HalfHeight,
				Right = x + HalfWidth / 2,
				Bottom = y + HalfHeight,
				//SupportsVelocity = this
			};
		}

		public double Density { get; set; }
		public int Stability { get; set; }

		public void StabilityReached()
		{
			//if (Level == null)
			//    return;

			//if (this.ToObstacle().Bottom < this.Level.WaterTop)
			//{
			//    if (this.Animation == AnimationEnum.Panic)
			//        this.Animation = AnimationEnum.Idle;
			//}
		}

		public double VelocityX { get; set; }
		public double VelocityY { get; set; }


		public double LastVelocity { get; set; }
		public double LastCollisionVelocity { get; set; }
		public double LastWaterCollisionVelocity { get; set; }

		public bool PhysicsDisabled
		{
			get
			{
				//if (CurrentVehicle != null)
				//    return true;

				//if (CurrentCave != null)
				//    return true;

				return false;

				//return Animation != AnimationEnum.Panic;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{

		}

		#endregion

		public Func<int, Action, int> HandleFutureFrame;

		public void GoToSleep()
		{
			this.IsSleeping = true;
			this.Animation = AnimationEnum.Left_Stun;

			// for the next handful of frames we will be sleeping
			this.HandleFutureFrame(500,
				delegate
				{
					this.IsSleeping = false;
					this.Animation = AnimationEnum.Left_Stare;
				}
			);
		}

		public View.SelectorPosition Position
		{
			get
			{
				return new View.SelectorPosition
				{
					ContentX = Convert.ToInt32((X - HalfWidth) / Zoom),
					ContentY = Convert.ToInt32((Y - HalfHeight) / Zoom)
				};
			}
		}

		// this needs to be removed
		public int Direction = -1;

		public bool CanWalkIntoDirection(int Direction)
		{
			if (Direction < 0)
			{
				if (HasPlatformUnderneath(-1, 2))
					if (HasPlatformUnderneath(0, 2))
						if (!HasPlatformUnderneath(-1, 0))
							if (!HasPlatformUnderneath(-1, 1))
								return true;

			}
			else
			{
				if (HasPlatformUnderneath(1, 2))
					if (HasPlatformUnderneath(2, 2))
						if (!HasPlatformUnderneath(2, 0))
							if (!HasPlatformUnderneath(2, 1))
								return true;

			}

			return false;
		}


		public bool HasPlatformUnderneath(int TileOffset, int TileOffsetY)
		{
			var TriggerPosition = Position[TileOffset, TileOffsetY];

			var o_trigger = Obstacle.Of(TriggerPosition, Level.Zoom, 1, 1);

			if (Level.ToObstacles().Any(k => k.Intersects(o_trigger)))
				return true;

			return false;
		}

		public void Think()
		{
			if (this.IsSleeping)
			{
				if (this.VelocityY == 0)
					this.VelocityX *= 0.9;

				return;
			}

			if (this.Level.WaterTop < this.Y)
			{
				// we dont do anything under water
				this.Animation = AnimationEnum.Left_Hit;
				return;
			}

			if (this.VelocityY != 0)
			{
				// we dont do anything not on ground
				this.Animation = AnimationEnum.Left_Hit;
				return;
			}



			if (this.CanWalkIntoDirection(this.Direction))
			{
				// we can continue to walk

				// top speed
				if (this.Direction < 0)
				{
					if (this.VelocityX >= -Zoom * 0.8)
						this.VelocityX -= Zoom * 0.05;
					this.Animation = AnimationEnum.Left_Walk;
				}
				else
				{
					if (this.VelocityX <= Zoom * 0.8)
						this.VelocityX += Zoom * 0.05;
					this.Animation = AnimationEnum.Right_Walk;
				}


			}
			else
			{
				this.VelocityX *= 0.8;

				if (this.VelocityX == 0)
				{
					// there is no use in turning around either
					if (this.CanWalkIntoDirection(-this.Direction))
					{
						this.Direction *= -1;
					}
				}


				if (this.Direction < 0)
					this.Animation = AnimationEnum.Left_Stand;
				else
					this.Animation = AnimationEnum.Right_Stand;

			}


			return;
		}

		#region ISupportsLocationChanged Members


		public event Action LocationChanged;

		#endregion
	}
}
