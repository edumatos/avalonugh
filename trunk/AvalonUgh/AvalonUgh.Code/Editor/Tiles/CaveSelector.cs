using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class CaveSelector
	{
		public const string Identifier = "C";

		public static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				//new Size_1x1()
				new Size_2x2()
				//new Size_4x2(),
				//new Size_2x4(),
				//new Size_2x3(),
				//new Size_2x1(),
			};


		[Script]
		public class Size_2x2 : TileSelector
		{

			public Size_2x2()
			{
				PrimitiveTileCountX = 2;
				PrimitiveTileCountY = 2;

				var Caves = new[]
				{
					(Assets.Shared.KnownAssets.Path.Tiles + "/cave0_2x2.png"),
					(Assets.Shared.KnownAssets.Path.Tiles + "/cave1_2x2.png"),
				};

				var CavesCyclic = Caves.AsCyclicEnumerable().GetEnumerator();

				Invoke =
					(View,  Position) =>
					{
						// add a new fence tile

						new Image
						{
							Source = CavesCyclic.Take().ToSource(),
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
