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
            if (ExternalInterface.available)
                Abstractatech.ActionScript.ConsoleFormPackage.ConsoleFormPackageExperience.Initialize();

            this.InvokeWhenStageIsReady(
                () =>
                {


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
                }
            );
        }

    }
}
