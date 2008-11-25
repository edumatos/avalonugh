using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.IO;
using AvalonUgh.Code.Editor.Tiles;

namespace AvalonUgh.Code
{
	partial class Level
	{


		public override string ToString()
		{
			using (var s = new StringWriter())
			{
				var w = this.Map.Width;

				s.WriteLine("# generated map");
				s.WriteLine(new String('#', w));

				var Map = Enumerable.Range(0, this.Map.Height).ToArray(i => new string(' ', w));

				#region WriteTiles
				Action<Tile[]> WriteTiles =
					tiles =>
					{
						foreach (var k in tiles)
						{
							var p = k.Position;


							for (int y = 0; y < k.Selector.PrimitiveTileCountY; y++)
							{
								var old = Map[p.TileY + y];

								if (y == 0)
								{
									Map[p.TileY] =
										old.Substring(0, p.TileX) + k.GetIdentifier() +
										new string(ASCIITileSizeInfo.HorizontalLine[0], k.Selector.PrimitiveTileCountX - 1) +
										old.Substring(p.TileX + k.Selector.PrimitiveTileCountX);
								}
								else
								{
									Map[p.TileY + y] =
										old.Substring(0, p.TileX) + ASCIITileSizeInfo.VerticalLine +
										old.Substring(p.TileX + 1);
								}
							}
						}
					};
				#endregion



				WriteTiles(this.KnownStones.ToArray());
				WriteTiles(this.KnownCaves.ToArray());
				WriteTiles(this.KnownRidges.ToArray());
				WriteTiles(this.KnownFences.ToArray());
				WriteTiles(this.KnownPlatforms.ToArray());


				foreach (var x in Map)
					s.WriteLine(x);

				s.WriteLine(new String('#', w));

				return s.ToString();
			}
		}
	}
}
