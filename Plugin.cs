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
        
        private void Awake()
        {
            BlockingTypeGoalEnemy = Config.Bind(
                "Main Settings",
                "Enabled means GoalEnemy Method, Disabled means IsVisible Method",
                false,
                "Set True or False to preferred method");
        }

        public void Start()
        {
            new BushPatch().Enable();
        }
        
    }
}
