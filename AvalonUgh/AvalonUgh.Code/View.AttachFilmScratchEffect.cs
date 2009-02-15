using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Tween;
using System.Windows.Threading;

namespace AvalonUgh.Code
{
	partial class View
	{
		

		private void AttachFilmScratchEffect()
		{
			var Scratches =
				 Enumerable.Range(1, 9).Select(
				 index =>
					 new Image
					 {
						 Source = (Assets.Shared.KnownAssets.Path.FilmScratch + "/" + index.ToString().PadLeft(2, '0') + ".png").ToSource(),
						 Stretch = Stretch.Fill,
						 Width = 32,
						 Height = 32,
						 Visibility = System.Windows.Visibility.Hidden
					 }.AttachTo(this.FilmScratchContainer)
			 ).ToArray();

			this.IsFilmScratchEffectEnabledChanged +=
				delegate
				{

					this.ContentShakeX = 0;
					this.ContentShakeY = 0;

					foreach (var s in Scratches)
						s.Show(this.IsFilmScratchEffectEnabled);

					this.FilmScratchContainer.Show(this.IsFilmScratchEffectEnabled);
					this.ColorOverlay.Show(this.IsFilmScratchEffectEnabled);
				};

			var Shaker = new Random();

			AnimationTimer_FilmScratchEffect = (1000 / 20).AtInterval(
				delegate
				{
					if (this.IsFilmScratchEffectEnabled)
					{

						foreach (var s in Scratches)
						{
							s.MoveTo(
								Shaker.NextDouble() * this.ContentExtendedWidth - 16,
								Shaker.NextDouble() * this.ContentExtendedHeight - 16
								);
						}
						this.FlashlightContainer.Opacity = (Shaker.NextDouble() * 0.3 + 0.8).Min(1);
					}

					if (this.IsShakerEnabled)
					{
						var Shake = 2.0;
						this.ContentShakeX = (Shaker.NextDouble() * Shake) - Shake / 2;
						this.ContentShakeY = (Shaker.NextDouble() * Shake) - Shake / 2;
						this.MoveContentTo();
					}
				}
			);
		}


		DispatcherTimer AnimationTimer_FilmScratchEffect;



		public event Action IsFilmScratchEffectEnabledChanged;
		bool _IsFilmScratchEffectEnabled;
		public bool IsFilmScratchEffectEnabled
		{
			get
			{
				return _IsFilmScratchEffectEnabled;
			}
			set
			{
				_IsFilmScratchEffectEnabled = value;
				if (IsFilmScratchEffectEnabledChanged != null)
					IsFilmScratchEffectEnabledChanged();
			}
		}


		public event Action IsShakerEnabledChanged;
		bool _IsShakerEnabled;
		public bool IsShakerEnabled
		{
			get
			{
				return _IsShakerEnabled;
			}
			set
			{
				this.ColorOverlay.Background = Brushes.Brown;

				_IsShakerEnabled = value;
				if (IsShakerEnabledChanged != null)
					IsShakerEnabledChanged();
			}
		}

	}
}
