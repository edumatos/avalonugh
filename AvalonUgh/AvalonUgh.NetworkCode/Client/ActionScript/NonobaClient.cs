﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.Nonoba.api;
using ScriptCoreLib.ActionScript.flash.display;
using AvalonUgh.NetworkCode.Shared;

namespace AvalonUgh.NetworkCode.Client.ActionScript
{
	[Script]
	public class NonobaClient : Shared.NetworkClient
	{
		public const int NonobaChatWidth = 200;

		public const int DefaultWidth = 600 + NonobaChatWidth;
		public const int DefaultHeight = 600;

		public NonobaClient()
		{
			this.Container.InvokeWhenStageIsReady(
				stage =>
				{
					var c = NonobaAPI.MakeMultiplayer(stage
						//, "192.168.3.102"
						//, "192.168.1.119"
					);

				}
			);
		}

		public static void SendMessage(Connection c, Communication.Messages m, params object[] e)
		{
			var i = new Message(((int)m).ToString());

			foreach (var z in e)
			{
				i.Add(z);
			}

			c.Send(i);
		}


		private void Initialize(Stage stage)
		{
			var c = NonobaAPI.MakeMultiplayer(stage
				//, "192.168.3.102"
				//, "192.168.1.119"
				);

			var MyEvents = new Communication.RemoteEvents();
			var MyMessages = new Communication.RemoteMessages
			{
				Send = e => SendMessage(c, e.i, e.args)
			};



			Events = MyEvents;
			Messages = MyMessages;

			this.InitializeEvents();

			#region Dispatch
			Func<Message, bool> Dispatch =
			   e =>
			   {
				   var type = (Communication.Messages)int.Parse(e.Type);

				   //Converter<uint, byte[]> GetMemoryStream =
				   //    index =>
				   //    {
				   //        var a = e.GetByteArray(index);

				   //        if (a == null)
				   //            throw new Exception("bytearray missing at " + index + " - " + e.ToString());

				   //        return a.ToArray();
				   //    };

				   if (MyEvents.Dispatch(type,
						 new Communication.RemoteEvents.DispatchHelper
						 {
							 GetLength = i => e.length,
							 GetInt32 = e.GetInt,
							 GetDouble = e.GetNumber,
							 GetString = e.GetString,
							 //				 GetMemoryStream = GetMemoryStream
						 }
					 ))
					   return true;

				   return false;
			   };
			#endregion

			//c.MessageDirect +=
			//    e =>
			//    {
			//        throw new Exception("got first message");
			//    };

			c.Init +=
				delegate
				{
					this.Connect();
				};

			c.Disconnect +=
				delegate
				{
					this.Disconnect();
				};

			#region message
			c.Message +=
				e =>
				{
					//InitializeMapOrSkip();


					var Dispatched = false;

					try
					{
						Dispatched = Dispatch(e.message);
					}
					catch (Exception ex)
					{
						System.Console.WriteLine("error at dispatch: " + e.message.Type);

						throw ex;
					}

					if (Dispatched)
						return;

					System.Console.WriteLine("not on dispatch: " + e.message.Type);

				};
			#endregion

		

		}

	}
}
