using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using AvalonUgh.Code.Input;
using AvalonUgh.Code.Editor.Sprites;

namespace AvalonUgh.Code
{
	[Script]
	public class PlayerInfo : ISupportsLocationChanged
	{
		// player can have
		// a cave (where he is in)
		// a man (as which he can move)
		// a vehicle (in which he can fly)
		// input

		/// <summary>
		/// Each client connected to the server gets its unique number,
		/// Name of the player or name of the team that is playing on this client
		/// </summary>
		public PlayerIdentity Identity = new PlayerIdentity();


		/// <summary>
		/// Each client can create multiple players for splitview game mode
		/// </summary>
		public int IdentityLocal;



		public override string ToString()
		{
			return new { IdentityLocal, Identity.Number, Identity.Name }.ToString();
		}


		Actor InternalActor;
		public Actor Actor
		{
			get
			{
				return InternalActor;
			}
			set
			{
				if (InternalActor != null)
				{
					InternalActor.LocationChanged -= InternalActor_LocationChanged;
					InternalActor.CurrentVehicleChanged -= InternalActor_CurrentVehicleChanged;
				}

				InternalActor = value;

				if (InternalActor != null)
				{
					InternalActor.LocationChanged += InternalActor_LocationChanged;
					InternalActor.CurrentVehicleChanged += InternalActor_CurrentVehicleChanged;
				}
			}
		}

		Vehicle InternalActor_CurrentVehicle;
		void InternalActor_CurrentVehicleChanged()
		{
			if (InternalActor_CurrentVehicle != null)
			{
				InternalActor_CurrentVehicle.LocationChanged -= InternalActor_LocationChanged;
				InternalActor_CurrentVehicle.CurrentWeaponChanged -= InternalActor_CurrentVehicle_CurrentWeaponChanged;
			}
			InternalActor_CurrentVehicle = InternalActor.CurrentVehicle;
			if (InternalActor_CurrentVehicle != null)
			{
				InternalActor_CurrentVehicle.LocationChanged += InternalActor_LocationChanged;
				InternalActor_CurrentVehicle.CurrentWeaponChanged += InternalActor_CurrentVehicle_CurrentWeaponChanged; 
			}
		}

		void InternalActor_CurrentVehicle_CurrentWeaponChanged()
		{
			if (CurrentVehicle_CurrentWeaponChanged != null)
				CurrentVehicle_CurrentWeaponChanged();
		}

		void InternalActor_LocationChanged()
		{
			if (this.LocationChanged != null)
				this.LocationChanged();
		}


		public PlayerInput Input;

		public void AddAcceleration()
		{
			if (Input == null)
				return;

			if (Actor.CurrentVehicle != null)
				Actor.CurrentVehicle.AddAcceleration(Input);
			else
				Actor.AddAcceleration(Input);
		}

		#region ISupportsLocationChanged Members

		public double X
		{
			get
			{
				if (Actor.CurrentVehicle != null)
					return Actor.CurrentVehicle.X;
				else
					return Actor.X;
			}
		}

		public double Y
		{
			get
			{
				if (Actor.CurrentVehicle != null)
					return Actor.CurrentVehicle.Y;
				else
					return Actor.Y;
			}
		}

		public event Action CurrentVehicle_CurrentWeaponChanged;
		public event Action LocationChanged;

		#endregion
	}
}
