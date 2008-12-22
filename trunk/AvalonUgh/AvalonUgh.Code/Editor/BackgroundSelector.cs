using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class BackgroundSelector : SelectorBase
	{
		public BackgroundSelector()
		{
			this.ImageWidth = 32;
			this.ImageHeight = 20;

			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "btn_background",
					Index = -1,
					Extension = "png"
				};

			this.Sizes =
				new View.SelectorInfo[]
				{
					new Size_Vertical()
				};


		}

		[Script]
		internal class Size_Vertical : View.SelectorInfo
		{
			public Size_Vertical()
			{
				// we are setting a width that should be greater than the view width
				// we might add a support for -1 to expand to to view later
				this.Width = 0;

				this.Height = 0;
			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				
			}

		}
	}
	
}
