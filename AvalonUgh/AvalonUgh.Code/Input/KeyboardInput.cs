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


			[Script]
			public class IJKL : Arguments
			{
				public IJKL()
				{
					Left = Key.J;
					Right = Key.L;
					Up = Key.I;
					Down = Key.K;
					Drop = Key.U;
					Enter = Key.O;
				}
			}

			[Script]
			public class WASD : Arguments
			{
				public WASD()
				{
					Left = Key.A;
					Right = Key.D;
					Up = Key.W;
					Down = Key.S;
					Drop = Key.Q;
					Enter = Key.E;
				}
			}

			[Script]
			public class Arrows : Arguments
			{
				public Arrows()
				{
					Left = Key.Left;
					Right = Key.Right;
					Up = Key.Up;
					Down = Key.Down;
					Drop = Key.Space;
					Enter = Key.Enter;
				}
			}

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

		readonly Arguments ConstructorArguments;

		static Arguments DefaultArguments = new Arguments();

		public Key ToDefaultTranslation(Key key)
		{
			var y = DefaultArguments;

			if (key == ConstructorArguments.Up)
				return y.Up;

			if (key == ConstructorArguments.Down)
				return y.Down;

			if (key == ConstructorArguments.Left)
				return y.Left;

			if (key == ConstructorArguments.Right)
				return y.Right;

			if (key == ConstructorArguments.Enter)
				return y.Enter;

			if (key == ConstructorArguments.Drop)
				return y.Drop;

			return key;
		}

		public Key FromDefaultTranslation(Key key)
		{
			var a = DefaultArguments;
			var y = this.ConstructorArguments;

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

		public bool Disabled { get; set; }


		public KeyboardInput(Arguments e)
		{
			this.ConstructorArguments = e;

			this.KeyState = new Dictionary<Key, bool>
			{
				{e.Drop, false},
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
						if (Disabled)
							return;

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
						if (Disabled)
							return;

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

					
					};
			}





		}

		public bool IsPressedDrop { get { return KeyState[ConstructorArguments.Drop]; } }
		public bool IsPressedEnter { get { return KeyState[ConstructorArguments.Enter]; } }
		public bool IsPressedDown { get { return KeyState[ConstructorArguments.Down]; } }
		public bool IsPressedUp { get { return KeyState[ConstructorArguments.Up]; } }
		public bool IsPressedLeft { get { return KeyState[ConstructorArguments.Left]; } }
		public bool IsPressedRight { get { return KeyState[ConstructorArguments.Right]; } }

	}
}
