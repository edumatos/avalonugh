using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Assets.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.Code.Dialogs
{
	[Script]
	public class DialogTextBox : ISupportsContainer
	{
        public int FontWidth = PrimitiveFont.Width;
        public int FontHeigth = PrimitiveFont.Heigth;

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

		public bool AutoSize;

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

		public readonly Rectangle TouchOverlay;

		public event Action Click;

		public void Show(bool value)
		{
			this.Container.Show(value);
			this.TouchOverlay.Show(value);
		}

		public DialogTextBox()
		{
			this.Container = new Canvas
			{
				Name = "DialogTextBox_Container",
			};

			this.TouchOverlay = new Rectangle
			{
				Name = "DialogTextBox_TouchOverlay",
				Fill = Brushes.Red,
				Opacity = 0,
				Cursor = Cursors.Hand
			};

			this.TouchOverlay.MouseEnter +=
				delegate
				{
					this.Color = Colors.Blue;
				};

			this.TouchOverlay.MouseLeave +=
				delegate
				{
					this.Color = Colors.Brown;
				};

			this.TouchOverlay.MouseLeftButtonUp +=
				delegate
				{
					if (this.Click != null)
						this.Click();
				};
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

            if (this.Color == Colors.White)
                FontPath = Assets.Shared.KnownAssets.Path.Fonts.White;


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

			var _Width = 0.0;
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

						var px = x * (FontWidth + 1) * Zoom;

						var nLength = n.Length;

						if (n.EndsWith("?"))
							nLength--;

						if (this.TextAlignment == TextAlignment.Center)
							px += (this.Width - nLength * (FontWidth + 1) * Zoom) / 2;

						var py = y * this.FontHeigth * Zoom;

						new Image
						{
							Source = (FontPath + "/" + f + ".png").ToSource(),
							Stretch = Stretch.Fill,
							Width = FontWidth * Zoom,
							Height = FontHeigth * Zoom
						}.AttachTo(this).MoveTo(
							px,
							py
						).AddTo(this.Chars);

						_Width = _Width.Max(px + FontWidth * Zoom);
						x++;
					}

					y++;
				}
			}

			this._Height = Convert.ToInt32(FontHeigth * Zoom * y);
			// render em

			if (this.AutoSize)
				this._Width = Convert.ToInt32(_Width);

            this.Container.SizeTo(_Width, _Height);
            this.TouchOverlay.SizeTo(_Width, _Height);
		}

		public Visibility Visibility
		{
			get
			{
				return this.Container.Visibility;
			}
			set
			{
				this.Container.Visibility = value;
				this.TouchOverlay.Visibility = value;
			}
		}
	}
}
