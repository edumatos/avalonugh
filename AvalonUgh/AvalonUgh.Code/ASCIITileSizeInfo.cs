using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code
{
	[Script]
	public class ASCIITileSizeInfo
	{
		public readonly string Value;

		public readonly int Width = 1;
		public readonly int Height = 1;

		/* X33
		 * 444
		 * 444
		 * 444
		 * 
		 * Value = X
		 * Width = 3
		 * Height = 4
		 */

		public readonly bool IsNumericSize;

		public ASCIITileSizeInfo(Func<int, int, string> Select)
		{
			this.Value = Select(0, 0);

			var x = Select(1, 0);

			#region numeric width
			if (char.IsNumber(x, 0))
			{
				var Width = int.Parse(x);

				if (Width > 2)
				{
					for (int ix = 2; ix < Width; ix++)
					{
						if (Select(ix, 0) != x)
							Width = 1;
					}
				}

				if (Width > 1)
					IsNumericSize = true;

				this.Width = Width;
			}
			#endregion


			var y = Select(0, 1);

			#region numeric height
			if (char.IsNumber(y, 0))
			{
				var Height = int.Parse(y);

				if (Height > 2)
				{
					for (int iy = 2; iy < Height; iy++)
					{
						if (Select(0, iy) != y)
							Height = 1;
					}
				}


				if (Height > 1)
					IsNumericSize = true;
				this.Height = Height;
			}
			#endregion

			if (IsNumericSize)
				for (int ix = 1; ix < this.Width; ix++)
					for (int iy = 1; iy < this.Height; iy++)
					{
						if (!char.IsNumber(Select(ix, iy), 0))
						{
							this.Width = 1;
							this.Height = 1;
						}
					}
		}
	}


}
