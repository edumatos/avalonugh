using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonUgh.Code.Input
{
	[Script]
	public class PlayerInput
	{
		#region Touch
		TouchInput InternalTouch;

		public TouchInput Touch
		{
			get
			{
				return InternalTouch;
			}
			set
			{
				if (InternalTouch != null)
				{
					InternalTouch.Click -= new Action(InternalTouch_Click);
					InternalTouch.DoubleClick -= new Action(InternalTouch_DoubleClick);
				}

				InternalTouch = value;

				if (InternalTouch != null)
				{
					InternalTouch.Click += new Action(InternalTouch_Click);
					InternalTouch.DoubleClick += new Action(InternalTouch_DoubleClick);
				}
			}
		}
		#endregion

		#region Keyboard
		KeyboardInput InternalKeyboard;

		public KeyboardInput Keyboard
		{
			get
			{
				return InternalKeyboard;
			}
			set
			{
				//if (InternalKeyboard != null)
				//{
				//    InternalKeyboard.Drop -= new Action(InternalKeyboard_Drop);
				//    InternalKeyboard.Enter -= new Action(InternalKeyboard_Enter);
				//}

				InternalKeyboard = value;

				//if (InternalKeyboard != null)
				//{
				//    InternalKeyboard.Drop += new Action(InternalKeyboard_Drop);
				//    InternalKeyboard.Enter += new Action(InternalKeyboard_Enter);
				//}
			}
		}


		#endregion

		public bool HasInput
		{
			get
			{

				if (this.Touch != null)
					if (this.Touch.IsPressed)
						return true;

				if (this.Keyboard != null)
					if (this.Keyboard.KeyState.Any(k => k.Value))
						return true;

				return false;
			}
		}

		void InternalTouch_DoubleClick()
		{
			Console.WriteLine("DoubleClick");
			InternalKeyboard_Enter();
		}

		void InternalTouch_Click()
		{
			Console.WriteLine("Click");
			InternalKeyboard_Drop();
			
		}

		public event Action Enter;
		public event Action Drop;

		void InternalKeyboard_Enter()
		{
			if (Enter != null)
				Enter();
		}

		void InternalKeyboard_Drop()
		{
			if (Drop != null)
				Drop();
		}

		
	}
}

