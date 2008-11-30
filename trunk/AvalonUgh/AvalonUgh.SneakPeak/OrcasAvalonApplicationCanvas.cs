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
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Input;
using System.Windows.Input;
using AvalonUgh.Assets.Shared;

namespace AvalonUgh.SneakPeak.Shared
{
	[Script]
	public class OrcasAvalonApplicationCanvas : Canvas
	{
		public const int Zoom = 4;

		public const int Padding = 0;

		public const int DefaultWidth = 600 + Padding * 2;
		public const int DefaultHeight = 400 + Padding * 2;

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

					var Level = new Level(LevelText, Zoom);

					// subtract statusbar
					var View = new View(DefaultWidth, DefaultHeight, Level);


					#region create vehicle
					var xveh = new Vehicle(Zoom)
					{
						ColorStripe = Colors.Yellow,
						CurrentLevel = Level
					};

					Level.KnownVehicles.Add(xveh);

					xveh.AttachContainerTo(View.Entities);
					xveh.MoveTo(Level.ActualWidth / 2, xveh.HalfHeight);
					#endregion


					#region collision sound


					Level.Physics.CollisionAtVelocity +=
						Velocity =>
						{
							var Volume = (Velocity / (Level.Zoom * 3.0)).Max(0).Min(1);

							Console.WriteLine(new { Volume, Velocity });

							if (Volume > 0)
								(Assets.Shared.KnownAssets.Path.Audio + "/bounce.mp3").PlaySound().SetVolume(Volume);

							//Console.WriteLine(Velocity);
						};

					#endregion

					#region focus issues


					View.Container.FocusVisualStyle = null;
					View.Container.Focusable = true;
					View.Container.Focus();

					View.Container.MouseLeftButtonDown +=
						(sender, args) =>
						{
							View.Container.Focus();
						};


					#endregion


					#region player setup

					View.LocationTracker.Target = xveh;

					var k1 = new KeyboardInput(
						new KeyboardInput.Arguments
						{
							Left = Key.Left,
							Right = Key.Right,
							Up = Key.Up,
							Down = Key.Down,
							Drop = Key.Space,
							Enter = Key.Enter,

							InputControl = View.Container,
							//Vehicle = xveh,
							//View = View
						}
					);

					var k3 = new PlayerInput
					{
						Keyboard = k1,
						Touch = View.TouchInput
					};

					ISupportsPlayerInput k3target = xveh;

					Actor xveh_man = new Actor.man0(Zoom)
					{
						Animation = Actor.AnimationEnum.Panic,
						RespectPlatforms = true,
						Level = Level,
						CanBeHitByVehicle = false
					};

					xveh_man.GoldStash.ForEachNewItem(
						gold =>
						{
							(Assets.Shared.KnownAssets.Path.Audio + "/treasure.mp3").PlaySound();

							View.ColorOverlay.Background = Brushes.Yellow;
							View.ColorOverlay.Opacity = 0.7;
							View.ColorOverlay.Show();
							View.ColorOverlay.FadeOut();
						}
					);

					xveh_man.CurrentVehicle = xveh;



					k3.Enter +=
						delegate
						{
							if (xveh.IsUnmanned)
							{
								if (xveh_man != null)
								{
									// AI is controlling our man
									// possibly entering the cave already
									if (xveh_man.AIInputEnabled)
										return;

									if (xveh_man.CurrentCave != null)
									{
										;

										return;
									}

									var ManAsObstacle = xveh_man.ToObstacle();

									// are we trying to enter a cave?
									var NearbyCave = Level.KnownCaves.FirstOrDefault(k => k.ToObstacle().Intersects(ManAsObstacle));

									if (NearbyCave != null)
									{
										// we need to align us in front of the cave
										// and show entering animation

										AIDirector.WalkActorToTheCaveAndEnter(xveh_man, NearbyCave,
											delegate
											{
												AIDirector.ActorExitAnyCave(xveh_man, Level.KnownCaves.Where(k => k != xveh_man.CurrentCave).Random());
											}
										);

										Console.WriteLine("entering a cave");

										return;
									}


									if (!ManAsObstacle.Intersects(xveh.ToObstacle()))
									{
										Console.WriteLine("try geting closer to a vehicle!");
										return;
									}

									Level.KnownActors.Remove(xveh_man);

									xveh_man.CurrentVehicle = xveh;
									xveh_man.OrphanizeContainer();
								}

								xveh.IsUnmanned = false;
								View.LocationTracker.Target = xveh;
								k3target = xveh;
							}
							else
							{
								xveh.IsUnmanned = true;
								xveh_man.CurrentVehicle = null;


								View.LocationTracker.Target = xveh_man;
								k3target = xveh_man;

								xveh_man.MoveTo(xveh.X, xveh.Y - xveh_man.ToObstacle().Height / 2);

								xveh_man.AttachContainerTo(View.Entities);
								Level.KnownActors.Add(xveh_man);
							}
						};

					k3.Drop +=
						delegate
						{
							var rock = xveh.CurrentWeapon;

							if (rock == null)
								return;

							rock.MoveTo(xveh.X, xveh.Y);
							rock.VelocityX = xveh.VelocityX;
							rock.VelocityY = xveh.VelocityY;

							rock.Container.Show();
							rock.PhysicsDisabled = false;
							rock.Stability = 0;

							xveh.CurrentWeapon = null;


						};

					#endregion

					// physics
					(1000 / 40).AtInterval(
						delegate
						{
							k3target.AddAcceleration(k3);


							Level.Physics.Apply();

						}
					);


					//View.Flashlight.Visible = true;


					


					View.AttachContainerTo(this);


					(Assets.Shared.KnownAssets.Path.Audio + "/newlevel.mp3").PlaySound();

					#region level editor
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
					#endregion


					View.Container.KeyUp +=
						(sender, args) =>
						{
							//if (args.Key == Key.F)
							//    View.Flashlight.Visible = !View.Flashlight.Visible;

							//if (args.Key == Key.G)
							//    View.IsFilmScratchEffectEnabled = !View.IsFilmScratchEffectEnabled;

							//if (args.Key == Key.T)
							//{
							//    et.Container.ToggleVisible();
							//    View.EditorSelectorRectangle.ToggleVisible();
							//}

							if (args.Key == Key.M)
								ApplyNextMusicVolume();
						};

					et.Container.ToggleVisible();
					View.EditorSelectorRectangle.ToggleVisible();

					Level.KnownGold.ListChanged +=
						delegate
						{
							Console.WriteLine("gold left: " + Level.KnownGold.Count);

							if (Level.KnownGold.Count == 0)
							{
								View.ColorOverlay.Background = Brushes.Black;
								View.ColorOverlay.Opacity = 0;
								View.ColorOverlay.Show();
								View.ColorOverlay.FadeIn();
							}
						};
				}
			);


		}
	}
}
