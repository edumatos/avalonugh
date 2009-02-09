using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.GameWorkspace.PassangerAIDomain;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{

		private void ThinkForComputerPlayers(View view)
		{

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

			var PickupVehicles = Enumerable.ToArray(
				from Vehicle in view.Level.KnownVehicles
				where Vehicle.CurrentDriver != null
				where Vehicle.CurrentPassenger == null
				where Vehicle.GetVelocity() == 0
				let VehicleObstacle = Vehicle.ToObstacle()
				select new { Vehicle, VehicleObstacle }
			);

			var KnownPassengers = view.Level.KnownPassengers.ToArray();

			var Passengers = Enumerable.ToArray(
				from Passenger in KnownPassengers
				where !Passenger.Memory_CaveAction
				where Passenger.CurrentCave == null
				where Passenger.VelocityY == 0
				let PassengerObstacle = Passenger.ToObstacle()
				let Platform = view.Level.ToPlatformSnapshots().FirstOrDefault(k => k.IncludedSpace.Intersects(PassengerObstacle))
				where Platform != null
				let NearestPickup = Enumerable.FirstOrDefault(
					from k in PickupVehicles
					where k.VehicleObstacle.Intersects(Platform.IncludedSpace)
					orderby k.VehicleObstacle.X - PassengerObstacle.X
					select k
				)
				let PickupArrived = NearestPickup != null
				select new { Passenger, PassengerObstacle, Platform, PickupArrived, NearestPickup }
			);




			foreach (var i in Passengers.Where(k => k.Passenger.CurrentPassengerVehicle == null))
			{
				if (i.Passenger.Memory_LogicState_IsConfused)
				{
					i.Passenger.Memory_LogicState++;

					if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_ConfusedEnd)
					{
						i.Passenger.KnownBubbles.RemoveAll();
						i.Passenger.Animation = Actor.AnimationEnum.Idle;
						i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Waiting;
						Console.WriteLine("Memory_LogicState_ConfusedEnd");
					}
				}
				else
				{
					if (i.PickupArrived)
					{
						if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_Boarding)
						{
							// walk to the vehicle

							if (Math.Abs(i.PassengerObstacle.X - i.NearestPickup.VehicleObstacle.X) <= view.Level.Zoom)
							{
								i.Passenger.Animation = Actor.AnimationEnum.Hidden;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;

								i.NearestPickup.Vehicle.CurrentPassenger = i.Passenger;
								i.Passenger.CurrentPassengerVehicle = i.NearestPickup.Vehicle;
							}
							else
							{
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = (i.NearestPickup.VehicleObstacle.X - i.Passenger.X) > i.Platform.WaitPosition.Width;
								i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = (i.NearestPickup.VehicleObstacle.X - i.Passenger.X) < -i.Platform.WaitPosition.Width;
							}

						}
						else
						{
							if (i.Passenger.Memory_LogicState_IsTalking)
							{
								i.Passenger.Memory_LogicState++;

								if (i.Passenger.Memory_LogicState == Actor.Memory_LogicState_TalkEnd)
								{
									i.Passenger.KnownBubbles.RemoveAll();
									i.Passenger.Animation = Actor.AnimationEnum.Idle;
									i.Passenger.Memory_LogicState = Actor.Memory_LogicState_Boarding;
									Console.WriteLine("Memory_LogicState_Boarding");

								}
							}
							else
							{
								Console.WriteLine("Memory_LogicState_TalkStart");


								#region Memory_LogicState_TalkStart


								SoundBoard.Default.talk0_00();

								i.Passenger.KnownBubbles.Add(
									new Actor.Bubble(view.Level.Zoom, 0)
								);

								i.Passenger.Memory_LogicState = Actor.Memory_LogicState_TalkStart;
								i.Passenger.Animation = Actor.AnimationEnum.Talk;

								#endregion
							}
						}

					}
					else
					{
						if (i.Passenger.Memory_LogicState_WouldBeConfusedIfVehicleLeft)
						{
							Console.WriteLine("Memory_LogicState_ConfusedStart");

							SoundBoard.Default.talk0_01();


							i.Passenger.Memory_LogicState = Actor.Memory_LogicState_ConfusedStart;
							i.Passenger.Animation = Actor.AnimationEnum.Talk;
							i.Passenger.VelocityX = 0;
							i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = false;
							i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = false;

							i.Passenger.KnownBubbles.Add(
								new Actor.Bubble(view.Level.Zoom, -1)
							);
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

			// simulate cave life

			var NextPassengerToWalkOutOfTheCave = KnownPassengers.FirstOrDefault(k => k.CurrentCave != null);

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

						if (KnownPassengers.First() == NextPassengerToWalkOutOfTheCave)
						{
							// we dont have to wait for the previous passanger to stand still
							AIDirector.ActorExitCave(NextPassengerToWalkOutOfTheCave);
							NextPassengerToWalkOutOfTheCave.Memory_LogicState = Actor.Memory_LogicState_Waiting;
						}
						else
						{
							var PreviousPassanger = KnownPassengers.Previous(k => k == NextPassengerToWalkOutOfTheCave);

							if (PreviousPassanger.Memory_FirstWait)
							{
								AIDirector.ActorExitCave(NextPassengerToWalkOutOfTheCave);
								NextPassengerToWalkOutOfTheCave.Memory_LogicState = Actor.Memory_LogicState_Waiting;
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
