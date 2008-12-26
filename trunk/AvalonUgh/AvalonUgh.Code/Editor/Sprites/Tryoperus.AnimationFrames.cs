using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code.Editor.Sprites
{
	public partial class Tryoperus
	{
		public enum AnimationEnum
		{
			Left_Stun,
			Left_Hit,
			Left_Run,
			Left_Stare,
			Left_Walk,

			Right_Stun,
			Right_Hit,
			Right_Run,
			Right_Stare,
			Right_Walk
		}

		[Script]
		public static class AnimationFrames
		{
			[Script]
			public static class Left
			{
				public const int StunOffset = 430;
				public const int StunLength = 6;

				public const int HitOffset = 420;

				public const int RunOffset = 410;
				public const int RunLength = 4;

				public const int StareOffset = 400;
				public const int StareLength = 7;

				public const int WalkOffset = 200;
				public const int WalkLength = 12;
			}





			public const int WalkRightOffset = 100;
			public const int WalkRightLength = 12;
		}
	}
}
