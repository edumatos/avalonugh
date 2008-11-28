using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Editor.Tiles;
using System.ComponentModel;
using System.IO;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public partial class Level
	{
		/// <summary>
		/// name of the level to show to the users
		/// </summary>
		public readonly Attribute.String AttributeText = "text";

		/// <summary>
		///  the code to unlock this level
		/// </summary>
		public readonly Attribute.String AttributeCode = "code";

		public readonly Attribute.String AttributeBackground = "background";

		public readonly Attribute.Int32 AttributeBackgroundWidth = "background-width";
		public readonly Attribute.Int32 AttributeBackgroundHeight = "background-height";
		public readonly Attribute.Int32 AttributeWater = "water";
		public readonly Attribute.Int32 AttributeWind = "wind";
		public readonly Attribute.Int32 AttributeGravity = "gravity";

		public readonly Attribute.Int32 AttributeBorderTop = "border-top";
		public readonly Attribute.Int32 AttributeBorderLeft = "border-left";
		public readonly Attribute.Int32 AttributeBorderRight = "border-right";
		public readonly Attribute.Int32 AttributeBorderBottom = "border-bottom";

		public readonly Attribute.Int32 AttributeFlashlightOpacity = "flashlight-opacity";
		public readonly Attribute.Int32 AttributeAutoscroll = "autoscroll";

		public readonly ASCIIImage Map;

		//public Water KnownWater { get; set; }

	

		public readonly Image BackgroundImage;

		/// <summary>
		/// The caves are important because actors need to exit and enter them
		/// </summary>
		public readonly BindingList<Cave> KnownCaves = new BindingList<Cave>();
		public readonly BindingList<Stone> KnownStones = new BindingList<Stone>();
		public readonly BindingList<Ridge> KnownRidges = new BindingList<Ridge>();
		public readonly BindingList<Fence> KnownFences = new BindingList<Fence>();
		public readonly BindingList<Platform> KnownPlatforms = new BindingList<Platform>();
		public readonly BindingList<Bridge> KnownBridges = new BindingList<Bridge>();


		const string Comment = "#";
		const string Assignment = ":";

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

		public readonly List<Tree> KnownTrees = new List<Tree>();
		public readonly List<Sign> KnownSigns = new List<Sign>();
		public readonly List<Rock> KnownRocks = new List<Rock>();
		public readonly List<Obstacle> KnownObstacles = new List<Obstacle>();




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

		readonly TreeSelector TreeSelector;

		public Level(string source, int Zoom)
		{
			this.TreeSelector = new TreeSelector(this);

			this.AttributeFlashlightOpacity.Value = 255;

			this.Zoom = Zoom;

			var Create = new
			{
				//Tree = (Attribute.Int32)"tree",
				Rock = (Attribute.Int32)"rock",
				Sign = (Attribute.Int32_Int32)"sign"
			};

			//Create.Tree.Assigned +=
			//    x_ =>
			//    {
			//        var x = x_ * Zoom;
			//        var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

			//        new Tree(Zoom)
			//        {

			//        }.AddTo(KnownTrees).MoveBaseTo(x, y);
			//    };

			Create.Rock.Assigned +=
				x_ =>
				{
					var x = x_ * Zoom;
					var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

					new Rock(Zoom)
					{

					}.AddTo(KnownRocks).MoveBaseTo(x, y);
				};

			Create.Sign.Assigned +=
				(x_, SignValue) =>
				{
					var x = x_ * Zoom;
					var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

					new Sign(Zoom)
					{
						Value = SignValue
					}.AddTo(KnownSigns).MoveBaseTo(x, y);
				};


			var Commands = new AttributeDictonary
			{
				Create.Rock,
				Create.Sign,
				//Create.Tree,

				this.TreeSelector.Attribute,

				AttributeWind,
				AttributeWater,
				AttributeBackgroundWidth,
				AttributeBackgroundHeight,
				AttributeCode,
				AttributeText,
				AttributeBackground,

				AttributeBorderTop,
				AttributeBorderLeft, 
				AttributeBorderRight,
				AttributeBorderBottom,

				AttributeFlashlightOpacity,
				AttributeAutoscroll
			};


			this.Map = new ASCIIImage(
				new ASCIIImage.ConstructorArguments
				{
					value = source,
					IsComment = k => DoCommand(Commands, k)

				}
			);




			var BackgroundImageSource = Assets.Shared.KnownAssets.Path.Backgrounds + "/" + this.AttributeBackground.Value + ".png";

			// cannot use FileNames in partial trust at this time

			//if (Assets.Shared.KnownAssets.Default.FileNames.Contains(BackgroundImageSource))
			if (!string.IsNullOrEmpty(this.AttributeBackground.Value))
			{
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

			this.KnownObstacles.AddRange(
				this.BorderObstacleTop,
				this.BorderObstacleRight,
				this.BorderObstacleLeft,
				this.BorderObstacleBottom
			);




			#region Background
			this.Map.ForEach(
				k =>
				{
					var Tile = new ASCIITileSizeInfo(k);

					// ridge
					if (k.Value == RidgeSelector.Identifier)
					{
						RidgeSelector.AttachToLevel(k, Tile, this);

						return;
					}

					// cave
					if (k.Value == CaveSelector.Identifier)
					{
						CaveSelector.AttachToLevel(k, Tile, this);

						return;
					}

					// fence
					if (k.Value == FenceSelector.Identifier)
					{
						FenceSelector.AttachToLevel(k, Tile, this);

						return;
					}

					// stone
					if (k.Value == StoneSelector.Identifier)
					{
						StoneSelector.AttachToLevel(k, Tile, this);

						return;
					}

					// platform
					if (k.Value == PlatformSelector.Identifier)
					{
						PlatformSelector.AttachToLevel(k, Tile, this);

						return;
					}

					// bridge
					if (k.Value == BridgeSelector.Identifier)
					{
						BridgeSelector.AttachToLevel(k, Tile, this);

						return;
					}


				}
			);

			#endregion


		}


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
						KnownPlatforms
					}.WhereListChanged(
						delegate
						{
							Console.WriteLine("ToObstacles");

							var Bridges = this.KnownBridges.Select(k => k.ToObstacle());
							var Ridges = this.KnownRidges.Select(k => k.ToObstacle());
							var Platforms = this.KnownPlatforms.Select(k => k.ToObstacle());


							var value = this.KnownObstacles.AsEnumerable()
								.Concat(Bridges)
								.Concat(Ridges)
								.Concat(Platforms).ToArray().AsEnumerable();

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
									Entity.Image.Orphanize();
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
										Entity.Image.Orphanize();
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
										Entity.Image.Orphanize();
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
										Entity.Image.Orphanize();
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
										Entity.Image.Orphanize();
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
										Entity.Image.Orphanize();
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
				);
		}


	}
}
