using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using EFT;
using Comfort.Common;
using System.Diagnostics.Tracing;

namespace NoBushESP
{

    [BepInPlugin("com.fenix.NoAI_ESP", "Fenix.NoAI_ESP", "1.0.0")]
    class AIPatcherPlugin : BaseUnityPlugin
    {

        private void Awake()
        {

            new AIESPPatcherAimPlayer().Enable();
            
        }
        
    }
}
