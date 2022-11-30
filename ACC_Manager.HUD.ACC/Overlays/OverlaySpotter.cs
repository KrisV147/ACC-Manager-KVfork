﻿using ACCManager.HUD.Overlay.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCManager.HUD.ACC.Overlays
{
#if DEBUG
    [Overlay(Name = "Spotter", Version = 1.00, OverlayType = OverlayType.Release,
   Description = "TODO (spots things?)")]
#endif
    internal class OverlaySpotter : AbstractOverlay
    {
        public OverlaySpotter(Rectangle rectangle) : base(rectangle, "Spotter")
        {
        }

        public override bool ShouldRender() => DefaultShouldRender();

        public override void BeforeStart()
        {
        }

        public override void BeforeStop()
        {
        }

        public override void Render(Graphics g)
        {
        }
    }
}
