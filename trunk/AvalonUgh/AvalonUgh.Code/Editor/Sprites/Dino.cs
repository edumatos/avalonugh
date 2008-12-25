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

		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);
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

		public void Animate(int SyncFrame)
		{
			AnimationFrame.Orphanize();

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
