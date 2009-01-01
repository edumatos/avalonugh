using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Editor;
using System.ComponentModel;
using AvalonUgh.Code.Dialogs;
using System.Windows.Input;
using ScriptCoreLib.Shared.Avalon.Tween;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using System.Windows.Shapes;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Code
{
	partial class Workspace
	{
		[Script]
		public class Port : ISupportsContainer
		{
			public readonly Window Window = new Window();

			public Canvas Container
			{
				get
				{
					return this.Window.Container;
				}
			}

			public int PortIdentity;

			public Level Level;
			public View View;

			public int StatusbarHeight;

			public int Padding { get { return this.Window.Padding; } set { this.Window.Padding = value; } }

			public int Width { get { return this.Window.ClientWidth; } set { this.Window.ClientWidth = value; } }
			public int Height { get { return this.Window.ClientHeight; } set { this.Window.ClientHeight = value; } }

			public int Zoom;

			public KnownSelectors Selectors;

			LevelReference InternalLevelReference;
			public LevelReference LevelReference
			{
				get
				{
					return InternalLevelReference;
				}
				set
				{
					if (Level != null)
						Level.Clear();

					if (View != null)
						View.OrphanizeContainer();


					Level = null;
					View = null;

					InternalLevelReference = value;

					Action<string> ApplyData =
						Data =>
						{
							if (Level != null)
								throw new Exception("InternalLevelReference");

							this.Level = new Level(Data, this.Zoom, this.Selectors);

							this.View = new View(Width, Height - StatusbarHeight, this.Level);
							this.View.Show(this.InternalVisible);
							this.View.AttachContainerTo(this.Window.ContentContainer);

							// we are doing some advanced layering now
							var TouchContainerForViewContent = new Canvas
							{
								// we need to update this if the level changes
								// in size
								Width = View.ContentExtendedWidth,
								Height = View.ContentExtendedHeight
							}.AttachTo(this.Window.OverlayContainer);

							View.ContentExtendedContainerMoved +=
								(x, y) => TouchContainerForViewContent.MoveTo(x, y);

							// raise that event so we stay in sync
							View.MoveContentTo();
							View.TouchOverlay.Orphanize().AttachTo(TouchContainerForViewContent);

							

							if (this.Loaded != null)
								this.Loaded();

						};

					if (value.Data == null)
					{
						InternalLevelReference.Location.Embedded.ToString().ToStringAsset(
							ApplyData
						);
					}
					else
					{
						ApplyData(value.Data);
					}
				}
			}

			bool InternalVisible = true;
			public bool Visible
			{
				get
				{
					return InternalVisible;
				}
				set
				{
					InternalVisible = value;

					if (this.View != null)
						this.View.Show(value);
				}
			}

			public event Action Loaded;

			public void WhenLoaded(Action e)
			{
				Loaded += e;

				if (Level != null)
				{
					e();
				}
			}
		}
	}
}
