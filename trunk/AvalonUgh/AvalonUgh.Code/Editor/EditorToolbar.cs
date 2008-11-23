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
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class EditorToolbar : Window
	{
		public View.SelectorInfo EditorSelector;
		public event Action EditorSelectorChanged;

		[Script]
		public class  Button
		{
			public Image Image;
			public Rectangle TouchOverlay;
		}

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


		

			var Buttons = new List<Button>();
		
			Func<int> ButtonsWidth = () => Padding + Buttons.Count * (PrimitiveTile.Width * 2 + Padding);

			CreateButtons(Buttons, ButtonsWidth);

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

		private void CreateButtons(List<Button> Buttons, Func<int> ButtonsWidth)
		{
			var SelectionMarker = new Rectangle
			{
				Fill = Brushes.LightGreen,
				Opacity = 0.5,
				Width = PrimitiveTile.Width * 2 + 4,
				Height = PrimitiveTile.Heigth * 2 + 4
			}.AttachTo(this);

			Action<int, int> SelectionMarkerMove = NumericEmitter.Of(
				(x, y) => SelectionMarker.MoveTo(x, y)
			);

			SelectionMarkerMove(Padding - 2, Padding - 2);

			bool DefaultSelectionMade = false;

			#region AddButton
			Action<Image, View.SelectorInfo[]> AddButton =
				(Image, EditorSelector) =>
				{
					var EditorSelectorCycle = EditorSelector.AsCyclicEnumerable().GetEnumerator();

					var x = ButtonsWidth();
					var y = Padding;

					Image.AttachTo(this);

					var TouchOverlay = new Rectangle
					{
						Width = PrimitiveTile.Width * 2,
						Height = PrimitiveTile.Heigth * 2,
						Fill = Brushes.Red,
						Opacity = 0,
						Cursor = Cursors.Hand
					}.AttachTo(this).MoveTo(x, y);

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

					Action Select =
						delegate
						{
							SelectionMarkerMove(x - 2, y - 2);


							if (EditorSelectorCycle.MoveNext())
							{
								DefaultSelectionMade = true;
								SelectionMarker.Fill = Brushes.LightGreen;

								this.EditorSelector = EditorSelectorCycle.Current;
								if (EditorSelectorChanged != null)
									EditorSelectorChanged();
							}
							else
							{
								SelectionMarker.Fill = Brushes.Red;
							}
						};

					TouchOverlay.MouseLeftButtonUp +=
						delegate
						{
							Select();
							
						};

					if (!DefaultSelectionMade)
						Select();

					new Button
					{
						Image = Image,
						TouchOverlay = TouchOverlay
					}.AddTo(Buttons);
				};
			Action<string, View.SelectorInfo[]> AddButton_2x2 =
				(Image, EditorSelector) =>
				{
					AddButton(
						new Image
						{
							Source = Image.ToSource(),
							Width = PrimitiveTile.Width * 2,
							Height = PrimitiveTile.Heigth * 2
						}.MoveTo(ButtonsWidth(), Padding)
					, EditorSelector);


				};

			Action<string, View.SelectorInfo[]> AddButton_1x1 =
				(Image, EditorSelector) =>
				{
					AddButton(
						new Image
						{
							Source = Image.ToSource(),
							Width = PrimitiveTile.Width,
							Height = PrimitiveTile.Heigth
						}.MoveTo(
							ButtonsWidth() + PrimitiveTile.Width / 2,
							Padding + PrimitiveTile.Width / 2
						)
					, EditorSelector);


				};
			#endregion


			AddButtons(ButtonsWidth, AddButton, AddButton_2x2, AddButton_1x1);
		}

		private void AddButtons(Func<int> ButtonsWidth, Action<Image, View.SelectorInfo[]> AddButton, Action<string, View.SelectorInfo[]> AddButton_2x2, Action<string, View.SelectorInfo[]> AddButton_1x1)
		{
			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/stone1_2x2.png",
				Editor.Tiles.StoneSelector.Sizes
			);

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/cave0_2x2.png",
				Editor.Tiles.CaveSelector.Sizes
			);

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/platform0_2x2.png", null);
			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/ridge0_2x2.png", null);
			
			
			AddButton_1x1(Assets.Shared.KnownAssets.Path.Tiles + "/bridge0.png", null);

			AddButton_1x1(Assets.Shared.KnownAssets.Path.Tiles + "/fence0.png",
				new[]
					{
						new Editor.Tiles.FenceSelector.Size_1x1()
					}
			);


			AddButton_2x2(Assets.Shared.KnownAssets.Path.Sprites + "/tree0_2x2.png", 
				new []
				{
					new Editor.Sprites.TreeSelector.Size_2x2()
				}
			);

			AddButton_1x1(Assets.Shared.KnownAssets.Path.Sprites + "/sign0.png",
				new[]
				{
				    new Editor.Sprites.SignSelector.Size_1x1()
				}	
			);
			AddButton_1x1(Assets.Shared.KnownAssets.Path.Sprites + "/rock0.png",
				new[]
				{
					new Editor.Sprites.RockSelector.Size_1x1()
				}	
			);

			

		

			var Demolish = 
				new Image
				{
					Source = (Assets.Shared.KnownAssets.Path.Assets + "/btn_demolish.png").ToSource(),
					Width = 20,
					Height = 20
				}.MoveTo(ButtonsWidth() + PrimitiveTile.Width - 10, Padding + PrimitiveTile.Heigth - 10);

			AddButton(Demolish, Editor.DemolishSelector.Sizes);
		}
	}
}
