using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class FenceSelector 
	{

		public static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_1x1()
				//new Size_2x2(),
				//new Size_4x2(),
				//new Size_2x4(),
				//new Size_2x3(),
				//new Size_2x1(),
			};


		[Script]
		public class Size_1x1 : TileSelector
		{

			public Size_1x1() 
			{

				Invoke =
					(View,  Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = (Assets.Shared.KnownAssets.Path.Tiles + "/fence0.png").ToSource(),
							Stretch = System.Windows.Media.Stretch.Fill,
							Width = this.Width * View.Level.Zoom,
							Height = this.Height * View.Level.Zoom,
						}.AttachTo(View.Platforms).MoveTo(
							Position.ContentX * View.Level.Zoom,
							Position.ContentY * View.Level.Zoom
						);
					};
			}

		}
	}
}
