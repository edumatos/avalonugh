using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public partial class Tryoperus : ISupportsContainer, ISupportsPhysics, IDisposable
	{
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

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);
		}

		public bool IsSleeping { get; set; }

		public Tryoperus(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			var Frame_Hit =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = "tryo",
					Index = 0,
					AnimationFrame = Tryoperus.AnimationFrames.Left.HitOffset,
					Extension = "png",
					Width = 2,
					Height = 2,
					Zoom = Zoom
				};

			Frame_Hit.ToImage().AttachTo(this);
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
				Left = x - HalfWidth,
				Top = y - HalfHeight,
				Right = x + HalfWidth,
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

		public double LastCollisionVelocity { get; set; }

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
	}
}
