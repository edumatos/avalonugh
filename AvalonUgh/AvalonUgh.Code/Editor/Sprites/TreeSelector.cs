using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class TreeSelector
	{
		public readonly Level.Attribute.Int32 Attribute = "tree";
		public readonly Level Level;

		public TreeSelector(Level Level)
		{
			// we need to know the context in which our attribute
			// is called
			this.Level = Level;

			// if the level is loading
			// this attribute will be assigned
			// each time the tree command is found
			Attribute.Assigned +=
				x_ =>
				{
					var x = x_ * Level.Zoom;
					var y = Level.TileRowsProcessed * PrimitiveTile.Heigth * Level.Zoom;

					// at this time we do have the level reference
					// but we do not have the view reference
					// this should enable us to change levels
					// in the same view
					new Tree(Level.Zoom)
					{

					}.AddTo(Level.KnownTrees).MoveBaseTo(x, y);
				};
		}

		// clicking on the toolbar will shuffle between those sizes
		// also while loading tiles the map will tell us which size to use
		public View.SelectorInfo[] Sizes =
			new[]
			{
				new Size_2x2()
			};

		[Script]
		public class Size_2x2 : SpriteSelector
		{
			public Size_2x2()
			{
				Width = PrimitiveTile.Width * 2;
				Height = PrimitiveTile.Heigth * 2;
				PercisionX = PrimitiveTile.Width / 2;
				PercisionY = PrimitiveTile.Heigth;

				Invoke =
					(View,  Position) =>
					{
						RemoveEntities(this, View.Level, Position);

						new Tree(View.Level.Zoom)
						{
							Selector = this
						}.AttachContainerTo(View.Entities).AddTo(View.Level.KnownTrees).MoveTo(
							(Position.ContentX + this.HalfWidth) * View.Level.Zoom,
							(Position.ContentY + this.HalfHeight) * View.Level.Zoom
						);
					};
			}
		}

	}
}
