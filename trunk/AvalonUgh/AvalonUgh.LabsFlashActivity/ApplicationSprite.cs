using net.hires.debug;
using ScriptCoreLib.ActionScript.Extensions;
using ScriptCoreLib.ActionScript.flash.display;
using ScriptCoreLib.ActionScript.flash.external;
using ScriptCoreLib.Extensions;
using System;

namespace AvalonUgh.LabsFlashActivity
{

    public sealed class ApplicationSprite : Sprite
    {
        public const int DefaultWidth = ApplicationCanvas.DefaultWidth;
        public const int DefaultHeight = ApplicationCanvas.DefaultHeight;

        public readonly ApplicationCanvas content = new ApplicationCanvas();

        public ApplicationSprite()
        {
            // give my my console!
            // not inside chrome webview?
            if (ExternalInterface.available)
                Abstractatech.ActionScript.ConsoleFormPackage.ConsoleFormPackageExperience.Initialize();

            this.InvokeWhenStageIsReady(
                () =>
                {
                    this.stage.align = StageAlign.BOTTOM_LEFT;


                    content.AttachToContainer(this);
                    content.AutoSizeTo(this.stage);


                    Action AtResize =
                        delegate
                        {
                            var s = content.ToSprite();


                            var r = this.stage.stageWidth / (double)DefaultWidth; ;

                            s.scaleX = r;
                            s.scaleY = r;


                        };

                    AtResize();


                    this.stage.resize += delegate { AtResize(); };



                    // http://www.flare3d.com/support/index.php?topic=1101.0
                    this.addChild(new Stats { alpha = 0.5 });
                }
            );
        }

    }
}
