using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Assets.Shared
{
	[Script]
	public class PrimitiveFont
	{
		public const int Width = 15;
		public const int Heigth = 16;

		public static string ToFileName(string s)
		{
			if (s == "#")
				return "_Hash";

			if (s == "?")
				return "_Question";

			if (s == ":")
				return "_Colon";

			if (s == ".")
				return "_Dot";

			if (s == ",")
				return "_Comma";

			if (s == "'")
				return "_Apostrophe";

			return s.ToLower();
		}
	}
}
