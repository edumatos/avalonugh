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
using System.Windows.Input;
using System.Windows.Controls;
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Code.Diagnostics;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{

		private void InitializeEditorArrowClick()
		{
			var CurrentTravelWindows = new BindingList<Window>().AttachTo(this.Container);


			this.Editor.ArrowClick +=
				(Position, args) =>
				{



					var x = Position.ContentX * DefaultZoom;
					var y = Position.ContentY * DefaultZoom;

					var SelectedPassangers = this.Editor.Level.KnownPassengers.Where(k => k.ToObstacle().Contains(x, y)).ToArray();
					var SelectedSigns = this.Editor.Level.KnownSigns.Where(k => k.ToObstacle().Contains(x, y)).ToArray();

					#region AutoscrollEnabled
					if (CurrentTravelWindows.Count == 0)
						if (!SelectedPassangers.Any())
							if (!SelectedSigns.Any())
							{
								// if we did not click on an actor
								// we are probably wanting to change the follow mode
								// from fixed to center to follow the mouse mode

								if (this.Editor.View.AutoscrollEnabled)
								{
									this.Editor.View.AutoscrollEnabled = false;
									this.Editor.View.LocationTracker.Target = null;

									this.Editor.View.MovetToContainerCenter();

								}
								else
								{
									this.Editor.View.AutoscrollEnabled = true;
									this.Editor.View.LocationTracker.Target = this.Editor_LocalKnownMouseLocation;

									this.Editor_LocalKnownMouseLocation.RaiseLocationChanged();
								}

								return;
							}
					#endregion


					CurrentTravelWindows.RemoveAll();


					if (!SelectedPassangers.Any())
					{
						// we didnt click on a passanger
						// maybe we clicked on a sign


						if (SelectedSigns.Any())
						{
							// we could show a window here
							// or we can simple do our thing without a window

							ChangeWaitingPositionPreferences(SelectedSigns);


							return;
						}
					}

					SelectedPassangers.ForEach(
						(Actor Passanger, int index) =>
						{


							// show a dialog for travel order

							this.Console.WriteLine("Memory_Route: " + Passanger.Memory_Route.Value);

							var CurrentTravelWindow = new RouteWindow
							{
								DragContainer = this.Container,
								CurrentLevel = this.Editor.Level,
								CurrentRoute = Passanger.Memory_Route,
							};


							//CurrentTravelWindow.ContentContainer.Background = Brushes.Red;

							//CurrentTravelWindow.Container.WriteTreeToConsoleOnClick();



							var p = args.GetPosition(this.Container);

							CurrentTravelWindow.MoveContainerTo(p.X + 4, p.Y + 4 + CurrentTravelWindow.Height * index);
							CurrentTravelWindow.AddTo(CurrentTravelWindows);

						}
					);
				};
		}

		private static void ChangeWaitingPositionPreferences(Sign[] SelectedSigns)
		{
			var WaitPositionPreferences = new[]
			{
				Sign.WaitPositionPreferences.AtSign,
				Sign.WaitPositionPreferences.NearSign,
				Sign.WaitPositionPreferences.Middle,
				Sign.WaitPositionPreferences.NearCave,
				Sign.WaitPositionPreferences.AtCave,

				Sign.WaitPositionPreferences.BeforeCave,

				Sign.WaitPositionPreferences.OtherSideAtCave,
			};


			SelectedSigns.ForEach(
				q => q.WaitPositionPreference = WaitPositionPreferences.Next(k => k == q.WaitPositionPreference)
			);
		}


	}
}
