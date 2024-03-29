﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Editor.Tiles;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Shapes;
using System.Windows.Media;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public partial class LevelType : IDisposable
	{
		/// <summary>
		/// name of the level to show to the users
		/// </summary>
		public readonly Attribute.String AttributeText = "text";

		/// <summary>
		///  the code to unlock this level
		/// </summary>
		public readonly Attribute.String AttributeCode = "code";
		public readonly Attribute.String AttributeNextCode = "nextcode";

		public readonly Attribute.String AttributeBackground = "background";

		public readonly Attribute.Int32 AttributeBackgroundWidth = "background-width";
		public readonly Attribute.Int32 AttributeBackgroundHeight = "background-height";
		public readonly Attribute.Int32 AttributeWater = "water";
		public readonly Attribute.Int32 AttributeWind = "wind";
		public readonly Attribute.Int32 AttributeSnow = "snow";
		public readonly Attribute.Int32 AttributeGravity = "gravity";
		public readonly Attribute.Int32 AttributeWaterRise = "water-rise";
		public readonly Attribute.Int32 AttributeHeadCount = "headcount";

		public readonly Attribute.Int32 AttributeBorderTop = "border-top";
		public readonly Attribute.Int32 AttributeBorderLeft = "border-left";
		public readonly Attribute.Int32 AttributeBorderRight = "border-right";
		public readonly Attribute.Int32 AttributeBorderBottom = "border-bottom";

		public readonly Attribute.Int32 AttributeFlashlightOpacity = "flashlight-opacity";
		public readonly Attribute.Int32 AttributeAutoscroll = "autoscroll";

		public readonly ASCIIImage Map;

		//public Water KnownWater { get; set; }



		public Image BackgroundImage;

		/// <summary>
		/// The caves are important because actors need to exit and enter them
		/// </summary>
		public readonly BindingList<Cave> KnownCaves = new BindingList<Cave>();
		public readonly BindingList<Stone> KnownStones = new BindingList<Stone>();
		public readonly BindingList<Ridge> KnownRidges = new BindingList<Ridge>();
		public readonly BindingList<RidgeTree> KnownRidgeTrees = new BindingList<RidgeTree>();
		public readonly BindingList<Fence> KnownFences = new BindingList<Fence>();
		public readonly BindingList<Platform> KnownPlatforms = new BindingList<Platform>();
		public readonly BindingList<Bridge> KnownBridges = new BindingList<Bridge>();
		public readonly BindingList<Bird> KnownBirds = new BindingList<Bird>();

		public readonly BindingList<Actor> KnownPassengers = new BindingList<Actor>();
		public readonly BindingList<Actor> KnownActors = new BindingList<Actor>();
		public readonly BindingList<Vehicle> KnownVehicles = new BindingList<Vehicle>();
		public readonly BindingList<Tryoperus> KnownTryoperus = new BindingList<Tryoperus>();



		public IEnumerable<Tile> KnownLandingTiles
		{
			get
			{
				var Bridges = KnownBridges.Select(k => (Tile)k);
				var Platforms = KnownPlatforms.Select(k => (Tile)k);

				return Bridges.Concat(Platforms);
			}
		}

		public const string Comment = "#";
		public const string Assignment = ":";

		public int TileRowsProcessed;

		public bool DoCommand(AttributeDictonary Commands, string e)
		{
			if (!e.StartsWith(Comment))
			{
				TileRowsProcessed++;
				return false;
			}

			var i = e.IndexOf(Assignment);

			if (i > 0)
			{
				var Key = e.Substring(Comment.Length, i - Comment.Length).Trim().ToLower();
				if (Commands.ContainsKey(Key))
				{
					var Value = e.Substring(i + Assignment.Length).Trim();

					Commands[Key](Value);
				}
			}

			return true;

		}




		public readonly int Zoom;

		public readonly BindingList<Dino> KnownDinos = new BindingList<Dino>();
		public readonly BindingList<Tree> KnownTrees = new BindingList<Tree>();
		public readonly BindingList<Sign> KnownSigns = new BindingList<Sign>();
		public readonly BindingList<Flag> KnownFlags = new BindingList<Flag>();
		public readonly BindingList<Rock> KnownRocks = new BindingList<Rock>();
		public readonly BindingList<Fruit> KnownFruits = new BindingList<Fruit>();
		public readonly BindingList<Gold> KnownGold = new BindingList<Gold>();

		public readonly List<Obstacle> KnownInvisibleObstacles = new List<Obstacle>();




		public int ActualWidth
		{
			get
			{
				return (PrimitiveTile.Width * this.Map.Width * Zoom);
			}
		}

		public int ActualHeight
		{
			get
			{
				return (PrimitiveTile.Heigth * this.Map.Height * Zoom);
			}
		}

		//readonly TreeSelector TreeSelector;

		public readonly Physics Physics;


		public LevelType(string source, int Zoom, KnownSelectors Selectors)
		{
			this.AttributeCode.Value = "";
			this.AttributeNextCode.Value = "";

			//if (Selectors == null)
			//    throw new ArgumentNullException();
			this.LevelTime = 32;

			this.KnownPassengers.AttachTo(this.KnownActors);

			this.Physics = new Physics
			{
				Level = this,
			};

			this.AttributeGravity.Value = 20;
			this.AttributeWind.Value = 0;

			// the water shall be deep enough to enable diving
			this.AttributeBorderBottom.Value = 96;
			this.AttributeBackgroundWidth.Value = 320;
			this.AttributeBackgroundHeight.Value = 200;


			this.AttributeFlashlightOpacity.Value = 180;

			this.Zoom = Zoom;

			this.KnownActors.ForEachNewItem(
				NewActor =>
				{
					NewActor.LevelViaKnownActors = this;
				}
			);

			var Create = new
			{
				tryo = (Attribute.Int32)Sprites.Tryoperus.SpecificNameFormat.Alias,
				Vehicle = (Attribute.Int32)Sprites.VehicleSelector.Alias,
				Rock = (Attribute.Int32)Sprites.Rock.SpecificNameFormat.Alias,
				Dino = (Attribute.Int32)"dino",
				Tree = (Attribute.Int32)"tree",
				Gold = (Attribute.Int32)"gold",
				flag = (Attribute.Int32)"flag",
				//Sign = (Attribute.Int32_Int32)"sign",
			};



			Create.tryo.Assigned +=
				x =>
				{
					Selectors.Tryoperus.Size_2x2.CreateTo(this,
						new View.SelectorPosition
						{
							ContentX = x - PrimitiveTile.Width,
							ContentY = (this.TileRowsProcessed - 2) * PrimitiveTile.Heigth
						}
					);
				};

			#region vehicle
			Create.Vehicle.Assigned +=
				x =>
				{
					Selectors.Vehicle.Size_2x2.CreateTo(this,
						new View.SelectorPosition
						{
							ContentX = x - PrimitiveTile.Width,
							ContentY = (this.TileRowsProcessed - 2) * PrimitiveTile.Heigth
						}
					);
				};

			Attribute.Int32_Array SyncAttributeVehicle = "*vehicle";
			SyncAttributeVehicle.Assigned +=
				delegate
				{
					Selectors.Vehicle.Size_2x2.CreateTo(this, SyncAttributeVehicle);
				};
			#endregion

			#region Rock
			Create.Rock.Assigned +=
				x =>
				{
					Selectors.Rock.Size_1x1.CreateTo(this,
						new View.SelectorPosition
						{
							ContentX = x - PrimitiveTile.Width / 2,
							ContentY = (this.TileRowsProcessed - 1) * PrimitiveTile.Heigth
						}
					);
				};
			Attribute.Int32_Array SyncAttributeRock = "*rock";
			SyncAttributeRock.Assigned +=
				delegate
				{
					Selectors.Rock.Size_1x1.CreateTo(this, SyncAttributeRock);
				};
			#endregion

			Attribute.Int32_Array SyncAttributePassenger = "passenger";
			SyncAttributePassenger.Assigned +=
				delegate
				{

					Selectors.Passenger.Size_2x2.CreateTo(this, SyncAttributePassenger);

				};


			Attribute.Int32_Array SyncAttributeBird = "bird";
			SyncAttributeBird.Assigned +=
				delegate
				{

					Selectors.Bird.Size_2x3.CreateTo(this, SyncAttributeBird);

				};

			Create.Dino.Assigned +=
				x_ =>
				{
					var x = x_ * Zoom;
					var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

					new Dino(Zoom)
					{

					}.AddTo(KnownDinos).MoveBaseTo(x, y);
				};

			Create.Tree.Assigned +=
				x_ =>
				{
					var x = x_ * Zoom;
					var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

					new Tree(Zoom)
					{

					}.AddTo(KnownTrees).MoveBaseTo(x, y);
				};

			Create.Gold.Assigned +=
				x_ =>
				{
					var x = x_ * Zoom;
					var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

					new Gold(Zoom)
					{

					}.AddTo(KnownGold).MoveBaseTo(x, y);
				};

			Create.flag.Assigned +=
				x_ =>
				{
					var x = x_ * Zoom;
					var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

					new Flag(Zoom)
					{

					}.AddTo(KnownFlags).MoveBaseTo(x, y);
				};

			Attribute.Int32_Array SyncAttributeSign = "sign";
			SyncAttributeSign.Assigned +=
				delegate
				{

					Selectors.Sign.Size_1x1.CreateTo(this, SyncAttributeSign, this.TileRowsProcessed);

				};

		




			var Commands = new AttributeDictonary
			{
				Create.Dino,
				Create.Gold,
				
				SyncAttributeSign,

				//Create.Sign,
				Create.flag,
				Create.Tree,
				
				Create.tryo,
				Create.Rock,
				Create.Vehicle,

				SyncAttributeVehicle,
				SyncAttributeRock,
				SyncAttributePassenger,
				SyncAttributeBird,

				AttributeWind,
				AttributeSnow,
				AttributeGravity,

				AttributeWater,
				AttributeWaterRise,

				AttributeBackgroundWidth,
				AttributeBackgroundHeight,
				AttributeCode,
				AttributeNextCode,
				AttributeText,
				AttributeBackground,

				AttributeBorderTop,
				AttributeBorderLeft, 
				AttributeBorderRight,
				AttributeBorderBottom,

				AttributeFlashlightOpacity,
				AttributeAutoscroll,

				AttributeHeadCount
			};


			this.Map = new ASCIIImage(
				new ASCIIImage.ConstructorArguments
				{
					value = source,
					IsComment = k => DoCommand(Commands, k)

				}
			);




			// cannot use FileNames in partial trust at this time

			//if (Assets.Shared.KnownAssets.Default.FileNames.Contains(BackgroundImageSource))
			if (!string.IsNullOrEmpty(this.AttributeBackground.Value))
			{

				var BackgroundImageSource = Assets.Shared.KnownAssets.Path.Backgrounds + "/" + this.AttributeBackground.Value + ".png";

				this.BackgroundImage = new Image
				{
					Source = BackgroundImageSource.ToSource(),
					Stretch = System.Windows.Media.Stretch.Fill,
					Width = this.AttributeBackgroundWidth.Value * Zoom,
					Height = this.AttributeBackgroundHeight.Value * Zoom,
				};
			}



			// at this point we need to load the map tiles

			var BorderSize = Zoom * 10;
			var ZoomedBorder = this.ZoomedBorder;

			this.BorderObstacleTop = new Obstacle
			{
				Left = -ZoomedBorder.Left,
				Top = -ZoomedBorder.Top - BorderSize,
				Right = this.ActualWidth + ZoomedBorder.Right,
				Bottom = -ZoomedBorder.Top
			};

			this.BorderObstacleRight = new Obstacle
			{
				Left = this.ActualWidth + ZoomedBorder.Right,
				Top = -ZoomedBorder.Top,
				Right = this.ActualWidth + ZoomedBorder.Right + BorderSize,
				Bottom = this.ActualHeight + ZoomedBorder.Bottom
			};

			this.BorderObstacleLeft = new Obstacle
			{
				Left = -ZoomedBorder.Left - BorderSize,
				Top = -ZoomedBorder.Top,
				Right = -ZoomedBorder.Left,
				Bottom = this.ActualHeight + ZoomedBorder.Bottom
			};


			this.BorderObstacleBottom = new Obstacle
			{
				Left = -ZoomedBorder.Left,
				Top = this.ActualHeight + ZoomedBorder.Bottom,
				Right = this.ActualWidth + ZoomedBorder.Right,
				Bottom = this.ActualHeight + ZoomedBorder.Bottom + BorderSize
			};

			this.KnownInvisibleObstacles.AddRange(
				this.BorderObstacleTop,
				this.BorderObstacleRight,
				this.BorderObstacleLeft,
				this.BorderObstacleBottom
			);




			#region Background
			this.Map.ForEach(
				k =>
				{
					if (string.IsNullOrEmpty(k.Value))
						return;

					var i = new ASCIITileSizeInfo(k);

					Selectors.TileTypes.SingleOrDefault(q => q.GetIdentifier() == i.Value).Apply(q => q.AttachTileToLevel(k, i, this));



				}
			);

			#endregion


			Action<IBindingList> DetectModifications =
				e =>
				{
					e.ListChanged +=
						delegate
						{
							this.IsDirty = true;
						};
				};


			DetectModifications.AsParamsAction()(
				this.KnownBridges,
				this.KnownCaves,
				this.KnownDinos,
				this.KnownFences,
				this.KnownGold,
				this.KnownPlatforms,
				this.KnownRidges,
				this.KnownRidgeTrees,
				this.KnownRocks,
				this.KnownSigns,
				this.KnownStones,
				this.KnownTrees,
				this.KnownTryoperus,
				this.KnownVehicles,
				this.KnownPassengers
			);

		}

		public bool IsDirty;

		[Script]
		public class ZoomedBorderType
		{
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}

		public ZoomedBorderType ZoomedBorder
		{
			get
			{
				return new ZoomedBorderType
				{
					Left = this.AttributeBorderLeft.Value * this.Zoom,
					Top = this.AttributeBorderTop.Value * this.Zoom,
					Right = this.AttributeBorderRight.Value * this.Zoom,
					Bottom = this.AttributeBorderBottom.Value * this.Zoom,
				};
			}
		}

		public Obstacle BorderObstacleTop { get; set; }
		public Obstacle BorderObstacleRight { get; set; }
		public Obstacle BorderObstacleLeft { get; set; }
		public Obstacle BorderObstacleBottom { get; set; }

		Func<IEnumerable<Obstacle>> ToObstacles_Cache;
		public IEnumerable<Obstacle> ToObstacles()
		{
			// we will recalculate tile obstacles only when they actually change
			if (ToObstacles_Cache == null)
				ToObstacles_Cache =
					new IBindingList[]
					{
						KnownBridges,
						KnownRidges,
						KnownRidgeTrees,
						KnownPlatforms,
					}.WhereListChanged(
						delegate
						{
							Console.WriteLine("Level.ToObstacles");

							var Bridges = this.KnownBridges.Select(k => k.ToObstacle());
							var Ridges = this.KnownRidges.Select(k => k.ToObstacle());
							var RidgeTrees = this.KnownRidgeTrees.Select(k => k.ToObstacle());
							var Platforms = this.KnownPlatforms.Select(k => k.ToObstacle());


							var value = this.KnownInvisibleObstacles.AsEnumerable()
								.Concat(Bridges)
								.Concat(Ridges)
								.Concat(RidgeTrees)
								.Concat(Platforms)
								.ToArray().AsEnumerable();

							return value;
						}
					);

			return ToObstacles_Cache();
		}







	
		[Script]
		public class RemovableObject
		{
			public Obstacle Obstacle;
			public Action Dispose;
		}

		public IEnumerable<RemovableObject> GetRemovablePlatforms()
		{
			return
				this.KnownStones.Select(
					Entity =>
						new RemovableObject
						{
							Obstacle = Entity.ToObstacle(),
							Dispose =
								delegate
								{
									this.KnownStones.Remove(Entity);
								}
						}
				).Concat(
					this.KnownCaves.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownCaves.Remove(Entity);
									}
							}
					)
				).Concat(
					this.KnownRidges.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownRidges.Remove(Entity);
									}
							}
					)
				).Concat(
					this.KnownRidgeTrees.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownRidgeTrees.Remove(Entity);
									}
							}
					)
				).Concat(
					this.KnownFences.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownFences.Remove(Entity);
									}
							}
					)
				).Concat(
					this.KnownPlatforms.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownPlatforms.Remove(Entity);
									}
							}
					)
				).Concat(
					this.KnownBridges.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownBridges.Remove(Entity);
									}
							}
					)
				);
		}

		public IEnumerable<RemovableObject> GetRemovableEntities()
		{
			return
				this.KnownTrees.Select(
					Entity =>
						new RemovableObject
						{
							Obstacle = Entity.ToObstacle(),
							Dispose =
								delegate
								{
									this.KnownTrees.Remove(Entity);
									Entity.Dispose();
								}
						}
				).Concat(
					this.KnownRocks.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownRocks.Remove(Entity);
										Entity.Dispose();
									}
							}
					)
				).Concat(
					this.KnownPassengers.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownPassengers.Remove(Entity);
										Entity.Dispose();
									}
							}
					)
				).Concat(
					this.KnownGold.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownGold.Remove(Entity);
										Entity.Dispose();
									}
							}
					)

				).Concat(
					this.KnownSigns.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownSigns.Remove(Entity);
										Entity.Dispose();
									}
							}
					)
				).Concat(
					this.KnownFlags.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownFlags.Remove(Entity);
										Entity.Dispose();
									}
							}
					)

				).Concat(
					this.KnownVehicles.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownVehicles.Remove(Entity);
										Entity.Dispose();
									}
							}
					)
				).Concat(
					this.KnownDinos.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownDinos.Remove(Entity);
									}
							}
					)
				).Concat(
					this.KnownTryoperus.Select(
						Entity =>
							new RemovableObject
							{
								Obstacle = Entity.ToObstacle(),
								Dispose =
									delegate
									{
										this.KnownTryoperus.Remove(Entity);
										Entity.Dispose();
									}
							}
					)
				).Concat(
					this.KnownBirds.SelectMany(
						Entity =>
						{
							return new[]
							{
								new RemovableObject
								{
									Obstacle = Entity.ToObstacle(),
									Dispose =
										delegate
										{
											this.KnownBirds.Remove(Entity);
											Entity.Dispose();
										}
								},
									new RemovableObject
								{
									Obstacle = Entity.StartPosition.ToObstacle(),
									Dispose =
										delegate
										{
											this.KnownBirds.Remove(Entity);
											Entity.Dispose();
										}
								}
								// jsc bug: array should be converted to enumerable
							}.AsEnumerable();

						}
					)
				);
		}




		public readonly ColoredRectangleList ContentInfoColoredShapes_PlatformSnapshots = new ColoredRectangleList();




		int InternalLevelTime;
		public event Action LevelTimeChanged;
		/// <summary>
		/// The remaining time of the level will be shown on the status bar
		/// </summary>
		public int LevelTime
		{
			get
			{
				return InternalLevelTime;
			}
			set
			{
				InternalLevelTime = value;
				if (LevelTimeChanged != null)
					LevelTimeChanged();
			}
		}

		public void Dispose()
		{
			this.KnownPassengers.ToArray().Dispose();
			this.KnownTryoperus.ToArray().Dispose();
			this.KnownVehicles.ToArray().Dispose();
			this.KnownBirds.ToArray().Dispose();
			this.KnownVehicles.ToArray().Dispose();
		}

	}
}
