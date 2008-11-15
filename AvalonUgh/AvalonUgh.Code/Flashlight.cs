using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvalonUgh.Code
{
	[Script]
	public class Flashlight : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public readonly int Zoom;
		public readonly int Width;
		public readonly int Height;

		readonly Rectangle BlindTop;
		readonly Rectangle BlindLeft;
		readonly Rectangle BlindRight;
		readonly Rectangle BlindBottom;

		readonly Image flashlight;

		const int Size = 96;

		public void MoveTo(double x, double y)
		{
			var h = Size * Zoom / 2;

			flashlight.MoveTo(x - h, y - h);

			BlindTop.MoveTo(0, y - h - Height);
			BlindBottom.MoveTo(0, y + h);

			BlindLeft.MoveTo(x - h - Width, 0);
			BlindRight.MoveTo(x + h, 0);
		}


		bool _Visible = true;
		public bool Visible
		{
			get
			{
				return this._Visible;
			}
			set
			{
				_Visible = value;

				if (value)
					this.Container.Show();
				else
					this.Container.Hide();
			}
		}
		public Flashlight(int Zoom, int Width, int Height)
		{
			this.Zoom = Zoom;
			this.Width = Width;
			this.Height = Height;

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height
			};

			this.Container.ClipToBounds = true;

			this.flashlight = new Image
			{
				Source = (Assets.Shared.KnownAssets.Path.Assets + "/flashlight_x96.png").ToSource(),
				Stretch = Stretch.Fill,
				Width = Size * Zoom,
				Height = Size * Zoom,
			}.AttachTo(this.Container);


			this.BlindTop = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black
			}.AttachTo(this.Container);

			this.BlindLeft = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black
			}.AttachTo(this.Container);

			this.BlindRight = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black
			}.AttachTo(this.Container);

			this.BlindBottom = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black
			}.AttachTo(this.Container);
		}
	}
}
