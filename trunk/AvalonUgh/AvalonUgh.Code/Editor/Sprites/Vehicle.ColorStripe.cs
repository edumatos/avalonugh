using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvalonUgh.Code.Input;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using ScriptCoreLib;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Assets.Avalon;

namespace AvalonUgh.Code.Editor.Sprites
{
    partial class Vehicle
    {

        Color InternalColorStripe;
        public Color ColorStripe
        {
            get
            {
                return InternalColorStripe;
            }
            set
            {
                //if (value == null)
                //    throw new Exception("set ColorStripe value null");

                InternalColorStripe = value;

                //            TypeError: Error #1009: Cannot access a property or method of a null object reference.
                //at AvalonUgh.Code.Editor.Sprites::Vehicle$/AssertKeyColor_100664450()[R:\web\AvalonUgh\Code\Editor\Sprites\Vehicle.as:933]
                //at AvalonUgh.Code.Editor.Sprites::Vehicle/set ColorStripe()[R:\web\AvalonUgh\Code\Editor\Sprites\Vehicle.as:426]
                //at AvalonUgh.Code.Editor.Sprites::Vehicle/set IsUnmanned()[R:\web\AvalonUgh\Code\Editor\Sprites\Vehicle.as:391]

                AssertValueColor(value);

                Console.WriteLine(
                    "set ColorStripe " + new
                    {
                        SupportedColorStripes = new
                        {
                            this.SupportedColorStripes.Count
                        }
                    }
                );

                foreach (var v in this.SupportedColorStripes)
                {
                    var Key = v.Key;
                    Console.WriteLine("set ColorStripe " + new { Key });

                    //set ColorStripe { SupportedColorStripes = { Count = 4 } }
                    //set ColorStripe { Key = }

                    AssertKeyColor(Key);


                    v.Value.Show(Key == value);
                }

            }
        }

        static void AssertKeyColor(Color c)
        {
            var r = c.R;
        }

        static void AssertValueColor(Color c)
        {
            // its a struct!
            var r = c.R;
        }


        public Dictionary<Color, Image> SupportedColorStripes;

        private void InitializeColorStripe()
        {
            var Name = new NameFormat
            {
                Name = "vehicle",
                Width = 2,
                Height = 2,
                ToSource = n => (Assets.Shared.KnownAssets.Path.Sprites + "/" + n + ".png").ToSource(),
                Zoom = this.Zoom
            };


            //dictionary_22 = new ScriptCoreLib.ActionScript.BCLImplementation.System.Collections.Generic.__Dictionary_2(null);
            //dictionary_22.Add_100669663(__Colors.Red, NameFormat.op_Implicit_100663311(format0.ToAnimationFrame_100663306("red")));
            //dictionary_22.Add_100669663(__Colors.Blue, NameFormat.op_Implicit_100663311(format0.ToAnimationFrame_100663306("blue")));
            //dictionary_22.Add_100669663(__Colors.Yellow, NameFormat.op_Implicit_100663311(format0.ToAnimationFrame_100663306("yellow")));
            //dictionary_22.Add_100669663(__Colors.Gray, NameFormat.op_Implicit_100663311(format0.ToAnimationFrame_100663306("gray")));

            var Red = Colors.Red;

            this.SupportedColorStripes = new Dictionary<Color, Image>
			{
				{ Red, Name.ToAnimationFrame("red") },
				{ Colors.Blue, Name.ToAnimationFrame("blue") },
				{ Colors.Yellow, Name.ToAnimationFrame("yellow") },
				{ Colors.Gray, Name.ToAnimationFrame("gray") }
			};

            foreach (var v in this.SupportedColorStripes)
            {
                // wtf?
                var Key = v.Key;
                Console.WriteLine("InitializeColorStripe SupportedColorStripes:" + new { Key });

            }

            this.SupportedColorStripes.Values.ForEach(
                k =>
                {
                    k.Hide();
                    k.AttachTo(this.Container);
                }
            );


        }

    }
}
