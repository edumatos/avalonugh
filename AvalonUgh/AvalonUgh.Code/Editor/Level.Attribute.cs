using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;

namespace AvalonUgh.Code.Editor
{
	partial class Level
	{
		[Script]
		public sealed class AttributeDictonary : Dictionary<string, Action<string>>
		{
			public void Add(Attribute e)
			{
				KeyValuePair<string, Action<string>> i = e;

				this.Add(i);
			}
		}

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
				int InternalValue;
				public int Value
				{
					get
					{
						return InternalValue;
					}
					set
					{
						InternalValue = value;

						if (this.Assigned != null)
							this.Assigned(value);
					}
				}

				public event Action<int> Assigned;


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

				public static implicit operator bool(Attribute.Int32 e)
				{
					return e.Value > 0;
				}

				public override string ToString()
				{
					return this.Key + ": " + this.Value;
				}
			}

			[Script]
			public sealed class Int32_Int32 : Attribute
			{
				public int Value0;
				public int Value1;

				public event Action<int, int> Assigned;

				public static implicit operator Attribute.Int32_Int32(string Key)
				{
					return new Attribute.Int32_Int32 { Key = Key }.Apply(
						k =>
							k.Assign =
								v =>
								{
									var p = v.Split(';');

									if (p.Length > 0)
										k.Value0 = int.Parse(p[0]);

									if (p.Length > 1)
										k.Value1 = int.Parse(p[1]);

									if (k.Assigned != null)
										k.Assigned(k.Value0, k.Value1);
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
