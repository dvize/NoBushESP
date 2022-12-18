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
        public static EFT.WildSpawnType sptUsec = (EFT.WildSpawnType)Enum.Parse(typeof(EFT.WildSpawnType), "sptUsec");
        public static EFT.WildSpawnType sptBear = (EFT.WildSpawnType)Enum.Parse(typeof(EFT.WildSpawnType), "sptBear");
        public void Start()
        {
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
