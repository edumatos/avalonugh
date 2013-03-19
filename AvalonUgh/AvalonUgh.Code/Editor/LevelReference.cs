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


		public string Data { get { return DataFuture.Value; } set { DataFuture.Value = value; } }
		public readonly Future<string> DataFuture = new Future<string>();

		public LevelReference()
			: this(null)
		{

		}

		public LevelReference(StorageLocation Location)
		{
			if (Location == null)
				Location = new StorageLocation { Cookie = "edited" };

			this.Location = Location;

			// the level might not be embedded nor saved yet
			if (Location.Embedded != null)
				Location.Embedded.ToString().ToStringAsset(
					Data =>
					{
						this.DataFuture.Value = Data;
					}
				);

			var Commands = new LevelType.AttributeDictonary
			{
				AttributeWater,
				AttributeCode,
				AttributeText,
				AttributeBackground,
				AttributeEpisode,
				AttributeNextCode
			};

			this.AttributeBackground.Value = "";
			this.AttributeBackground.Assigned +=
				value =>
				{
					if (value == "null")
						throw new Exception("null trap");

				};

			this.DataFuture.Continue(
				LoadedData =>
				{
					this.Map = new ASCIIImage(
						new ASCIIImage.ConstructorArguments
						{
							value = LoadedData,
							IsComment = e => DoCommand(Commands, e)
						}
					);
				}
			);

		}

		public bool DoCommand(LevelType.AttributeDictonary Commands, string e)
		{
			if (!e.StartsWith(LevelType.Comment))
			{
				//TileRowsProcessed++;
				return false;
			}

			var i = e.IndexOf(LevelType.Assignment);

			if (i > 0)
			{
				var Key = e.Substring(LevelType.Comment.Length, i - LevelType.Comment.Length).Trim().ToLower();
				if (Commands.ContainsKey(Key))
				{
					var Value = e.Substring(i + LevelType.Assignment.Length).Trim();

					if (Value == "null")
						throw new Exception("null trap 3" + new { e, i });
					 
					Commands[Key](Value);
				}
			}

			return true;

		}

		public ASCIIImage Map;


		/// <summary>
		/// name of the level to show to the users
		/// </summary>
		public readonly LevelType.Attribute.String AttributeText = "text";

		/// <summary>
		///  the code to unlock this level
		/// </summary>
		public readonly LevelType.Attribute.String AttributeCode = "code";

		public readonly LevelType.Attribute.String AttributeBackground = "background";

		public readonly LevelType.Attribute.Int32 AttributeWater = "water";

		public readonly LevelType.Attribute.String AttributeEpisode = "episode";
		public readonly LevelType.Attribute.String AttributeNextCode = "nextcode";


		public string Code
		{
			get
			{
				return this.AttributeCode.Value;
			}
		}

		public string Text
		{
			get
			{
				return this.AttributeText.Value;
			}
		}

		public string Background
		{
			get
			{
				return this.AttributeBackground.Value;
			}
		}

		public int Water
		{
			get
			{
				return this.AttributeWater.Value;
			}
		}


		[Script]
		public class SizeType
		{
			public int Width;
			public int Height;

			// this could have been just a tuple if it would be supported by c# 
		}

		public SizeType Size
		{
			get
			{
				var n = new SizeType();

				if (this.Data == null)
					return n;

				using (var r = new StringReader(this.Data))
				{
					var e = r.ReadLine();

					while (e != null)
					{
						if (!e.StartsWith(LevelType.Comment))
						{
							n.Width = n.Width.Max(e.Length);
							n.Height++;
						}

						e = r.ReadLine();
					}
				}

				return n;
			}
		}
	}
}
