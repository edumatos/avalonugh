﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code
{
	partial class Actor
	{
		[Script]
		public class man1 : Actor
		{
			public man1(int Zoom): base(Zoom)
			{


				Initialize();
			}
		}
	}
}
