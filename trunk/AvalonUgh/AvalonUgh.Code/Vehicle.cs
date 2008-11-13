﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class Vehicle : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public readonly int Zoom;

		public readonly int Width;
		public readonly int Height;

		public bool IsAnimated { get; set; }

		public void MoveTo(double x, double y)
		{
			this.Container.MoveTo(x - Width / 2, y - Height / 2);
		}

		readonly Image ColorStripeRed;
		readonly Image ColorStripeBlue;
		readonly Image ColorStripeYellow;

		public Color ColorStripe
		{
			set
			{
				ColorStripeBlue.Hide();
				ColorStripeRed.Hide();
				ColorStripeYellow.Hide();

				if (value == Colors.Red)
				{
					ColorStripeRed.Show();
					return;
				}

				if (value == Colors.Blue)
				{
					ColorStripeBlue.Show();
					return;
				}

				if (value == Colors.Yellow)
				{
					ColorStripeYellow.Show();
					return;
				}
			}
		}

		public Vehicle(int Zoom)
		{
			this.Zoom = Zoom;

			this.Width = 16 * Zoom * 2;
			this.Height = 12 * Zoom * 2;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			this.IsAnimated = true;

			var frames = Enumerable.Range(0, 6).ToArray(
				index =>
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_" + ("" + index).PadLeft(2, '0') + "_2x2.png").ToSource(),
						Stretch = Stretch.Fill,
						Width = this.Width,
						Height = this.Height,
						Visibility = Visibility.Hidden
					}.AttachTo(this.Container)
			);

			this.ColorStripeRed = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_red_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			this.ColorStripeBlue = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_blue_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			this.ColorStripeYellow = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_yellow_2x2.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = this.Width,
				Height = this.Height,
				Visibility = Visibility.Hidden
			}.AttachTo(this.Container);

			frames.AsCyclicEnumerable().ForEach(
				(Image value, Action SignalNext) =>
				{
					value.Visibility = Visibility.Visible;

					(1000 / 30).AtIntervalWithTimer(
						t =>
						{
							if (IsAnimated)
							{
								value.Visibility = Visibility.Hidden;
								SignalNext();
								t.Stop();
							}
						}
					);
				}
			);
		}
	}

}
