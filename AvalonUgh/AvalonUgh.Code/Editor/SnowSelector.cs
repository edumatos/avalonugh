using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class SnowSelector : SelectorBase
	{
		public SnowSelector()
		{
			this.ImageWidth = 20;
			this.ImageHeight = 20;

			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "btn_snow",
					Index = -1,
					Extension = "png"
				};

			this.Sizes =
				new View.SelectorInfo[]
				{
					new Size_Any()
				};


		}

		[Script]
		internal class Size_Any : View.SelectorInfo
		{


			public Size_Any()
			{
				this.Width = 0;
				this.Height = 0;


			}

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				if (Level.AttributeSnow.Value == 0)
				{
					Level.AttributeSnow.Value = 1;
					Level.AttributeWind.Value = -1;
				}
				else
				{
					if (Level.AttributeWind.Value == -1)
						Level.AttributeWind.Value = 1;
					else if (Level.AttributeWind.Value == 1)
						Level.AttributeWind.Value = 0;
					else
					{
						Level.AttributeSnow.Value = 0;
						Level.AttributeWind.Value = 0;
					}
				}

			}

		}
	}

}
