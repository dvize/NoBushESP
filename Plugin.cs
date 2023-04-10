using BepInEx;
using BepInEx.Configuration;

namespace NoBushESP
{

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "1.4.4")]
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

        public void Start() => new BushPatch().Enable();

    }
}
