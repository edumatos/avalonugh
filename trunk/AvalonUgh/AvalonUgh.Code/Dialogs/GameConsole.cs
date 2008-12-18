﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class GameConsole : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public Rectangle Shade { get; set; }

		public TextBox TextBox { get; set; }

		public GameConsole()
		{
			this.Container = new Canvas
			{

			};

			this.Shade = new Rectangle
			{
				Fill = Brushes.Black,
				Opacity = 0.5
			}.AttachTo(this);

			this.TextBox = new TextBox
			{
				FontFamily = new FontFamily("Courier New"),
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0),
				Foreground = Brushes.Yellow,
				AcceptsReturn = true,
				IsReadOnly = true
			}.AttachTo(this);

			MaxLogQueueCount = 10;
		}

		Queue<string> LogQueue = new Queue<string>();

		public int Height { get; set; }

		public void SizeTo(int w, int h)
		{
			this.Height = h;

			this.Container.SizeTo(w, h);
			this.Shade.SizeTo(w, h);
			this.TextBox.SizeTo(w, h);
		}

		public int MaxLogQueueCount { get; set; }

		int Counter = 0;

		public void WriteLine(string Text)
		{
			Counter = (Counter + 1) % 1000;

			var Padding = new string(' ', this.Tasks.Count * 2);

			LogQueue.Enqueue(Counter.ToString().PadLeft(3, '0') + " " + Padding + Text);

			while (LogQueue.Count > MaxLogQueueCount)
				LogQueue.Dequeue();

			var EmptyPadding = Enumerable.Range(0, MaxLogQueueCount - LogQueue.Count).Aggregate("",
				(Value, Index) =>
				{
					return Value + Environment.NewLine;
				}
			);

			TextBox.Text = LogQueue.Aggregate(EmptyPadding,
				(Value, QueueText) =>
				{
					if (Value == "")
						return QueueText;

					return Value + Environment.NewLine + QueueText;
				}
			);
		}

		int _AnimatedTop;
		Action<int, int> _AnimatedPositionApply;

		public int AnimatedTop
		{
			get
			{
				return _AnimatedTop;
			}
			set
			{
				if (_AnimatedPositionApply == null)
					_AnimatedPositionApply = NumericEmitter.Of((x, y) => this.MoveContainerTo(x, y));

				_AnimatedTop = value;
				_AnimatedPositionApply(0, value);
			}
		}

		[Script]
		public class Task : IDisposable
		{
			readonly Action Done;

			public Task(GameConsole e, string Task)
			{

				e.WriteLine(Task);

				e.Tasks.Push(this);

				Done =
					delegate
					{
						e.Tasks.Pop();
					};
			}

			#region IDisposable Members

			public void Dispose()
			{
				Done();
			}

			#endregion
		}

		public readonly Stack<Task> Tasks = new Stack<Task>();

		public IDisposable this[string Task]
		{
			get
			{
				return new Task(this, Task);
			}
		}
	}
}
