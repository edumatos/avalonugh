﻿using System;
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
using System.Windows.Input;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Game.Shared
{
	[Script]
	public class GameCanvas : Canvas
	{
		public const int Zoom = 2;

		public const int DefaultWidth = 640;
		public const int DefaultHeight = 400;

		public GameConsole Console { get; set; }
		public Canvas TouchContainer { get; set; }
		public PlayerInput DefaultPlayerInput { get; set; }

		public GameCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			// prototype the new menu

			var LobbyLevel = KnownAssets.Path.Assets + "/level00.txt";

			#region setting up our console
			var con = new GameConsole();

			con.SizeTo(DefaultWidth, DefaultHeight / 2);
			con.WriteLine("Avalon Ugh! Console ready.");
			con.AnimatedTop = 0;
			#endregion

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

					#region editor

					var et = new EditorToolbar(this);

					// move it to bottom center
					et.MoveContainerTo((DefaultWidth - et.Width) / 2, DefaultHeight - et.Padding * 2 - PrimitiveTile.Heigth * 2);


					et.EditorSelectorChanged +=
						() => View.EditorSelector = et.EditorSelector;

					View.EditorSelector = et.EditorSelector;

					// serialize current level
					et.LevelText.GotFocus +=
						delegate
						{
							et.LevelText.Text = Level.ToString();
						};

					#endregion



					#region some menu mockup
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
					#endregion

				


		

			
					this.Console = con;

					this.KeyUp +=
						(sender, args) =>
						{
							// oem7 will trigger the console
							if (args.Key == Key.Oem7)
							{
								if (con.AnimatedTop == 0)
								{
									con.WriteLine("hide console");
									con.AnimatedTop = -con.Height;
								}
								else
								{
									con.WriteLine("show console");
									con.AnimatedTop = 0;
								}

								// the console is on top
								// of the game view
								// and under the transparent touch overlay
								// when the view is in editor mode
							}
						};

					con.AttachContainerTo(this);

					using (this.Console["updating touch layers"])
					{
						TouchContainer = new Canvas
						{
							Background = Brushes.Black,
							Opacity = 0,
							Width = DefaultWidth,
							Height = DefaultHeight
						};
						TouchContainer.AttachTo(this);

						// we are doing some advanced layering now
						var TouchContainerForViewContent = new Canvas
						{
							// we need to update this if the level changes
							// in size
							Width = View.ContentExtendedWidth,
							Height = View.ContentExtendedHeight
						}.AttachTo(TouchContainer);

						View.ContentExtendedContainerMoved +=
							(x, y) => TouchContainerForViewContent.MoveTo(x, y);

						// raise that event so we stay in sync
						View.MoveContentTo();
						View.TouchOverlay.Orphanize().AttachTo(TouchContainerForViewContent);
					}

					// we are going for the keyboard input
					// we want to enable the tilde console feature
					this.FocusVisualStyle = null;
					this.Focusable = true;
					this.Focus();

					var DefaultPlayerInput = new PlayerInput
					{
						Keyboard =  new KeyboardInput(
							new KeyboardInput.Arguments
							{
								Left = Key.Left,
								Right = Key.Right,
								Up = Key.Up,
								Down = Key.Down,
								Drop = Key.Space,
								Enter = Key.Enter,

								InputControl = this,
							}
						),
						Touch = View.TouchInput
					};

					DefaultPlayerInput.Enter +=
						delegate
						{
							Console.WriteLine("you pressed enter");
						};

					// at this time we should add a local player

					TouchContainer.MouseLeftButtonDown +=
						(sender, args) =>
						{
							this.Focus();
						};

					// toolbar is on top of our touch container
					et.AttachContainerTo(this);

				}
			);
		}
	}
}
