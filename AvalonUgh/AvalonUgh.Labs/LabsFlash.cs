﻿using System;
using System.Collections.Generic;
using ScriptCoreLib;
using ScriptCoreLib.ActionScript;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.flash.display;
using ScriptCoreLib.ActionScript.flash.text;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Labs.ActionScript
{
	using TargetCanvas = global::AvalonUgh.Labs.Shared.LabsCanvas;

	/// <summary>
	/// Default flash player entrypoint class. See 'tools/build.bat' for adding more entrypoints.
	/// </summary>
	[Script, ScriptApplicationEntryPoint(Width = TargetCanvas.DefaultWidth, Height = TargetCanvas.DefaultHeight)]
	[SWF(
		width = TargetCanvas.DefaultWidth, 
		height = TargetCanvas.DefaultHeight, 
		backgroundColor = 0,
		frameRate = TargetCanvas.DefaultFramerate)]
	public class LabsFlash : Sprite
	{
		public LabsFlash()
		{


			var c = new TargetCanvas(true);

			c.GameWorkspace.Audio_Music.Enabled = true;
			c.GameWorkspace.Arguments.ToFullscreen = () =>
				{
					this.stage.scaleMode = StageScaleMode.SHOW_ALL;
					this.stage.SetFullscreen(true);
				};

			// spawn the wpf control
			AvalonExtensions.AttachToContainer(c, this);
		}

		static LabsFlash()
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

			AvalonUgh.Assets.ActionScript.KnownEmbeddedAssets.RegisterTo(Handlers);
			ScriptCoreLib.ActionScript.Avalon.Carousel.KnownEmbeddedAssets.RegisterTo(Handlers);

			//// assets from referenced assemblies
			Handlers.Add(e => global::ScriptCoreLib.ActionScript.Avalon.Cursors.EmbeddedAssets.Default[e]);
			//Handlers.Add(e => global::ScriptCoreLib.ActionScript.Avalon.TiledImageButton.Assets.Default[e]);

		}
	}
}