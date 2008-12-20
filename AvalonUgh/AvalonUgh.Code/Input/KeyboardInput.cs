using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Input;
using System.Windows;

namespace AvalonUgh.Code.Input
{
	[Script]
	public class KeyboardInput
	{
		[Script]
		public class Arguments
		{
			public UIElement InputControl;

			public Key Left = Key.Left;
			public Key Right = Key.Right;
			public Key Up = Key.Up;
			public Key Down = Key.Down;

			public Key Drop = Key.Space;
			public Key Enter = Key.Enter;
		}

		public readonly Dictionary<Key, bool> KeyState;

		public event Action<Key, bool> KeyStateChanged;


		public event Action Enter;
		public event Action Drop;

		readonly Arguments a;

		static Arguments DefaultArguments = new Arguments();

		public Key ToDefaultTranslation(Key key)
		{
			var y = DefaultArguments;

			if (key == a.Up)
				return y.Up;

			if (key == a.Down)
				return y.Down;

			if (key == a.Left)
				return y.Left;

			if (key == a.Right)
				return y.Right;

			if (key == a.Enter)
				return y.Enter;

			if (key == a.Drop)
				return y.Drop;

			return key;
		}

		public Key FromDefaultTranslation(Key key)
		{
			var a = DefaultArguments;
			var y = this.a;

			if (key == a.Up)
				return y.Up;

			if (key == a.Down)
				return y.Down;

			if (key == a.Left)
				return y.Left;

			if (key == a.Right)
				return y.Right;

			if (key == a.Enter)
				return y.Enter;

			if (key == a.Drop)
				return y.Drop;

			return key;
		}

		public KeyboardInput(Arguments e)
		{
			this.a = e;

			this.KeyState = new Dictionary<Key, bool>
						{
							{e.Enter, false},
							{e.Up, false},
							{e.Down, false},
							{e.Right, false},
							{e.Left, false},
						};

			if (e.InputControl != null)
			{
				e.InputControl.KeyDown +=
					(sender, args) =>
					{
						if (KeyState.ContainsKey(args.Key))
						{
							args.Handled = true;

							if (KeyState[args.Key] == false)
							{
								KeyState[args.Key] = true;

								if (KeyStateChanged != null)
									KeyStateChanged(args.Key, true);
							}
						}
					};

				e.InputControl.KeyUp +=
					(sender, args) =>
					{
						if (KeyState.ContainsKey(args.Key))
						{
							args.Handled = true;

							if (KeyState[args.Key] == true)
							{
								KeyState[args.Key] = false;

								if (KeyStateChanged != null)
									KeyStateChanged(args.Key, false);
							}
						}

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





		}

		public bool IsPressedEnter { get { return KeyState[a.Enter]; } }
		public bool IsPressedDown { get { return KeyState[a.Down]; } }
		public bool IsPressedUp { get { return KeyState[a.Up]; } }
		public bool IsPressedLeft { get { return KeyState[a.Left]; } }
		public bool IsPressedRight { get { return KeyState[a.Right]; } }

	}
}
