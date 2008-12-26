using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
	public partial class Tryoperus
	{
		public enum AnimationEnum
		{
			Unknown,

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
				public const int StunCount = 6;

				public const int HitOffset = 420;

				public const int RunOffset = 410;
				public const int RunCount = 4;

				public const int StareOffset = 400;
				public const int StareCount = 7;

				public const int WalkOffset = 200;
				public const int WalkCount = 12;
			}

			[Script]
			public static class Right
			{
				public const int WalkOffset = 100;
				public const int WalkCount = 12;
			}
		}

		[Script]
		public class SpecificNameFormat : NameFormat
		{
			// this will be used to find the embedded resource files
			// and within the map loader
			public const string Alias = "tryo";

			public SpecificNameFormat()
			{
				Path = Assets.Shared.KnownAssets.Path.Sprites;
				Name = Alias;
				Index = 0;
				Extension = "png";
				Width = 2;
				Height = 2;
			}
		}

		public AnimationEnum Animation
		{
			get
			{
				return (AnimationEnum)InternalAnimation.Frame;
			}
			set
			{
				InternalAnimation.PlayFrame((int)value);
			}
		}
	}
}
