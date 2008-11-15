using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;

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

		public bool ParseProperty(string e)
		{
			const string Comment = "#";
			const string Assignment = ":";

			if (!e.StartsWith(Comment))
				return false;

			var i = e.IndexOf(Assignment);

			if (i > 0)
			{
				var Key = e.Substring(Comment.Length, i - Comment.Length).Trim().ToLower();
				if (SetProperty.ContainsKey(Key))
				{
					var Value = e.Substring(i + Assignment.Length).Trim();

					SetProperty[Key](Value);
				}
			}

			return true;
		}

		readonly Dictionary<string, Action<string>> SetProperty;

		public readonly int Zoom;

		public Level(string source, int Zoom)
		{
			this.Zoom = Zoom;

			SetProperty = new Dictionary<string, Action<string>>
			{
				{"text", e => Text = e},
				{"code", e => Code = e},
				{"background", e => Background = e},
				{"water", e => Water = e},
				{"wind", e => Wind = e}
			};

			this.Map = new ASCIIImage(
				new ASCIIImage.ConstructorArguments
				{
					value = source,
					IsComment = ParseProperty
		
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
