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

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "1.4.0")]
    class NoBushESPPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> BlockingTypeGoalEnemy;
        public static ConfigEntry<float> TestRayRadius;

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
                10.0f,
                "Don't set this not too small");
           
            
        }

        public void Start()
        {
            
            new BushPatch().Enable();

        }
        
    }
}
