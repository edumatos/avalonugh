using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Editor;
using System.ComponentModel;
using AvalonUgh.Code.Dialogs;
using System.Windows.Input;
using ScriptCoreLib.Shared.Avalon.Tween;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using System.Windows.Shapes;
using AvalonUgh.Code.Input;

namespace AvalonUgh.Code
{
	partial class Workspace
	{
		void Think()
		{
			if (this.LocalIdentity.SyncFramePaused)
			{
				if (this.LocalIdentity.SyncFramePausedSkip)
				{
					this.LocalIdentity.SyncFramePausedSkip = false;
				}
				else
				{
					return;
				}
			}

			if (this.LocalIdentity.SyncFrameLimit > 0)
			{
				if (this.LocalIdentity.SyncFrameLimit <= this.LocalIdentity.SyncFrame)
				{
					return;
				}
			}

			if (this.Ports.Any(k => k.Level == null))
				return;


			//// we could pause the game here
			foreach (var p in Players)
			{
				p.AddAcceleration();
			}

			foreach (var p in this.Ports)
			{
				if (this.LocalIdentity.SyncFrame % 30 == 0)
					if (p.Level.AttributeWaterRise.BooleanValue)
						p.Level.AttributeWater.Value++;

				// some animations need to be synced by frame
				foreach (var dino in p.Level.KnownDinos)
				{
					dino.Animate(this.LocalIdentity.SyncFrame);
				}

				foreach (var t in p.Level.KnownTryoperus)
				{
					t.Think();
				}

				p.Level.Physics.Apply();
			}


			this.LocalIdentity.SyncFrame++;
		}


	}
}
