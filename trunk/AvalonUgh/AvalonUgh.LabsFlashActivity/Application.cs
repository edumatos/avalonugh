using AvalonUgh.LabsFlashActivity.Design;
using AvalonUgh.LabsFlashActivity.HTML.Pages;
using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using ScriptCoreLib.JavaScript;
using ScriptCoreLib.JavaScript.Components;
using ScriptCoreLib.JavaScript.DOM;
using ScriptCoreLib.JavaScript.DOM.HTML;
using ScriptCoreLib.JavaScript.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AvalonUgh.LabsFlashActivity
{
    /// <summary>
    /// Your client side code running inside a web browser as JavaScript.
    /// </summary>
    public sealed class Application
    {


        /// <summary>
        /// This is a javascript application.
        /// </summary>
        /// <param name="page">HTML document rendered by the web server which can now be enhanced.</param>
        public Application(IDefault page)
        {
            #region TheServer
            dynamic self = Native.self;
            dynamic self_chrome = self.chrome;
            object self_chrome_socket = self_chrome.socket;

            if (self_chrome_socket != null)
            {
                chrome.Notification.DefaultTitle = "AvalonUgh";
                chrome.Notification.DefaultIconUrl = new HTML.Images.FromAssets.Preview128().src;
                ChromeTCPServer.TheServer.Invoke(
                    DefaultSource.Text
                );


                return;
            }
            #endregion

            //ApplicationWebService service = new ApplicationWebService();

            ApplicationSprite sprite = new ApplicationSprite();
            sprite.AutoSizeSpriteTo(page.ContentSize);
            sprite.AttachSpriteTo(page.Content);

        }

    }
}
