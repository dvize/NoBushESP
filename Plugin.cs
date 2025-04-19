using BepInEx;
using BepInEx.Logging;

namespace NoBushESP
{

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "2.0.0")]
    class NoBushESPPlugin : BaseUnityPlugin
    {
        public void Awake() 
        {
            new BushPatch().Enable();
        }
    }
}
