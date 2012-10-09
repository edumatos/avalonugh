using ScriptCoreLib.Extensions;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace AvalonUgh.LabsActivity
{
    public class ApplicationCanvas : AvalonUgh.Labs.Shared.LabsCanvas
    {
        public readonly Rectangle r = new Rectangle();

        public ApplicationCanvas()
        {
            this.GameWorkspace.Audio_Music.Enabled = true;


            //r.Fill = Brushes.Red;
            //r.AttachTo(this);
            //r.MoveTo(8, 8);
            //this.SizeChanged += (s, e) => r.SizeTo(this.Width - 16.0, this.Height - 16.0);
        }

    }
}
