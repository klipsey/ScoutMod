using RoR2;
using UnityEngine;
using OfficialScoutMod.Scout;
using OfficialScoutMod.Scout.Achievements;

namespace OfficialScoutMod.Scout.Content
{
    public static class ScoutUnlockables
    {
        public static UnlockableDef characterUnlockableDef;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            if(!ScoutConfig.forceUnlock.Value)
            {
                characterUnlockableDef = Modules.Content.CreateAndAddUnlockableDef(ScoutUnlockAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ScoutUnlockAchievement.unlockableIdentifier),
                ScoutSurvivor.instance.assetBundle.LoadAsset<Sprite>("texScoutIcon"));
            }
        }
    }
}
