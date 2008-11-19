using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;

namespace AvalonUgh.Code
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

		public readonly ASCIIImage Map;

		//public Water KnownWater { get; set; }

		public readonly int WaterTop;
		public readonly int WaterHeight;

		public readonly Image BackgroundImage;

		/// <summary>
		/// The caves are important because actors need to exit and enter them
		/// </summary>
		public readonly List<Cave> KnownCaves = new List<Cave>();

		const string Comment = "#";
		const string Assignment = ":";

		int TileRowsProcessed;

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

		public Level(string source, int Zoom)
		{
			this.Zoom = Zoom;

			var Create = new
			{
				Tree = (Attribute.Int32)"tree",
				Rock = (Attribute.Int32)"rock",
				Sign = (Attribute.Int32_Int32)"sign"
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
				Create.Tree,

				AttributeWind,
				AttributeWater,
				AttributeBackgroundWidth,
				AttributeBackgroundHeight,
				AttributeCode,
				AttributeText,
				AttributeBackground
			};


			this.Map = new ASCIIImage(
				new ASCIIImage.ConstructorArguments
				{
					value = source,
					IsComment = k => DoCommand(Commands, k)

				}
			);




			var BackgroundImageSource = Assets.Shared.KnownAssets.Path.Backgrounds + "/" + this.AttributeBackground.Value + ".png";

			if (Assets.Shared.KnownAssets.Default.FileNames.Contains(BackgroundImageSource))
			{
				this.BackgroundImage = new Image
				{
					Source = BackgroundImageSource.ToSource(),
					Stretch = System.Windows.Media.Stretch.Fill,
					Width = this.AttributeBackgroundWidth.Value * Zoom,
					Height = this.AttributeBackgroundHeight.Value * Zoom,
				};
			}

			// water top should depend on tiles found instead
			this.WaterHeight = this.AttributeWater.Value * Zoom;
			this.WaterTop = this.ActualHeight - this.WaterHeight;

			// at this point we need to load the map tiles
		}
	}
}
