using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

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



		public readonly BindingList<PlayerInfo> Players = new BindingList<PlayerInfo>();

		/// <summary>
		/// This will reflect the clients name and number,
		/// We could be in control of none or multiple actors or vehicles
		/// </summary>
		public readonly PlayerIdentity LocalIdentity = new PlayerIdentity { Name = "LocalPlayer" };



		public readonly Stack<PlayerInput> AvailableInputs;

		// what if we would allow multiple levels to be loaded at the same time?
		// like some could enter a cave and such
		public View View { get; set; }

		public GameCanvas()
		{
			(AvalonUgh.Assets.Shared.KnownAssets.Path.Audio + "/ugh_music.mp3").Apply(
				(Source, Retry) =>
				{
					var Music = Source.PlaySound();

					Music.PlaybackComplete += Retry;
					Music.SetVolume(0.10);
				}
			);

			#region AvailableInputs
			this.AvailableInputs = new Stack<PlayerInput>();

			this.AvailableInputs.Push(
				new PlayerInput
				{
					Keyboard =
						new KeyboardInput(
							new KeyboardInput.Arguments.IJKL
							{
								InputControl = this,
							}
						)
				}
			);

			this.AvailableInputs.Push(
				new PlayerInput
				{
					Keyboard = new KeyboardInput(
						new KeyboardInput.Arguments.WASD
						{
							InputControl = this,
						}
					),
					// no touch - we do not have multitouch yet
					//Touch = View.TouchInput
				}
			);

			this.AvailableInputs.Push(
				new PlayerInput
				{
					Keyboard =
						new KeyboardInput(
							new KeyboardInput.Arguments.Arrows
							{
								InputControl = this,
							}
						)
				}
			);
			#endregion

			#region Locals

			this.LocalIdentity.Locals.ForEachNewItem(i => Players.Add(i));
			this.LocalIdentity.Locals.ForEachItemDeleted(i => Players.Remove(i));

			#endregion


			Width = DefaultWidth;
			Height = DefaultHeight;

			// prototype the new menu

			var LobbyLevel = KnownAssets.Path.Assets + "/level01.txt";

			#region setting up our console
			this.Console = new GameConsole();

			this.Console.SizeTo(DefaultWidth, DefaultHeight / 2);
			this.Console.WriteLine("Avalon Ugh! Console ready.");
			this.Console.AnimatedTop = -this.Console.Height;
			#endregion

			LobbyLevel.ToStringAsset(
				LevelText =>
				{
					var Level = new Level(LevelText, Zoom);

					Level.Physics.CollisionAtVelocity +=
						Velocity =>
						{
							var Volume = (Velocity / (Level.Zoom * 3.0)).Max(0).Min(1);


							if (Volume > 0)
								(Assets.Shared.KnownAssets.Path.Audio + "/bounce.mp3").PlaySound().SetVolume(Volume);

							// we dont want to log collision info anymore... maybe later
							//Console.WriteLine("CollisionAtVelocity " + new { Velocity, Volume });
						};

					// in menu mode the view does not include status bar
					// yet later in game we should adjust that
					View = new View(DefaultWidth, DefaultHeight, Level);

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








					this.KeyUp +=
						(sender, args) =>
						{
							// oem7 will trigger the console
							if (args.Key == Key.Oem7)
							{
								args.Handled = true;
								if (Console.AnimatedTop == 0)
								{
									Console.WriteLine("hide console");
									Console.AnimatedTop = -Console.Height;
								}
								else
								{
									Console.WriteLine("show console");
									Console.AnimatedTop = 0;
								}

								// the console is on top
								// of the game view
								// and under the transparent touch overlay
								// when the view is in editor mode
							}

							if (args.Key == Key.Tab)
							{
								args.Handled = true;

								if (LocalIdentity.Locals.Count > 0)
								{
									var NextFocusedLocal = this.LocalIdentity.Locals.Next(k => k == this.View.LocationTracker.Target);

									this.View.LocationTracker.Target = NextFocusedLocal;
									this.Focus();
								}
							}

							if (args.Key == Key.F)
								View.Flashlight.Visible = !View.Flashlight.Visible;

							if (args.Key == Key.G)
							{
								View.IsFilmScratchEffectEnabled = !View.IsFilmScratchEffectEnabled;
							}

							if (args.Key == Key.T)
							{
								et.Container.ToggleVisible();
								View.EditorSelectorRectangle.ToggleVisible();
							}
						};

					Console.AttachContainerTo(this);

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


					Players.ForEachNewItem(
						NewPlayer =>
						{
							// here we will need to create an actor
							// or the vehicle where he is in?
							Console.WriteLine("new ingame player added: " + NewPlayer);

							// lets create a dummy actor
							NewPlayer.Actor = new Actor.woman0(Zoom)
							{
								Animation = Actor.AnimationEnum.Panic,
								RespectPlatforms = true,
								Level = Level,
								CanBeHitByVehicle = false
							};

							if (View.LocationTracker.Target == null)
								View.LocationTracker.Target = NewPlayer;

							// we are not inside a vehicle
							// nor are we inside a cave

							// where are the spawnpoints in this level?
							NewPlayer.Actor.MoveTo((DefaultWidth / 4) + (DefaultWidth / 2).Random(), DefaultHeight / 2);

							// we need to play jumping sound
							NewPlayer.Actor.Jumping +=
								delegate
								{
									(Assets.Shared.KnownAssets.Path.Audio + "/jump.mp3").PlaySound();
								};

							NewPlayer.Actor.EnterCave +=
								delegate
								{
									var ManAsObstacle = NewPlayer.Actor.ToObstacle();

									// are we trying to enter a cave?
									var NearbyCave = Level.KnownCaves.FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

									if (NearbyCave != null)
									{
										// we need to align us in front of the cave
										// and show entering animation

										Console.WriteLine("entering a cave");

										AIDirector.WalkActorToTheCaveAndEnter(NewPlayer.Actor, NearbyCave,
											delegate
											{
												Console.WriteLine("inside a cave");

												// should we load another level and enter that?
												// for the first version lets keep it simple
												// lets just exit another cave

												if (Level.KnownCaves.Count == 0)
												{
													// whatif the cave is destroyed?
													AIDirector.ActorExitCaveFast(NewPlayer.Actor);
													return;
												}

												var NextCave = Level.KnownCaves.Next(k => k == NearbyCave);


												AIDirector.ActorExitAnyCave(NewPlayer.Actor, NextCave);
											}
										);


										return;
									}
								};

							NewPlayer.Actor.EnterVehicle +=
								delegate
								{
									// exiting a vehicle is easy
									// entering is a bit harder
									// as we need to find it and reserve its use for us

									var ManAsObstacle = NewPlayer.Actor.ToObstacle();

									var NearbyVehicle = Level.KnownVehicles.Where(k => k.CurrentDriver == null).FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

									if (NearbyVehicle != null)
									{
										NearbyVehicle.CurrentDriver = NewPlayer.Actor;
									}
								};


							NewPlayer.Actor.CurrentVehicleChanged +=
								delegate
								{
									(Assets.Shared.KnownAssets.Path.Audio + "/enter.mp3").PlaySound();
								};

							// every actor could act differently on gold collected
							NewPlayer.Actor.GoldStash.ForEachNewItem(
								gold =>
								{
									(Assets.Shared.KnownAssets.Path.Audio + "/treasure.mp3").PlaySound();

									View.ColorOverlay.Background = Brushes.Yellow;
									View.ColorOverlay.Opacity = 0.7;
									View.ColorOverlay.Show();
									View.ColorOverlay.FadeOut();
								}
							);

							NewPlayer.Actor.AttachContainerTo(View.Entities);
							Level.KnownActors.Add(NewPlayer.Actor);
						}
					);

					Players.ForEachItemDeleted(
						DeletedPlayer =>
						{
							if (View.LocationTracker.Target == DeletedPlayer.Actor)
								View.LocationTracker.Target = null;

							Console.WriteLine("ingame player deleted: " + DeletedPlayer);

							if (DeletedPlayer.Actor.VelocityY == 0)
							{
								// we can perform the walk out to the horizon
								DeletedPlayer.Actor.PlayAnimation(Actor.AnimationEnum.CaveEnter,
									delegate
									{
										DeletedPlayer.Actor.Dispose();
										Level.KnownActors.Remove(DeletedPlayer.Actor);
									}
								);

								return;
							}

							DeletedPlayer.Actor.Dispose();
							Level.KnownActors.Remove(DeletedPlayer.Actor);
						}
					);



					Action Locals_Increase =
						delegate
						{
							// do we have any predefined key sets to allow 
							// another player?
							if (this.AvailableInputs.Count == 0)
								return;

							var i = this.AvailableInputs.Pop();

							var p = new PlayerInfo
							{
								Identity = this.LocalIdentity,
								Input = i,
							};

							// the first player can be controlled by
							// touch input and if we had multitouch so would others
							if (this.LocalIdentity.Locals.Count == 0)
							{
								// not yet
								//i.Touch = View.TouchInput;
							}



							// add this new player to the list
							// thus making it also visible
							this.LocalIdentity.Locals.Add(p);

							(Assets.Shared.KnownAssets.Path.Audio + "/newlevel.mp3").PlaySound();
						};

					Action Locals_Decrase =
						delegate
						{
							// we are going to remove a local player

							// are we playerless already?
							if (this.LocalIdentity.Locals.Count == 0)
								return;

							var p = this.LocalIdentity.Locals.Last();
							var i = p.Input;

							i.Touch = null;

							this.AvailableInputs.Push(i);
							this.LocalIdentity.Locals.Remove(p);

							if (this.LocalIdentity.Locals.Count == 0)
								this.View.LocationTracker.Target = null;

							(Assets.Shared.KnownAssets.Path.Audio + "/gameover.mp3").PlaySound();
						};

					this.KeyUp +=
						(sender, args) =>
						{
							// are we observing or do we do 
							// single player, two player or 3 player?

							if (args.Key == Key.Insert)
							{
								Locals_Increase();
							}

							if (args.Key == Key.Delete)
							{
								Locals_Decrase();
							}
						};


					// at this time we should add a local player
					TouchContainer.MouseLeftButtonDown +=
						(sender, args) =>
						{
							this.Focus();
						};

					// toolbar is on top of our touch container
					et.AttachContainerTo(this);

					this.View.EditorSelectorApplied +=
						(Selector, Position) =>
						{
							if (Selector.Width == 0)
								return;

							Console.WriteLine("EditorSelectorApplied");

							(Assets.Shared.KnownAssets.Path.Audio + "/place_tile.mp3").PlaySound();
						};

					this.View.EditorSelectorNextSize += () => et.EditorSelectorNextSize();
					this.View.EditorSelectorPreviousSize += () => et.EditorSelectorPreviousSize();

					// activate the game loop

					this.LocalIdentity.SyncFrameRate = 1000 / 60;
					//this.LocalIdentity.SyncFrameRate = 1000 / (40.Random() + 20);

					this.KeyUp +=
						(sender, args) =>
						{
							// allow single frame step

							if (args.Key == Key.PageUp)
							{
								this.LocalIdentity.SyncFramePausedSkip = true;
							}
						};


					Action FrameTick =
						delegate
						{
							if (this.LocalIdentity.SyncFramePaused)
							{
								if (this.LocalIdentity.SyncFramePausedSkip)
								{
									this.LocalIdentity.SyncFramePausedSkip = false;
								}
								else
								{
									return;
								}
							}



							// we could pause the game here
							foreach (var p in Players)
							{
								p.AddAcceleration();
							}

							Level.Physics.Apply();

							this.LocalIdentity.SyncFrame++;
						};

					FrameTick.AtInterval(
						() =>
						{
							var r = this.LocalIdentity.SyncFrameRate;

							if (this.LocalIdentity.SyncFrameRateCorrection > 0)
							{
								this.LocalIdentity.SyncFrameRateCorrection--;

								r -= r / 10;
							}
							else if (this.LocalIdentity.SyncFrameRateCorrection < 0)
							{
								this.LocalIdentity.SyncFrameRateCorrection++;

								r += r / 10;
							}

							return r;
						}
					);



					Console.WriteLine("load complete!");


					5000.AtDelay(
						() => (Assets.Shared.KnownAssets.Path.Audio + "/bird_cry.mp3").PlaySound()
					);

					25000.AtDelay(
						() => (Assets.Shared.KnownAssets.Path.Audio + "/bird_cry.mp3").PlaySound()
					);

					// add the first local
					Locals_Increase();
				}
			);
		}



	}
}
