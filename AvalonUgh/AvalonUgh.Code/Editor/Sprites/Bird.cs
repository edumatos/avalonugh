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
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code
{
	[Script]
	public class Bird : ISupportsContainer, ISupportsObstacle
	{
		// PTERODACTYL
		//-----------
		//HE IS THE MOST DANGEROUS OF ALL OPPONENTS. IF YOU TOUCH HIM YOUR HELICOPTER
		//WILL CRASH AT ONCE. JUST BEFORE PTERODACTYL APPEARS YOU WILL HEAR A CRY
		//AS SOON AS YOU HEAR THIS UNUSUAL NOISE, GET READY TO TAKE EVASIVE ACTION.


		public Canvas Container { get; set; }

		public readonly int Zoom;

		public readonly int Width;
		public readonly int Height;


		public double X { get; set; }
		public double Y { get; set; }


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

		public Bird(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = PrimitiveTile.Width * Zoom * 2;
			this.Height = PrimitiveTile.Heigth * Zoom * 3;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};


			var frames = Enumerable.Range(0, 13).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/bird0_" + ("" + index).PadLeft(2, '0') + "_2x3.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			);

			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Visibility = Visibility.Visible;

					(1000 / 30).AtDelay(
						delegate
						{
							value.Visibility = Visibility.Hidden;
							SignalNext();
						}
					);
				}
			);
		}

		public Obstacle ToObstacle(double x, double y)
		{
			return new Obstacle
			{
				Left = x - HalfWidth,
				Top = y - HalfHeight / 2,
				Right = x + HalfWidth,
				Bottom = y + HalfHeight / 2,
				//SupportsVelocity = this
			};
		}
	}
}
