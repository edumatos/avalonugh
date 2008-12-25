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

		public readonly KnownSelectors Selectors;

		public EditorToolbar(Canvas DragContainer, KnownSelectors Selectors)
		{
			this.Selectors = Selectors;
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
				FontSize = 12,
				AcceptsReturn = true,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0)
			};

			var Buttons = new List<Button>();

			Func<int> ButtonsWidth = () => Padding + Convert.ToInt32(Buttons.Count / 2) * (PrimitiveTile.Width * 2 + Padding);

			CreateButtons(Buttons, ButtonsWidth);

			this.Width = ButtonsWidth() + (PrimitiveTile.Width * 2 + Padding);
			this.Height = this.Width * 2 / 3;

			DraggableArea.Width = this.Width;
			DraggableArea.Height = this.Height;

			this.Update();

			#region LevelText


			var LevelTextBackground = new Rectangle
			{
				Fill = Brushes.LightGreen,
				Opacity = 0.2
			}.AttachTo(this)
			.SizeTo(
				Width - Padding * 2,
				Height - PrimitiveTile.Heigth * 4 - Padding * 4
			).MoveTo(Padding, PrimitiveTile.Heigth * 4 + Padding * 3);

			var WaterMark = new TextBox
			{
				FontFamily = new FontFamily("Courier New"),
				FontSize = 12,
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0),
				Text = "Avalon Ugh",
				Foreground = Brushes.DarkGreen,
				TextAlignment = TextAlignment.Right
			}.AttachTo(this).SizeTo(
				Width - Padding * 2,
				15
			).MoveTo(Padding, Height - 15 - Padding);

			this.LevelText.AttachTo(this)
			.SizeTo(
				Width - Padding * 2,
				Height - PrimitiveTile.Heigth * 4 - Padding * 4
			).MoveTo(Padding, PrimitiveTile.Heigth * 4 + Padding * 3);



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
			Action<SelectorBase> AddButton =
				(Selector) =>
				{
					var w = Selector.ImageWidth;

					if (w == 0)
						w = Selector.ToolbarImage.Width * PrimitiveTile.Width;

					var h = Selector.ImageHeight;

					if (h == 0)
						h = Selector.ToolbarImage.Height * PrimitiveTile.Heigth;

					var x = ButtonsWidth();
					var y = Padding;

					if (Buttons.Count % 2 == 1)
						y += PrimitiveTile.Heigth * 2 + Padding;

					var Image =
						new Image
						{
							Stretch = Stretch.Fill,
							Source = Selector.ToolbarImage.ToString().ToSource(),
							Width = w,
							Height = h
						}.MoveTo(
							x + PrimitiveTile.Width - w / 2,
							y + PrimitiveTile.Heigth - h / 2
						);


					var Sizes = Selector.Sizes;

					var EditorSelectorDefault = Sizes.FirstOrDefault();


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

							if (Sizes.Length == 0)
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
									EditorSelectorDefault = Sizes.Next(k => k == EditorSelectorDefault);
									this.EditorSelector = EditorSelectorDefault;
								};

							this.EditorSelectorPreviousSize =
								delegate
								{
									EditorSelectorDefault = Sizes.Previous(k => k == EditorSelectorDefault);
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

			#endregion


			foreach (var Selector in this.Selectors.Types)
			{

				AddButton(Selector);


			}
		}




	}
}
