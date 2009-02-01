﻿using System;
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
				InternalColorStripe = value;

				AssertValueColor(value);
				foreach (var v in this.SupportedColorStripes)
				{
					AssertKeyColor(v.Key);
					v.Value.Show(v.Key == value);
				}

			}
		}

		static void AssertKeyColor(Color c)
		{
			var r = c.R;
		}

		static void AssertValueColor(Color c)
		{
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

	
		}

	}
}
