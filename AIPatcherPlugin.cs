using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using Newtonsoft.Json;
using EFT;
using System.IO;
using System.Reflection;

namespace NoBushESP
{

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "1.0.0")]
    class AIPatcherPlugin : BaseUnityPlugin
    {
        
        private void Awake()
        {
            new AIESPPatcherAimPlayer().Enable();
        }

        public static theConfig config;
        public static object sptUsec;
        public static object sptBear;
        public void Start()
        {
            Type wildspawnType = Type.GetType("WildSpawnType");

            if (wildspawnType != null)
            {
                // Use the Enum.Parse method to get the enum value for "sptUsec"
                sptUsec = Enum.Parse(wildspawnType, "sptUsec");

                // Use the Enum.Parse method to get the enum value for "sptBear"
                sptBear = Enum.Parse(wildspawnType, "sptBear");
            }
            
            string fileName = "dvize.BushNoESPConfig.json";
            var json = File.ReadAllText(fileName);
            config = JsonConvert.DeserializeObject<theConfig>(json);

        }
        
        public class theConfig
        {
            public bool BossesStillSee { get; set; }
            public bool BossFollowersStillSee { get; set; }
            public bool PMCsStillSee { get; set; }
            public bool ScavsStillSee { get; set; }
        }
    }
}
