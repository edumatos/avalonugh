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

			if (this.LocalIdentity.SyncFrame % 20 != 0)
				return;

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
				where Vehicle.GetVelocity() == 0
				let VehicleObstacle = Vehicle.ToObstacle()
				select new { Vehicle, VehicleObstacle }
			);

			var Passengers = Enumerable.ToArray(
				from Passenger in view.Level.KnownPassengers
				where Passenger.VelocityY == 0
				let PassengerObstacle = Passenger.ToObstacle()
				let Platform = view.Level.ToPlatformSnapshots().FirstOrDefault(k => k.IncludedSpace.Intersects(PassengerObstacle))
				where Platform != null
				let PickupArrived = PickupVehicles.Any(k => k.VehicleObstacle.Intersects(Platform.IncludedSpace))
				select new { Passenger, PassengerObstacle, Platform, PickupArrived }
			);




			foreach (var i in Passengers)
			{
				if (i.PickupArrived)
				{
					if (i.Passenger.Animation != Actor.AnimationEnum.Talk)
					{
						i.Passenger.Animation = Actor.AnimationEnum.Talk;

						SoundBoard.Default.talk0_00();

						i.Passenger.KnownBubbles.Add(
							
							// show where shall we go

							new Actor.Bubble(view.Level.Zoom)
						);
					}

				}
				else
				{
					i.Passenger.KnownBubbles.RemoveAll();

					if (i.Passenger.Animation != Actor.AnimationEnum.Idle)
						i.Passenger.Animation = Actor.AnimationEnum.Idle;

					if (i.PassengerObstacle.Intersects(i.Platform.WaitPosition))
						i.Passenger.VelocityX *= 0.3;

					i.Passenger.DefaultPlayerInput.Keyboard.IsPressedRight = (i.Platform.WaitPosition.X - i.Passenger.X) > i.Platform.WaitPosition.Width;
					i.Passenger.DefaultPlayerInput.Keyboard.IsPressedLeft = (i.Platform.WaitPosition.X - i.Passenger.X) < -i.Platform.WaitPosition.Width;

				}
			}


		}

		private void ThinkForComputerPlayers_OldImplementation(Level level)
		{
			foreach (var p in level.KnownPassengers)
			{
				var p_AsObstacle = p.ToObstacle().ToPercision(
					PrimitiveTile.Width * level.Zoom / 2
				);

				if (p.AIInputEnabled)
				{
					// yay, some other action needs to complete
				}
				else
				{
					p.FramesWaitedForNextAction++;

					if (p.CurrentCave != null)
					{
						if (p.FramesWaitedForNextAction % 100 == 0)
						{
							AIDirector.ActorExitCave(p);
							p.FramesWaitedForNextAction = 0;
						}
					}
					else
					{
						// we are out of the cave
						// we shall slowly walk half or more way to the sign

						// if there is no sign we will go back to the cave!!!

						var SignFound = level.KnownSigns.FirstOrDefault(k => k.ToObstacle().Intersects(p_AsObstacle));

						var ShouldStopWalking = false;

						if (SignFound != null)
						{
							ShouldStopWalking = true;
						}

						if (p.VelocityY != 0)
						{
							ShouldStopWalking = true;
						}

						// where to?
						// how far are we from the sign anyhow?

						Func<ISupportsObstacle[], int, bool> Intersects =
							(source, x) =>
							{
								var o = p_AsObstacle.WithOffset(level.Zoom * PrimitiveTile.Width * x, 0);

								return source.Any(k => k.ToObstacle().Intersects(o));
							};

						var SignDistanceThisWay = Enumerable.FirstOrDefault(
							from i in Enumerable.Range(0, 10)
							where Intersects(level.KnownSigns.ToArray(), i)
							select new { i }
						);

						var CaveDistanceTheOtherWay = Enumerable.FirstOrDefault(
							from i in Enumerable.Range(0, 10)
							where Intersects(level.KnownCaves.ToArray(), -i)
							select new { i }
						);


						if (CaveDistanceTheOtherWay != null)
							if (SignDistanceThisWay != null)
								if (SignDistanceThisWay.i <= CaveDistanceTheOtherWay.i)
									ShouldStopWalking = true;

						if (ShouldStopWalking)
						{
							p.VelocityX *= 0.7;
						}
						else
						{
							if (SignDistanceThisWay == null)
							{
								if (CaveDistanceTheOtherWay != null)
									p.VelocityX = (p.VelocityX - 0.1).Max(-0.7);
							}
							else
							{
								// get going!
								p.VelocityX = (p.VelocityX + 0.1).Min(0.7);
							}


						}

						if (p.VelocityX > 0)
							if (p.Animation != Actor.AnimationEnum.WalkRight)
								p.Animation = Actor.AnimationEnum.WalkRight;


						if (p.VelocityX < 0)
							if (p.Animation != Actor.AnimationEnum.WalkLeft)
								p.Animation = Actor.AnimationEnum.WalkLeft;


						if (p.VelocityX == 0)
							if (p.VelocityY == 0)
								if (p.Animation != Actor.AnimationEnum.Idle)
									p.Animation = Actor.AnimationEnum.Idle;


					}
				}
			}

			if (level.KnownPassengers.Count > 0)
				return;

			if (level.KnownCaves.Count == 0)
				return;

			var c = level.KnownCaves.AtModulus(this.LocalIdentity.SyncFrame);

			var a = new Actor.woman0(level.Zoom);

			level.KnownPassengers.Add(a);

			a.MoveTo(c.X, c.Y);
			a.Animation = Actor.AnimationEnum.Hidden;
			a.CurrentCave = c;
		}
	}
}
