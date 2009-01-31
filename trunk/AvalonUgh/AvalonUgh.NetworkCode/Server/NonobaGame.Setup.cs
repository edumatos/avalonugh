using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using Nonoba.GameLibrary;
using AvalonUgh.NetworkCode.Shared;
using ScriptCoreLib.Shared.Nonoba.Generic;

namespace AvalonUgh.NetworkCode.Server
{


	[GameSetup.Boolean(
		"mojo",
		"Enable mojo",
		"Enable mojo in game",
		false)]
	[GameSetup.String(
		"password",
		"Level Password",
		"Level Password",
		"")]
	[GameSetup.Integer(
		"players",
		"Local Players",
		"Local Players",
		0, 3, 
		0)]

	[GameSetup.Integer(
		"framelimit",
		"Frame Limit",
		"A larger limit is needed for larger lag.",
		3, 12,
		7)]
	
	partial class NonobaGame 
	{
		

	}
}
