using System;
using System.Collections.Generic;
using System.IO;
using AvalonUgh.Labs.Monetized.Shared;
using ScriptCoreLib;
using ScriptCoreLib.ActionScript;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.flash.display;
using ScriptCoreLib.ActionScript.flash.text;
using ScriptCoreLib.ActionScript.flash.ui;

namespace AvalonUgh.Labs.Monetized.ActionScript
{
	using TargetCanvas = global::AvalonUgh.Labs.Shared.LabsCanvas;
	using TargetFlash = global::AvalonUgh.Labs.ActionScript.LabsFlash;

	/// <summary>
	/// Default flash player entrypoint class. See 'tools/build.bat' for adding more entrypoints.
	/// </summary>
	[Script, ScriptApplicationEntryPoint]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight, backgroundColor = 0)]
	public class MonetizedFlash : Sprite
	{



		/// <summary>
		/// Default constructor
		/// </summary>
		public MonetizedFlash()
		{
			KnownEmbeddedResources.Default[KnownAssets.Path.Assets + "/Preview.png"].
				ToBitmapAsset().AttachTo(this).
				MoveTo((TargetCanvas.DefaultWidth - 640) / 2, (TargetCanvas.DefaultHeight - 400) / 2);
		}

		static MonetizedFlash()
		{
			// add resources to be found by ImageSource
			KnownEmbeddedAssets.RegisterTo(
				KnownEmbeddedResources.Default.Handlers
			);
		}

	}

	[Script]
	public class KnownEmbeddedAssets
	{
		[EmbedByFileName]
		public static Class ByFileName(string e)
		{
			throw new NotImplementedException();
		}

		public static void RegisterTo(List<Converter<string, Class>> Handlers)
		{
			// assets from current assembly
			Handlers.Add(e => ByFileName(e));

			//AvalonUgh.Assets.ActionScript.KnownEmbeddedAssets.RegisterTo(Handlers);

			//// assets from referenced assemblies
			//Handlers.Add(e => global::ScriptCoreLib.ActionScript.Avalon.Cursors.EmbeddedAssets.Default[e]);
			//Handlers.Add(e => global::ScriptCoreLib.ActionScript.Avalon.TiledImageButton.Assets.Default[e]);

		}
	}

}