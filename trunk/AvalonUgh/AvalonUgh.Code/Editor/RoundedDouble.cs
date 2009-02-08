using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class RoundedDouble
	{
		public double DoubleToIntegerScale = 100.0;

		public int StoredValue;

		public double Value
		{
			get
			{
				return StoredValue / DoubleToIntegerScale;
			}
			set
			{
				StoredValue = (int)Math.Round(value * DoubleToIntegerScale);
			}
		}
	}
}
