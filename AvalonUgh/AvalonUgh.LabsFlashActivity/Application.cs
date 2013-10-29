using AvalonUgh.LabsFlashActivity.Design;
using AvalonUgh.LabsFlashActivity.HTML.Pages;
using chrome;
using ScriptCoreLib;
using ScriptCoreLib.Delegates;
using ScriptCoreLib.Extensions;
using ScriptCoreLib.JavaScript;
using ScriptCoreLib.JavaScript.BCLImplementation.System.Windows.Forms;
using ScriptCoreLib.JavaScript.Components;
using ScriptCoreLib.JavaScript.DOM;
using ScriptCoreLib.JavaScript.DOM.HTML;
using ScriptCoreLib.JavaScript.Extensions;
using ScriptCoreLib.JavaScript.Runtime;
using ScriptCoreLib.JavaScript.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
        public Application(IApp page)
        {
            FormStyler.AtFormCreated =
               s =>
               {
                   s.Context.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                   //var x = new ChromeTCPServerWithFrameNone.HTML.Pages.AppWindowDrag().AttachTo(s.Context.GetHTMLTarget());
                   var x = new ChromeTCPServerWithFrameNone.HTML.Pages.AppWindowDragWithShadow().AttachTo(s.Context.GetHTMLTarget());
               };


            #region ChromeTCPServer
            dynamic self = Native.self;
            dynamic self_chrome = self.chrome;
            object self_chrome_socket = self_chrome.socket;

            if (self_chrome_socket != null)
            {
                chrome.Notification.DefaultTitle = "AvalonUgh";
                chrome.Notification.DefaultIconUrl = new HTML.Images.FromAssets.Preview().src;



                ChromeTCPServer.TheServerWithStyledForm.Invoke(AppSource.Text,
                    DefaultWidth: ApplicationSprite.DefaultWidth,
                    DefaultHeight: ApplicationSprite.DefaultHeight,
                    AtFormCreated: FormStyler.AtFormCreated

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
