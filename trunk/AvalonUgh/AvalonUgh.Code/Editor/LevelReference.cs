using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using System.Windows.Controls;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class LevelReference
	{
		[Script]
		public class StorageLocation
		{
			public string Cookie;

			public NameFormat Embedded;

			public static implicit operator StorageLocation(int LevelNumber)
			{
				return new StorageLocation
				{
					Embedded =
						new NameFormat
						{
							Path = Assets.Shared.KnownAssets.Path.Levels,
							Name = "level",
							Extension = "txt",
							AnimationFrame = LevelNumber,
						}
				};
			}

		}

		public readonly StorageLocation Location;

		public readonly Image Preview;
		public readonly Image SmallPreview;

		public LevelReference(StorageLocation Location)
		{
			this.Location = Location;

			var PreviewSource = Location.Embedded.Clone();

			PreviewSource.Extension = "png";

			this.Preview = new Image
			{
				Stretch = System.Windows.Media.Stretch.Fill,
				Source = PreviewSource,
				Width = 160,
				Height = 100,
			};


			this.SmallPreview = new Image
			{
				Stretch = System.Windows.Media.Stretch.Fill,
				Source = PreviewSource,
				Width = 48,
				Height = 30,
			};
		}



	}
}
