﻿using RoR2;
using OfficialScoutMod.Modules.Achievements;
using OfficialScoutMod.Scout;

namespace OfficialScoutMod.Scout.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 0)]
    public class ScoutMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ScoutSurvivor.SCOUT_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ScoutSurvivor.SCOUT_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ScoutSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}