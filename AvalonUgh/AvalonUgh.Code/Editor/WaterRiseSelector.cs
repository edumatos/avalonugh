using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.Editor
{
	[Script]
	public class WaterRiseSelector : SelectorBase
	{
		public WaterRiseSelector()
		{
			this.ImageWidth = 20;
			this.ImageHeight = 20;

			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "btn_waterrise",
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
				this.Width = 1000;

				this.Height = 4;
			}

			public override void CreateTo(LevelType Level, View.SelectorPosition Position)
			{
	

				// the view should listen to this event
				// and update our water animation at runtime
				Level.AttributeWaterRise.BooleanValue = !Level.AttributeWaterRise.BooleanValue;
			}

		}




	}
}
