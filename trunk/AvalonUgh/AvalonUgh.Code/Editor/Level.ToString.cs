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
			return ToString(ToStringMode.ForSavedLevel);
		}

		public enum ToStringMode
		{
			ForSavedLevel,
			ForSync
		}

		public string ToString(ToStringMode Mode)
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
					AttributeWind,
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
							from tree in this.KnownTrees
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

						if (Mode == ToStringMode.ForSavedLevel)
						{
							// for saved levels we store only the start positions
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
						}


						s.WriteLine(row);
					}
				);

				s.WriteLine(new string('#', w));

				if (Mode == ToStringMode.ForSync)
				{
					foreach (var i in this.KnownRocks)
					{
						WriteAttribute(SerializeRock(i));
					}

					// vehicles can contain rocks, thus the ordering

					foreach (var i in this.KnownVehicles)
					{
						WriteAttribute(SerializeVehicle(i));
					}


				}

				return s.ToString();
			}
		}

		private Attribute.Int32_Array SerializeRock(AvalonUgh.Code.Editor.Sprites.Rock i)
		{
			var StartPosition = i.StartPosition;

			Attribute.Int32_Array a = "*rock";

			a[0] = i.X;
			a[1] = i.Y;
			a[2] = i.VelocityX;
			a[3] = i.VelocityY;

			// use the sync version of values
			i.X = a[0];
			i.Y = a[1];
			i.VelocityX = a[2];
			i.VelocityY = a[3];

			if (StartPosition != null)
			{
				// bool StartPosition
				a.Value[4] = 1;
				a[5] = StartPosition.X;
				a[6] = StartPosition.Y;

				StartPosition.X = a[5];
				StartPosition.Y = a[6];
			}


			return a;
		}

		private Attribute.Int32_Array SerializeVehicle(AvalonUgh.Code.Editor.Sprites.Vehicle i)
		{
			var StartPosition = i.StartPosition;

			Attribute.Int32_Array a = "*vehicle";

			a[0] = i.X;
			a[1] = i.Y;
			a[2] = i.VelocityX;
			a[3] = i.VelocityY;

			// use the sync version of values
			i.X = a[0];
			i.Y = a[1];
			i.VelocityX = a[2];
			i.VelocityY = a[3];

			if (StartPosition != null)
			{
				// bool StartPosition
				a.Value[4] = 1;
				a[5] = StartPosition.X;
				a[6] = StartPosition.Y;

				StartPosition.X = a[5];
				StartPosition.Y = a[6];
			}

			if (i.CurrentDriver != null)
			{
				a.Value[7] = 1;

				// who is the driver?
				a.Value[8] = i.CurrentDriver.PlayerInfo.Identity.NetworkNumber;
				a.Value[9] = i.CurrentDriver.PlayerInfo.IdentityLocal;
			}

			if (i.CurrentWeapon != null)
			{
				a.Value[10] = 1;
				a.Value[11] = this.KnownRocks.IndexOf(i.CurrentWeapon);

			}

			return a;
		}
	}
}
