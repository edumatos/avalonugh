using System;
using System.Collections.Generic;
using System.IO;
using ScriptCoreLib;
using ScriptCoreLib.ActionScript;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.flash.display;
using ScriptCoreLib.ActionScript.flash.text;
using ScriptCoreLib.ActionScript.flash.ui;
using ScriptCoreLib.ActionScript.MochiLibrary;
using ScriptCoreLib.Shared;

namespace AvalonUgh.Labs.Monetized.ActionScript
{
	using TargetCanvas = global::AvalonUgh.Labs.Shared.LabsCanvas;
	using TargetFlash = global::AvalonUgh.Labs.ActionScript.LabsFlash;


	/// <summary>
	/// Default flash player entrypoint class. See 'tools/build.bat' for adding more entrypoints.
	/// </summary>
	[Script, ScriptApplicationEntryPoint]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight, backgroundColor = 0)]
	[Frame(typeof(MochiPreloader))]
	public class MonetizedFlash : TargetFlash
	{
	}

	[Script]
	public class MochiPreloader : MochiAdPreloader
	{
		[TypeOfByNameOverride]
		public override Type GetTargetType()
		{
			return typeof(MonetizedFlash);
		}

		public MochiPreloader() : base(AvalonUgh.Promotion.Info.MochiAds.Key)
		{
			
		}
	}



}