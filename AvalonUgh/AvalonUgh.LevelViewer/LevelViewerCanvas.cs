using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonUgh.Code.Editor;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonUgh.LevelViewer.Shared
{
	[Script]
	public class LevelViewerCanvas : Canvas
	{
		public const int DefaultWidth = 640;
		public const int DefaultHeight = 400;

		public LevelViewerCanvas()
		{
			Width = DefaultWidth;
			Height = DefaultHeight;

			this.ClipToBounds = true;

			Colors.Blue.ToGradient(Colors.Black, DefaultHeight / 4).Select(
				(c, i) =>
					new Rectangle
					{
						Fill = new SolidColorBrush(c),
						Width = DefaultWidth,
						Height = 4,
					}.MoveTo(0, i * 4).AttachTo(this)
			).ToArray();

			new TextBox
			{
				Text = "Episode editor concept"
			}.AttachTo(this);

			var levels = new KnownLevels();

			var a_Relation = new AvalonUgh.Code.Window
			{
				Width = 16,
				Height = 16,
				BackgroundColor = Colors.YellowGreen
			}.AttachContainerTo(this);

			a_Relation.Update();

			var a = new MiniLevelWindow(
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
				}
			)
			{
				LevelReference = levels.DefaultLobbyLevel,
				DragContainer = this
			}.AttachContainerTo(this).MoveContainerTo(200, 8);

			var b = new MiniLevelWindow(
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
				}
			)
			{
				LevelReference = levels.DefaultMissionLevel,
				DragContainer = this
			}.AttachContainerTo(this).MoveContainerTo(200, 200);

			var cc = new MiniLevelWindow(
				new MiniLevelWindow.ConstructorArgumentsInfo
				{
				}
			)
			{
				LevelReference = levels.Levels.Random(),
				DragContainer = this
			}.AttachContainerTo(this).MoveContainerTo(200, 200);


			Action UpdateRelations =
				delegate
				{
					a_Relation.Show();
					a_Relation.MoveContainerTo(
						Convert.ToInt32(Canvas.GetLeft(a.Container) + (a.Width - a_Relation.Width) / 2),
						Convert.ToInt32(Canvas.GetTop(a.Container) + a.Height)
					);

					var h = (Canvas.GetTop(b.Container) - Canvas.GetTop(a.Container) - a.Height).Max(12);

					a_Relation.Height = Convert.ToInt32(h);
					a_Relation.Update();
				};

			a.DragBehavior.DragStart +=
				delegate
				{
					a_Relation.Hide();
				};
			a.DragBehavior.DragStop +=
				delegate
				{
					UpdateRelations();
				};

			b.DragBehavior.DragStart +=
				delegate
				{
					a_Relation.Hide();
				};

			b.DragBehavior.DragStop +=
				delegate
				{
					UpdateRelations();
				};

			UpdateRelations();
		}
	}
}
