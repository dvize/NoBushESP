using BepInEx;

namespace NoBushESP
{

    [BepInPlugin("com.dvize.BushNoESP", "dvize.BushNoESP", "1.8.0")]
    //[BepInDependency("com.spt-aki.core", "3.7.4")]
    class NoBushESPPlugin : BaseUnityPlugin
    {
        private void Awake()
        {

        }

        public void Start() => new BushPatch().Enable();

    }


}
