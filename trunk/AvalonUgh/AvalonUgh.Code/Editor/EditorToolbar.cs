using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.TiledImageButton;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class EditorToolbar : Window
	{
		public Action EditorSelectorNextSize;
		public Action EditorSelectorPreviousSize;

		View.SelectorInfo InternalEditorSelector;
		public View.SelectorInfo EditorSelector
		{
			get
			{
				return InternalEditorSelector;
			}
			set
			{
				InternalEditorSelector = value;

				if (EditorSelectorChanged != null)
					EditorSelectorChanged();
			}
		}

		public event Action EditorSelectorChanged;

		[Script]
		public class Button
		{
			public Image Image;
			public Rectangle TouchOverlay;
		}

		public EditorToolbar(Canvas DragContainer)
		{

			this.Padding = 8;

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



			this.LevelText = new TextBox
			{
				FontFamily = new FontFamily("Courier New"),
				AcceptsReturn = true,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0)
			};

			var Buttons = new List<Button>();

			Func<int> ButtonsWidth = () => Padding + Buttons.Count * (PrimitiveTile.Width * 2 + Padding);

			CreateButtons(Buttons, ButtonsWidth);

			this.Width = ButtonsWidth();
			this.Height = this.Width * 2 / 3;

			DraggableArea.Width = this.Width;
			DraggableArea.Height = this.Height;

			this.Update();

			#region LevelText
			var LevelTextBackground = new Rectangle
			{
				Fill = Brushes.LightGreen,
				Opacity = 0.2,
				Width = Width - Padding - Padding,
				Height = Height - PrimitiveTile.Heigth * 2 - Padding * 3,
			}.AttachTo(this).MoveTo(Padding, PrimitiveTile.Heigth * 2 + Padding + Padding);



			this.LevelText.AttachTo(this).SizeTo(
				Width - Padding * 2,
				Height - PrimitiveTile.Heigth * 2 - Padding * 3
			).MoveTo(Padding, PrimitiveTile.Heigth * 2 + Padding * 2);



			this.LevelText.GotFocus += delegate
			{
				LevelTextBackground.Opacity = 0.7;

				// now we should serialize 
			};

			LevelText.LostFocus += delegate { LevelTextBackground.Opacity = 0.2; };
			#endregion
		}

		public readonly TextBox LevelText;

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


			#region AddButton
			Action<Image, View.SelectorInfo[]> AddButton =
				(Image, EditorSelector) =>
				{
					var EditorSelectorDefault = EditorSelector.FirstOrDefault();

					//var EditorSelectorCycle = EditorSelector.AsCyclicEnumerable().GetEnumerator();

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

							if (EditorSelector.Length == 0)
							{
								SelectionMarker.Fill = Brushes.Red;
								this.EditorSelectorNextSize = null;
								this.EditorSelectorPreviousSize = null;
								this.EditorSelector = null;
								return;
							}

							SelectionMarker.Fill = Brushes.LightGreen;
							this.EditorSelectorNextSize =
								delegate
								{
									EditorSelectorDefault = EditorSelector.Next(k => k == EditorSelectorDefault);
									this.EditorSelector = EditorSelectorDefault;
								};

							this.EditorSelectorPreviousSize =
								delegate
								{
									EditorSelectorDefault = EditorSelector.Previous(k => k == EditorSelectorDefault);
									this.EditorSelector = EditorSelectorDefault;
								};

							this.EditorSelector = EditorSelectorDefault;
						};

					TouchOverlay.MouseLeftButtonUp +=
						delegate
						{
							Select();
						};



					if (Buttons.Count == 0)
					{
						Select();


						// first button shall be default
						this.LevelText.GotFocus +=
							delegate
							{
								Select();
							};
					}

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
			var Arrow =
				new Image
				{
					Source = (Assets.Shared.KnownAssets.Path.Assets + "/Aero_Arrow.png").ToSource(),
					Width = 32,
					Height = 32
				}.MoveTo(ButtonsWidth() + PrimitiveTile.Width - 16, Padding + PrimitiveTile.Heigth - 16);

			AddButton(Arrow,
					new[] { new View.SelectorInfo() }
				);

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/stone1_2x2.png",
				Editor.Tiles.StoneSelector.Sizes
			);

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/cave0_2x2.png",
				Editor.Tiles.CaveSelector.Sizes
			);

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/platform0_2x2.png",
				Editor.Tiles.PlatformSelector.Sizes
			);

			AddButton_2x2(Assets.Shared.KnownAssets.Path.Tiles + "/ridge0_2x2.png",
				Editor.Tiles.RidgeSelector.Sizes
			);


			AddButton_1x1(Assets.Shared.KnownAssets.Path.Tiles + "/bridge0.png",
				Editor.Tiles.BridgeSelector.Sizes
			);

			AddButton_1x1(Assets.Shared.KnownAssets.Path.Tiles + "/fence0.png",
				Editor.Tiles.FenceSelector.Sizes
			);


			AddButton_2x2(Assets.Shared.KnownAssets.Path.Sprites + "/tree0_2x2.png",
				Editor.Sprites.TreeSelector.Sizes
			);

			AddButton_1x1(Assets.Shared.KnownAssets.Path.Sprites + "/sign0.png",
				Editor.Sprites.SignSelector.Sizes
			);


			AddButton_1x1(
				Editor.Sprites.GoldSelector.ToolbarImage.ToString(),
				Editor.Sprites.GoldSelector.Sizes
			);

			AddButton_1x1(
				Editor.Sprites.RockSelector.ToolbarImage.ToString(),
				Editor.Sprites.RockSelector.Sizes
			);

			AddButton_2x2(
				Editor.Sprites.VehicleSelector.ToolbarImage.ToString(),
				Editor.Sprites.VehicleSelector.Sizes
			);


			var Demolish =
				new Image
				{
					Source = (Assets.Shared.KnownAssets.Path.Assets + "/btn_demolish.png").ToSource(),
					Width = 20,
					Height = 20
				}.MoveTo(ButtonsWidth() + PrimitiveTile.Width - 10, Padding + PrimitiveTile.Heigth - 10);

			AddButton(Demolish, Editor.DemolishSelector.Sizes);


			var WaterLevel =
				new Image
				{
					Source = (Assets.Shared.KnownAssets.Path.Assets + "/btn_river.png").ToSource(),
					Width = 20,
					Height = 20
				}.MoveTo(ButtonsWidth() + PrimitiveTile.Width - 10, Padding + PrimitiveTile.Heigth - 10);

			AddButton(WaterLevel, Editor.WaterLevelSelector.Sizes);


		}


	}
}
