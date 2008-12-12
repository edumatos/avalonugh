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

			var y = 0.0;

			var TextContainers = new List<Canvas>();

			Canvas TextContainer = null;

			#region WriteBitmapFont

			Action<string, string> WriteBitmapFont =
				(font, text) =>
				{
					var x = 0;
					
					var UseShortNewLine = false;

					foreach (char c in text.ToLower())
					{
						var s = Convert.ToString(c);

						if (s == "?")
							s = "_Question";

						if (s == ":")
							s = "_Colon";

						if (s == ".")
							s = "_Dot";

						if (s == " ")
						{
							x++;
							continue;
						}

						if (s == "-")
						{
							UseShortNewLine = true;
							continue;
						}

						if (s == "\r")
						{
							continue;
						}

						if (s == "\n")
						{
							if (UseShortNewLine) 
							{
								y += 0.5;
								UseShortNewLine = false;
							}
							else
							{
								y++;
							}
							x = 0;

							continue;
						}

						new Image
						{
							Source =
								(font + "/" + s + ".png").ToSource(),
							Stretch = Stretch.Fill,
							Width = PrimitiveFont.Width * Zoom,
							Height = PrimitiveFont.Heigth * Zoom
						}.AttachTo(TextContainer).MoveTo(x * (PrimitiveFont.Width + 1) * Zoom, y * PrimitiveFont.Heigth * Zoom);

						x++;

					}

					y++;
				};
			#endregion

			var WriteBlue = WriteBitmapFont.FixFirstParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Blue);
			var WriteBrown = WriteBitmapFont.FixFirstParam(AvalonUgh.Assets.Shared.KnownAssets.Path.Fonts.Brown);

			#region hiscore
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Background  = Brushes.Black
			}.AttachTo(this).AddTo(TextContainers);

			WriteBrown(@" 



   congratulations
 you are one of the
  five best cabbies

enter your name:
"
);
			WriteBlue("zproxy?");

			#endregion


			#region main menu
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),
				
				Width = DefaultWidth,
				Height = DefaultHeight,


			}.AttachTo(TextContainer);

			WriteBrown(@" 



-
 F1: start  game
 F2: enter password
 F3: enter medium
 F4: multiplayer
 F5: control options
-
password:"
);

			WriteBlue("none?");
			#endregion


			#region top five
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 



-
      top five
ingognitus     11199
ingognitus     11199
ingognitus     11199
ingognitus     67890
ingognitus     12345
"
);

			#endregion


			#region level start
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Background = Brushes.Black
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
	

			WriteBrown(@" 



      Level 68
-
    neptuns fork



     password:");
			
			WriteBlue("zerosex?");


			#endregion

			#region programmed by
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 




     programmed
         by
   arvo sulakatko
        with
    jsc compiler
");



			#endregion

			#region programmed by
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 



    dos  version
     programmed
         by
   mario knezovic
        with
  carsten neubauer
");



			#endregion

			#region failed
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
				Background = Brushes.Black
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;


			WriteBrown(@" 



       bad luck
      you failed



");



			#endregion


			#region options
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 




player 1:
 f1: keyboard
player 2:
 f2: keyboard
...

Esc: main menu
");



			#endregion


			#region levels
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 




       levels 
      designed
         by
    peter schmitz
         and
      björn roy
");
			#endregion




			#region intros
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 



    dos version
    intros coded 
         by
   mario knezovic
        with
   claudia scholz
");


			#endregion

			#region amiga version
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 



      original
    amiga version
      programmed
         by
   thomas klinger
        and
     björn roy
");


			#endregion

			#region amiga gfx
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 




      original
   amiga graphics
       drawn
         by
   thomas klinger
");


			#endregion

			#region pc gfx
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 




    pc graphics
        by
  michael detert
       with
  carsten neubauer
   mario knezovic
");


			#endregion


			#region music and fx
			TextContainer = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this).AddTo(TextContainers);
			y = 0;
			new Image
			{
				Stretch = Stretch.Fill,
				Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/005.png").ToSource(),

				Width = DefaultWidth,
				Height = DefaultHeight,

			}.AttachTo(TextContainer);

			WriteBrown(@" 





    music and fx
        by
  maiko ruttmann
");


			#endregion

			TextContainers.ForEach(k => k.Hide());

			TextContainer = TextContainers.Next(k => k == TextContainer);
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
