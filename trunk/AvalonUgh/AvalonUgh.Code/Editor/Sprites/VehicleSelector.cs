using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor.Sprites
{
	[Script]
	public class VehicleSelector : SelectorBase
	{
		public const string Alias = "vehicle";

		public readonly SelectorSize_2x2 Size_2x2 = new SelectorSize_2x2();


		public VehicleSelector()
		{
			this.ToolbarImage =
				new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Sprites,
					Name = VehicleSelector.Alias,
					Index = 1,
					AnimationFrame = 4,
					Width = 2,
					Height = 2,
					Extension = "png"
				};

			// clicking on the toolbar will shuffle between those sizes
			// also while loading tiles the map will tell us which size to use
	
			this.Sizes = new[]
			{
				Size_2x2
			};
		}


	

		[Script]
		public class SelectorSize_2x2 : SpriteSelector
		{
			public SelectorSize_2x2()
			{
				Width = PrimitiveTile.Width * 2;
				Height = PrimitiveTile.Heigth * 2;
				PercisionX = PrimitiveTile.Width / 2;
				PercisionY = PrimitiveTile.Heigth;
			}


			public override void CreateTo(Level Level, View.SelectorPosition Position)
			{
				var x = (Position.ContentX + this.HalfWidth) * Level.Zoom;
				var y = (Position.ContentY + this.HalfHeight) * Level.Zoom;

				RemoveEntities(this, Level, Position);

				var g = new Vehicle(Level.Zoom);

				g.CurrentDriver = null;
				g.MoveTo(x, y);
				g.Container.Opacity = 0.5;


				var v = new Vehicle(Level.Zoom);

				v.CurrentLevel = Level;
				v.CurrentDriver = null;
				v.MoveTo(x, y);
				v.StartPosition = g;

				Level.KnownVehicles.Add(v);

			}

			public void CreateTo(Level level, Level.Attribute.Int32_Array SyncAttributeVehicle)
			{
				var v = new Vehicle(level.Zoom);

				// this kind of serializing and deserializing should be hard typed

				v.CurrentLevel = level;
				v.CurrentDriver = null;
				v.MoveTo(
					SyncAttributeVehicle[0], 
					SyncAttributeVehicle[1]
				);
				v.VelocityX = SyncAttributeVehicle[2];
				v.VelocityY = SyncAttributeVehicle[3];

				if (SyncAttributeVehicle.Value[4] > 0)
				{
					var g = new Vehicle(level.Zoom);

					g.CurrentDriver = null;
					g.MoveTo(
						SyncAttributeVehicle[5],
						SyncAttributeVehicle[6]	
					);
					g.Container.Opacity = 0.5;


					v.StartPosition = g;
				}

				if (SyncAttributeVehicle.Value[7] > 0)
				{
					// we have to find the driver
					// he is either in our world
					// or comes at a future point of time
					var NetworkNumber = SyncAttributeVehicle.Value[8];
					var IdentityLocal = SyncAttributeVehicle.Value[9];

					//
					v.IsUnmanned = false;


					level.KnownActors.ForEachNewOrExistingItem(
						value =>
						{
							if (v == null)
								return;

							if (value.PlayerInfo == null)
								return;

							if (value.PlayerInfo.IdentityLocal != IdentityLocal)
								return;

							if (value.PlayerInfo.Identity.NetworkNumber != NetworkNumber)
								return;

							v.CurrentDriver = value;
							v = null;
						}
					);
				}

				level.KnownVehicles.Add(v);
			}
		}
	}
}
