using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Editor;

namespace AvalonUgh.Code
{
	[Script]
	public class Physics
	{
		public Level Level;


		// mass = density / volume
		// length * width * height = volume
		// http://ca.youtube.com/watch?v=VDSYXmvjg6M
		// concrete density = 2.3g / cm^3
		// wood density = 0.4g / cm^3
		// water density = 1g / cm^3
		// air density = 0.001g / cm^3

		// http://www.engineeringtoolbox.com/density-specific-weight-gravity-d_290.html

		// wood density
		// http://www.engineeringtoolbox.com/wood-density-d_40.html
		// http://www.engineeringtoolbox.com/accelaration-gravity-d_340.html
		// http://www.phynet.de/mechanik/dynamik/hydrodynamik/die-auftriebskraft-in-flussigkeiten

		// http://www.regentsprep.org/Regents/physics/phys01/accgravi/index.htm
		// http://www.glenbrook.k12.il.us/GBSSCI/PHYS/Class/1DKin/U1L5b.html
		// http://www.regentsprep.org/Regents/physics/phys-topic.cfm?Course=PHYS&TopicCode=01a
		// http://en.wikipedia.org/wiki/Vector_(spatial)
		// http://www.netcomuk.co.uk/~jenolive/homevec.html
		// http://farside.ph.utexas.edu/teaching/301/lectures/node23.html
		// http://www2.swgc.mun.ca/physics/physlets.html
		// http://www.icoachmath.com/SiteMap/MagnitudeofaVector.html
		// http://www.physicsforums.com/showthread.php?t=154533
		// http://en.wikipedia.org/wiki/Buoyancy
		// http://ca.youtube.com/watch?v=VDSYXmvjg6M

		public void Apply()
		{
			this.Level.KnownVehicles.ForEach(Apply);
			this.Level.KnownRocks.ForEach(Apply);
			this.Level.KnownActors.ForEach(Apply);
			this.Level.KnownTryoperus.ForEach(Apply);
		}

		void Apply(ISupportsPhysics twin)
		{
			if (twin.PhysicsDisabled)
				return;

			// y relative to water
			var y = (twin.Y - this.Level.WaterTop) ;


			// 0..1 how much volume is in air

			var AirVolume = (-y / twin.HalfHeight).Min(1).Max(0);
			var WaterVolume = ((y + twin.HalfHeight * twin.MassCenterModifier) / twin.HalfHeight).Min(1).Max(0);

	

			// add gravity
			twin.VelocityY += Level.AttributeGravity.Value * 0.01 * AirVolume * AirVolume;


			const double WaterDensity = 1.0;
			var DeltaDensity = twin.Density - WaterDensity;
			var Bouyancy = DeltaDensity * WaterVolume * WaterVolume * 0.2;

			// add water levitation
			twin.VelocityY += Bouyancy;

			const double WaterFriction = 0.05;
			const double AirFriction = 0.03;

			// friction in water
			twin.VelocityY *= 1.0 - (WaterFriction * WaterVolume);
			twin.VelocityX *= 1.0 - (WaterFriction * WaterVolume);

			// friction in air water
			twin.VelocityY *= 1.0 - (AirFriction * AirVolume);
			twin.VelocityX *= 1.0 - (AirFriction * AirVolume);


			var newX = twin.X + twin.VelocityX;
			var newY = twin.Y + twin.VelocityY;

			var vehXY = twin.ToObstacle(newX, newY);
			var vehX = twin.ToObstacle(newX, twin.Y);
			var vehY = twin.ToObstacle(twin.X, newY);

			var Obstacles = this.Level.ToObstacles();

			var veh = twin as Vehicle;
			if (veh != null)
				if (!veh.IsUnmanned)
				{
					Obstacles = Obstacles.Concat(this.Level.KnownVehicles.Where(k => k != twin).Where(k => !k.IsUnmanned).Select(k => k.ToObstacle()));

					if (veh.CurrentWeapon == null)
					{
						// we can pickup a rock for our weapon now

						this.Level.KnownRocks.Where(k => k.ReadyForPickup).Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
							rock_ =>
							{
								veh.CurrentWeapon = rock_;
							}
						);
					}

					this.Level.KnownActors.Where(k => k.CanBeHitByVehicle).Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
						actor_ =>
						{
							actor_.CanBeHitByVehicle = false;
							actor_.RespectPlatforms = false;
							actor_.Animation = Actor.AnimationEnum.Panic;

							//// we did hit an actor that repsects platforms
							//// as such he cannot fall thro it to water
							//if (actor_.RespectPlatforms)
							//    return;

							//// we did will hit a tree
							//actor_.Animation = Actor.AnimationEnum.Panic;
						}
					);

				}

			bool DinoWindEnabled = true;

			var actor = twin as Actor;
			if (actor != null)
			{
				if (!actor.RespectPlatforms)
					Obstacles = new Obstacle[0].AsEnumerable();

				//if (actor.ReadyForGoldPickup)
				//{
				//    actor.ReadyForGoldPickup = false;

				var ActorAsObstacle = actor.ToObstacle();

				foreach (Gold g in this.Level.KnownGold.ToArray())
				{
					if (g.ToObstacle().Intersects(ActorAsObstacle))
					{
						g.OrphanizeContainer();

						this.Level.KnownGold.Remove(g);

						actor.GoldStash.Add(g);
					}
				}

				if (actor.AIInputEnabled)
					DinoWindEnabled = false;

			}

			#region Rock
			var rock = twin as Rock;
			if (rock != null)
			{
				if (rock.Stability < 10)
				{
					if (!rock.IsSleeping)
					{
						Obstacles = Obstacles.Concat(
							this.Level.KnownTrees.WhereNot(k => k.IsSleeping).Select(k => k.ToObstacle()).ToArray()
						);



						this.Level.KnownTrees.WhereNot(k => k.IsSleeping).Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
							tree =>
							{
								// we did will hit a tree
								tree.GoToSleep();
								rock.GoToSleep();
							}
						);



						Obstacles = Obstacles.Concat(
							this.Level.KnownTryoperus.WhereNot(k => k.IsSleeping).Select(k => k.ToObstacle()).ToArray()
						);

						this.Level.KnownTryoperus.WhereNot(k => k.IsSleeping).Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
							tryo =>
							{
								// we did will hit a tree
								tryo.GoToSleep();
								rock.GoToSleep();
							}
						);


						//Obstacles = Obstacles.Concat(
						//    this.Birds.Select(k => k.ToObstacle()).ToArray()
						//);

						//this.Birds.Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
						//    bird =>
						//    {
						//        // we did will hit a tree
						//        //tree.GoToSleep();
						//        rock.GoToSleep();
						//    }
						//);
					}

				}
			}
			#endregion

			if (DinoWindEnabled)
				foreach (var dino in this.Level.KnownDinos)
				{
					if (dino.SnoreArea.Intersects(vehXY))
						twin.VelocityX += dino.SnoreWind * dino.SnoreWindAmpilfier * this.Level.Zoom;

				}


			var ObstacleX = Obstacles.FirstOrDefault(k => k.Intersects(vehX));
			var ObstacleY = Obstacles.FirstOrDefault(k => k.Intersects(vehY));

			var CollisionAtVelocity = 0.0;
			var CollisionAtVelocityEnabled = false;

			if (ObstacleX != null)
			{
				CollisionAtVelocityEnabled = true;
				CollisionAtVelocity += Math.Pow(twin.VelocityX * 0.5, 2);

				if (ObstacleX.SupportsVelocity != null)
				{
					var fx = ObstacleX.SupportsVelocity.VelocityX / 2;


					ObstacleX.SupportsVelocity.VelocityX += twin.VelocityX / 2;
					twin.VelocityX = fx;
				}
				else
				{

					if (Math.Abs(twin.VelocityX) < Level.Zoom)
						twin.VelocityX = 0;
					else
						twin.VelocityX *= -0.5;
				}
			}

			if (ObstacleY != null)
			{
				CollisionAtVelocityEnabled = true;
				CollisionAtVelocity += Math.Pow(twin.VelocityY * 0.5, 2);

				if (ObstacleY.SupportsVelocity != null)
				{
					var fy = ObstacleY.SupportsVelocity.VelocityY / 2;
					ObstacleY.SupportsVelocity.VelocityY += twin.VelocityY / 2;
					twin.VelocityY = fy;
				}
				else
				{
					if (Math.Abs(twin.VelocityY) < Level.Zoom)
						twin.VelocityY = 0;
					else
						twin.VelocityY *= -0.5;
				}
			}
			else
			{
				if (twin is Vehicle)
					twin.VelocityX += Level.AttributeWind.Value * 0.01 * AirVolume;
			}

			if (CollisionAtVelocityEnabled)
			{
				CollisionAtVelocity = Math.Sqrt(CollisionAtVelocity);

				if (CollisionAtVelocity != twin.LastCollisionVelocity)
					if (this.CollisionAtVelocity != null)
						this.CollisionAtVelocity(CollisionAtVelocity);

				twin.LastCollisionVelocity = CollisionAtVelocity;

			}

			if (WaterVolume > AirVolume)
			{
				if (twin.LastWaterCollisionVelocity == 0)
				{
					var WaterCollisionAtVelocity = twin.GetVelocity();


					if (this.WaterCollisionAtVelocity != null)
						this.WaterCollisionAtVelocity(WaterCollisionAtVelocity);

					if (actor != null)
						actor.RaiseWaterCollision();

					twin.LastWaterCollisionVelocity = WaterCollisionAtVelocity;
				}
			}
			else
			{
				twin.LastWaterCollisionVelocity = 0;
			}

			if (twin.GetVelocity() < 0.01)
			{
				if (twin.Y < this.Level.WaterTop)
				{
					twin.Stability++;

					// how stable must the object be?
					if (twin.Stability >= 3)
					{
						if (twin.Stability == 3)
						{
							twin.StabilityReached();


						}

						twin.VelocityX = 0;
						twin.VelocityY = 0;

						//return;
					}
				}
			}
			else
			{
				twin.Stability = 0;
			}

			if (twin.VelocityX == 0)
				if (twin.VelocityY == 0)
					return;

			newX = twin.X + twin.VelocityX;
			newY = twin.Y + twin.VelocityY;

			twin.MoveTo(newX, newY);

		}

		public event Action<double> CollisionAtVelocity;
		public event Action<double> WaterCollisionAtVelocity;
	}


	[Script]
	public interface ISupportsVelocity
	{

		double VelocityX { get; set; }
		double VelocityY { get; set; }

	}

	[Script]
	public interface ISupportsObstacle
	{
		double X { get; }
		double Y { get; }

		Obstacle ToObstacle(double x, double y);
	}

	[Script]
	public interface ISupportsMoveTo
	{


		int HalfHeight { get; }
		int HalfWidth { get; }

		void MoveTo(double x, double y);
	}

	[Script]
	public interface ISupportsPhysics : ISupportsVelocity, ISupportsObstacle, ISupportsMoveTo
	{
		int Stability { get; set; }
		void StabilityReached();

		bool PhysicsDisabled { get; }

		double MassCenterModifier { get; }

		double Density { get; set; }

		double LastCollisionVelocity { get; set; }
		double LastWaterCollisionVelocity { get; set; }
	}

	[Script]
	public static class SupportPhysicsExtensions
	{


		public static void MoveBaseTo(this ISupportsMoveTo e, double x, double y)
		{
			e.MoveTo(x, y - e.HalfHeight);
		}

		public static double GetVelocity(this ISupportsVelocity e)
		{
			return Math.Sqrt(e.VelocityX * e.VelocityX + e.VelocityY * e.VelocityY);
		}

		public static Obstacle ToObstacle(this ISupportsObstacle e)
		{
			return e.ToObstacle(e.X, e.Y);
		}

	}
}
