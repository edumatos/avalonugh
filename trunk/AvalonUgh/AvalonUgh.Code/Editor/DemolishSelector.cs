using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.Editor
{
	[Script]
	public class DemolishSelector : SelectorBase
	{
		public DemolishSelector()
		{
			this.ImageWidth = 20;
			this.ImageHeight = 20;

			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Assets,
					Name = "btn_demolish",
					Index = -1,
					Extension = "png"
				};

			this.Sizes =
				new View.SelectorInfo[]
				{
					new Size_Generic(1, 1),
					new Size_Generic(2, 2),
					new Size_Generic(4, 4),
					new Size_Generic(8, 8),
				
				};
		}




		[Script]
		internal class Size_Generic : Tiles.TileSelector
		{
			public Size_Generic(int x, int y) : base(x, y) { }

			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{

				var z = Level.Zoom;
				var x = Position.ContentX * z;
				var y = Position.ContentY * z;

				var o = new Obstacle
				{
					Left = x,
					Top = y,
					Right = x + this.Width * z,
					Bottom = y + this.Height * z
				};

				// we will remove the first object we 
				var a = Level.GetRemovableEntities().Where(k => k.Obstacle.Intersects(o)).ToArray();

				if (a.Any())
				{
					a.ForEach(k => k.Dispose());

					return;
				}

				Level.GetRemovablePlatforms().Where(k => k.Obstacle.Intersects(o)).ToArray().ForEach(k => k.Dispose());


			}

		}




	}
}
