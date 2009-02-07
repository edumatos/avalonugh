using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Editor.Sprites;
using System.ComponentModel;
using System.Windows.Media;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code
{
	partial class Actor
	{
		[Script]
		public class Bubble : ISupportsContainer
		{
			public Canvas Container { get; set; }

			public readonly int CaveSign;


			public readonly int OffsetX;
			public readonly int OffsetY;

			public Bubble(int Zoom)
			{
				this.OffsetX = 2 * Zoom;
				this.OffsetY = -22 * Zoom;

				this.Container = new Canvas
				{
					Width = 16 * Zoom,
					Height = 24 * Zoom
				};

				new NameFormat
				{
					Path = KnownAssets.Path.Sprites,
					Name = "bubble_question",
					Index = 0,

					Extension = "png",
					Zoom = Zoom
				}.ToImage(16, 24).AttachTo(this);

			}
		}

		public readonly BindingList<Bubble> KnownBubbles = new BindingList<Bubble>();

	}
}
