using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Editor;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Cursors;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.Tween;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using AvalonUgh.Code.Dialogs;
namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		[Script]
		public class MissionPort : Port
		{
			public readonly LevelIntroDialog Intro;
			public readonly Dialog Fail;

			[Script]
			public class ConstructorArguments
			{
				public int Padding;
				public int Width;
				public int Height;
				public int Zoom;
			}

			public MissionPort(ConstructorArguments args)
			{
				var Statusbar = new Statusbar(
					  new Statusbar.ConstructorArguments
					  {
						  Zoom = args.Zoom
					  }
				  );

				this.Padding = args.Padding;
				this.Zoom = args.Zoom;
				this.StatusbarHeight = Statusbar.Height + 2 * args.Zoom;

				this.Width = args.Width;
				this.Height = args.Height;

				Statusbar.MoveContainerTo(0, args.Height - Statusbar.Height - 1 * args.Zoom);
				Statusbar.AttachContainerTo(this.Window.ContentContainer);


				this.Intro = new LevelIntroDialog
				{
					Width = args.Width,
					Height = args.Height,
					Zoom = args.Zoom,
				};

				this.Intro.AttachContainerTo(this.Window.OverlayContainer);

				this.Fail = new Dialog
				{
					Width = args.Width,
					Height = args.Height,
					Zoom = args.Zoom,
					BackgroundVisible = false,
					VerticalAlignment = VerticalAlignment.Center,
					Text = @"
				   bad luck
				  you failed
				"
				};

				this.Fail.AttachContainerTo(this.Window.OverlayContainer);


				this.WhenLoaded(
					delegate
					{
						this.Intro.BringContainerToFront();
					}
				);


				//this.PlayerJoined +=
				//    k =>
				//    {
				//        k.Actor.CurrentLevel = this.Level;


				//        var StartVehicle = this.Level.KnownVehicles.FirstOrDefault(i => i.CurrentDriver == null);

				//        if (StartVehicle == null)
				//        {
				//            var StartPositionStone = this.Level.KnownStones.Random(i => i.Selector.PrimitiveTileCountX > 1 && i.Selector.PrimitiveTileCountY > 1);

				//            StartVehicle = new Vehicle(this.Level.Zoom).AddTo(this.Level.KnownVehicles);
				//            StartVehicle.MoveTo(StartPositionStone.X, StartPositionStone.Y);
				//        }

				//        k.Actor.CurrentVehicle = StartVehicle;
				//    };


			}
		}
	}
}
