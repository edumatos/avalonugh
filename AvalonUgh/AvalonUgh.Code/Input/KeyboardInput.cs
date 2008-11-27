using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Input;
using System.Windows;

namespace AvalonUgh.Code
{
	[Script]
	public class KeyboardInput
	{
		[Script]
		public class Arguments
		{
			public UIElement InputControl;

			public Vehicle Vehicle;

			public Key Left;
			public Key Right;
			public Key Up;
			public Key Down;

			public Key Drop;
			public Key Enter;

			public View View;
		}

		public readonly Dictionary<Key, bool> KeyState;

		public event Action Enter;
		public event Action Drop;

		readonly Arguments a;

		public KeyboardInput(Arguments e)
		{
			this.a = e;

			this.KeyState = new Dictionary<Key, bool>
						{
							{e.Up, false},
							{e.Down, false},
							{e.Right, false},
							{e.Left, false},
						};

			e.InputControl.KeyDown +=
				(sender, args) =>
				{
					if (KeyState.ContainsKey(args.Key))
					{
						KeyState[args.Key] = true;
						args.Handled = true;
					}
				};


			e.InputControl.KeyUp +=
				(sender, args) =>
				{
					if (KeyState.ContainsKey(args.Key))
					{
						KeyState[args.Key] = false;
						args.Handled = true;
					}
				};

			e.InputControl.KeyUp +=
				(sender, args) =>
				{
					if (args.Key == e.Drop)
					{
						if (Drop != null)
							Drop();

						args.Handled = true;
					}

					if (args.Key == e.Enter)
					{
						if (Enter != null)
							Enter();

						args.Handled = true;
					}
				};





		}

		public bool IsPressedDown { get { return KeyState[a.Down]; } }
		public bool IsPressedUp { get { return KeyState[a.Up]; } }
		public bool IsPressedLeft { get { return KeyState[a.Left]; } }
		public bool IsPressedRight { get { return KeyState[a.Right]; } }

	}
}
