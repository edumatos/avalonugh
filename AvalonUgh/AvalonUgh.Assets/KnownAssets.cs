using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib.Shared;
using ScriptCoreLib;

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
	#endregion
}
