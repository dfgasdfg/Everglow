﻿using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.IIIDModule
{
    internal class IIIDModule : IModule
    {
        string IModule.Name => "IIID";
        void IModule.Load()
        {
           // On.Terraria.Main.DrawCapture += Main_DrawCapture;
        }        
        void IModule.Unload()
        {
           // On.Terraria.Main.DrawCapture -= Main_DrawCapture;
        }


    }
}