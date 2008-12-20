using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public class CaveSelector
	{
		public const string Identifier = "C";

		public static readonly View.SelectorInfo[] Sizes =
			new View.SelectorInfo[]
			{
				new Size_Generic(2, 2, 2),
			};

		[Script]
		private class Size_Generic : TileSelector.Named
		{

			public Size_Generic(int x, int y, int variations)
				: base(x, y, variations, "cave")
			{
			
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				Name.Index = (Name.Index + 1) % Name.IndexCount;

				RemovePlatforms(this, Level, Position);

				var u = new Cave(Level, this)
				{
					Position = Position,
					Image = ToImage(Level, Position)
				};

				Level.KnownCaves.Add(u);
			}
		}

		public static void AttachToLevel(ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		{
			TileSelector.AttachToLevel(Sizes, Position, Tile, Level);
		}
	}
}
