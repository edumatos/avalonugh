using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class Dino : ISupportsContainer, ISupportsMoveTo, ISupportsObstacle
	{
		//DINOSAUR
		//--------
		//HE SLEEPS PEACEFULLY ON HIS PLATFORM, AND YOU MIGHT THINK THAT HE CAN`T DO
		//ANY HARM, HOWEVER, HIS HEAVY SNORING CONSTANTLY THREATENS TO THROW YOUR
		//HELICOPTER OFF COURSE.

		public View.SelectorInfo Selector { get; set; }

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

		public event Action LocationChanged;

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);

			if (LocationChanged != null)
				LocationChanged();
		}


		readonly Image[] AnimationFrames;
		Image AnimationFrame;

		public Dino(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 3;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			var Frame = new NameFormat
			{
				Path = Assets.Shared.KnownAssets.Path.Sprites,
				Name = "dino",
				AnimationFrame = 0,
				Width = 3,
				Height = 2,
				Extension = "png",
				Zoom = Zoom
			};

			AnimationFrames = Enumerable.Range(0, 10).ToArray(i => Frame.ToAnimationFrame(i).ToImage());
			AnimationFrame = AnimationFrames.First();
			AnimationFrame.AttachTo(this);

		}

		public Obstacle SnoreArea
		{
			get
			{
				return this.ToObstacle().WithOffset(
					PrimitiveTile.Width * Zoom * -3,
					0
				);
			}
		}

		public double SnoreWindAmpilfier = 0.1;
		public double SnoreWind;

		public void Animate(int SyncFrame)
		{
			AnimationFrame.Orphanize();

			this.SnoreWind = Math.Cos(0.1 * SyncFrame / AnimationFrames.Length * 8);

			AnimationFrame = AnimationFrames.AtModulus(SyncFrame / 8);
			AnimationFrame.AttachTo(this);
		}

		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth,
				Top = y - HalfHeight,
				Right = x + HalfWidth,
				Bottom = y + HalfHeight,
			};
		}

		public int BaseY
		{
			get
			{
				return Convert.ToInt32((this.Y + HalfHeight) / (PrimitiveTile.Heigth * Zoom));
			}
		}
	}
}
