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
using AvalonUgh.Code.Editor;
using AvalonUgh.Code;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Dialogs;

namespace AvalonUgh.Game.Shared
{
	[Script]
	public class GameCanvas : Canvas
	{
		public const int Zoom = 2;

		public const int DefaultWidth = 640;
		public const int DefaultHeight = 400;

		public GameCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			// prototype the new menu

			var LobbyLevel = KnownAssets.Path.Assets + "/level00.txt";

			LobbyLevel.ToStringAsset(
				LevelText =>
				{
					var Level = new Level(LevelText, Zoom);
				
					// in menu mode the view does not include status bar
					// yet later in game we should adjust that
					var View = new View(DefaultWidth, DefaultHeight, Level);

					View.AttachContainerTo(this);
					View.EditorSelector = null;

					// to modify the first level we are enabling the 
					// editor
					
					/*
					var et = new EditorToolbar(this);

					et.MoveContainerTo((DefaultWidth - et.Width) / 2, DefaultHeight - et.Padding * 2 - PrimitiveTile.Heigth * 2);
					et.AttachContainerTo(this);

					et.EditorSelectorChanged +=
						() => View.EditorSelector = et.EditorSelector;

					View.EditorSelector = et.EditorSelector;

					et.LevelText.GotFocus +=
						delegate
						{
							et.LevelText.Text = Level.ToString();
						};
					*/

					
					new Image
					{
						Source = (Assets.Shared.KnownAssets.Path.Levels + "/level0_02.png").ToSource(),
						Stretch = Stretch.Fill
					}.SizeTo(80, 50).MoveTo(DefaultWidth - 160, DefaultHeight / 2 - 50).AttachTo(this);

					new DialogTextBox
					{
						Text = " start game",
						Zoom = Zoom,
						Width = DefaultWidth
					}.MoveContainerTo(0, DefaultHeight / 2 - 50).AttachContainerTo(this);

				}
			);
		}
	}
}
