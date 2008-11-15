using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code
{
	[Script]
	public class Physics
	{
		public double WaterTop;

		public IEnumerable<Obstacle> Obstacles;

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


		public void Apply(AvalonUgh.Code.Vehicle twin)
		{
			// y relative to water
			var y = twin.Y - WaterTop;


			// 0..1 how much volume is in air

			var AirVolume = (-y / twin.HalfHeight).Min(1).Max(0);
			var WaterVolume = (y / twin.HalfHeight).Min(1).Max(0);

			// add gravity
			twin.VelocityY += 0.25 * AirVolume;


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

			var vehX = new Obstacle
			{
				Left = newX - twin.HalfWidth + 6 * twin.Zoom,
				Right = newX + twin.HalfWidth - 6 * twin.Zoom,
				Top = twin.Y - twin.HalfHeight + 6 * twin.Zoom,
				Bottom = twin.Y + twin.HalfHeight,
			};

			var vehY = new Obstacle
			{
				Left = twin.X - twin.HalfWidth + 6 * twin.Zoom,
				Right = twin.X + twin.HalfWidth - 6 * twin.Zoom,
				Top = newY - twin.HalfHeight + 6 * twin.Zoom,
				Bottom = newY + twin.HalfHeight,
			};

			var ObstacleX = Obstacles.FirstOrDefault(k => k.Intersects(vehX));
			var ObstacleY = Obstacles.FirstOrDefault(k => k.Intersects(vehY));

			if (ObstacleX != null)
			{
				twin.VelocityX *= -0.5;
			}

			if (ObstacleY != null)
			{

				twin.VelocityY *= -0.5;

			}

			newX = twin.X + twin.VelocityX;
			newY = twin.Y + twin.VelocityY;

			twin.MoveTo(newX, newY);

		}
	}
}
