using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Controls;
using ScriptCoreLib.Shared.Avalon.TiledImageButton;
using System.Windows;

namespace AvalonUgh.Code
{
	[Script]
	public class EditorToolbar : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public EditorToolbar(Canvas DragContainer)
		{
			const int Width = 300;
			const int Height = 200;
			const int BorderWidth = 1;
			const int Padding = BorderWidth + 4;

			this.Container = new Canvas
			{
				Width = Width,
				Height = Height,
			};


			#region borders
			var ThreeD_Top = new Rectangle
			{
				Width = Width,
				Height = BorderWidth,
				Fill = Brushes.LightGreen,
			}.AttachTo(this.Container).MoveTo(0, 0);

			var ThreeD_Left = new Rectangle
			{
				Width = BorderWidth,
				Height = Height - BorderWidth * 2,
				Fill = Brushes.LightGreen,
			}.AttachTo(this.Container).MoveTo(0, BorderWidth);

			var ThreeD_Right = new Rectangle
			{
				Width = BorderWidth,
				Height = Height - BorderWidth * 2,
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.Container).MoveTo(Width - BorderWidth, BorderWidth);

			var ThreeD_Bottom = new Rectangle
			{
				Width = Width,
				Height = BorderWidth,
				Fill = Brushes.DarkGreen,
			}.AttachTo(this.Container).MoveTo(0, Height - BorderWidth);

			var ThreeD_Fill = new Rectangle
			{
				Width = Width - BorderWidth * 2,
				Height = Height - BorderWidth * 2,
				Fill = Brushes.Green,
				Opacity = 0.8
			}.AttachTo(this.Container).MoveTo(BorderWidth, BorderWidth);
			#endregion

			var DraggableArea = new Rectangle
			{
				Width = Width,
				Height = Height,
				Fill = Brushes.Black,
				Opacity = 0
			}.AttachTo(this.Container).MoveTo(0, 0);

			var Drag = new DragBehavior(DraggableArea, Container, DragContainer)
			{
				SnapX = x => x.Max(Padding - Width).Min(DragContainer.Width - Padding),
				SnapY = y => y.Max(Padding - Height).Min(DragContainer.Height - Padding)
			};


			// how to turn off arrow key tabbing? 
			// http://www.eggheadcafe.com/software/aspnet/29317268/how-to-turn-off-arrow-key.aspx

			var Navbar = new AeroNavigationBar();

			Navbar.AttachContainerTo(this).MoveContainerTo(Padding, Padding);

			var LevelTextBackground = new Rectangle
			{
				Fill = Brushes.LightGreen,
				Opacity = 0.2,
				Width = Width - Padding - Padding,
				Height = Height - Navbar.Height - Padding * 3,
			}.AttachTo(this).MoveTo(Padding, Navbar.Height + Padding + Padding);

			var LevelText = new TextBox
			{
				AcceptsReturn = true,
				Width = Width - Padding - Padding,
				Height = Height - Navbar.Height - Padding * 3,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0)
			}.AttachTo(this).MoveTo(Padding, Navbar.Height + Padding + Padding);



			LevelText.GotFocus += delegate { LevelTextBackground.Opacity = 0.7; };
			LevelText.LostFocus += delegate { LevelTextBackground.Opacity = 0.2; };

		}
	}
}
