using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Assets.Avalon;
using System.Windows.Media;

namespace AvalonUgh.Code
{
    [Script]
    public class Statusbar : ISupportsContainer
    {
        public Canvas Container { get; set; }




        public int Width
        {
            get
            {
                return 320 * Arguments.Zoom;
            }
        }

        public int Height
        {
            get
            {
                return 7 * Arguments.Zoom;
            }
        }

        [Script]
        public class StatusbarWindow : Window
        {
            public readonly Statusbar Statusbar = new Statusbar();

            public StatusbarWindow()
            {
                ClientWidth = Statusbar.Width;
                ClientHeight = Statusbar.Height;

                Statusbar.AttachContainerTo(this.ContentContainer);

                DraggableArea.BringToFront();

                this.BackgroundColor = Colors.Black;
            }
        }

        public Statusbar()
            : this(null)
        {

        }

        [Script]
        public class ConstructorArguments
        {
            public int Zoom = 2;
        }

        public readonly ConstructorArguments Arguments;
        public Statusbar(ConstructorArguments Arguments)
        {
            if (Arguments == null)
                Arguments = new ConstructorArguments();

            this.Arguments = Arguments;

            this.Container = new Canvas
            {
                Width = Width,
                Height = Height
            };

            var BackgroundSolo = new Image
            {
                Width = Width,
                Height = Height,
                Stretch = System.Windows.Media.Stretch.Fill,
                Source = new NameFormat
                {
                    Path = Assets.Shared.KnownAssets.Path.Statusbar,
                    Name = "solo",
                    Index = 0,
                    Extension = "png"
                }
            };

            BackgroundSolo.AttachTo(this);
        }
    }
}
