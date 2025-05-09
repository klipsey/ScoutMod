using BepInEx.Configuration;
using OfficialScoutMod.Modules;

namespace OfficialScoutMod.Scout.Content
{
    public static class ScoutConfig
    {
        public static ConfigEntry<bool> forceUnlock;

        public static ConfigEntry<bool> gainAtomicGaugeDuringAtomicBlast;
        public static ConfigEntry<float> adjustShotgunRecoil;
        public static ConfigEntry<float> adjustRifleRecoil;

        public static ConfigEntry<float> maxHealth;
        public static ConfigEntry<float> healthRegen;
        public static ConfigEntry<float> armor;
        public static ConfigEntry<float> shield;

        public static ConfigEntry<int> jumpCount;

        public static ConfigEntry<float> damage;
        public static ConfigEntry<float> attackSpeed;
        public static ConfigEntry<float> crit;

        public static ConfigEntry<float> moveSpeed;
        public static ConfigEntry<float> acceleration;
        public static ConfigEntry<float> jumpPower;

        public static ConfigEntry<bool> autoCalculateLevelStats;

        public static ConfigEntry<float> healthGrowth;
        public static ConfigEntry<float> regenGrowth;
        public static ConfigEntry<float> armorGrowth;
        public static ConfigEntry<float> shieldGrowth;

        public static ConfigEntry<float> damageGrowth;
        public static ConfigEntry<float> attackSpeedGrowth;
        public static ConfigEntry<float> critGrowth;

        public static ConfigEntry<float> moveSpeedGrowth;
        public static ConfigEntry<float> jumpPowerGrowth;

        public static ConfigEntry<float> shotgunDamageCoefficient;

        public static ConfigEntry<float> baseballDamageCoefficient;

        public static ConfigEntry<float> cleaverDamageCoefficient;

        public static ConfigEntry<float> swingDamageCoefficient;

        public static ConfigEntry<float> atomicBlastDamageCoefficient;

        public static ConfigEntry<float> rifleDamageCoefficient;

        public static void Init()
        {
            string section = "Stats - 01";
            string section2 = "QOL - 02";

            damage = Config.BindAndOptions(section, "Change Base Damage Value", 12f);

            maxHealth = Config.BindAndOptions(section, "Change Max Health Value", 110f);
            healthRegen = Config.BindAndOptions(section, "Change Health Regen Value", 1f);
            armor = Config.BindAndOptions(section, "Change Armor Value", 0f);
            shield = Config.BindAndOptions(section, "Change Shield Value", 0f);

            jumpCount = Config.BindAndOptions(section, "Change Jump Count", 2);

            attackSpeed = Config.BindAndOptions(section, "Change Attack Speed Value", 1f);
            crit = Config.BindAndOptions(section, "Change Crit Value", 1f);

            moveSpeed = Config.BindAndOptions(section, "Change Move Speed Value", 7f);
            acceleration = Config.BindAndOptions(section, "Change Acceleration Value", 80f);
            jumpPower = Config.BindAndOptions(section, "Change Jump Power Value", 15f);

            autoCalculateLevelStats = Config.BindAndOptions(section, "Auto Calculate Level Stats", true);

            healthGrowth = Config.BindAndOptions(section, "Change Health Growth Value", 0.3f);
            regenGrowth = Config.BindAndOptions(section, "Change Regen Growth Value", 0.2f);
            armorGrowth = Config.BindAndOptions(section, "Change Armor Growth Value", 0f);
            shieldGrowth = Config.BindAndOptions(section, "Change Shield Growth Value", 0f);

            damageGrowth = Config.BindAndOptions(section, "Change Damage Growth Value", 0.2f);
            attackSpeedGrowth = Config.BindAndOptions(section, "Change Attack Speed Growth Value", 0f);
            critGrowth = Config.BindAndOptions(section, "Change Crit Growth Value", 0f);

            moveSpeedGrowth = Config.BindAndOptions(section, "Change Move Speed Growth Value", 0f);
            jumpPowerGrowth = Config.BindAndOptions(section, "Change Jump Power Growth Value", 0f);

            shotgunDamageCoefficient = Config.BindAndOptions(section, "Change Splattergun Damage Coefficient", 0.65f);
            rifleDamageCoefficient = Config.BindAndOptions(section, "Change Dastardly Dwarf Damage Coefficient", 2.4f);
            baseballDamageCoefficient = Config.BindAndOptions(section, "Change Spike Ball Damage Coefficient", 3f);
            cleaverDamageCoefficient = Config.BindAndOptions(section, "Change Toxic Cleaver Damage Coefficient", 4f);
            swingDamageCoefficient = Config.BindAndOptions(section, "Change Elephants Foot Damage Coefficient", 3.2f);
            atomicBlastDamageCoefficient = Config.BindAndOptions(section, "Change Atomic Blast Damage Coefficient", 10f);

            forceUnlock = Config.BindAndOptions(
                section2,
                "Unlock Scout",
                false,
                "Unlock Scout.", true);

            gainAtomicGaugeDuringAtomicBlast = Config.BindAndOptions(
                section2,
                "Gain Gauge During Atomic",
                false,
                "Lets you fill Atomic Core while it drains.", false);

            adjustShotgunRecoil = Config.BindAndOptions(
                section2,
                "Adjust Shotgun Recoil",
                40f,
                "Adjust the screen shake of the shotgun.", false);

            adjustRifleRecoil = Config.BindAndOptions(
                section2,
                "Adjust Rifle Recoil",
                4f,
                "Adjust the screen shake of the Rifle.", false);
        }
    }
}
