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

namespace AvalonUgh.Code.Editor
{
	partial class Level
	{


		public override string ToString()
		{
			using (var s = new StringWriter())
			{
				var w = this.Map.Width;

				s.WriteLine("# generated map");

				Action<Attribute> WriteAttribute =
					a => s.WriteLine("# " + a);

				WriteAttribute.AsParamsAction()(
					AttributeText,
					AttributeCode,
					AttributeAutoscroll,
					AttributeWater,
					AttributeWaterRise,
					AttributeSnow,
					AttributeBackground
				);

				s.WriteLine(new string('#', w));

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
				WriteTiles(this.KnownRidgeTrees.ToArray());
				WriteTiles(this.KnownFences.ToArray());
				WriteTiles(this.KnownPlatforms.ToArray());
				WriteTiles(this.KnownBridges.ToArray());

				
				Map.ForEach(
					(row, index) =>
					{
						WriteAttribute.InvokeAsEnumerable(
							from dino in this.KnownDinos
							where dino.BaseY == index
							select new Attribute.Int32 { Key = "dino", Value = dino.UnscaledX }
						);


						WriteAttribute.InvokeAsEnumerable(
							from tree in  this.KnownTrees
							where tree.BaseY == index
							select new Attribute.Int32 { Key = "tree", Value = tree.UnscaledX }
						);

				

						WriteAttribute.InvokeAsEnumerable(
							from i in this.KnownGold
							where i.BaseY == index
							select new Attribute.Int32 { Key = "gold", Value = i.UnscaledX }
						);

						WriteAttribute.InvokeAsEnumerable(
							from i in this.KnownSigns
							where i.BaseY == index
							select new Attribute.Int32_Int32 { Key = "sign", Value0 = i.UnscaledX, Value1 = i.Value }
						);

					
						WriteAttribute.InvokeAsEnumerable(
							from i in this.KnownVehicles
							let StartPosition = i.StartPosition
							where StartPosition != null
							where StartPosition.BaseY == index
							select new Attribute.Int32 { Key = "vehicle", Value = StartPosition.UnscaledX }
						);

						WriteAttribute.InvokeAsEnumerable(
							from i in this.KnownTryoperus
							let StartPosition = i.StartPosition
							where StartPosition != null
							where StartPosition.BaseY == index
							select new Attribute.Int32 { Key = Sprites.Tryoperus.SpecificNameFormat.Alias, Value = StartPosition.UnscaledX }
						);

						WriteAttribute.InvokeAsEnumerable(
							from i in this.KnownRocks
							let StartPosition = i.StartPosition
							where StartPosition != null
							where StartPosition.BaseY == index
							select new Attribute.Int32 { Key = Sprites.Rock.SpecificNameFormat.Alias, Value = StartPosition.UnscaledX }
						);


						s.WriteLine(row);
					}
				);

				s.WriteLine(new string('#', w));

				return s.ToString();
			}
		}
	}
}
