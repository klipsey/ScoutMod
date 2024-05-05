using BepInEx.Configuration;
using ScoutMod.Modules;

namespace ScoutMod.Scout.Content
{
    public static class ScoutConfig
    {
        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> gainAtomicGaugeDuringAtomicBlast;

        public static void Init()
        {
            string section = "Scout";

            //add more here or else you're cringe
            forceUnlock = Config.BindAndOptions(
                section,
                "Unlock Scout",
                false,
                "Unlock Scout.", true);

            gainAtomicGaugeDuringAtomicBlast = Config.BindAndOptions(
                section,
                "Gain Gauge During Atomic",
                false,
                "Lets you fill Atomic Core while it drains.", false);
        }
    }
}
