using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code
{
	[Script]
	public class LocationTracker : ISupportsLocationChanged
	{
		ISupportsLocationChanged _Target;
		public ISupportsLocationChanged Target
		{
			get
			{
				return _Target;
			}
			set
			{
				if (_Target != null)
					_Target.LocationChanged -= _Target_LocationChanged;

				_Target = value;

				if (_Target != null)
				{
					_Target.LocationChanged += _Target_LocationChanged;

					_Target_LocationChanged();
				}
			}
		}

		void _Target_LocationChanged()
		{
			if (LocationChanged != null)
				LocationChanged();
		}


		public double X { get { return Target.X; } }
		public double Y { get { return Target.Y; } }
		public event Action LocationChanged;

	}

	[Script]
	public interface ISupportsLocationChanged
	{
		double X { get; }
		double Y { get; }
		event Action LocationChanged;

	}
}
