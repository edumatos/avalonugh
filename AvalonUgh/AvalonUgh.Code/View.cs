﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Editor;
using AvalonUgh.Code.Input;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows;

namespace AvalonUgh.Code
{
	/// <summary>
	/// View enables to display maps that are larger than the provided
	/// client area by enabling autoscroll. In the editor mode the scroll
	/// should follow the mouse while in playmode it should follow
	/// the active player be it a vehicle or an actor
	/// </summary>
	[Script]
	public partial class View : ISupportsContainer, IDisposable
	{
		public Canvas Container { get; set; }

		public Canvas Background { get; set; }

		public Canvas Content { get; set; }

		public Canvas Platforms { get; set; }
		public Canvas PlatformsInfoOverlay { get; set; }

		public Canvas Entities { get; set; }
		public Canvas StartPositionsContainer { get; set; }
		public Canvas BubbleContainer { get; set; }

		public Canvas ContentExtendedContainer { get; set; }

		public Canvas SnowContainer { get; set; }
		public Canvas WaterContainer { get; set; }

		public Canvas FilmScratchContainer { get; set; }

		public Canvas FlashlightContainer { get; set; }

		public Canvas ColorOverlay { get; set; }

		public Canvas ContentInfoOverlay { get; set; }

		public Canvas TouchOverlay { get; set; }
		public TouchInputType TouchInput { get; set; }

		public LevelType Level { get; set; }

		public int ContentActualWidth
		{
			get { return this.Level.ActualWidth; }
		}

		public int ContentActualHeight
		{
			get { return this.Level.ActualHeight; }
		}

		public int ContentExtendedWidth
		{
			get
			{
				return this.ContentActualWidth + (this.ContainerWidth - this.ContentActualWidth).Max(0) + MaxShakeSize * 2;
			}
		}

		public int ContentExtendedHeight
		{
			get
			{
				return this.ContentActualHeight + (this.ContainerHeight - this.ContentActualHeight).Max(0) + MaxShakeSize * 2;
			}
		}

		public LocationTrackerType LocationTracker { get; set; }

		public FlashlightType Flashlight { get; set; }

		public int ContainerWidth { get; set; }
		public int ContainerHeight { get; set; }

		public const int MaxShakeSize = 4;

		public Water CurrentWater;

		public View(int width, int height, LevelType level)
		{
			this.ContainerWidth = width;
			this.ContainerHeight = height;

			// for the fist configuration we do need to know all the 
			// variables, even if we allow them to be changed later
			// like enable actual scrolling at a later point of time
			// or change the size of the view

			this.Container = new Canvas
			{

				Width = width,
				Height = height,
				Background = Brushes.Black,
				ClipToBounds = true
			};

			this.Background = new Canvas
			{

				Width = width,
				Height = height,
			}.AttachTo(this.Container);

			// Content actual size depends on level
			this.Level = level;

			if (Level.BackgroundImage != null)
				Level.BackgroundImage.AttachTo(this.Background);

			this.Level.AttributeBackground.Assigned +=
				value =>
				{
					if (Level.BackgroundImage != null)
					{
						Level.BackgroundImage.Orphanize();
						Level.BackgroundImage = null;
					}

					if (!string.IsNullOrEmpty(value))
					{
						Level.BackgroundImage = new Image
						{
							Stretch = Stretch.Fill,
							Source = (Assets.Shared.KnownAssets.Path.Backgrounds + "/" + value + ".png").ToSource(),
							Width = width,
							Height = height
						};

						Level.BackgroundImage.AttachTo(this.Background);
					}
				};

			this.Content = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight,
			}.AttachTo(this.Container);

			this.Platforms = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.PlatformsInfoOverlay = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.StartPositionsContainer = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);

			this.StartPositionsContainer = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight
			}.AttachTo(this.Content);



			this.Entities = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight,
				//Background = Brushes.Red,
				//Opacity = 0.5

			}.AttachTo(this.Content);

			this.BubbleContainer = new Canvas
			{
				Width = this.ContentActualWidth,
				Height = this.ContentActualHeight,
				//Background = Brushes.Red,
				//Opacity = 0.5
			}.AttachTo(this.Content);



			this.ContentExtendedContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.Container);


			this.SnowContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.ContentExtendedContainer);

			this.WaterContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.ContentExtendedContainer);

			this.FilmScratchContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.ContentExtendedContainer);


			this.FlashlightContainer = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.ContentExtendedContainer);

			this.ColorOverlay = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight,
				Background = Brushes.Tan,
				Opacity = 0.3,
				Visibility = System.Windows.Visibility.Hidden
			}.AttachTo(this.ContentExtendedContainer);

			this.ContentInfoOverlay = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight
			}.AttachTo(this.ContentExtendedContainer);


			this.TouchOverlay = new Canvas
			{
				Width = this.ContentExtendedWidth,
				Height = this.ContentExtendedHeight,
				Background = Brushes.Tan,
				Opacity = 0
			}.AttachTo(this.ContentExtendedContainer);


			// this level is now frozen to this view!
			// unless we unbind from events being created

			#region sync the level platforms into view

			this.Level.KnownSigns.AttachTo(this.Entities);
			this.Level.KnownSigns.WithEvents(
				value =>
				{
					value.WaitPositionPreferenceChanged += this.Level.ToPlatformSnapshotsWithReset;

					return delegate
					{
						value.WaitPositionPreferenceChanged -= this.Level.ToPlatformSnapshotsWithReset;
					};
				}
			);

			this.Level.KnownRocks.AttachTo(this.Entities);
			this.Level.KnownRocks.AttachTo(k => k.StartPosition, this.StartPositionsContainer);
			//this.Level.KnownRocks.WithEvents(this.LogicForInfoLabel);


			this.Level.KnownTrees.AttachTo(this.Entities);

			this.Level.KnownBirds.AttachTo(this.Entities);
			this.Level.KnownBirds.AttachTo(k => k.StartPosition, this.StartPositionsContainer);

			this.Level.KnownActors.AttachTo(this.Entities);
			this.Level.KnownActors.AttachTo(k => k.StartPosition, this.StartPositionsContainer);
			this.Level.KnownActors.AttachTo(k => k.KnownBubbles, this.BubbleContainer);
			//this.Level.KnownActors.WithEvents(this.LogicForInfoLabel);

			this.Level.KnownVehicles.AttachTo(this.Entities);
			this.Level.KnownVehicles.AttachTo(k => k.StartPosition, this.StartPositionsContainer);
			this.Level.KnownVehicles.WithEvents(
				value =>
				{
					value.CurrentLevel = this.Level;

					return delegate
					{
						value.CurrentLevel = null;
					};
				}
			);


		

			this.Level.KnownDinos.AttachTo(this.Entities);




			this.Level.KnownTryoperus.AttachTo(this.Entities);
			this.Level.KnownTryoperus.AttachTo(k => k.StartPosition, this.StartPositionsContainer);




			this.Level.KnownStones.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownStones.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);

			this.Level.KnownPlatforms.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownPlatforms.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);

			this.Level.KnownCaves.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownCaves.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);


			this.Level.KnownRidges.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownRidges.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);


			this.Level.KnownRidgeTrees.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownRidgeTrees.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);


			this.Level.KnownFences.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownFences.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);


			this.Level.KnownBridges.ForEachNewOrExistingItem(
				k => k.Image.AttachTo(this.Platforms)
			);
			this.Level.KnownBridges.ForEachItemDeleted(
				k => k.Image.Orphanize()
			);





			this.Level.KnownGold.AttachTo(this.Entities);

			this.Level.KnownFlags.AttachTo(this.Entities);
			this.Level.KnownFruits.AttachTo(this.Entities);

			//this.Level.KnownFlags.AttachTo(this.StartPositionsContainer);





			#endregion


			//this.Level.KnownPassengers.WithEvents(
			//    NewPassanger =>
			//    {
			//        var Info = new TextBox
			//        {
			//            Width = 100,
			//            Height = 24,
			//            Background = Brushes.Transparent,
			//            Foreground = Brushes.Yellow,
			//            BorderThickness = new Thickness(0),
			//            TextAlignment = TextAlignment.Center,
			//        };

			//        NewPassanger.Memory_LogicStateChanged +=
			//            delegate
			//            {
			//                Info.Text = "" + NewPassanger.Memory_LogicState;
			//            };

			//        Info.AttachTo(this.Entities);

			//        NewPassanger.LocationChanged +=
			//            delegate
			//            {
			//                Info.MoveTo(NewPassanger.X - 50, NewPassanger.ToObstacle().Bottom);
			//            };

			//        return delegate
			//        {
			//            Info.Orphanize();
			//        };
			//    }
			//);

			this.CurrentWater = new Water(
				new Water.Info
				{
					DefaultWidth = this.ContentExtendedWidth,
					DefaultHeight = this.ContentExtendedHeight,

					WaterTop = Convert.ToInt32(this.Level.WaterTop + ContentOffsetY),
					Zoom = this.Level.Zoom,

					// maybe the map should be able to set this color?
					WaterColorBottom = Colors.Green
				}
			).AttachContainerTo(this.WaterContainer);

			var CurrentSnow = new Snow(
				this

			).AttachContainerTo(this.SnowContainer);

			if (!Level.AttributeSnow.BooleanValue)
			{
				CurrentSnow.Timer.Stop();
				CurrentSnow.Hide();
			}

			this.Level.AttributeSnow.Assigned +=
				delegate
				{
					CurrentSnow.Timer.IsEnabled = this.Level.AttributeSnow.BooleanValue;
					CurrentSnow.Show(this.Level.AttributeSnow.BooleanValue);
				};

			{
				var WaterTop = Convert.ToInt32(this.Level.WaterTop.Max(0) + ContentOffsetY);

				CurrentSnow.Container.ClipTo(0, 0, this.ContentExtendedWidth, WaterTop);

				CurrentWater.MoveContainerTo(0, WaterTop);
			}

			// we are now listening to water attribute in the context of Level
			// if the Level changes we need to adjust our binding
			this.Level.AttributeWater.Assigned +=
				delegate
				{
					var WaterTop = Convert.ToInt32(this.Level.WaterTop.Max(0) + ContentOffsetY);

					CurrentSnow.Container.ClipTo(0, 0, this.ContentExtendedWidth, WaterTop);

					CurrentWater.MoveContainerTo(0, WaterTop);
				};

			this.LocationTracker = new LocationTrackerType();

			MovetToContainerCenter();

			this.Flashlight = new FlashlightType(
				this.Level.Zoom,
				ContentExtendedWidth,
				ContentExtendedHeight
			).AttachContainerTo(this.FlashlightContainer);

			this.AutoscrollEnabled = this.Level.AttributeAutoscroll;

			this.Flashlight.Container.Opacity = this.Level.AttributeFlashlightOpacity.Value.Max(0).Min(255) / 255.0;
			this.Flashlight.Visible = false;
			this.Flashlight.VisibleChanged +=
				delegate
				{
					// - no performance boost at all at this time

					//if (!this.Flashlight.Visible)
					//{
					//    this.Content.ClipTo(
					//        MaxShakeSize - (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2,
					//        MaxShakeSize - (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2,
					//        ContentExtendedWidth,
					//        ContentExtendedHeight
					//    );
					//}
				};

			this.LocationTracker.LocationChanged += new Action(Autoscroll);
			this.LocationTracker.LocationChanged +=
				delegate
				{

					if (this.Flashlight.Visible)
					{
						var x = this.LocationTracker.X + (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2;
						var y = this.LocationTracker.Y + (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2;

						// - no performance boost at all at this time
						//this.Content.ClipTo(
						//    Convert.ToInt32(this.LocationTracker.X - this.Flashlight.Size * this.Level.Zoom / 2),
						//    Convert.ToInt32(this.LocationTracker.Y - this.Flashlight.Size * this.Level.Zoom / 2),
						//    this.Flashlight.Size * this.Level.Zoom,
						//    this.Flashlight.Size * this.Level.Zoom
						//);

						this.Flashlight.MoveTo(
							x,
							y
						);
					}
				};


			AttachFilmScratchEffect();

			this.Level.AttributeWaterRise.Assigned +=
				delegate
				{
					this.IsShakerEnabled = this.Level.AttributeWaterRise.BooleanValue;
				};

			this.IsShakerEnabled = this.Level.AttributeWaterRise.BooleanValue;

			AttachEditorSelector();

			this.TouchInput = new TouchInputType(this.TouchOverlay)
			{
				OffsetX = ContentOffsetX,
				OffsetY = ContentOffsetY,
			};

			this.Level.ContentInfoColoredShapes_PlatformSnapshots.AttachToFrameworkElement(this.StartPositionsContainer);


			//this.Level.ContentInfoColoredShapes_PlatformSnapshots.Add(
			//    new Obstacle { Left = 100, Top = 100, Width = 100, Height = 100 }, Brushes.Cyan
			//);
		}

		public void MovetToContainerCenter()
		{
			this.MoveContentTo(
						 (this.ContainerWidth - ContentActualWidth) / 2,
						 (this.ContainerHeight - ContentActualHeight) / 2 + 1
					 );
		}


		public double ContentOffsetX
		{
			get
			{
				return MaxShakeSize + (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2;
			}
		}

		public double ContentOffsetY
		{
			get
			{
				return MaxShakeSize + (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2;
			}
		}

		public double ContentX { get; set; }
		public double ContentY { get; set; }

		public double ContentShakeX { get; set; }
		public double ContentShakeY { get; set; }

		public void MoveContentTo()
		{
			InternalMoveContentTo(ContentX + ContentShakeX, ContentY + ContentShakeY);
		}

		public void MoveContentTo(double x, double y)
		{
			ContentX = x;
			ContentY = y;

			MoveContentTo();
		}

		public event Action<int, int> ContentExtendedContainerMoved;

		private void InternalMoveContentTo(double x_, double y_)
		{
			var x = Convert.ToInt32(x_);
			var y = Convert.ToInt32(y_);


			var ex = x - MaxShakeSize - (this.ContainerWidth - this.ContentActualWidth).Max(0) / 2;
			var ey = y - MaxShakeSize - (this.ContainerHeight - this.ContentActualHeight).Max(0) / 2;

			this.ContentExtendedContainer.MoveTo(ex, ey);

			if (ContentExtendedContainerMoved != null)
				ContentExtendedContainerMoved(ex, ey);

			this.Content.MoveTo(
				x,
				y
			);
		}

		#region IDisposable Members

		public void Dispose()
		{
			// we need to kill the timers

			this.CurrentWater.Dispose();
			this.CurrentWater = null;

			this.AttachEditorSelector_RectangleAnimator.Stop();
			this.AttachEditorSelector_RectangleAnimator = null;

			AnimationTimer_FilmScratchEffect.Stop();

			if (this.Level != null)
				this.Level.Dispose();
		}

		#endregion

		Action LogicForInfoLabel(ISupportsPhysics value)
		{
			if (this.StartPositionsContainer.Visibility == Visibility.Hidden)
				return delegate
				{
					// do nothing
				};

			var i = new TextBox
			{
				AcceptsReturn = true,
				IsReadOnly = true,
				Text = "info",
				Background = Brushes.Transparent,
				BorderThickness = new Thickness(0),
				Foreground = Brushes.Yellow,
				Width = 200,
				Height = 200
			};

			//i.AttachTo(this.StartPositionsContainer);
			i.AttachTo(this.Entities);

			Action UpdateLocation =
				delegate
				{
					i.Text =
						"x: " + value.X + "\n" +
						"y: " + value.Y;


					i.MoveTo(
						value.X,
						value.Y
					);
				};


			value.LocationChanged += UpdateLocation;

			return delegate
			{
				value.LocationChanged -= UpdateLocation;
				i.Orphanize();
			};
		}

		public event Action Memory_ScoreChanged;
		int InternalMemory_Score;
		public int Memory_Score
		{
			get
			{
				return InternalMemory_Score;
			}
			set
			{
				InternalMemory_Score = value;
				if (Memory_ScoreChanged != null)
					Memory_ScoreChanged();
			}
		}

		public event Action Memory_ScoreMultiplierChanged;
		int InternalMemory_ScoreMultiplier = 1;
		public int Memory_ScoreMultiplier
		{
			get
			{
				return InternalMemory_ScoreMultiplier;
			}
			set
			{
				InternalMemory_ScoreMultiplier = value;
				if (Memory_ScoreMultiplierChanged != null)
					Memory_ScoreMultiplierChanged();
			}
		}
	}
}
