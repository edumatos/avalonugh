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

								var ButtonsToBeDeleted = Buttons.Select((k, j) => new { k, j }).Where(k => k.j > i).Select(k => k.k).ToArray();

								foreach (var v in ButtonsToBeDeleted)
								{
									Buttons.Remove(v);
								}

								for (int j = i; j < InternalCurrentRoute.Elements.Length; j++)
								{
									InternalCurrentRoute.Elements[j] = 0;
								}
							}
							else
							{
								if (Buttons.Count < 10)
									if (Buttons.Last() == NewButton)
									{
										new DestinationButton { SignValue = -1 }.AddTo(Buttons);
									}

								this.InternalCurrentRoute.Elements[Buttons.IndexOf(NewButton)] = (uint)(NewButton.SignValue + 1);
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

			//this.CurrentRoute = (2 << (3 * 2)) + (3 << (3 * 1)) + 4;


		}

		PackedInt32 InternalCurrentRoute = new PackedInt32(3);

		public PackedInt32 CurrentRoute
		{
			get
			{
				return InternalCurrentRoute;
			}
			set
			{
				InternalCurrentRoute = value;

				this.Buttons.RemoveAll();

				InternalCurrentRoute.Elements.ToFlaggable().ForEach(
					k =>
					{
						new DestinationButton { SignValue = (int)k.Current - 1 }.AddTo(Buttons);

						if (k.Current == 0)
						{
							k.Stream.SkipElements = true;
						}
					}
				);
				


			}
		}
	}
}
