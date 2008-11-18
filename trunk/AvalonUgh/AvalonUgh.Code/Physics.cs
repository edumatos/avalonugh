using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code
{
	[Script]
	public class Physics
	{
		public Level Level;

		public IEnumerable<Obstacle> Obstacles;
		public IEnumerable<Vehicle> Vehicles;
		public IEnumerable<Rock> Rocks;
		public IEnumerable<Bird> Birds;
		public IEnumerable<Actor> Actors;

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
			this.Vehicles.ForEach(Apply);
			this.Rocks.ForEach(Apply);
			this.Actors.ForEach(Apply);
		}

		void Apply(ISupportsPhysics twin)
		{
			if (twin.PhysicsDisabled)
				return;

			// y relative to water
			var y = twin.Y - this.Level.WaterTop;


			// 0..1 how much volume is in air

			var AirVolume = (-y / twin.HalfHeight).Min(1).Max(0);
			var WaterVolume = (y / twin.HalfHeight).Min(1).Max(0);

			// add gravity
			twin.VelocityY += 0.3 * AirVolume;


			const double WaterDensity = 1.0;
			var DeltaDensity = twin.Density - WaterDensity;
			var Bouyancy = DeltaDensity * WaterVolume * 0.2;

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

			var Obstacles = this.Obstacles;

			if (twin is Vehicle)
			{
				Obstacles = Obstacles.Concat(this.Vehicles.Where(k => k != twin).Select(k => k.ToObstacle()));

				this.Rocks.Where(k => k.ReadyForPickup).Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
					rock_ =>
					{
						rock_.PhysicsDisabled = true;
						rock_.Container.Hide();
					}
				);

				this.Actors.Where(k => k.Animation != Actor.AnimationEnum.Panic).Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
					actor_ =>
					{
						// we did will hit a tree
						actor_.Animation = Actor.AnimationEnum.Panic;
					}
				);

			}

			var actor = twin as Actor;
			if (actor != null)
			{
				if (!actor.RespectPlatforms)
					Obstacles = new Obstacle[0].AsEnumerable();
			}

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
							this.Birds.Select(k => k.ToObstacle()).ToArray()
						);

						this.Birds.Where(k => k.ToObstacle().Intersects(vehXY)).ForEach(
							bird =>
							{
								// we did will hit a tree
								//tree.GoToSleep();
								rock.GoToSleep();
							}
						);
					}

				}
			}

			var ObstacleX = Obstacles.FirstOrDefault(k => k.Intersects(vehX));
			var ObstacleY = Obstacles.FirstOrDefault(k => k.Intersects(vehY));


			if (ObstacleX != null)
			{
				if (ObstacleX.SupportsVelocity != null)
				{
					var fx = ObstacleX.SupportsVelocity.VelocityX / 2;
					ObstacleX.SupportsVelocity.VelocityX += twin.VelocityX / 2;
					twin.VelocityX = fx;
				}
				else
				{
					if (Math.Abs(twin.VelocityX) < 1.0)
						twin.VelocityX = 0;
					else
						twin.VelocityX *= -0.5;
				}
			}


			if (ObstacleY != null)
			{
				if (ObstacleY.SupportsVelocity != null)
				{
					var fy = ObstacleY.SupportsVelocity.VelocityY / 2;
					ObstacleY.SupportsVelocity.VelocityY += twin.VelocityY / 2;
					twin.VelocityY = fy;
				}
				else
				{
					if (Math.Abs(twin.VelocityY) < 1.0)
						twin.VelocityY = 0;
					else
						twin.VelocityY *= -0.5;
				}
			}

			if (twin.GetVelocity() < 0.5)
			{
				twin.Stability++;

				if (twin.Stability > 3)
				{
					twin.StabilityReached();
				}
			}
			else
			{
				twin.Stability = 0;
			}

			newX = twin.X + twin.VelocityX;
			newY = twin.Y + twin.VelocityY;

			twin.MoveTo(newX, newY);

		}
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
	public interface ISupportsPhysics : ISupportsVelocity, ISupportsObstacle
	{
		int Stability { get; set; }
		void StabilityReached();

		bool PhysicsDisabled { get; }

		int HalfHeight { get; }
		int HalfWidth { get; }
		double Density { get; set; }



		void MoveTo(double x, double y);
	}

	[Script]
	public static class SupportPhysicsExtensions
	{
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
