using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonUgh.Code.Dialogs;
using AvalonUgh.Code.Editor;
using AvalonUgh.Promotion;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using AvalonUgh.Assets.Shared;
using AvalonUgh.Code.Diagnostics;

namespace AvalonUgh.Code.GameWorkspace
{
	partial class Workspace
	{


		[Script]
		public class LobbyPort : Port
		{
			[Script]
			public class ConstructorArguments
			{
				public int Padding;
				public int Zoom;
				public int Width;
				public int Height;
			}

			public readonly ConstructorArguments Arguments;

			public readonly ModernMenu Menu;
			public GameSocialLinks SocialLinks;
			public GameMenu SocialLinksMenu;

			public LobbyPort(ConstructorArguments args)
			{
				this.Arguments = args;

				this.Padding = args.Padding;

				this.Zoom = DefaultZoom;

				this.Width = args.Width;
				this.Height = args.Height;

				this.Menu = new ModernMenu(args.Zoom, args.Width, args.Height);

				this.Menu.AttachContainerTo(this.Window.OverlayContainer);

				this.Window.Container.WriteTreeToConsoleOnClick();


				this.WhenLoaded(
					delegate
					{
						//this.Menu.BringContainerToFront();
						//this.SocialLinksMenu.BringContainerToFront();
						//this.Window.ColorOverlay.Element.BringToFront();
						//this.SocialLinks.BringContainerToFront();
					}
				);


				new Image
				{
					Stretch = Stretch.Fill,
					Source = (Assets.Shared.KnownAssets.Path.Assets + "/jsc.png").ToSource(),
					Width = 96,
					Height = 96
				}.MoveTo(args.Width - 96, args.Height - 96).AttachTo(this.Window.OverlayContainer);



				this.AttachSocialLinks();

			}

			public Tuple GetRandomEntrypoint<Tuple>(Func<double, double, Tuple> CreateTuple)
			{
				return CreateTuple(
					(this.View.ContentActualWidth / 4) +
					(this.View.ContentActualWidth / 2).Random(),
					(this.View.ContentActualHeight / 2)
				);
			}

			private void AttachSocialLinks()
			{


				// redefine the ctor to fit our context
				Func<string, string, string, GameMenu.Option> Option =
					(Text, Image, href) =>
						new GameMenu.Option
						{
							Text = "Play " + Text + "!",
							Source = (KnownAssets.Path.SocialLinks + "/" + Image + ".png").ToSource(),
							Hyperlink = new Uri(href),
							MarginAfter = Math.PI / 3
						};

				var ShadowSize = 40;



				this.SocialLinksMenu = new GameMenu(this.Arguments.Width, this.Arguments.Height, ShadowSize)
				{
					Option("FreeCell", "Preview_FreeCell",  "http://nonoba.com/zproxy/avalon-freecell"),
					Option("Spider Solitaire", "Preview_Spider",  "http://nonoba.com/zproxy/avalon-spider-solitaire"),
					Option("Treasure Hunt", "Preview_TreasureHunt",  "http://nonoba.com/zproxy/treasure-hunt"),
					Option("FlashMinesweeper:MP", "Preview_Minesweeper", "http://nonoba.com/zproxy/flashminesweepermp"),
					Option("Multiplayer Mahjong", "Preview_Mahjong", "http://nonoba.com/zproxy/mahjong-multiplayer"),
					Option("Multiplayer SpaceInvaders", "Preview_SpaceInvaders", "http://nonoba.com/zproxy/flashspaceinvaders"),
				};

				this.SocialLinksMenu.IdleText = "More games!";

				this.SocialLinksMenu.AttachContainerTo(this.Window.OverlayContainer);

				this.Menu.MoreGames += this.SocialLinksMenu.Show;


				this.SocialLinks = new GameSocialLinks(this.Window.OverlayContainer)
				{
					new GameSocialLinks.Button { 
						Source = (Assets.Shared.KnownAssets.Path.Assets + "/plus_google.png").ToSource(),
						Width = 62,
						Height = 17,
						Hyperlink = new Uri(Info.GoogleGadget.AddLink)
					},
					new GameSocialLinks.Button { 
						Source = (Assets.Shared.KnownAssets.Path.Assets + "/su.png").ToSource(),
						Width = 16,
						Height = 16,
						Hyperlink = new Uri( "http://www.stumbleupon.com/submit?url=" + Info.Nonoba.URL)
					}
				};
			}
		}





	}
}
