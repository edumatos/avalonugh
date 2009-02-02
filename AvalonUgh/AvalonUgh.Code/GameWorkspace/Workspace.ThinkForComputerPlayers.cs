using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using System;
using System.Collections.Generic;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{

		private void ThinkForComputerPlayers(Level level)
		{
			foreach (var p in level.KnownComputerActors)
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

			if (level.KnownComputerActors.Count > 0)
				return;

			if (level.KnownCaves.Count == 0)
				return;

			var c = level.KnownCaves.AtModulus(this.LocalIdentity.SyncFrame);

			var a = new Actor.woman0(level.Zoom);

			level.KnownComputerActors.Add(a);

			a.MoveTo(c.X, c.Y);
			a.Animation = Actor.AnimationEnum.Hidden;
			a.CurrentCave = c;

		}
	}
}
