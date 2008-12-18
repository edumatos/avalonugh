using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.IO;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class DialogTextBox : ISupportsContainer
	{
		public Canvas Container { get; set; }

		Color _Color = Colors.Brown;
		public Color Color { get { return _Color; } set { _Color = value; Update(); } }

		TextAlignment _TextAlignment;
		public TextAlignment TextAlignment { get { return _TextAlignment; } set { _TextAlignment = value; Update(); } }

		public event Action TextChanged;
		string _Text;
		public string Text
		{
			get { return _Text; }
			set
			{
				_Text = value;
				Update(); if (TextChanged != null) TextChanged();
			}
		}

		int _Zoom = 1;
		public int Zoom { get { return _Zoom; } set { _Zoom = value; Update(); } }


		int _Width;
		public int Width
		{
			get
			{
				return _Width;
			}
			set
			{
				_Width = value;
				this.Container.Width = value;
				Update();
			}
		}

		int _Height;
		public int Height
		{
			get
			{
				return _Height;
			}
		}

		public DialogTextBox()
		{
			this.Container = new Canvas();
		}



		readonly List<Image> Chars = new List<Image>();



		void Update()
		{
			// we are supporting colors brown and blue
			// and alignments left and center where we will trim spaces in lines

			Chars.ToArray().Orphanize();
			Chars.Clear();

			if (string.IsNullOrEmpty(this.Text))
				return;

			var FontPath = Assets.Shared.KnownAssets.Path.Fonts.Brown;

			if (this.Color == Colors.Blue)
				FontPath = Assets.Shared.KnownAssets.Path.Fonts.Blue;


			var a = new List<string>();
			
			var w = 0;

			using (var s = new StringReader(this.Text))
			{
				var n = s.ReadLine();

				while (n != null)
				{
					w = n.Length.Max(w);

				
					if (this.TextAlignment == TextAlignment.Center)
						a.Add(n.Trim());
					else
						a.Add(n);

					n = s.ReadLine();
				}
			}

			this.Container.Height = _Height;

			var y = 0.0;

			foreach (var n in a)
			{
				if (n == "-")
				{
					y += 0.5;
				}
				else
				{
					var x = 0;

					foreach (var c in n)
					{
						var s = Convert.ToString(c);

						//char.IsWhiteSpace
						if (s == "\t")
						{
							x++;
							continue;
						}

						if (s == " ")
						{
							x++;
							continue;
						}

						var f = PrimitiveFont.ToFileName(s);

						var px = x * (PrimitiveFont.Width + 1) * Zoom;

						if (this.TextAlignment == TextAlignment.Center)
							px += (this.Width - n.Length * (PrimitiveFont.Width + 1) * Zoom) / 2;

						var py = y * PrimitiveFont.Heigth * Zoom;

						new Image
						{
							Source = (FontPath + "/" + f + ".png").ToSource(),
							Stretch = Stretch.Fill,
							Width = PrimitiveFont.Width * Zoom,
							Height = PrimitiveFont.Heigth * Zoom
						}.AttachTo(this).MoveTo(
							px,
							py
						).AddTo(this.Chars);

						x++;
					}

					y++;
				}
			}

			this._Height = Convert.ToInt32( PrimitiveFont.Heigth * Zoom * y);
			// render em
		}


	}
}
