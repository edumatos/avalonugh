﻿using System;
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
    partial class LevelType
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
                string InternalValue;
                public string Value
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

                public event Action<string> Assigned;

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

                public override string ToString()
                {
                    // seems like actionscript acts differently while concating nulls

                    if (this.Value == null)
                        return this.Key + ":";

                    return this.Key + ": " + this.Value;
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

                public bool BooleanValue
                {
                    get
                    {
                        return Convert.ToBoolean(this.Value);
                    }
                    set
                    {
                        this.Value = Convert.ToInt32(value);
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
            public sealed class Int32_Array : Attribute
            {
                public int[] Value = new int[16];

                public event Action<int[]> Assigned;

                public static implicit operator Attribute.Int32_Array(string Key)
                {
                    return new Attribute.Int32_Array { Key = Key }.Apply(
                        k =>
                            k.Assign =
                                v =>
                                {
                                    var p = v.Split(';');

                                    for (int i = 0; i < k.Value.Length; i++)
                                    {
                                        // if the file does not have all the elements
                                        if (i < p.Length)
                                            k.Value[i] = int.Parse(p[i]);
                                        else
                                            k.Value[i] = 0;
                                    }

                                    k.Assigned(k.Value);
                                }
                    );
                }

                public double DoubleToIntegerScale = 100.0;



                public double this[int index]
                {
                    get
                    {
                        return this.Value[index] / DoubleToIntegerScale;
                    }
                    set
                    {
                        this.Value[index] = Convert.ToInt32(value * DoubleToIntegerScale);
                    }
                }

                public override string ToString()
                {
                    var w = new StringBuilder();

                    w.Append(this.Key + ": ");

                    for (int i = 0; i < this.Value.Length; i++)
                    {
                        if (i > 0)
                            w.Append(";");

                        w.Append(this.Value[i]);
                    }

                    return w.ToString();
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

                public override string ToString()
                {
                    return this.Key + ": " + this.Value0 + ";" + Value1;
                }
            }


            public string Key;

            protected Action<string> Assign;

            public static implicit operator KeyValuePair<string, Action<string>>(Attribute e)
            {
                return new KeyValuePair<string, Action<string>>(
                    e.Key, e.Assign
                );
            }

        }

        //public void Clear()
        //{
        //    foreach (var k in this.GetRemovableEntities().ToArray())
        //    {
        //        k.Dispose();
        //    }

        //    foreach (var k in this.GetRemovablePlatforms().ToArray())
        //    {
        //        k.Dispose();
        //    }
        //}


    }
}
