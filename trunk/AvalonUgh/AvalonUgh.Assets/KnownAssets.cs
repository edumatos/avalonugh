using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib.Shared;
using ScriptCoreLib;

// jsc:php: does not yet support the newest asset inclusing tech
[assembly: ScriptResources(AvalonUgh.Assets.Shared.KnownAssets.Path.Assets)]
[assembly: ScriptResources(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Blue)]
[assembly: ScriptResources(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Brown)]


namespace AvalonUgh.Assets
{
	namespace Shared
	{

		[Script]
		public class KnownAssets : AssetsImplementationDetails
		{
			public static readonly KnownAssets Default = new KnownAssets();

			[Script, ScriptResources]
			public static class Path
			{
				// constants defined here define also the assets embedded
				// to the assembly
				// example: web/assets/OrcasAvalonApplication/about.txt

				public const string Assets = "assets/AvalonUgh.Assets";
				public const string Tiles = "assets/AvalonUgh.Assets.Tiles";
				public const string Sprites = "assets/AvalonUgh.Assets.Sprites";
				public const string Audio = "assets/AvalonUgh.Assets.Audio";
				public const string Backgrounds = "assets/AvalonUgh.Assets.Backgrounds";
				public const string FilmScratch = "assets/AvalonUgh.Assets.FilmScratch";
				public const string Levels = "assets/AvalonUgh.Assets.Levels";

				[Script, ScriptResources]
				public static class Fonts
				{
					public const string Brown = "assets/AvalonUgh.Assets.Fonts/Brown";
                    public const string Blue = "assets/AvalonUgh.Assets.Fonts/Blue";
                    public const string White = "assets/AvalonUgh.Assets.Fonts/White";
				}
			}


		}

		#region AssetsImplementationDetails
		public class AssetsImplementationDetails
		{
			// This class has the native implementation
			// JavaScript and ActionScript have their own implementations!

			public string[] FileNames
			{
				get
				{
					return ScriptCoreLib.CSharp.Extensions.EmbeddedResourcesExtensions.GetEmbeddedResources(null, this.GetType().Assembly);
				}
			}

		}
		#endregion


	}

	#region AssetsImplementationDetails
	namespace JavaScript
	{
		[Script(Implements = typeof(Shared.AssetsImplementationDetails))]
		internal class __AssetsImplementationDetails
		{
			public string[] FileNames
			{
				[EmbedGetFileNames]
				get
				{
					throw new NotImplementedException();
				}
			}
		}
	}

	namespace ActionScript
	{
		[Script(Implements = typeof(Shared.AssetsImplementationDetails))]
		internal class __AssetsImplementationDetails
		{
			public string[] FileNames
			{
				[EmbedGetFileNames]
				get
				{
					throw new NotImplementedException();
				}
			}

		}
	}

	namespace Server
	{
		[Script(Implements = typeof(Shared.AssetsImplementationDetails))]
		public class __AssetsImplementationDetails
		{
			public string[] FileNames
			{
				[EmbedGetFileNames]
				get
				{
					throw new NotImplementedException("FileNames");
				}
			}

		}
	}

	#endregion
}
