using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Nonoba.Generic;

namespace AvalonUgh.NetworkCode.Shared
{
	[Script]
	public partial class VirtualGame : ServerGameBase<Communication.IEvents, Communication.IMessages, VirtualPlayer>
	{

		public void WriteLine(string Text)
		{
			Console.WriteLine("Server > Avalon Ugh: " + Text);
		}


		public override void UserJoined(VirtualPlayer player)
		{
			WriteLine("UserJoined " + player.Username);


			//var a100 = new AvailibleAchievement(player.AwardAchievement, "a100");
			//var a1000 = new AvailibleAchievement(player.AwardAchievement, "a1000");
			//var a50000 = new AvailibleAchievement(player.AwardAchievement, "a50000");
			//var aLC = new AvailibleAchievement(player.AwardAchievement, "alc");

			//var score = 0;

			//player.FromPlayer.AddScore += e =>
			//{
			//    score += e.score;

			//    player.AddScore("score", e.score);

			//    if (score > 100)
			//        a100.Give();

			//    if (score > 1000)
			//        a1000.Give();

			//    if (score > 50000)
			//        a50000.Give();
			//};

			//player.FromPlayer.AwardAchievementLayoutCompleted +=
			//    e =>
			//    {
			//        aLC.GiveMultiple();
			//    };

			////var firstblood = new AvailibleAchievement(player.AwardAchievement, "firstblood");
			////var portalfound = new AvailibleAchievement(player.AwardAchievement, "portalfound");
			////var getrich = new AvailibleAchievement(player.AwardAchievement, "getrich");
			////var massacre = new AvailibleAchievement(player.AwardAchievement, "massacre");
			////var levelup = new AvailibleAchievement(player.AwardAchievement, "levelup");


			////var x = AnyOtherUser(player);

			////player.FromPlayer.LockGame += e => this.GameState = MyGame.GameStateEnum.ClosedGameInProgress;
			////player.FromPlayer.UnlockGame += e => this.GameState = MyGame.GameStateEnum.OpenGameInProgress;

			////var total_score = 0;
			////var total_kills = 0;
			////var total_level = 0;

			////// registered nonoba rankings
			////player.FromPlayer.ReportScore +=
			////    e =>
			////    {
			////        if (e.level > 0)
			////            exitfound.Give();

			////        if (e.kills > 0)
			////            firstblood.Give();

			////        if (e.teleports > 0)
			////            portalfound.Give();

			////        total_score += e.score;
			////        total_kills += e.kills;
			////        total_level += e.level;

			////        if (total_score > 2000)
			////            getrich.Give();

			////        if (total_kills > 50)
			////            massacre.Give();

			////        if (total_level > 15)
			////            levelup.Give();

			////        player.AddScore("score", e.score);
			////        player.AddScore("kills", e.kills);
			////        player.AddScore("level", e.level);
			////        player.AddScore("teleports", e.teleports);
			////        player.SetScore("fps", e.fps);
			////    };



			////player.FromPlayer.AwardAchievementFirst += e => player.AwardAchievement("first");
			////player.FromPlayer.AwardAchievementFiver += e => player.AwardAchievement("fiver");
			////player.FromPlayer.AwardAchievementUFOKill += e => player.AwardAchievement("ufokill");
			////player.FromPlayer.AwardAchievementMaxGun += e => player.AwardAchievement("maxgun");

			////var user_with_map = -1;

			////if (x != null)
			////{
			////    user_with_map = x.UserId;
			////}

			//var navbar = 1;
			//var layoutinput = 1;
			//var vote = 1;
			//var hints = 0;

			//if (this.Settings.GetBoolean(SettingsInfo.navbar, false))
			//    navbar = 0;

			//if (this.Settings.GetBoolean(SettingsInfo.layoutinput, false))
			//    layoutinput = 0;

			//if (this.Settings.GetBoolean(SettingsInfo.vote, false))
			//    vote = 0;

			//if (this.Settings.GetBoolean(SettingsInfo.hints, true))
			//    hints = 1;

			TestStorage(player);

			player.ToPlayer.Server_Message("tag: " + player.SavedLevels["tag"].Value);
			player.SavedLevels["tag"].Value = "ok";
			player.ToPlayer.Server_Message("tag: " + player.SavedLevels["tag"].Value);

			TestStorage(player);
			TestStorage(player);

			if (player.SavedLevelsCount == 0)
			{
				player.SavedLevelsCount = 5;

				player.ToPlayer.Server_Message("First 5 slots have been created for you!");
			}


			// let new player know how it is named, also send magic bytes to verify
			player.ToPlayer.Server_Hello(
				player.UserId,
				player.Username,
				this.Users.Count - 1,
				player.SavedLevelsCount
				//navbar,
				//vote,
				//layoutinput,
				//hints,
				//new Handshake().Bytes
			);


			for (int i = 0; i < player.SavedLevelsCount; i++)
			{
				player.ToPlayer.Server_LoadLevel(i, player.SavedLevels[i].Value);
			}

			// let other players know that there is a new player in the map
			player.ToOthers.Server_UserJoined(
			   player.UserId,
			   player.Username
			);

			player.FromPlayer.Server_LoadLevel +=
				e =>
				{
					if (e.index >= player.SavedLevelsCount)
						player.SavedLevelsCount = (e.index + 1);

					Console.WriteLine("save slot:" + e.index);

					var Slot = player.SavedLevels[e.index];

					Slot.Value = e.data;

					player.ToPlayer.Server_Message(
						"before: " + e.data.Length + " after: " + Slot.Value.Length
					);
				};

			//var PreventStatic = 0;

			//player.FromPlayer.ServerPlayerHello +=
			//    e =>
			//    {
			//        var StaticPrevented = PreventStatic;

			//        new Handshake().Verify(e.handshake);
			//    };

			//player.FromPlayer.UserMapResponse +=
			//    e =>
			//    {
			//        var StaticPrevented = PreventStatic;

			//        Console.WriteLine("map: " + e.bytes.Length);

			//    };

		}

		private void TestStorage(VirtualPlayer player)
		{
			var t = player.Data["test"];

			var c = 0;

			for (int i = 1; i <= 128; i++)
			{
				var k = t[i];

				var len = i * 8;

				if (k.Value.Length == len)
					c = i;

				k.Value = new string('_', len);
			}

			player.ToPlayer.Server_Message("TestStorage: " + c + " - " + (c * 8) + " bytes");
 
		}



		public override void UserLeft(VirtualPlayer player)
		{
			WriteLine("UserLeft " + player.Username);

			player.ToOthers.Server_UserLeft(player.UserId, player.Username);
		}

		public override void GameStarted()
		{
			WriteLine("GameStarted");
		}

	}
}
