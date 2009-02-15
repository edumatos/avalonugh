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
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;

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
							 this.Lobby.Menu.Players = 1;

							 Console.WriteLine("lobby loaded");

							 // we should load lobby only once

							 //this.Lobby.Players.AddRange(this.LocalIdentity.Locals.ToArray());

							 Action done = () => Lobby.Window.ColorOverlay.Opacity = 0;

							 if (this.Arguments.ShowCredits)
							 {
								 ShowSplashCreditLogo(
									 delegate
									 {
										 ShowSplashCredit(TextContainers, done);
									 }
								 );
							 }
							 else
							 {
								 done();
							 }


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

		void ShowSplashCredit(List<Dialog> TextContainers, Action done)
		{
			this.LocalIdentity.HandleFutureFrameInTime(500,
				 delegate
				 {


					 var CurrentCredit = TextContainers.Random().AttachContainerTo(this.Lobby.Window.OverlayContainer);

					 // we could show credits in the meantime!
					 Lobby.Window.ColorOverlay.Opacity = 0;


					 this.LocalIdentity.HandleFutureFrameInTime(1500,
						 delegate
						 {
							 Lobby.Window.ColorOverlay.Opacity = 1;


							 this.LocalIdentity.HandleFutureFrameInTime(400,
								 delegate
								 {
									 CurrentCredit.OrphanizeContainer();

									 done();

								 }
							 );
						 }
					 );
				 }
			 );
		}

		void ShowSplashCreditLogo(Action done)
		{
			this.LocalIdentity.HandleFutureFrameInTime(500,
				 delegate
				 {


					 var CurrentCredit =
						 new NameFormat
						 {
							 Path = KnownAssets.Path.Backgrounds,
							 Name = "003",
							 Index = -1,
							 Extension = "png"
						 }.ToImage(Arguments.PortWidth, Arguments.PortHeight).AttachTo(this.Lobby.Window.OverlayContainer);


					 // we could show credits in the meantime!
					 Lobby.Window.ColorOverlay.Opacity = 0;


					 this.LocalIdentity.HandleFutureFrameInTime(1500,
						 delegate
						 {
							 Lobby.Window.ColorOverlay.Opacity = 1;


							 this.LocalIdentity.HandleFutureFrameInTime(400,
								 delegate
								 {
									 CurrentCredit.Orphanize();

									 done();

								 }
							 );
						 }
					 );
				 }
			 );
		}


	}
}
