using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Assets.Avalon
{
	[Script]
	public class SoundBoard
	{
		public static readonly SoundBoard Default = new SoundBoard();

		public readonly Action enter = f("enter");
		public readonly Action talk0_00 = f("talk0_00");

		static Action f(string e)
		{
			return () => (KnownAssets.Path.Audio + "/" + e + ".mp3").PlaySound();
		}
	}
}
