using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class Level
	{
		public string Text;
		public string Code;
		public string Background;
		public string Water;
		public string Wind;

		public readonly ASCIIImage Map;

		public readonly int WaterTop;
		public readonly int WaterHeight;

		public readonly Image BackgroundImage;

		public const int BackgroundImageWidth = 320;
		public const int BackgroundImageHeight = 200;

		public readonly List<Cave> KnownCaves = new List<Cave>();

		const string Comment = "#";
		const string Assignment = ":";

		int TileRowsProcessed;

		public bool DoCommand(string e)
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



		readonly Dictionary<string, Action<string>> Commands;

		public readonly int Zoom;

		public readonly List<Tree> KnownTrees = new List<Tree>();
		public readonly List<Sign> KnownSigns = new List<Sign>();
		public readonly List<Rock> KnownRocks = new List<Rock>();

		public void CreateTree(string value)
		{
			var x = int.Parse(value) * Zoom;
			var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

			new Tree(Zoom)
			{

			}.AddTo(KnownTrees).MoveBaseTo(x, y);
		}

		public void CreateRock(string value)
		{
			var x = int.Parse(value) * Zoom;
			var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

			new Rock(Zoom)
			{

			}.AddTo(KnownRocks).MoveBaseTo(x, y);
		}

		public void CreateSign(string value)
		{
			var args = value.Split(';');

			var x = int.Parse(args[0]) * Zoom;
			var y = this.TileRowsProcessed * PrimitiveTile.Heigth * Zoom;

			new Sign(Zoom)
			{
				Value = int.Parse(args[1])
			}.AddTo(KnownSigns).MoveBaseTo(x, y);
		}

		public Level(string source, int Zoom)
		{
			this.Zoom = Zoom;

			Commands = new Dictionary<string, Action<string>>
			{
				{"text", e => Text = e},
				{"code", e => Code = e},
				{"background", e => Background = e},
				{"water", e => Water = e},
				{"wind", e => Wind = e},
				{"tree", CreateTree},
				{"sign", CreateSign},
				{"rock", CreateRock},
			};

			this.Map = new ASCIIImage(
				new ASCIIImage.ConstructorArguments
				{
					value = source,
					IsComment = DoCommand

				}
			);

			this.WaterHeight = int.Parse(this.Water) * Zoom;
			this.WaterTop = (BackgroundImageHeight * Zoom) - this.WaterHeight;

			var BackgroundImageSource = Assets.Shared.KnownAssets.Path.Backgrounds + "/" + this.Background + ".png";

			if (Assets.Shared.KnownAssets.Default.FileNames.Contains(BackgroundImageSource))
			{
				this.BackgroundImage = new Image
				{
					Source = BackgroundImageSource.ToSource(),
					Stretch = System.Windows.Media.Stretch.Fill,
					Width = BackgroundImageWidth * Zoom,
					Height = BackgroundImageHeight * Zoom,
				};
			}
		}
	}
}
