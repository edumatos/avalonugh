using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
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
			public readonly Image Image;
			public readonly Rectangle TouchOverlay;

			public event Action Click;

			public void RaiseClick()
			{
				if (Click != null)
					Click();
			}

			public SelectorBase SelectorType;

			public Button(Image Image)
			{
				this.Image = Image;

				this.TouchOverlay = new Rectangle
				{
					Width = PrimitiveTile.Width * 2,
					Height = PrimitiveTile.Heigth * 2,
					Fill = Brushes.Red,
					Opacity = 0,
					Cursor = Cursors.Hand
				};

				this.TouchOverlay.MouseEnter +=
					delegate
					{
						this.Image.Opacity = 0.6;
					};

				this.TouchOverlay.MouseLeave +=
					delegate
					{
						this.Image.Opacity = 1;
					};

				this.TouchOverlay.MouseLeftButtonUp +=
					delegate
					{
						if (this.Click != null)
							this.Click();
					};
			}
			public void MoveTo(int x, int y)
			{
				Image.MoveTo(x + PrimitiveTile.Width - Image.Width / 2, y + PrimitiveTile.Heigth - Image.Height / 2);
				TouchOverlay.MoveTo(x, y);
			}


		}

		public readonly KnownSelectors Selectors;

		public readonly BindingList<Button> Buttons = new BindingList<Button>();

		public event Action LoadClicked;
		public event Action SaveClicked;

		internal SelectorBase InternalSelectorType;
		public SelectorBase SelectorType
		{
			get
			{
				return InternalSelectorType;
			}
			set
			{
				this.Buttons.FirstOrDefault(k => k.SelectorType == value).Apply(k => k.RaiseClick());
			}
		}

		int ButtonsWidth
		{
			get
			{
				return
					Padding + (Buttons.Count / 2) * (PrimitiveTile.Width * 2 + Padding);
			}
		}

		public EditorToolbar(KnownSelectors Selectors)
		{
			this.Selectors = Selectors;
			this.Padding = 8;

			InitializeAnimatedOpacity();



			// how to turn off arrow key tabbing? 
			// http://www.eggheadcafe.com/software/aspnet/29317268/how-to-turn-off-arrow-key.aspx

			//var Navbar = new AeroNavigationBar();

			//Navbar.AttachContainerTo(this).MoveContainerTo(Padding, Padding);






			// non-selectors: load save

			Buttons.ForEachNewOrExistingItem(
				(value, index) =>
				{
					var x = Padding + (index / 2) * (PrimitiveTile.Width * 2 + Padding);
					var y = Padding;

					if (index % 2 == 1)
						y += PrimitiveTile.Heigth * 2 + Padding;

					value.MoveTo(
						x, y
					);

					value.Image.AttachTo(this);
					value.TouchOverlay.AttachTo(this);
				}
			);

			var ButtonLoad =
				new Button(
					new Image
					{
						Width = 16,
						Height = 16,
						Stretch = Stretch.Fill,
						Source = new NameFormat
						{
							Path = Assets.Shared.KnownAssets.Path.Assets,
							Index = -1,
							Name = "btn_load",
							Extension = "png"
						}
					}
				);


			ButtonLoad.Click +=
				delegate
				{

					if (LoadClicked != null)
						LoadClicked();
				};

			ButtonLoad.AddTo(Buttons);

			var ButtonSave = new Button(
				new Image
				{
					Width = 16,
					Height = 16,
					Stretch = Stretch.Fill,
					Source = new NameFormat
					{
						Path = Assets.Shared.KnownAssets.Path.Assets,
						Index = -1,
						Name = "btn_save",
						Extension = "png"
					}
				}
			);

			ButtonSave.Click += RaiseSaveClicked;



			ButtonSave.AddTo(Buttons);

			CreateButtons();

			this.ClientWidth = ButtonsWidth + (PrimitiveTile.Width * 2);
			this.ClientHeight = PrimitiveTile.Heigth * 2 * 2 + Padding;



			this.Update();






		}

		private void RaiseSaveClicked()
		{
			if (SaveClicked != null)
				SaveClicked();
		}

		private void InitializeAnimatedOpacity()
		{
			var AnimatedOpacity = this.BackgroundContainer.ToAnimatedOpacity();
			var DelayedMouseEvents = this.Container.ToDelayedMouseEvents();

			AnimatedOpacity.Opacity = 0.4;

			DelayedMouseEvents.MouseEnter += () => AnimatedOpacity.Opacity = 0.9;
			DelayedMouseEvents.MouseLeave += () => AnimatedOpacity.Opacity = 0.4;
		}

		//public readonly TextBox LevelText;

		private void CreateButtons()
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

					var x = ButtonsWidth;
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
						};


					var Sizes = Selector.Sizes;
					var EditorSelectorDefault = Sizes.FirstOrDefault();




					Action Select =
						delegate
						{
							this.InternalSelectorType = Selector;

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

					var btn = new Button(Image)
					{
						SelectorType = Selector
					};

					btn.Click += Select;




					if (Selector == Selectors.Arrow)
					{
						Select();



					}

					btn.AddTo(Buttons);
				};

			#endregion


			foreach (var Selector in this.Selectors.Types)
			{

				AddButton(Selector);


			}
		}




		public event Action VisibilityChanged;

		public void ToggleVisibility()
		{
			this.Container.ToggleVisible();

			if (VisibilityChanged != null)
				VisibilityChanged();
		}
	}
}
