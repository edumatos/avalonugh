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
			string[] SupportedBackgrounds;

			public Size_Vertical()
			{
				this.Width = 0;
				this.Height = 0;

				this.SupportedBackgrounds =
					Enumerable.Range(1, 5).Select(k => k.ToString().PadLeft(3, '0')).ConcatSingle("").ToArray();

			}

			public override void CreateTo(LevelType Level, View.SelectorPosition Position)
			{
				if (Level.AttributeBackground.Value == null)
					Level.AttributeBackground.Value = "";

				Level.AttributeBackground.Value =
					this.SupportedBackgrounds.Next(k => k == Level.AttributeBackground.Value);

			}

		}
	}
	
}
