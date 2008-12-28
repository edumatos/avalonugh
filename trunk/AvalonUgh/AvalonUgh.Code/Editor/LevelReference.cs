using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.IO;

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

		public string Data;

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

			Location.Embedded.ToString().ToStringAsset(
				Data =>
				{
					this.Data = Data;
				}
			);
				
		}


		public string Code
		{
			get
			{
				var value = "";

				ApplyAttribute("code", k => value = k);

				return value;
			}
		}

		public string Text
		{
			get
			{
				var value = "";

				ApplyAttribute("text", k => value = k);

				return value;
			}
		}

		public void ApplyAttribute(string AttributeKey, Action<string> handler)
		{
			if (this.Data == null)
				return;

			using (var r = new StringReader(this.Data))
			{
				var e = r.ReadLine();

				while (e != null)
				{
					if (e.StartsWith(Level.Comment))
					{
						var i = e.IndexOf(Level.Assignment);

						if (i > 0)
						{
							var Key = e.Substring(Level.Comment.Length, i - Level.Comment.Length).Trim().ToLower();

							if (AttributeKey.ToLower() == Key)
							{
								var Value = e.Substring(i + Level.Assignment.Length).Trim();

								handler(Value);
							}
						}
					}

					e = r.ReadLine();
				}
			}
		}
	}
}
