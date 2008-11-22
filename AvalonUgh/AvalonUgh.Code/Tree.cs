using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace AvalonUgh.Code
{
	[Script]
	public class Tree : ISupportsContainer, ISupportsObstacle, ISupportsMoveTo, IDisposable
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


		public void MoveTo(double x, double y)
		{
			this.X = x;
			this.Y = y;

			this.Container.MoveTo(x - HalfWidth, y - HalfHeight);
		}

		public bool IsSleeping { get; set; }

		public Tree(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			var ShowFrame = Enumerable.Range(0, 3).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/tree" + index + "_2x2.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			).ToShowFrame(
				k =>
					new
					{
						ok = k.FixParam(0),
						blink = k.FixParam(1),
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
						ShowFrame.blink();

						100.AtDelay(
							delegate
							{
								if (IsSleeping)
									return;

								ShowFrame.ok();
							}
						);
					};
				};

			ShowFrame.ok();
			this.AnimationTimer = 1000.AtInterval(UpdateFrame);

		}

		public readonly Action UpdateFrame;

		public void MoveToTile(double x, double y)
		{
			MoveTo(PrimitiveTile.Width * x * Zoom + HalfWidth, PrimitiveTile.Heigth * y * Zoom + HalfHeight);

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

		public void GoToSleep()
		{
			if (this.IsSleeping)
				return;

			this.IsSleeping = true;
			this.UpdateFrame();


			5000.AtDelay(
				delegate
				{
					this.IsSleeping = false;
					this.UpdateFrame();
				}
			);
		}

		DispatcherTimer AnimationTimer;

		public void Dispose()
		{
			this.Container.Orphanize();
			AnimationTimer.Stop();
		}
	}
}
