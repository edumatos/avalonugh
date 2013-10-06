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
            #region ChromeTCPServer
            dynamic self = Native.self;
            dynamic self_chrome = self.chrome;
            object self_chrome_socket = self_chrome.socket;

            if (self_chrome_socket != null)
            {
                chrome.Notification.DefaultTitle = "AvalonUgh";
                chrome.Notification.DefaultIconUrl = new HTML.Images.FromAssets.Preview128().src;

                #region FormStyler
                FormStyler.AtFormCreated = s =>
                {
                    // X:\jsc.svn\examples\javascript\IsometricTycoonViewWithToolbar\IsometricTycoonViewWithToolbar\Application.cs
                    // X:\jsc.internal.svn\core\com.abstractatech.web\com.abstractatech.web\Domains\discover.xavalon.net\discover_xavalon_net.cs

                    // browser popup will use this color
                    ((__Form)s.Context).HTMLTargetContainerRef.style.backgroundColor = JSColor.FromRGB(0, 108, 0);

                    s.Caption.style.backgroundColor = JSColor.FromRGB(0, 108, 0);
                    s.TargetOuterBorder.style.boxShadow = "rgba(0, 108, 0, 0.3) 0px 0px 6px 3px";
                    s.TargetOuterBorder.style.borderColor = JSColor.FromRGB(0, 108, 0);

                    s.TargetInnerBorder.style.borderWidth = "0px";

                    s.CloseButton.style.color = JSColor.White;
                    s.CloseButton.style.backgroundColor = JSColor.None;
                    s.CloseButton.style.borderWidth = "0px";
                    s.CloseButtonContent.style.borderWidth = "0px";

                    s.TargetResizerPadding.style.left = "0px";
                    s.TargetResizerPadding.style.top = "0px";
                    s.TargetResizerPadding.style.right = "0px";
                    s.TargetResizerPadding.style.bottom = "0px";

                };
                #endregion



                #region __Form
                {
                    var windows = new List<AppWindow>();


                    __Form.InternalHTMLTargetAttachToDocument =
                       async (that, yield) =>
                       {

                           //Error in event handler for app.runtime.onLaunched: Error: Invalid value for argument 2. Property 'transparentBackground': Expected 'boolean' but got 'integer'.
                           var transparentBackground = true;


                           // http://src.chromium.org/viewvc/chrome/trunk/src/chrome/common/extensions/api/app_window.idl
                           var xappwindow = await chrome.app.window.create(
                                 Native.document.location.pathname,
                                 new
                                 {
                                     frame = "none"
                                     //,transparentBackground
                                 }
                            );


                           // Uncaught TypeError: Cannot read property 'contentWindow' of undefined 

                           Console.WriteLine("appwindow loading... " + new { xappwindow });
                           Console.WriteLine("appwindow loading... " + new { xappwindow.contentWindow });

                           // our window frame non client area plus inner body margin
                           xappwindow.resizeTo(
                            ApplicationSprite.DefaultWidth + 32,
                            ApplicationSprite.DefaultWidth + 64
                           );

                           xappwindow.With(
                               appwindow =>
                               {

                                   #region onload
                                   Action<IEvent> onload =

                                        delegate
                                        {
                                            var c = that;
                                            var f = (Form)that;
                                            var ff = c;

                                            windows.Add(appwindow);

                                            // http://sandipchitale.blogspot.com/2013/03/tip-webkit-app-region-css-property.html

                                            (ff.CaptionForeground.style as dynamic).webkitAppRegion = "drag";

                                            //(ff.ResizeGripElement.style as dynamic).webkitAppRegion = "drag";
                                            // cant have it yet
                                            ff.ResizeGripElement.Orphanize();

                                            f.StartPosition = FormStartPosition.Manual;


                                            f.Left = 0;
                                            f.Top = 0;


                                            f.FormClosing +=
                                                delegate
                                                {
                                                    Console.WriteLine("FormClosing");
                                                    appwindow.close();
                                                };

                                            appwindow.onRestored.addListener(
                                                new Action(
                                                    delegate
                                                    {
                                                        that.CaptionShadow.Hide();

                                                    }
                                                )
                                            );

                                            appwindow.onMaximized.addListener(
                                            new Action(
                                                    delegate
                                                    {
                                                        that.CaptionShadow.Show();

                                                    }
                                            )
                                            );

                                            appwindow.onClosed.addListener(
                                                new Action(
                                                    delegate
                                                    {
                                                        Console.WriteLine("onClosed");
                                                        windows.Remove(appwindow);

                                                        f.Close();
                                                    }
                                            )
                                            );

                                            // wont fire yet
                                            //appwindow.contentWindow.onbeforeunload +=
                                            //    delegate
                                            //    {
                                            //        Console.WriteLine("onbeforeunload");
                                            //    };

                                            //appwindow.onBoundsChanged.addListener(
                                            //        new Action(
                                            //        delegate
                                            //        {
                                            //            Console.WriteLine("appwindow.onBoundsChanged");

                                            //            f.SizeTo(
                                            //                appwindow.contentWindow.Width,
                                            //                appwindow.contentWindow.Height
                                            //            );
                                            //        }
                                            //    )
                                            //);


                                            appwindow.contentWindow.onresize +=
                                                //appwindow.onBoundsChanged.addListener(
                                                //    new Action(
                                                    delegate
                                                    {

                                                        Console.WriteLine("appwindow.contentWindow.onresize SizeTo " +
                                                            new
                                                            {
                                                                appwindow.contentWindow.Width,
                                                                appwindow.contentWindow.Height
                                                            }
                                                            );

                                                        f.Width = appwindow.contentWindow.Width;
                                                        f.Height = appwindow.contentWindow.Height;

                                                    }
                                                //)
                                                //)
                                            ;

                                            f.Width = appwindow.contentWindow.Width;
                                            f.Height = appwindow.contentWindow.Height;


                                            //Console.WriteLine("appwindow contentWindow onload");


                                            that.HTMLTarget.AttachTo(
                                                appwindow.contentWindow.document.body
                                            );



                                            yield();
                                            //Console.WriteLine("appwindow contentWindow onload done");
                                        };
                                   #endregion

                                   //Uncaught TypeError: Cannot read property 'contentWindow' of undefined 



                                   appwindow.contentWindow.onload +=
                                       onload;
                               }
                           );





                       };


                }
                #endregion

                #region __WebBrowser
                {
                    // X:\jsc.svn\examples\javascript\chrome\ChromeFormsWebBrowserExperiment\ChromeFormsWebBrowserExperiment\Application.cs
                    __WebBrowser.InitializeInternalElement = that =>
                    {
                        var webview = Native.document.createElement("webview");
                        // You do not have permission to use <webview> tag. Be sure to declare 'webview' permission in your manifest. 
                        webview.setAttribute("partition", "p1");

                        that.InternalElement = (IHTMLIFrame)(object)webview;
                    };

                }
                #endregion


                #region open
                Action<string> open =
                    uri =>
                    {

                        var f = new Form
                        {

                            Text = chrome.Notification.DefaultTitle,
                            ShowIcon = false
                        };



                        //Refused to frame 'http://192.168.43.252:8877/' because it violates the following Content Security Policy directive: "frame-src 'self' data: chrome-extension-resource:"

                        var w = new WebBrowser { }.AttachTo(f);

                        f.SizeChanged +=
                            delegate
                            {
                                Console.WriteLine("SizeChanged");

                                var ClientSize = f.ClientSize;


                                w.Width = ClientSize.Width;
                                w.Height = ClientSize.Height;

                            };
                        w.Navigate(uri);

                        f.Show();
                    };
                #endregion


                ChromeTCPServer.TheServer.Invoke(AppSource.Text, open);

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