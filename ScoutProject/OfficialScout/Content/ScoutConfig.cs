using BepInEx.Configuration;
using OfficialScoutMod.Modules;

namespace OfficialScoutMod.Scout.Content
{
    public static class ScoutConfig
    {
        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> gainAtomicGaugeDuringAtomicBlast;
        public static ConfigEntry<float> adjustShotgunRecoil;

        public static void Init()
        {
            string section = "General - 01";
            string section2 = "Balance - 02";
            string section3 = "Visuals - 03";
            //add more here or else you're cringe
            forceUnlock = Config.BindAndOptions(
                section,
                "Unlock Scout",
                false,
                "Unlock Scout.", true);

            gainAtomicGaugeDuringAtomicBlast = Config.BindAndOptions(
                section2,
                "Gain Gauge During Atomic",
                false,
                "Lets you fill Atomic Core while it drains.", false);

            adjustShotgunRecoil = Config.BindAndOptions(
                section3,
                "Adjust Shotgun Recoil",
                40f,
                "Adjust the screen shake of the shotgun.", false);
        }
    }
}
