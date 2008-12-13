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
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Dialogs;

namespace AvalonUgh.Menu.Shared
{
	[Script]
	public class MenuCanvas : Canvas
	{
		public const int Zoom = 2;

		public const int DefaultWidth = 320 * Zoom;
		public const int DefaultHeight = 200 * Zoom;

		public MenuCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			Background = Brushes.Black;

			var TextContainers = new List<Canvas>();

			Canvas TextContainer = null;


			new TopFiveDialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				Scores = new[]
				{
					new TopFiveDialog.Entry("ken", 100),
					new TopFiveDialog.Entry("ken", 200),
					new TopFiveDialog.Entry("ken", 300),
					new TopFiveDialog.Entry("ken", 400),
					new TopFiveDialog.Entry("ken", 500),
				}
			}.AttachContainerTo(this).Container.AddTo(TextContainers);

			new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				BackgroundVisible = false,
				Text = @"

      Level 68
		
    neptuns fork





     password:",
				InputText = "zerosex"
			}.AttachContainerTo(this).Container.AddTo(TextContainers);

			new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				BackgroundVisible = false,
				Text = @"-

   congratulations
 you are one of the
  five best cabbies

",
				LabelText = "enter your name:",
				InputText = "zproxy?"
			}.AttachContainerTo(this).Container.AddTo(TextContainers);

			new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Left,
				Text = @"-
 F1: start game
 F2: enter password
 F3: medium
 F4: multiplayer
 F5: control options
-",
				LabelText = "password:",
				InputText = "none?"
			}.AttachContainerTo(this).Container.AddTo(TextContainers);


			





			new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				TextAlignment = TextAlignment.Left,
				Text = @"
player 1:
 f1: keyboard
player 2:
 f2: keyboard


esc: main menu
				"
			}.AttachContainerTo(this).Container.AddTo(TextContainers);

			new Dialog
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Zoom = Zoom,
				BackgroundVisible = false,
				Text = @"




				   bad luck
				  you failed
				"
			}.AttachContainerTo(this).Container.AddTo(TextContainers);


			var Credits = @"
				 programmed
					 by
			   arvo sulakatko
					with
				jsc compiler
				    in c#
			===
				dos  version
				 programmed
					 by
			   mario knezovic
					with
			  carsten neubauer
			===
				   levels 
				  designed
					 by
				peter schmitz
					 and
				  björn roy
			===
				dos version
				intros coded 
					 by
			   mario knezovic
					with
			   claudia scholz
			===
			      original
			    amiga version
			      programmed
			         by
			   thomas klinger
			        and
			     björn roy
			===
				  original
			   amiga graphics
				   drawn
					 by
			   thomas klinger
			===
			   pc graphics
					by
			  michael detert
				   with
			  carsten neubauer
			   mario knezovic
			===

				music and fx
					by
				maiko ruttmann
			";

			Credits.Split(k => k.Trim() == "===").ForEach(
				Text =>
					new Dialog
					{
						Width = DefaultWidth,
						Height = DefaultHeight,
						Zoom = Zoom,
						Text = Text
					}.AttachContainerTo(this).Container.AddTo(TextContainers)
			);



			TextContainers.ForEach(k => k.Hide());
			TextContainer = TextContainers.First();
			TextContainer.Show();

			//var Status = new TextBox
			//{
			//    Text = "status"
			//}.AttachTo(this);

			var MouseLeftButtonUpDisabled = false;
			this.MouseLeftButtonUp +=
				delegate
				{
					if (MouseLeftButtonUpDisabled)
						return;
					MouseLeftButtonUpDisabled = true;
					//Status.Text = "click";

					TextContainer.Opacity = 1;
					TextContainer.FadeOut(
						delegate
						{
							//Status.Text = "fadeout complete";
							TextContainer = TextContainers.Next(k => k == TextContainer);
							TextContainer.Opacity = 0;
							TextContainer.Show();
							TextContainer.FadeIn(
								delegate
								{
									//Status.Text = "fadein complete";
									MouseLeftButtonUpDisabled = false;

								}
							);
						}
					);


				};



		}
	}
}
