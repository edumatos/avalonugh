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
		public class Port
		{
			public int PortIdentity;

			public Level Level;
			public View View;

			public Canvas Container;

			public int Left;
			public int Top;
			public int Width;
			public int Height;

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

							this.View = new View(Width, Height, this.Level);
							this.View.Show(this.InternalVisible);
							this.View.MoveContainerTo(this.Left, this.Top).AttachContainerTo(this.Container);

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
		}
	}
}
