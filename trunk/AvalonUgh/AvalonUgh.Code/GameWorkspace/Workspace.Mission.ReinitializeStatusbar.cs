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
using AvalonUgh.Code.Editor.Sprites;
using AvalonUgh.Assets.Avalon;
namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		partial class MissionPort
		{
			private void ReinitializeStatusbar()
			{
				this.View.Level.LevelTimeChanged +=
					() => this.Statusbar.LevelTime = this.View.Level.LevelTime;

				this.View.Level.KnownVehicles.WithEvents(
					  vehicle =>
					  {
						  // when a passanger enter
						  // we will start tracking the fare
						  // the fare will go down from 9999 to 250
						  // when the passanger exits we will stop
						  // tracking its fare

						  // while passanger onboard listen state changed event

						  vehicle.CurrentWeaponChanged +=
							  delegate
							  {
								  if (vehicle.CurrentWeapon == null)
									  Statusbar.SetLeftSign(-2);
								  else
									  Statusbar.SetLeftSign(-1);

							  };

						  var passangers = vehicle.CurrentPassengers.WithEvents(
							  passanger =>
							  {
								  // passanger has entered our vehicle

								  Statusbar.SetLeftSign(
									  Level.ToPlatformSnapshots().AtModulus(passanger.Memory_Route_NextPlatformIndex).CaveSigns.First().Value
								  );

								  Action Memory_LogicStateChanged =
									  delegate
									  {
										  // if we are being displayed to the user
										  // then update the numbers

										  this.Statusbar.CurrentFareScore = passanger.Memory_LogicState - Actor.Memory_LogicState_FareMin;
									  };

								  passanger.Memory_LogicStateChanged += Memory_LogicStateChanged;
								  Memory_LogicStateChanged();

								  return delegate
								  {
									  // passanger has exited our vehicle

									  Statusbar.SetLeftSign(-2);

									  passanger.Memory_LogicStateChanged -= Memory_LogicStateChanged;

									  // if we are being displayed to the user
									  // then clear the numbers
									  this.Statusbar.CurrentFareScore = 0;
								  };
							  }
						  );


						  return delegate
						  {
							  // vehicle was removed
							  
							  passangers.Dispose();
						  };
					  }
				 );
			}
		}
	}
}
