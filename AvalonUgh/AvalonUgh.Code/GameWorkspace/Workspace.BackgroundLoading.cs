using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonUgh.Code.Dialogs;
using ScriptCoreLib.PHP.Runtime;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		public DialogTextBox BackgroundLoading;

		void InitializeBackgroundLoading()
		{
			this.BackgroundLoading = new DialogTextBox
			{
				Zoom = 2,
				Text = "loading...",
				Visibility = Visibility.Hidden
			}.AttachContainerTo(this);

			this.BackgroundLoading.MoveContainerTo(0, Convert.ToInt32(this.Container.Height - BackgroundLoading.Height));

		}
	}
}
