using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class RouteWindow : Window
	{
		public RouteWindow()
		{

			this.ClientWidth = 200;
			this.ClientHeight = PrimitiveTile.Heigth * 2 + 10;

			var i = new NameFormat
			{
				Path = Assets.Shared.KnownAssets.Path.Sprites,
				Name = "sign",
				Index = 0,
				Extension = "png",
				Zoom = 2
			};

			var Button1 = new Window.Button(
				i.ToImage(), PrimitiveTile.Width * 2, PrimitiveTile.Heigth * 2
			);

			Button1.Update();

			Button1.Text = "";
			Button1.AttachContainerTo(this.OverlayContainer);


		}
	}
}
