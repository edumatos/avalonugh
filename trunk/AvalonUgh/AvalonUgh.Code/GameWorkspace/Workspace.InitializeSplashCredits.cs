using System;
using System.ComponentModel;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;
using System.Linq;
using AvalonUgh.Code.Input;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Collections.Generic;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Dialogs;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{

		private void InitializeSplashCredits(List<Dialog> TextContainers)
		{
			Action StartGameWithCredits =
				 delegate
				 {
					 this.Lobby.WhenLoaded(
						 delegate
						 {
							 // we will default to 1 local player
							 //this.Lobby.Menu.Players = 1;

							 Console.WriteLine("lobby loaded");

							 // we should load lobby only once

							 //this.Lobby.Players.AddRange(this.LocalIdentity.Locals.ToArray());

							 Action done = () => Lobby.Window.ColorOverlay.Opacity = 0;

#if DEBUG
							 done(); 
#else
							 ShowSplashCredit(TextContainers, done);
#endif


						 }
					 );

				 };

			this.LocalIdentity.SyncFramePausedChanged +=
				delegate
				{
					if (!this.LocalIdentity.SyncFramePaused)
					{
						if (StartGameWithCredits != null)
							StartGameWithCredits();

						StartGameWithCredits = null;
					}


				};
		}


	}
}
