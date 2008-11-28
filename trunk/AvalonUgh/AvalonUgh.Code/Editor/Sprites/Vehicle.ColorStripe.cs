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

namespace AvalonUgh.Code.Editor.Sprites
{
	partial class Vehicle
	{
		//readonly Image ColorStripeRed;
		//readonly Image ColorStripeBlue;
		//readonly Image ColorStripeYellow;
		//readonly Image ColorStripeGray;

		public Color ColorStripe
		{
			set
			{
				foreach (var v in this.SupportedColorStripes)
				{
					v.Value.Show(v.Key == value);
				}

				//ColorStripeBlue.Hide();
				//ColorStripeRed.Hide();
				//ColorStripeYellow.Hide();

				//if (value == Colors.Red)
				//{
				//    ColorStripeRed.Show();
				//    return;
				//}

				//if (value == Colors.Blue)
				//{
				//    ColorStripeBlue.Show();
				//    return;
				//}

				//if (value == Colors.Yellow)
				//{
				//    ColorStripeYellow.Show();
				//    return;
				//}

				//if (value == Colors.Gray)
				//{
				//    ColorStripeGray.Show();
				//    return;
				//}
			}
		}

		 Dictionary<Color, Image> SupportedColorStripes;

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


			this.SupportedColorStripes = new Dictionary<Color, Image>
			{
				{ Colors.Red, Name.ToAnimationFrame("red") },
				{ Colors.Blue, Name.ToAnimationFrame("blue") },
				{ Colors.Yellow, Name.ToAnimationFrame("yellow") },
				{ Colors.Gray, Name.ToAnimationFrame("gray") }
			};

			this.SupportedColorStripes.Values.ForEach(
				k =>
				{
					k.Hide();
					k.AttachTo(this.Container);
				}
			);

			//Func<string, Image> CreateStripe =
			//             color =>
			//                 new Image
			//                 {
			//                     Source = (Assets.Shared.KnownAssets.Path.Sprites + "/vehicle0_" + color + "_2x2.png").ToSource(),
			//                     Stretch = Stretch.Fill,
			//                     Width = this.Width,
			//                     Height = this.Height,
			//                     Visibility = Visibility.Hidden
			//                 }.AttachTo(this.Container);

			//this.ColorStripeRed = CreateStripe("red");
			//this.ColorStripeBlue = CreateStripe("blue");
			//this.ColorStripeYellow = CreateStripe("yellow");
			//this.ColorStripeGray = CreateStripe("gray");
		}

	}
}
