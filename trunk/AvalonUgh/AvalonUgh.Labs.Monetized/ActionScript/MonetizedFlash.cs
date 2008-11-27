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
	using ScriptCoreLib.Shared;
	using ScriptCoreLib.ActionScript.MochiLibrary;

	/// <summary>
	/// Default flash player entrypoint class. See 'tools/build.bat' for adding more entrypoints.
	/// </summary>
	[Script, ScriptApplicationEntryPoint]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight, backgroundColor = 0)]
	[Frame(typeof(MochiPreloader))]
	public class MonetizedFlash : TargetFlash
	{



		/// <summary>
		/// Default constructor
		/// </summary>
		public MonetizedFlash()
		{
			//KnownEmbeddedResources.Default[KnownAssets.Path.Assets + "/Preview.png"].
			//    ToBitmapAsset().AttachTo(this).
			//    MoveTo((TargetCanvas.DefaultWidth - 640) / 2, (TargetCanvas.DefaultHeight - 400) / 2);
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
	[SWF]
	public class MochiPreloader : MochiAdPreloaderBase
	{
		[TypeOfByNameOverride]
		public override DisplayObject CreateInstance()
		{
			return Activator.CreateInstance(typeof(MonetizedFlash)) as DisplayObject;
		}

		public override bool AutoCreateInstance()
		{
			return false;
		}


		public MochiPreloader()
		{
			var Ready = default(Action);

			Ready = delegate
			{
				// 1 more
				Ready = delegate
				{
					// done
					Ready = delegate
					{
						// nothing
					};

					CreateInstance().AttachTo(this);
				};
			};

			this.InvokeWhenStageIsReady(
				delegate
				{
					stage.scaleMode = StageScaleMode.NO_SCALE;
					stage.align = StageAlign.TOP_LEFT;

					_mochiads_game_id = AvalonUgh.Promotion.Info.MochiAds.Key;

					showPreGameAd(
						() => Ready()
						, stage.stageWidth, stage.stageHeight
					);
				}
			);

			this.LoadingComplete += () => Ready();
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