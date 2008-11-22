using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.TiledImageButton;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Input;

namespace AvalonUgh.Code
{
	[Script]
	public class EditorToolbar : Window
	{

		public EditorToolbar(Canvas DragContainer)
		{

			this.Padding = 6;

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

			//var Navbar = new AeroNavigationBar();

			//Navbar.AttachContainerTo(this).MoveContainerTo(Padding, Padding);



			var Buttons =
				new[]
				{
					new 
					{
						Image = default(Image),
						TouchOverlay = default(Rectangle),
					}
				}.ToList();
			
			Buttons.Clear();

			Func<int> ButtonsWidth = () => Padding + Buttons.Count * (PrimitiveTile.Width * 2 + Padding);

			#region AddButton
			Action<Image> AddButton =
				Image =>
				{
					var x = ButtonsWidth();

					Image.AttachTo(this);

					var TouchOverlay = new Rectangle
					{
						Width = PrimitiveTile.Width * 2,
						Height = PrimitiveTile.Heigth * 2,
						Fill = Brushes.Red,
						Opacity = 0,
						Cursor = Cursors.Hand
					}.AttachTo(this).MoveTo(x, Padding);

					TouchOverlay.MouseEnter +=
						delegate
						{
							Image.Opacity = 0.6;
						};

					TouchOverlay.MouseLeave +=
						delegate
						{
							Image.Opacity = 1;
						};


					new
					{
						Image,
						TouchOverlay
					}.AddTo(Buttons);
				};
			Action<string> AddButton_2x2 =
				AssetSource =>
				{
					AddButton(
						new Image
						{
							Source = AssetSource.ToSource(),
							Width = PrimitiveTile.Width * 2,
							Height = PrimitiveTile.Heigth * 2
						}.MoveTo(ButtonsWidth(), Padding)
					);

					
				};

			Action<string> AddButton_1x1 =
				AssetSource =>
				{
					AddButton(
						new Image
						{
							Source = AssetSource.ToSource(),
							Width = PrimitiveTile.Width,
							Height = PrimitiveTile.Heigth
						}.MoveTo(
							ButtonsWidth() + PrimitiveTile.Width / 2,
							Padding + PrimitiveTile.Width / 2
						)
					);


				};
			#endregion


			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/stone1_2x2.png");
			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/platform0_2x2.png");
			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/ridge0_2x2.png");
			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/cave0_2x2.png");

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Sprites + "/tree0_2x2.png");

			AddButton_1x1(Assets.Shared.KnownAssets.Path.Sprites + "/sign0.png");
			AddButton_1x1(Assets.Shared.KnownAssets.Path.Sprites + "/rock0.png");

			AddButton_1x1(Assets.Shared.KnownAssets.Path.Tiles + "/fence0.png");
			AddButton_1x1(Assets.Shared.KnownAssets.Path.Tiles + "/bridge0.png");

			AddButton(
				new Image
				{
					Source = (Assets.Shared.KnownAssets.Path.Assets + "/btn_demolish.png").ToSource(),
					Width = 20,
					Height = 20
				}.MoveTo(ButtonsWidth() + PrimitiveTile.Width - 10, Padding +  PrimitiveTile.Heigth - 10)
			);

			this.Width = ButtonsWidth();
			this.Update();

			#region LevelText
			var LevelTextBackground = new Rectangle
			{
				Fill = Brushes.LightGreen,
				Opacity = 0.2,
				Width = Width - Padding - Padding,
				Height = Height - PrimitiveTile.Heigth * 2 - Padding * 3,
			}.AttachTo(this).MoveTo(Padding, PrimitiveTile.Heigth * 2 + Padding + Padding);

			var LevelText = new TextBox
			{
				AcceptsReturn = true,
				Width = Width - Padding - Padding,
				Height = Height - PrimitiveTile.Heigth * 2 - Padding * 3,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0)
			}.AttachTo(this).MoveTo(Padding, PrimitiveTile.Heigth * 2 + Padding + Padding);



			LevelText.GotFocus += delegate { LevelTextBackground.Opacity = 0.7; };
			LevelText.LostFocus += delegate { LevelTextBackground.Opacity = 0.2; };
			#endregion
		}
	}
}
