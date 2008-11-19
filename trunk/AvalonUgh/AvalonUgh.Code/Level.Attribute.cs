using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;

namespace AvalonUgh.Code
{
	partial class Level
	{

		[Script]
		public abstract class Attribute
		{
			// tuples
			// # key-subkey: value1; value2

			[Script]
			public sealed class String : Attribute
			{
				public string Value;

				public static implicit operator Attribute.String(string Key)
				{
					return new Attribute.String { Key = Key }.Apply(
						k =>
							k.Assign =
								v =>
								{
									k.Value = v;
								}
					);
				}
			}

			[Script]
			public sealed class Int32 : Attribute
			{
				public int Value;

				public static implicit operator Attribute.Int32(string Key)
				{
					return new Attribute.Int32 { Key = Key }.Apply(
						k =>
							k.Assign =
								v =>
								{
									k.Value = int.Parse(v);
								}
					);
				}
			}


			protected string Key;


			protected Action<string> Assign;

			public static implicit operator KeyValuePair<string, Action<string>>(Attribute e)
			{
				return new KeyValuePair<string, Action<string>>(
					e.Key, e.Assign
				);
			}

		}
	}
}
