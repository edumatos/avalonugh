using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;
using System.ComponentModel;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class RouteWindow : Window
	{
		[Script]
		public class DestinationButton : Window.Button
		{
			int InternalSignValue = -2;
			public int SignValue
			{
				get
				{
					return InternalSignValue;
				}
				set
				{
					if (InternalSignValue == value)
						return;

					InternalSignValue = value;

					if (value < 0)
					{
						this.ButtonImage = null;
					}
					else if (value > 5)
					{
						this.ButtonImage = null;
					}
					else
					{
						var i = new NameFormat
						{
							Path = Assets.Shared.KnownAssets.Path.Sprites,
							Name = "sign",
							Index = value,
							Extension = "png",
							Zoom = 2
						};

						this.ButtonImage = i.ToImage();
					}

				}
			}
			public DestinationButton()
				: base(null, PrimitiveTile.Width * 2, PrimitiveTile.Heigth * 2)
			{
				this.Text = "";
			}
		}

		public readonly BindingList<DestinationButton> Buttons = new BindingList<DestinationButton>();

		public RouteWindow()
		{

			this.ClientWidth = 200;
			this.ClientHeight = PrimitiveTile.Heigth * 2 + 10;

			Buttons.WithEvents(
				NewButton =>
				{
					// add number increment rule
					NewButton.Click +=
						delegate
						{
							NewButton.SignValue++;

							if (NewButton.SignValue == 6)
							{
								NewButton.SignValue = -1;

								// remove all following buttons
								var i = Buttons.IndexOf(NewButton);

								foreach (var v in Buttons.Where((k, j) => j > i).ToArray())
								{
									Buttons.Remove(v);
								}
							}
							else
							{
								if (Buttons.Last() == NewButton)
								{
									new DestinationButton { SignValue = -1 }.AddTo(Buttons);
								}
							}
						};

					NewButton.AttachContainerTo(this.OverlayContainer);

					NewButton.MoveContainerTo(
						Buttons.IndexOf(NewButton) * NewButton.Width, 0
					);


					this.ClientWidth = Buttons.Count * NewButton.Width;

					return delegate
					{
						NewButton.OrphanizeContainer();
						this.ClientWidth = Buttons.Count * NewButton.Width;
					};
				}
			);

			new DestinationButton { SignValue = 0 }.AddTo(Buttons);
			new DestinationButton { SignValue = -1 }.AddTo(Buttons);




		}
	}
}
