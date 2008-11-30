using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Code;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.SneakPeak.Shared
{
	[Script]
	public class OrcasAvalonApplicationCanvas : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 400;

		public OrcasAvalonApplicationCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			this.ClipToBounds = true;

			#region music

			Func<double> NextMusicVolume = new[] { 0.14, 0.07 }.AsCyclicEnumerator().Take;

			Action<double> SetMusicVolume = null;
			Action ApplyNextMusicVolume = () => SetMusicVolume(NextMusicVolume());

			(AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3").Apply(
				(Source, Retry) =>
				{
					var Music = Source.PlaySound();

					Music.PlaybackComplete += Retry;

					SetMusicVolume = Music.SetVolume;
					ApplyNextMusicVolume();

				}
			);

			#endregion

			var CurrentLevel = KnownAssets.Path.Assets + "/level0.txt";

			CurrentLevel.ToStringAsset(
				LevelText =>
				{
					Console.WriteLine(LevelText);

					var Level = new Level(LevelText, 3);

					// subtract statusbar
					var View = new View(DefaultWidth, DefaultHeight, Level);


					//var img = new Image
					//{
					//    Source = (KnownAssets.Path.Assets + "/jsc.png").ToSource()
					//}.MoveTo(DefaultWidth - 128, DefaultHeight - 128).AttachTo(View.TouchInput);

					View.Flashlight.Visible = true;

					View.AttachContainerTo(this);
				}
			);


		}
	}
}
