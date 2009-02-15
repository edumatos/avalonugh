using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AvalonUgh.Labs
{
	class TimerEventTracer
	{
		public static void Trace(Action handler)
		{
			var Trace = new[] { new { Target = default(Delegate), Ticks = 0L } }.ToList();
			Trace.Clear();

			Action<Delegate, long> Add = (Target, Ticks) => Trace.Add(new { Target, Ticks });

			ScriptCoreLib.Shared.Avalon.Extensions.AvalonSharedExtensions.TimerEvent += Add;

			handler();
			ScriptCoreLib.Shared.Avalon.Extensions.AvalonSharedExtensions.TimerEvent -= Add;

			Console.WriteLine("TimerEventTrace:");

			var query =
				from CurrentTypeGroup in
					from HandlerGroup in
						from e in Trace
						group e by e.Target.Method
					let Ticks = HandlerGroup.Aggregate(0L, (seed, value) => seed + value.Ticks)
					let Count = HandlerGroup.Count()
					let TypeGroup = new { HandlerGroup, Count, Ticks }
					group TypeGroup by TypeGroup.HandlerGroup.Key.DeclaringType
				let Ticks = CurrentTypeGroup.Aggregate(0L, (seed, value) => seed + value.Ticks)
				let Count = CurrentTypeGroup.Aggregate(0, (seed, value) => seed + value.Count)
				orderby Ticks descending, Count descending
				select new { CurrentTypeGroup, Ticks, Count };

			var TraceLog = new StringBuilder();

			foreach (var i in query)
			{

				TraceLog.AppendLine(
					TimeSpan.FromTicks(i.Ticks) + " " +

					i.Count.ToString("00000000")



					//i.x.Key.DeclaringType.FullName + " " + i.x.Key.Name
				);

				TraceLog.AppendLine("  " + i.CurrentTypeGroup.Key.FullName);

				foreach (var j in i.CurrentTypeGroup)
				{
					TraceLog.AppendLine(
						"    " +
						TimeSpan.FromTicks(j.Ticks) + " " +

						j.Count.ToString("00000000") + " " +

						 j.HandlerGroup.Key.Name
					);
				}

				TraceLog.AppendLine();
			}

			var log = new FileInfo("log/" + DateTime.Now.Ticks + ".txt");
			log.Directory.Create();
			File.WriteAllText(log.FullName, TraceLog.ToString());

			Console.WriteLine(log.FullName);

			

		}
	}
}
