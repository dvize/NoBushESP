using System;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using Newtonsoft.Json;
using EFT;
using System.IO;
using System.Reflection;

namespace NoBushESP
{

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "1.3.0")]
    class NoBushESPPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> BlockingTypeGoalEnemy;
        public static ConfigEntry<float> TestRayRadius;
        public static ConfigEntry<bool> BossesStillSee;
        public static ConfigEntry<bool> BossFollowersStillSee;
        public static ConfigEntry<bool> ScavsStillSee;
        public static ConfigEntry<bool> PMCsStillSee;

        private void Awake()
        {
            BlockingTypeGoalEnemy = Config.Bind(
                "Main Settings",
                "Enabled means GoalEnemy Method, Disabled means IsVisible Method",
                true,
                "Set True or False to preferred method");
            
            TestRayRadius = Config.Bind(
                "Main Settings",
                "Width of the Ray that checks if obstruction",
                1.0f,
                "Don't set this not too small");
            
            BossesStillSee = Config.Bind(
                "Role Types",
                "Bosses Still See Through Bushes",
                false,
                "Bosses Still See Through Bushes");

            BossFollowersStillSee = Config.Bind(
                "Role Types",
                "Boss Followers Still See Through Bushes",
                false,
                "Boss Followers Still See Through Bushes");

            ScavsStillSee = Config.Bind(
               "Role Types",
               "Scavs Still See Through Bushes",
               false,
               "Scavs Still See Through Bushes");

            PMCsStillSee = Config.Bind(
                "Role Types",
                "PMCs Still See Through Bushes",
                false,
                "PMCs Still See Through Bushes");
            
        }

        public static EFT.WildSpawnType sptBear;
        public static EFT.WildSpawnType sptUsec;
        public void Start()
        {

            sptBear = (WildSpawnType)Enum.Parse(typeof(EFT.WildSpawnType), "sptBear");
            sptUsec = (WildSpawnType)Enum.Parse(typeof(EFT.WildSpawnType), "sptUsec");
            
            new BushPatch().Enable();

        }
        
    }
}
