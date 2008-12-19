using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Code.Editor.Tiles;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code
{
	[Script]
	public class AIDirector
	{
		
		public static void WalkActorToTheCaveAndEnter(Actor a, Cave c, Action done)
		{
			a.AIInputEnabled = true;

			Func<double> DistanceToCave = () => (c.Location - a.Location).Length / a.Zoom;



			Action AreWeClouseEnough = null;

			AreWeClouseEnough =
				delegate
				{
					if (DistanceToCave() <= a.Zoom)
					{
						Console.WriteLine("we are in front of that cave");

						a.LocationChanged -= AreWeClouseEnough;

						// we need to play this onetime animation ourselves
						a.PlayAnimation(Actor.AnimationEnum.CaveEnter,
							delegate
							{
								// setting this field indicates that the
								// actor is inside this cave
								a.CurrentCave = c;
								a.AIInputEnabled = false;

								Console.WriteLine("inside the cave!");

								if (done != null)
									done();
							}
						);

						// we stop and start entering the cave
						a.VelocityX = 0;

						return;
					}

					a.VelocityX += Math.Sign(c.X - a.X) * Actor.DefaultAcceleraton * a.Zoom;

					if (a.VelocityX > 0)
					{
						if (a.Animation != Actor.AnimationEnum.WalkRight)
							a.Animation = Actor.AnimationEnum.WalkRight;
					}
					else
						if (a.Animation != Actor.AnimationEnum.WalkLeft)
							a.Animation = Actor.AnimationEnum.WalkLeft;

					Console.WriteLine("AI: that cave is at distance of " + DistanceToCave());

				};

			a.LocationChanged += AreWeClouseEnough;

			AreWeClouseEnough();
		}

		public static void ActorExitCave(Actor a)
		{
			a.AIInputEnabled = true;
			a.MoveTo(a.CurrentCave.X, a.CurrentCave.Y);
			a.CurrentCave = null;

			a.PlayAnimation(Actor.AnimationEnum.CaveExit,
				delegate
				{
					a.AIInputEnabled = false;
					a.Animation = Actor.AnimationEnum.Idle;
				}
			);
		}

		public static void ActorExitAnyCave(Actor a, Cave c)
		{
			a.AIInputEnabled = true;
			a.MoveTo(c.X, c.Y);
			a.CurrentCave = null;

			a.PlayAnimation(Actor.AnimationEnum.CaveExit,
				delegate
				{
					a.AIInputEnabled = false;
					a.Animation = Actor.AnimationEnum.Idle;
				}
			);
		}
	}
}
