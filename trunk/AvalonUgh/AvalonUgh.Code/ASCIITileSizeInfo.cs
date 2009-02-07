using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Code.Editor.Tiles;

namespace AvalonUgh.Code
{
	[Script]
	public class ASCIITileSizeInfo
	{
		public readonly string Value;

		public readonly int Width = 1;
		public readonly int Height = 1;


		public Tile.VariationElement Variation;

		/* X33
		 * 444
		 * 444
		 * 444
		 * 
		 * Value = X
		 * Width = 3
		 * Height = 4
		 */

		//public readonly bool IsNumericSize;

		public const string HorizontalLine = "-";
		public const string VerticalLine = "|";

		public ASCIITileSizeInfo(Func<int, int, string> Select)
		{
			this.Value = Select(0, 0);

			var x = Select(Width, 0);
			while (x == HorizontalLine)
			{
				Width++;
				x = Select(Width, 0);
			}

			var y = Select(0, Height);
			while (y == VerticalLine)
			{
				Height++;
				y = Select(0, Height);
			}

			if (Width > 1)
				if (Height > 1)
				{
					var Variation = Select(1, 1);

					if (char.IsNumber(Variation, 0))
						this.Variation = new Tile.VariationElement { Value = int.Parse(Variation) };

				}

		}
	}


}
