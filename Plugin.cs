using System.Diagnostics;
using System;
using BepInEx;
using BepInEx.Configuration;
using System.Diagnostics;
using VersionChecker;

namespace NoBushESP
{

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "1.5.0")]
    class NoBushESPPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> BlockingTypeGoalEnemy;

        private void Awake()
        {
            CheckEftVersion();
        }

        public void Start() => new BushPatch().Enable();

        private void CheckEftVersion()
        {
            // Make sure the version of EFT being run is the correct version
            int currentVersion = FileVersionInfo.GetVersionInfo(BepInEx.Paths.ExecutablePath).FilePrivatePart;
            int buildVersion = TarkovVersion.BuildVersion;
            if (currentVersion != buildVersion)
            {
                Logger.LogError($"ERROR: This version of {Info.Metadata.Name} v{Info.Metadata.Version} was built for Tarkov {buildVersion}, but you are running {currentVersion}. Please download the correct plugin version.");
                EFT.UI.ConsoleScreen.LogError($"ERROR: This version of {Info.Metadata.Name} v{Info.Metadata.Version} was built for Tarkov {buildVersion}, but you are running {currentVersion}. Please download the correct plugin version.");
                throw new Exception($"Invalid EFT Version ({currentVersion} != {buildVersion})");
            }
        }

       
    }

    
}
