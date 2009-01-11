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
	using AvalonUgh.Promotion;

	[GoogleGadget(
	   author_email = Info.EMail,
	   author_link = Info.Blog,
	   author = Info.Author,
	   category = Info.GoogleGadget.Category1,
	   category2 = Info.GoogleGadget.Category2,
	   screenshot = Info.Flickr.ScreenshotURL,
	   thumbnail = Info.Flickr.ScreenshotSmallURL,
	   description = Info.Description,
	   width = TargetCanvas.DefaultWidth,
	   height = TargetCanvas.DefaultHeight,
	   title = Info.Title,
	   title_url = Info.Nonoba.URL

	)]
	partial class NonobaClientFlash
	{

	}


}