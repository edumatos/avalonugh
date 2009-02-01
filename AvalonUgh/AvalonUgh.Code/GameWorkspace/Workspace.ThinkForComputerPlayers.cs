using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{

		private void ThinkForComputerPlayers(Level level)
		{
			foreach (var p in level.KnownComputerActors)
			{
				var p_AsObstacle = p.ToObstacle();

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

						if (ShouldStopWalking)
						{
							p.VelocityX *= 0.7;
						}
						else
						{
							p.VelocityX = (p.VelocityX + 0.1).Min(0.7);

							if (p.Animation != Actor.AnimationEnum.WalkRight)
								p.Animation = Actor.AnimationEnum.WalkRight;
						}

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
