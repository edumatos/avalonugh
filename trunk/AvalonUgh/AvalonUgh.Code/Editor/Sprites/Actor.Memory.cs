using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Code.Editor.Tiles;
using AvalonUgh.Code.Editor.Sprites;
using System.ComponentModel;
using System.Windows.Media;

namespace AvalonUgh.Code
{
	partial class Actor 
	{
		// fields prefixed by Memory_ need to be synced

		// 1000...1999 tell where to, start walking
		// 2000...2999 be confused, walk back and start idle
		int InternalMemory_LogicState = 0;
		public event Action Memory_LogicStateChanged;
		public int Memory_LogicState
		{
			get { return InternalMemory_LogicState; }
			set
			{
				this.InternalMemory_LogicState = value;
				if (Memory_LogicStateChanged != null)
					Memory_LogicStateChanged();
			}
		}

		public readonly PackedInt32 Memory_Route = new PackedInt32(4);

		public bool Memory_CaveAction;
		public bool Memory_FirstWait;
		public bool Memory_CanBeHitByVehicle;


		public const int Memory_LogicState_Waiting = 0;
		public const int Memory_LogicState_Boarding = 1;
		public const int Memory_LogicState_LastMile = 2;
		public const int Memory_LogicState_Drowning = 3;

		public const int Memory_LogicState_BubbleLength = 120;

		public const int Memory_LogicState_TalkStart = 1000;
		public const int Memory_LogicState_TalkEnd = Memory_LogicState_TalkStart + Memory_LogicState_BubbleLength;

		public const int Memory_LogicState_ConfusedStart = 2000;
		public const int Memory_LogicState_ConfusedEnd = Memory_LogicState_ConfusedStart + Memory_LogicState_BubbleLength;

		public const int Memory_LogicState_CaveLifeStart = 3000;
		public const int Memory_LogicState_CaveLifeEnd = Memory_LogicState_CaveLifeStart + 240;

		public const int Memory_LogicState_WaitingRescueStart = 4000;
		public const int Memory_LogicState_WaitingRescueEnd = Memory_LogicState_WaitingRescueStart + 480;

		public const int Memory_LogicState_FareBase = 10000;
		public const int Memory_LogicState_FareMax = 19999;
		public const int Memory_LogicState_FareMin = 10250;



		public int Memory_Route_NextPlatformIndex
		{
			get
			{
				return (int)this.Memory_Route.Elements[0] - 1;
			}
		}

		public bool Memory_LogicState_IsWaitingRescue
		{
			get
			{
				if (Memory_LogicState < Memory_LogicState_WaitingRescueStart)
					return false;

				if (Memory_LogicState > Memory_LogicState_WaitingRescueEnd)
					return false;

				return true;
			}
		}

		public bool Memory_LogicState_IsFare
		{
			get
			{
				if (Memory_LogicState < Memory_LogicState_FareMin)
					return false;

				if (Memory_LogicState > Memory_LogicState_FareMax)
					return false;

				return true;
			}
		}

		public bool Memory_LogicState_IsTalking
		{
			get
			{
				if (Memory_LogicState < Memory_LogicState_TalkStart)
					return false;

				if (Memory_LogicState > Memory_LogicState_TalkEnd)
					return false;

				return true;
			}
		}

		public bool Memory_LogicState_IsConfused
		{
			get
			{
				if (Memory_LogicState < Memory_LogicState_ConfusedStart)
					return false;

				if (Memory_LogicState > Memory_LogicState_ConfusedEnd)
					return false;

				return true;
			}
		}

		public bool Memory_LogicState_IsCaveLife
		{
			get
			{
				if (Memory_LogicState < Memory_LogicState_CaveLifeStart)
					return false;

				if (Memory_LogicState > Memory_LogicState_CaveLifeEnd)
					return false;

				return true;
			}
		}

		public bool Memory_LogicState_WouldBeConfusedIfVehicleLeft
		{
			get
			{
				if (Memory_LogicState_IsTalking)
					return true;

				if (Memory_LogicState == Actor.Memory_LogicState_Boarding)
					return true;

				return false;
			}
		}
	}
}
