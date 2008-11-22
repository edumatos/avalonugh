using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Shapes;
using System.Windows.Media;

namespace AvalonUgh.Code
{
	[Script]
	public class Window : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }
		public int BorderWidth { get; set; }
		public int Padding { get; set; }

		public readonly Action Update;

		public Window()
		{
			Width = 360;
			Height = 200;
			BorderWidth = 1;
			Padding = BorderWidth + 4;

			this.Container = new Canvas
			{
			};

			#region borders
			var ThreeD_Top = new Rectangle
			{
				Fill = Brushes.LightGreen,
			}.AttachTo(this.Container);

			var ThreeD_Left = new Rectangle
			{
				Fill = Brushes.LightGreen,
			}.AttachTo(this.Container);

			var ThreeD_Right = new Rectangle
			{
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.Container);

			var ThreeD_Bottom = new Rectangle
			{
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.Container);

			var ThreeD_Fill = new Rectangle
			{
				Fill = Brushes.Green,
				Opacity = 0.8
			}.AttachTo(this.Container);
			#endregion

			Update =
				delegate
				{
					Container.SizeTo(
						Width,
						Height
					);

					ThreeD_Top.SizeTo(
						Width,
						BorderWidth
					).MoveTo(0, 0);

					ThreeD_Left.SizeTo(
						BorderWidth,
						Height - BorderWidth * 2
					).MoveTo(0, BorderWidth);

					ThreeD_Right.SizeTo(
						BorderWidth,
						Height - BorderWidth * 2
					).MoveTo(Width - BorderWidth, BorderWidth);
						
					ThreeD_Bottom.SizeTo(
						Width,
						BorderWidth
					).MoveTo(0, Height - BorderWidth);

					ThreeD_Fill.SizeTo(
						Width - BorderWidth * 2,
						Height - BorderWidth * 2
					).MoveTo(BorderWidth, BorderWidth);

				};

			Update();
		}
	}
}
