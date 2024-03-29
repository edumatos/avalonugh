﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using AvalonUgh.Assets.Avalon;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{

		private void ThinkForComputerPlayers(View view)
		{
			var PickupVehicles = Enumerable.ToArray(
				from Vehicle in view.Level.KnownVehicles
				where Vehicle.CurrentWeapon == null
				where Vehicle.CurrentDriver != null
				where Vehicle.CurrentPassengers.Count == 0
				where Vehicle.LastVelocity == 0
				where Vehicle.GetVelocity() == 0
				let VehicleObstacle = Vehicle.ToObstacle()
				select new { Vehicle, VehicleObstacle }
			);

			const double TryoSpeed = 1.5;

			#region KnownTryoperus
			foreach (var CurrentTryo in view.Level.KnownTryoperus)
			{
				if (CurrentTryo.VelocityY == 0)
				{
					var CurrentPlatform = view.Level.ToPlatformSnapshots().FirstOrDefault(k => k.IncludedSpace.Intersects(CurrentTryo.ToObstacle()));

					if (CurrentPlatform == null)
					{
						CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Left_Stand;
					}
					else
					{
						var CurrentPickup = PickupVehicles.FirstOrDefault(k => k.VehicleObstacle.Intersects(CurrentPlatform.IncludedSpace));

						if (CurrentPickup != null)
						{
							//if (CurrentPickup.VehicleObstacle.X < CurrentTryo.X)
							CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Left_Stare;
							//else
							//    CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Right_Stare;
						}
						else
						{
							#region just walk around
							if (CurrentTryo.VelocityX > 0)
							{
								if (CurrentTryo.ToObstacle(CurrentTryo.X + TryoSpeed, CurrentTryo.Y).Right < CurrentPlatform.IncludedSpace.Right)
								{
									CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Right_Walk;
									CurrentTryo.VelocityX = TryoSpeed;
								}
								else
								{
									CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Right_Stand;
									CurrentTryo.VelocityX = -TryoSpeed;
								}
							}
							else
							{
								if (CurrentTryo.ToObstacle(CurrentTryo.X - TryoSpeed, CurrentTryo.Y).Left > CurrentPlatform.IncludedSpace.Left)
								{
									CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Left_Walk;
									CurrentTryo.VelocityX = -TryoSpeed;
								}
								else
								{
									CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Left_Stand;
									CurrentTryo.VelocityX = TryoSpeed;
								}
							}
							#endregion

						}
					}
				}
				else
				{
					CurrentTryo.Animation = AvalonUgh.Code.Editor.Sprites.Tryoperus.AnimationEnum.Left_Hit;
				}
			}
			#endregion


			#region bird
			foreach (var bird in view.Level.KnownBirds)
			{
				if (bird.IsSleeping)
				{
					if (bird.Y > view.ContentExtendedHeight)
					{
						// bird has dorwned

						view.Level.KnownVehicles.Where(k => k.CurrentDriver != null).FirstOrDefault().Apply(k => bird.Y = k.Y);


						bird.Y = view.ContentActualHeight / 2;
						bird.X = -bird.Width;
						bird.VelocityX = 1.5 * view.Level.Zoom;
						bird.VelocityY = -0.1 * view.Level.Zoom;
						bird.IsSleeping = false;

						SoundBoard.Default.bird_cry();
					}
				}
				else
				{
					bird.VelocityY = -0.1 * view.Level.Zoom;

					if (bird.VelocityX <= 0)
					{
						bird.VelocityX = -1.5 * view.Level.Zoom;

						if (bird.X < -bird.Width)
						{
							view.Level.KnownVehicles.Where(k => k.CurrentDriver != null).FirstOrDefault().Apply(k => bird.Y = k.Y);


							bird.X = -bird.Width;
							bird.VelocityX = 1.5 * view.Level.Zoom;

							SoundBoard.Default.bird_cry();

						}
					}
					else
					{
						bird.VelocityX = 1.5 * view.Level.Zoom;

						if (bird.X > (view.ContentExtendedWidth + bird.Width))
						{
							view.Level.KnownVehicles.Where(k => k.CurrentDriver != null).FirstOrDefault().Apply(k => bird.Y = k.Y);

							bird.X = (view.ContentExtendedWidth + bird.Width);
							bird.VelocityX = -1.5 * view.Level.Zoom;


							SoundBoard.Default.bird_cry();
						}
					}

				}

			}
			#endregion



			//if (this.LocalIdentity.SyncFrame % 20 != 0)
			//    return;

			// a player is created on a different platform
			// if prvious actor has stopped walking and is at its 
			// wait position

			// if there is alreadysomeone waiting on the platform
			// the passenger will not exit the cave

			// only if the compiler could compile that previous dml statement :)


			// these beliefes on current environment
			// could be saved for some frames
			// altho when the map is synced
			// with new players
			// the beliefs should be dropped and recalculated
			// because we wont be syncing the beliefs on them selves
			// we are just syncing the state of elements
			// which should be stored as a few integers

			if (this.LocalIdentity.SyncFrame % 200 == 0)
				view.Level.LevelTime = (view.Level.LevelTime - 1).Max(0);



			var KnownPassengers = view.Level.KnownPassengers.ToArray();

			if (KnownPassengers.Length == 0)
				return;

			var Passengers = Enumerable.ToArray(
				from Passenger in KnownPassengers
				where Passenger.CurrentPassengerVehicle == null
				where !Passenger.Memory_CaveAction
				where Passenger.CurrentCave == null
				where Passenger.VelocityY == 0
				let PassengerObstacle = Passenger.ToObstacle()
				let Platform = view.Level.ToPlatformSnapshots().FirstOrDefault(k => k.IncludedSpace.Intersects(PassengerObstacle))
				where Platform != null
				let NearestPickup = Enumerable.FirstOrDefault(
					from k in PickupVehicles
					where Passenger.Memory_Route_NextPlatformIndex >= 0
					where k.VehicleObstacle.Intersects(Platform.IncludedSpace)
					orderby k.VehicleObstacle.X - PassengerObstacle.X
					select k
				)
				let PickupArrived = NearestPickup != null
				select new { Passenger, PassengerObstacle, Platform, PickupArrived, NearestPickup }
			);



			var PickupVehiclesInTheWater = Enumerable.ToArray(
				from Vehicle in view.Level.KnownVehicles
				where Vehicle.CurrentWeapon == null
				where Vehicle.CurrentDriver != null
				where Vehicle.CurrentPassengers.Count == 0
				//where Vehicle.LastVelocity == 0
				//where Vehicle.GetVelocity() == 0
				let VehicleObstacle = Vehicle.ToObstacle()
				where (VehicleObstacle.Bottom + VehicleObstacle.Height / 2) > view.Level.WaterTop
				select new { Vehicle, VehicleObstacle }
			);

			var PassengersInTheWater = Enumerable.ToArray(
				from Passenger in KnownPassengers
				where Passenger.CurrentPassengerVehicle == null
				where Passenger.Memory_LogicState != Actor.Memory_LogicState_Drowning
				let PassengerObstacle = Passenger.ToObstacle()
				where PassengerObstacle.Bottom > view.Level.WaterTop

				let NearestPickup = Enumerable.FirstOrDefault(
					from k in PickupVehiclesInTheWater
					where Passenger.Memory_Route_NextPlatformIndex >= 0
					orderby k.VehicleObstacle.X - PassengerObstacle.X
					select k
				)

				let PickupArrived = NearestPickup != null
				select new { Passenger, NearestPickup, PassengerObstacle, PickupArrived }
			);

			#region PassengersInTheWater
			foreach (var i in PassengersInTheWater)
			{
				if (i.Passenger.Memory_LogicState_IsConfused)
				{
					#region Memory_LogicState_IsConfused > Memory_LogicState_ConfusedEnd
					i.Passenger.Memory_LogicState++;

					if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_ConfusedEnd)
					{
						i.Passenger.KnownBubbles.RemoveAll();
						// not rescued
						i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Drowning;
						i.Passenger.Density = 2;
					}
					#endregion
				}
				else if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_Boarding)
				{
					if (i.PickupArrived)
					{
						if (Math.Abs(i.PassengerObstacle.X - i.NearestPickup.VehicleObstacle.X) <= view.Level.Zoom)
						{
							// enter the vehicle

							i.Passenger.Animation = Actor.AnimationEnum.Hidden;
							i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
							i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;
							i.Passenger.RespectPlatforms = true;
							i.Passenger.Memory_LogicState = Actor.Memory_LogicState_FareBase + i.Passenger.AvailableFare;

							i.NearestPickup.Vehicle.CurrentPassengers.Add(i.Passenger);
							i.Passenger.CurrentPassengerVehicle = i.NearestPickup.Vehicle;
						}
						else
						{
							i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = (i.NearestPickup.VehicleObstacle.X - i.Passenger.X) > view.Level.Zoom;
							i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = (i.NearestPickup.VehicleObstacle.X - i.Passenger.X) < -view.Level.Zoom;

						}
					}
					else
					{
						i.Passenger.StartTalkingConfusion();
						// confusion
					}
				}
				else if (i.Passenger.Memory_LogicState_IsTalking)
				{
					if (i.PickupArrived)
					{
						#region Memory_LogicState_IsTalking > Memory_LogicState_Boarding
						i.Passenger.Memory_LogicState++;

						if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_TalkEnd)
						{
							i.Passenger.KnownBubbles.RemoveAll();
							i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Boarding;
							Console.WriteLine("Memory_LogicState_Boarding");

						}
						#endregion
					}
					else
					{
						i.Passenger.StartTalkingConfusion();
					}
				}
				else if (i.Passenger.Memory_LogicState_IsWaitingRescue)
				{
					if (i.PickupArrived)
					{
						i.Passenger.StartTalking();

						// shall start talking!

					}
					else if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_WaitingRescueEnd)
					{
						// not rescued
						i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Drowning;
						i.Passenger.Density = 2;
					}
					else
					{
						i.Passenger.Memory_LogicState++;
					}
				}
				else
				{
					// if this passanger can swim...
					i.Passenger.Memory_LogicState = Actor.Memory_LogicState_WaitingRescueStart;
				}
			}
			#endregion


			#region Passengers not in the water
			foreach (var i in Passengers)
			{
				if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_LastMile)
				{
					#region LastMile
					// go to the cave dude

					if (Math.Abs(i.PassengerObstacle.X - i.Platform.Cave.X) <= view.Level.Zoom)
					{
						i.Passenger.Memory_Route.Pop();

						if (i.Passenger.Memory_Route_NextPlatformIndex < 0)
							view.Level.AttributeHeadCount.Value = (view.Level.AttributeHeadCount.Value - 1).Max(0);

						i.Passenger.CurrentCave = i.Platform.Cave;
						i.Passenger.Memory_CaveAction = false;
						i.Passenger.PlayAnimation(Actor.AnimationEnum.CaveEnter, null);
						i.Passenger.Memory_LogicState = Actor.Memory_LogicState_CaveLifeStart;
					}
					else
					{
						i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = (i.Platform.Cave.X - i.Passenger.X) > view.Level.Zoom;
						i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = (i.Platform.Cave.X - i.Passenger.X) < -view.Level.Zoom;
					}
					#endregion

				}
				else if (i.Passenger.Memory_LogicState_IsConfused)
				{
					#region Memory_LogicState_IsConfused > Memory_LogicState_ConfusedEnd
					i.Passenger.Memory_LogicState++;

					if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_ConfusedEnd)
					{
						i.Passenger.KnownBubbles.RemoveAll();
						i.Passenger.Animation = Actor.AnimationEnum.Idle;
						i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Waiting;
						Console.WriteLine("Memory_LogicState_ConfusedEnd");
					}
					#endregion
				}
				else
				{
					if (i.PickupArrived)
					{
						#region PickupArrived = talking or boarding
						if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_Boarding)
						{
							#region Memory_LogicState_Boarding
							// walk to the vehicle

							if (Math.Abs(i.PassengerObstacle.X - i.NearestPickup.VehicleObstacle.X) <= view.Level.Zoom)
							{
								// enter the vehicle

								i.Passenger.Animation = Actor.AnimationEnum.Hidden;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;

								i.Passenger.Memory_LogicState = Actor.Memory_LogicState_FareBase + i.Passenger.AvailableFare;

								i.NearestPickup.Vehicle.CurrentPassengers.Add(i.Passenger);
								i.Passenger.CurrentPassengerVehicle = i.NearestPickup.Vehicle;
							}
							else
							{
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = (i.NearestPickup.VehicleObstacle.X - i.Passenger.X) > view.Level.Zoom;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = (i.NearestPickup.VehicleObstacle.X - i.Passenger.X) < -view.Level.Zoom;
							}
							#endregion
						}
						else
						{
							if (i.Passenger.Memory_LogicState_IsTalking)
							{
								#region Memory_LogicState_IsTalking > Memory_LogicState_Boarding
								i.Passenger.Memory_LogicState++;

								if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_TalkEnd)
								{
									i.Passenger.KnownBubbles.RemoveAll();
									i.Passenger.Animation = Actor.AnimationEnum.Idle;
									i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Boarding;
									Console.WriteLine("Memory_LogicState_Boarding");

								}
								#endregion

							}
							else
							{
								i.Passenger.StartTalking();

							}
						}
						#endregion

					}
					else
					{
						if (i.Passenger.Memory_LogicState_WouldBeConfusedIfVehicleLeft)
						{
							#region Memory_LogicState_ConfusedStart
							Console.WriteLine("Memory_LogicState_ConfusedStart");

							i.Passenger.StartTalkingConfusion();

							#endregion

						}
						else
						{
							#region walk to waiting position
							if (i.Passenger.VelocityX == 0)
								if (i.Passenger.Animation != Actor.AnimationEnum.Idle)
									i.Passenger.Animation = Actor.AnimationEnum.Idle;

							if (Math.Abs(i.Platform.WaitPosition.X - i.Passenger.X) < i.Platform.WaitPosition.Width)
							{
								i.Passenger.VelocityX = 0;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;
								i.Passenger.Memory_FirstWait = true;
							}
							else
							{
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = (i.Platform.WaitPosition.X - i.Passenger.X) > i.Platform.WaitPosition.Width;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = (i.Platform.WaitPosition.X - i.Passenger.X) < -i.Platform.WaitPosition.Width;
							}
							#endregion
						}
					}
				}
			}
			#endregion



			#region passangers already on board
			foreach (var i_Passenger in KnownPassengers.Where(k => k.CurrentPassengerVehicle != null))
			{
				if (i_Passenger.Memory_LogicState_IsFare)
				{
					if (this.LocalIdentity.SyncFrame % 2 == 0)
						if (i_Passenger.Memory_LogicState != Actor.Memory_LogicState_FareMin)
							i_Passenger.Memory_LogicState = Math.Max(i_Passenger.Memory_LogicState - 4, Actor.Memory_LogicState_FareMin);
				}

				if (i_Passenger.CurrentPassengerVehicle.GetVelocity() == 0)
				{
					// if we are at the correct platform
					// we could release the passenger...
					var CurrentPassengerVehicle = i_Passenger.CurrentPassengerVehicle.ToObstacle();

					var CurrentPlatform = view.Level.ToPlatformSnapshots().AtModulus(i_Passenger.Memory_Route_NextPlatformIndex);
					if (CurrentPlatform.IncludedSpace.Intersects(CurrentPassengerVehicle))
					{


						i_Passenger.Animation = Actor.AnimationEnum.Idle;
						i_Passenger.MoveTo(
							i_Passenger.CurrentPassengerVehicle.X,
							i_Passenger.CurrentPassengerVehicle.Y - 2 * view.Level.Zoom
						);
						i_Passenger.VelocityX = 0;
						i_Passenger.VelocityY = 0;
						i_Passenger.CurrentPassengerVehicle.CurrentPassengers.Remove(i_Passenger);
						i_Passenger.CurrentPassengerVehicle = null;

						// add the fare to the highscore
						// this action will be done in sync in all clients

						view.Memory_Score += (i_Passenger.Memory_LogicState - Actor.Memory_LogicState_FareBase) * view.Memory_ScoreMultiplier;

						i_Passenger.Memory_LogicState = Actor.Memory_LogicState_LastMile;

						i_Passenger.Memory_CanBeHitByVehicle = false;
					}
				}
			}
			#endregion


			// simulate cave life

			var NextPassengerToWalkOutOfTheCave = KnownPassengers.Where(k => k.Memory_Route_NextPlatformIndex >= 0).FirstOrDefault(k => k.CurrentCave != null);
			// if the dude doesnt have anywhere to go anymore, it stays inside the cave

			if (NextPassengerToWalkOutOfTheCave != null)
			{
				if (!NextPassengerToWalkOutOfTheCave.Memory_LogicState_IsCaveLife)
				{
					// start the cave life
					NextPassengerToWalkOutOfTheCave.Memory_LogicState = Actor.Memory_LogicState_CaveLifeStart;
				}
				else
				{
					if (NextPassengerToWalkOutOfTheCave.Memory_LogicState == Actor.Memory_LogicState_CaveLifeEnd)
					{
						// passanger is ready to come out

						var ThePlatform = view.Level.ToPlatformSnapshots().Single(k => k.Cave == NextPassengerToWalkOutOfTheCave.CurrentCave);

						if (Enumerable.Any(
								from AnotherPassanger in KnownPassengers
								where AnotherPassanger.CurrentCave == null
								where AnotherPassanger.CurrentPassengerVehicle == null
								where AnotherPassanger.ToObstacle().Intersects(ThePlatform.IncludedSpace)
								select AnotherPassanger
							))
						{
							// there is someone already waiting

							// we will stay indoors for some time and check again
							NextPassengerToWalkOutOfTheCave.Memory_LogicState -= 10;
						}
						else
						{

							if (KnownPassengers.First() == NextPassengerToWalkOutOfTheCave)
							{
								//we dont have to wait for the previous passanger to stand still
								AIDirector.ActorExitCave(NextPassengerToWalkOutOfTheCave);
								NextPassengerToWalkOutOfTheCave.Memory_LogicState = Actor.Memory_LogicState_Waiting;
								NextPassengerToWalkOutOfTheCave.Memory_CanBeHitByVehicle = true;
							}
							else
							{
								var PreviousPassanger = KnownPassengers.Previous(k => k == NextPassengerToWalkOutOfTheCave);

								if (PreviousPassanger.Memory_FirstWait)
								{
									AIDirector.ActorExitCave(NextPassengerToWalkOutOfTheCave);
									NextPassengerToWalkOutOfTheCave.Memory_LogicState = Actor.Memory_LogicState_Waiting;
									NextPassengerToWalkOutOfTheCave.Memory_CanBeHitByVehicle = true;
								}
							}
						}
					}
					else
					{
						NextPassengerToWalkOutOfTheCave.Memory_LogicState++;
					}
				}
			}
		}

	}
}
