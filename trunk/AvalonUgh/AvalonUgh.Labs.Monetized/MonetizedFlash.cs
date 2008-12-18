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


	[Script]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight, backgroundColor = 0)]
	public class MonetizedFlashLean : TargetFlash
	{
		// this class is to be used
		// on mochiads with versioning and crypt
		// as ve define the key as a field here

		public static string _mochiads_game_id = AvalonUgh.Promotion.Info.MochiAds.Key;

		// multiplayer shall be enabled via hyperlink to nonobas version

		

		public MonetizedFlashLean()
		{
			//new TextField
			//{
			//    text = "multiplayer disabled"
			//}.AttachTo(this);
		}
	}

	
	[Script]
	[SWF(width = TargetCanvas.DefaultWidth, height = TargetCanvas.DefaultHeight, backgroundColor = 0)]
	[Frame(typeof(MochiPreloader))]
	public class MonetizedFlash : TargetFlash
	{
		// this class is to be used
		// on nonoba to get ads and multiplayer support

		// multiplayer shall be enabled internally
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