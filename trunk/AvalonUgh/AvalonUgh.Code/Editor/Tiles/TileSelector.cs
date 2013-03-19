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
using System.Windows.Media;

namespace AvalonUgh.Code.Editor.Tiles
{
	[Script]
	public abstract class TileSelector : View.SelectorInfo
	{
		public TileSelector()
			: this(1, 1)
		{

		}

		[Script]
		public class Named : TileSelector
		{
			public readonly NameFormat Name;

			public Named(int x, int y, int variations, string Name) : base(x,y)
			{
				this.Name = new NameFormat
				{
					Name = Name,
					IndexCount = variations,
					Width = x,
					Height = y,
				};
			}

			public Image ToImage(LevelType Level, View.SelectorPosition Position)
			{
				var u = new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Tiles + "/" + Name.ToString() + ".png").ToSource(),
						Stretch = System.Windows.Media.Stretch.Fill,

					};

				u.WithZoom(Level.Zoom)
					.MoveTo(Position.ContentX, Position.ContentY)
					.SizeTo(this.Width, this.Height);

				return u;
			}
		}

		public TileSelector(int PrimitiveTileCountX, int PrimitiveTileCountY)
		{
			this.PrimitiveTileCountX = PrimitiveTileCountX;
			this.PrimitiveTileCountY = PrimitiveTileCountY;

			PercisionX = PrimitiveTile.Width;
			PercisionY = PrimitiveTile.Heigth;
		}


		[Script]
		public sealed class Composite : TileSelector
		{
			readonly Action<LevelType, View.SelectorPosition> CreateToHandler;

			public Composite(int x, int y, Action<LevelType, View.SelectorPosition> CreateToHandler) : base(x, y)
			{
				this.CreateToHandler = CreateToHandler;
			}

			public override void CreateTo(LevelType Level, View.SelectorPosition Position)
			{
				this.CreateToHandler(Level, Position);
			}
		}

		public static void RemovePlatforms(View.SelectorInfo Selector, LevelType Level, View.SelectorPosition Position)
		{
			var z = Level.Zoom;
			var x = Position.ContentX * z;
			var y = Position.ContentY * z;

			var o = new Obstacle
			{
				Left = x,
				Top = y,
				Right = x + Selector.Width * z,
				Bottom = y + Selector.Height * z
			};


			Level.GetRemovablePlatforms().Where(k => k.Obstacle.Intersects(o)).ToArray().ForEach(k => k.Dispose());
		}

		public static void RemoveEntities(View.SelectorInfo Selector, LevelType Level, View.SelectorPosition Position)
		{
			var z = Level.Zoom;
			var x = Position.ContentX * z;
			var y = Position.ContentY * z;

			var o = new Obstacle
			{
				Left = x,
				Top = y,
				Right = x + Selector.Width * z,
				Bottom = y + Selector.Height * z
			};


			Level.GetRemovableEntities().Where(k => k.Obstacle.Intersects(o)).ToArray().ForEach(k => k.Dispose());
		}

		//public static void AttachToLevel(View.SelectorInfo[] Sizes, ASCIIImage.Entry Position, ASCIITileSizeInfo Tile, Level Level)
		//{
			
		//    var Selector = Sizes.SingleOrDefault(
		//        k => k.Equals(Tile)
		//    );

		//    if (Selector == null)
		//    {
		//        Console.WriteLine(
		//            new { InvalidSize = new { Tile.Width, Tile.Height }, Tile.Value, Position.X, Position.Y }.ToString()
		//        );

		//        return;
		//    }

		//    Selector.CreateTo(Level,
		//        new View.SelectorPosition
		//        {
		//            ContentX = Position.X * PrimitiveTile.Width,
		//            ContentY = Position.Y * PrimitiveTile.Heigth,
		//        }
		//    );
		//}
	}
}
