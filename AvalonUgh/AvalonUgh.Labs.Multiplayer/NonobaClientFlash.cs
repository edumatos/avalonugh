using System;
using System.Collections.Generic;
using ScriptCoreLib;
using ScriptCoreLib.ActionScript;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.flash.display;
using ScriptCoreLib.ActionScript.flash.text;
using ScriptCoreLib.ActionScript.MochiLibrary;
using ScriptCoreLib.Shared;

namespace AvalonUgh.Labs.Multiplayer.ActionScript
{
	using TargetCanvas = global::AvalonUgh.NetworkCode.Client.ActionScript.NonobaClient;


	/// <summary>
	/// Default flash player entrypoint class. See 'tools/build.bat' for adding more entrypoints.
	/// </summary>
	[Script, ScriptApplicationEntryPoint(Width = TargetCanvas.DefaultWidth, Height = TargetCanvas.DefaultHeight)]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight)]
	public class NonobaClientFlash : Sprite
	{
		public string _mochiads_game_id = AvalonUgh.Promotion.Info.MochiAds.Key;

		public NonobaClientFlash()
		{
			var c = new TargetCanvas();

			// spawn the wpf control
			AvalonExtensions.AttachToContainer(c.Container, this);
		}

		static NonobaClientFlash()
		{
			// add resources to be found by ImageSource
			KnownEmbeddedAssets.RegisterTo(
				KnownEmbeddedResources.Default.Handlers
			);

		}
	}

	[Script, ScriptApplicationEntryPoint(Width = TargetCanvas.DefaultWidth, Height = TargetCanvas.DefaultHeight)]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight)]
	[Frame(typeof(NonobaClientFlash_MonetizedPreloader))]
	public class NonobaClientFlash_Monetized : NonobaClientFlash
	{
		// this class is to be used
		// on nonoba to get ads and multiplayer support

		// multiplayer shall be enabled internally
	}

	[Script]
	public class NonobaClientFlash_MonetizedPreloader : MochiAdPreloader
	{
		[TypeOfByNameOverride]
		public override Type GetTargetType()
		{
			return typeof(NonobaClientFlash_Monetized);
		}

		public NonobaClientFlash_MonetizedPreloader()
			: base(AvalonUgh.Promotion.Info.MochiAds.Key)
		{

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

			AvalonUgh.Game.ActionScript.KnownEmbeddedAssets.RegisterTo(Handlers);
			AvalonUgh.Assets.ActionScript.KnownEmbeddedAssets.RegisterTo(Handlers);

			//// assets from referenced assemblies
			//Handlers.Add(e => global::ScriptCoreLib.ActionScript.Avalon.Cursors.EmbeddedAssets.Default[e]);
			//Handlers.Add(e => global::ScriptCoreLib.ActionScript.Avalon.TiledImageButton.Assets.Default[e]);

		}
	}
}