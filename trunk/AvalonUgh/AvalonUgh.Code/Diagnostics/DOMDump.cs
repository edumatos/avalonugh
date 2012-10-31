using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Diagnostics;
using System.Linq;

namespace AvalonUgh.Code.Diagnostics
{
    public static  class DOMDump
    {

#if false
		public static partial void WriteTreeToConsoleOnClick(this FrameworkElement x)
		{
			x.MouseLeftButtonUp +=
				(sender, sender_args) =>
				{
					Action<int, int, FrameworkElement, List<Panel>> dump = null;

					dump =
						(i, max, e, parents) =>
						{
							if (e == x)
								System.Console.ForegroundColor = ConsoleColor.Yellow;
							else if (parents.Contains(e as Panel))
								System.Console.ForegroundColor = ConsoleColor.Green;
							else if (parents.Contains(e.Parent as Panel))
								System.Console.ForegroundColor = ConsoleColor.Cyan;

							System.Console.WriteLine(new string('-', i * 2) + " " + e.Name + " " + e.GetType().Name);

							System.Console.ForegroundColor = ConsoleColor.Gray;

							if (i < max)
							{
								var ep = e as Panel;

								if (ep != null)
									foreach (FrameworkElement ec in ep.Children)
									{
										dump(i + 1, max, ec, parents);
									}
							}
						};

					Action<int, FrameworkElement> DumpWithParents =
						(max, e) =>
						{
							var p = e.GetParentPanel();
							var ps = new List<Panel>();

							while (p != null)
							{
								ps.Add(p);

								if (p.Parent is System.Windows.Window)
									p = null;
								else
									p = p.GetParentPanel();
							}

							ps.Reverse();
							ps.ForEach(
								(pv, pi) =>
								{
									dump(pi, pi, pv, ps);
								}
							);

							if (e is Panel)
							{
								ps.Add(e as Panel);
								dump(ps.Count - 1, ps.Count - 1 + max, e, ps);
							}
							else
							{
								dump(ps.Count, ps.Count + max, e, ps);
							}
						};

					System.Console.WriteLine();
					System.Console.WriteLine("click: ");
					DumpWithParents(2, sender_args.Source as FrameworkElement);
					System.Console.WriteLine();
					System.Console.WriteLine("dump: ");
					DumpWithParents(2, x);
				};
		}
#endif

    }
}
