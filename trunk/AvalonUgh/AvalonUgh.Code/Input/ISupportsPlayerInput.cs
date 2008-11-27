using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code.Input
{
	[Script]
	public interface ISupportsPlayerInput
	{
		void AddAcceleration(PlayerInput e);
	}
}
