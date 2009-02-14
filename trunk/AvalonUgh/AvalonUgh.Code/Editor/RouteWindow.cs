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
using System.Windows.Shapes;
using System.Windows.Media;
using AvalonUgh.Code.Editor.Tiles;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class RouteWindow : Window
	{
		[Script]
		public class DestinationButton : Window.Button
		{
			int InternalSignValue = -1;
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

			public event Action PlatformIndexChanged;
			int InternalPlatformIndex;
			public int PlatformIndex
			{
				get
				{
					return InternalPlatformIndex;
				}
				set
				{
					InternalPlatformIndex = value;
					if (PlatformIndexChanged != null)
						PlatformIndexChanged();
				}
			}

			public DestinationButton()
				: base(null, PrimitiveTile.Width * 2, PrimitiveTile.Heigth * 2)
			{
				this.Text = "";


			}
		}

		public readonly BindingList<DestinationButton> Buttons = new BindingList<DestinationButton>();

		public Level CurrentLevel;

		public RouteWindow()
		{

			this.ClientWidth = 200;
			this.ClientHeight = PrimitiveTile.Heigth * 2 + 10;

			var AddStopButton = new Window.Button(null, PrimitiveTile.Width * 2, PrimitiveTile.Heigth * 2)
			{
				Text = ""
			};

			AddStopButton.AttachContainerTo(this.OverlayContainer);
			AddStopButton.Click +=
				delegate
				{
					if (Buttons.Count < this.CurrentRoute.Elements.Length)
						Buttons.Add(
							new DestinationButton { SignValue = 1 }
						);
				};

			Buttons.WithEvents(
				NewButton =>
				{
					var SelectedCaves = new BindingList<Cave>();

					SelectedCaves.WithEvents(
						TargetCave =>
						{
							var TargetCaveRect = CurrentLevel.AddToContentInfoColoredShapes(
								TargetCave.ToObstacle(), Brushes.Yellow
							);

							return delegate
							{
								this.CurrentLevel.ContentInfoColoredShapes.Remove(TargetCaveRect);
							};
						}
					);

					NewButton.MouseEnter +=
						delegate
						{
							SelectedCaves.Add(
								this.CurrentLevel.ToPlatformSnapshots().AtModulus(NewButton.PlatformIndex).Cave
							);

						};

					NewButton.MouseLeave +=
						delegate
						{
							SelectedCaves.RemoveAll();
						};

					NewButton.PlatformIndexChanged +=
						delegate
						{
							var p = this.CurrentLevel.ToPlatformSnapshots().AtModulus(NewButton.PlatformIndex);

							NewButton.SignValue = p.CaveSigns.First().Value;
						};

					// add number increment rule
					NewButton.Click +=
						delegate
						{
							NewButton.PlatformIndex = (NewButton.PlatformIndex + 1) % this.CurrentLevel.ToPlatformSnapshots().Count();

							var p = this.CurrentLevel.ToPlatformSnapshots().AtModulus(NewButton.PlatformIndex);

							SelectedCaves.RemoveAll();
							SelectedCaves.Add(p.Cave);

							var i = Buttons.IndexOf(NewButton);

							this.CurrentRoute.Elements[i] = (uint)(NewButton.PlatformIndex + 1);
						};

					NewButton.AttachContainerTo(this.OverlayContainer);

					NewButton.MoveContainerTo(
						Buttons.IndexOf(NewButton) * NewButton.Width, 0
					);


					this.ClientWidth = (Buttons.Count + 1) * NewButton.Width + Padding;
					AddStopButton.MoveContainerTo((Buttons.Count) * NewButton.Width + Padding, 0);

					#region RemoveNewButton
					var RemoveNewButton = new Window.Button
						{
							BackgroundColor = Colors.Red,
							ClientWidth = 6,
							ClientHeight = 6,
							Text = ""
						}.AttachContainerTo(this.OverlayContainer);

					RemoveNewButton.MoveContainerTo(
						Buttons.IndexOf(NewButton) * NewButton.Width + NewButton.Width - RemoveNewButton.Width, 0
					);

					RemoveNewButton.Click +=
						delegate
						{
							// remove all following buttons
							var i = Buttons.IndexOf(NewButton);

							var ButtonsToBeDeleted = Buttons.Select((k, j) => new { k, j }).Where(k => k.j >= i).Select(k => k.k).ToArray();

							foreach (var v in ButtonsToBeDeleted)
							{
								Buttons.Remove(v);
							}

							for (int j = i; j < InternalCurrentRoute.Elements.Length; j++)
							{
								InternalCurrentRoute.Elements[j] = 0;
							}
						};

					#endregion

					return delegate
					{
						RemoveNewButton.OrphanizeContainer();
						NewButton.OrphanizeContainer();

						this.ClientWidth = (Buttons.Count + 1) * NewButton.Width + Padding;
						AddStopButton.MoveContainerTo((Buttons.Count) * NewButton.Width + Padding, 0);
					};
				}
			);

			//this.CurrentRoute = (2 << (3 * 2)) + (3 << (3 * 1)) + 4;


		}



		PackedInt32 InternalCurrentRoute = new PackedInt32(4);

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
						var PlatformIndex = (int)k.Current - 1;


						if (k.Current == 0)
						{
							k.Stream.SkipElements = true;
						}
						else
						{
							new DestinationButton().AddTo(Buttons).PlatformIndex = PlatformIndex;
						}
					}
				);



			}
		}
	}
}
