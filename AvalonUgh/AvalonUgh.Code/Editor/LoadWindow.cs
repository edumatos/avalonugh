using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using AvalonUgh.Assets.Avalon;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonUgh.Code.Editor
{
	[Script]
	public class LoadWindow : Window
	{
		public LoadWindow()
		{
			this.Width = 400;
			this.Height = 200;


			new Image
			{
				Width = 160,
				Height = 100,
				Stretch = System.Windows.Media.Stretch.Fill,
				Source = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Levels,
					Name = "level",
					Extension = "png",
					AnimationFrame = 1,
				},
			}.MoveTo(Padding, Padding).AttachTo(this);

			new TextBox
			{
				Text = "Name: Name"
			}.MoveTo(Padding * 2 + 160, Padding).AttachTo(this);

			// list

			new Image
			{

				Stretch = System.Windows.Media.Stretch.Fill,
				Source = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Levels,
					Name = "level",
					Extension = "png",
					AnimationFrame = 2,
				},
				Width = 48,
				Height = 30,
			}.MoveTo(Padding, Padding * 2 + 100).AttachTo(this);


			new Image
			{
		
				Stretch = System.Windows.Media.Stretch.Fill,
				Source = new NameFormat
				{
					Path = Assets.Shared.KnownAssets.Path.Levels,
					Name = "level",
					Extension = "png",
					AnimationFrame = 3,
				},
				Width = 48,
				Height = 30,
			}.MoveTo(Padding + 48 + Padding, Padding * 2 + 100).AttachTo(this);

			this.Update();
		}

	
	}
}
