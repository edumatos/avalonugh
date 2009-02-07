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
		public class Bubble
		{
			public readonly int CaveSign;

			public readonly Image Image;


			public Bubble(int Zoom)
			{
				this.Image = new NameFormat
				{
					Path = KnownAssets.Path.Sprites,
					Name = "bubble_question",
					Index = 0,

					Extension = "png",
					Zoom = Zoom
				}.ToImage(16, 24);
			}
		}

		public readonly BindingList<Bubble> KnownBubbles = new BindingList<Bubble>();

	}
}
