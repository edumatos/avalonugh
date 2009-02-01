using System;
using System.ComponentModel;
using ScriptCoreLib;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;
using System.Linq;
using AvalonUgh.Code.Input;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Collections.Generic;
using AvalonUgh.Code.Editor.Sprites;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{
		// these delegates need to be overriden to enable
		// network synchronization

		public Action<bool, string> Sync_SetPause;


		[Script]
		public delegate void DelegateTeleportTo(int user, int port, int local, double x, double y, double vx, double vy);
		public DelegateTeleportTo Sync_TeleportTo;

		[Script]
		public delegate void DelegateRemoveLocalPlayer(BindingList<PlayerInfo> a, int local);
		public DelegateRemoveLocalPlayer Sync_RemoveLocalPlayer;

		[Script]
		public delegate void DelegateLoadLevel(int port, int level, string custom);
		public DelegateLoadLevel Sync_LoadLevel;

		public void Sync_LoadLevelEx(Port port, LevelReference level)
		{
			// this is a "serializer" method
			if (level.Location.Embedded == null)
				this.Sync_LoadLevel(port.PortIdentity, -1, level.Data);
			else
				this.Sync_LoadLevel(port.PortIdentity, level.Location.Embedded.AnimationFrame, "");
		}


		[Script]
		public delegate void DelegateLoadLevelHint(int port);
		public DelegateLoadLevelHint Sync_RemoteOnly_LoadLevelHint;

		[Script]
		public delegate void DelegateEditorSelector(int port, int type, int size, int x, int y);
		public DelegateEditorSelector Sync_EditorSelector;


		[Script]
		public delegate void DelegateMouseMove(int port, double x, double y);
		public DelegateMouseMove Sync_RemoteOnly_MouseMove;


		[Script]
		public delegate void DelegateMissionStartHint(int user, int difficulty);
		public DelegateMissionStartHint Sync_MissionStartHint;


		[Script]
		public delegate void DelegateVehicalize(int user, int local);
		public DelegateVehicalize Sync_Vehicalize;


		public BindingList<PlayerIdentity> CoPlayers;

		public IEnumerable<PlayerIdentity> AllPlayers
		{
			get
			{
				return CoPlayers.ConcatSingle(this.LocalIdentity);
			}
		}

		public void InitializeSync()
		{
			#region CoPlayers

			// coplayers are remoted locals
			this.CoPlayers = new BindingList<PlayerIdentity>();
			this.CoPlayers.ForEachNewItem(
				c =>
				{
					c.Locals.AttachTo(this.Players);

				}
			);

			this.CoPlayers.ForEachItemDeleted(
				c =>
				{
					// if coplayers leave at different times it will cause desync
					this.Console.WriteLine("removing all locals for " + c);


					while (c.Locals.Count > 0)
						c.Locals.RemoveAt(0);
				}
			);

			#endregion



			this.Sync_Vehicalize =
				(int user, int local) =>
				{
					var c = this.AllPlayers.Single(k => k.NetworkNumber == user);
					var l = c[local];

					var v = new Vehicle(DefaultZoom);

					l.Actor.ColorStripe = v.SupportedColorStripes.Keys.AtModulus(user);

					v.MoveTo(l.X, l.Y);

					var Level = l.Actor.CurrentLevel;

					Level.KnownVehicles.Add(v);

					l.Actor.CurrentVehicle = v;
				};


			#region Sync_TeleportTo local implementation
			this.Sync_TeleportTo =
				(int user, int port, int local, double x, double y, double vx, double vy) =>
				{
					var c = this.AllPlayers.Single(k => k.NetworkNumber == user);
					BindingList<PlayerInfo> a = c.Locals;

					var p = a.SingleOrDefault(k => k.IdentityLocal == local);

					if (p == null)
					{
						p = new PlayerInfo
						{
							Identity = c,
							IdentityLocal = local,
							Input = new PlayerInput
							{

							}
						};

						if (a == this.LocalIdentity.Locals)
						{
							p.Input.Keyboard = this.SupportedKeyboardInputs[local];
						}
						else
							p.Input.Keyboard = new KeyboardInput(new KeyboardInput.Arguments.Arrows());

						a.Add(p);

						this.Console.WriteLine("created new player via teleport " + new { p.Identity, p.IdentityLocal });

						p.Actor.PlayerInfo = p;
					}





					var CurrentPort = this.Ports.SingleOrDefault(k => k.PortIdentity == port);

					p.Actor.CurrentVehicle = null;

					// assigning a level may assign a pending vehcile
					if (CurrentPort == null)
						p.Actor.CurrentLevel = null;
					else
						p.Actor.CurrentLevel = CurrentPort.Level;


					p.Actor.MoveTo(x, y);
					p.Actor.VelocityX = vx;
					p.Actor.VelocityY = vy;

					if (a == this.LocalIdentity.Locals)
					{
						// every actor could act differently on gold collected
						p.Actor.GoldStash.ForEachNewItem(
							gold =>
							{
								var _CurrentPort = this.Ports.Single(k => k.Level == p.Actor.CurrentLevel);

								_CurrentPort.View.ColorOverlay.Background = Brushes.Yellow;
								_CurrentPort.View.ColorOverlay.Opacity = 0.7;
								_CurrentPort.View.ColorOverlay.Show();
								_CurrentPort.View.ColorOverlay.FadeOut();
							}
						);

					}
				};
			#endregion

			#region Sync_LoadLevel
			this.Sync_LoadLevel =
				(int port, int level, string custom) =>
				{
					var CurrentPort = this.Ports.SingleOrDefault(k => k.PortIdentity == port);

					Console.WriteLine("loading level " + level + " for port " + port + " at frame " + this.LocalIdentity.SyncFrame);

					if (level == -1)
					{
						// will load custom level instead

						// we have just recieved a new level - we might want to save
						// it into our store
						// we need to take a hash of it tho

						var LevelReference =
							new LevelReference(new LevelReference.StorageLocation { Cookie = "editorlevel" })
							{
								Data = custom
							};

						// we might not to want to add duplicates
						// but we do not store hashes at this time

						this.DiscoveredLevels.Add(LevelReference);

						CurrentPort.LevelReference = LevelReference;
					}
					else
					{
						CurrentPort.LevelReference = this.EmbeddedLevels.Single(k => k.Location.Embedded.AnimationFrame == level);
					}

				};
			#endregion

		}
	}
}
