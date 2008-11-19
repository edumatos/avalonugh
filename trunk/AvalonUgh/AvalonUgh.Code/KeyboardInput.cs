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

		Dictionary<Key, bool> KeyState;

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
						KeyState[args.Key] = true;
				};


			e.InputControl.KeyUp +=
				(sender, args) =>
				{
					if (KeyState.ContainsKey(args.Key))
						KeyState[args.Key] = false;
				};

			e.InputControl.KeyUp +=
				(sender, args) =>
				{
					if (args.Key == e.Drop)
					{
						if (Drop != null)
							Drop();

					}

					if (args.Key == e.Enter)
					{
						if (Enter != null)
							Enter();

					}
				};





		}

		public void Tick()
		{
			var xveh = a.Vehicle;


			if (xveh.IsUnmanned)
				return;

			Func<Key, bool> IsKeyDown =
				k => KeyState[k];

			if (KeyState.Any(k => k.Value))
			{
				xveh.IsAnimated = true;
			}
			else
			{
				xveh.IsAnimated = false;
			}


			if (IsKeyDown(a.Up))
			{
				xveh.VelocityY -= xveh.Acceleration * 2;
			}
			else if (IsKeyDown(a.Down))
			{
				if (xveh.Y > a.View.Level.WaterTop)
					xveh.IsAnimated = false;
				else
					xveh.VelocityY += xveh.Acceleration;
			}

			if (IsKeyDown(a.Left))
			{
				xveh.VelocityX -= xveh.Acceleration;
			}
			else if (IsKeyDown(a.Right))
			{
				xveh.VelocityX += xveh.Acceleration;
			}
		}
	}
}
