using System;
using ScoutMod.Modules;
using ScoutMod.Scout;
using ScoutMod.Scout.Achievements;

namespace ScoutMod.Scout.Content
{
    public static class ScoutTokens
    {
        public static void Init()
        {
            AddScoutTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Scout.txt");
            //todo guide
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddScoutTokens()
        {
            #region Scout
            string prefix = ScoutSurvivor.SCOUT_PREFIX;

            string desc = "The Scout is an extremely mobile, burst damage survivor that can focus down singular enemies with ease.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use your Splatterguns knockback to reach high locations or to avoid enemy attacks." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Comboing your Baseball and Cleaver can deal massive damage from far away." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Scouts primary reloads quicker with cooldown reduction effectively increasing your damage per second." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bat benefits the most from attack speed making it extremely effective when Atomic Blast activates." + Environment.NewLine + Environment.NewLine;

            /*
             * its not really shown yet in the anims but the panel on the top of the robe/heart area opens up to her artificial heart. 
             * the lore is something along the lines of her being a human who was plagued with illness during her life forcing her to have an artificial heart. 
             * while traveling on a civilian ship, the ship was intercepted by imps attempting to escape petrichor V causing it 
             * to be sent into the red plane where everyone in the ship either died immediately to the dimension or killed off by imps. 
             * Due to her heart being artificial, she didnt immediately die but instead got corrupted by the plane instead. I was thinking that 
             * maybe she IS killed in the plane by another survivor with her heart corrupting her but idk about that yet. For the scissors all 
             * i know is that they are sentient but i dont really have anything for why they would be 
            */
            string lore = "Me and a bucket of chicken";
            string outro = ".. grass grows, birds fly, sun shines and brother, I hurt people .";
            string outroFailure = "..what the hell was that crap?";
            
            Language.Add(prefix + "NAME", "Scout");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Force of Nature");
            Language.Add(prefix + "LORE", lore);
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Atomic Core");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", $"Scout can <style=cIsUtility>jump twice</style>. Deal <style=cIsDamage>damage</style> to build up <style=cHumanObjective>Atomic Core</style>. " +
                $"Taking damage reduces <style=cHumanObjective>Atomic Core</style>. <style=cHumanObjective>Atomic Core</style> increases <style=cIsUtility>movement speed</style>.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SPLATTERGUN_NAME", "Splattergun");
            Language.Add(prefix + "PRIMARY_SPLATTERGUN_DESCRIPTION", $"{Tokens.agilePrefix}. Fire a scattergun burst for <style=cIsDamage>12x{100f * ScoutStaticValues.shotgunDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "PRIMARY_BONK_NAME", "Bonk");
            Language.Add(prefix + "PRIMARY_BONK_DESCRIPTION", $"{Tokens.agilePrefix}. Swing your bat for <style=cIsDamage>{100f * ScoutStaticValues.baseballDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_CLEAVER_NAME", "Toxic Cleaver");
            Language.Add(prefix + "SECONDARY_CLEAVER_DESCRIPTION", $"{Tokens.agilePrefix}. Throw your cleaver <style=cIsDamage>blighting</style> and dealing <style=cIsDamage>{100f * ScoutStaticValues.cleaverDamageCoefficient}% damage</style>. " +
                $"<style=cIsDamage>Critically Strikes</style> and <style=cIsHealing>poisons</style> <style=cIsDamage>stunned</style> enemies.");

            Language.Add(prefix + "SECONDARY_SPIKEDBALL_NAME", "Spike Ball");
            Language.Add(prefix + "SECONDARY_SPIKEDBALL_DESCRIPTION", $"{Tokens.agilePrefix}. Hit your baseball <style=cIsDamage>stunning</style> and dealing <style=cIsDamage>{100f * ScoutStaticValues.baseballDamageCoefficient}% damage </style>. " +
                "<style=cIsDamage>Stun</style> duration scales with distance traveled.");
            #endregion

            #region Utility 
            Language.Add(prefix + "UTILITY_ATOMICBLAST_NAME", "Atomic Blast");
            Language.Add(prefix + "UTILITY_ATOMICBLAST_DESCRIPTION", $"{Tokens.agilePrefix}. Drain your <style=cHumanObjective>Atomic Core</style> gaining " +
                $"<style=cIsDamage>mini crits</style>, <style=cIsDamage>attack speed</style>, and <style=cIsUtility>movement speed</style>. " +
                $"If drained at max charge, deal <style=cIsDamage>{100f * ScoutStaticValues.atomicBlastDamageCoefficient}% damage</style> around you.");

            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_SWAP_NAME", "Swap");
            Language.Add(prefix + "SPECIAL_SWAP_DESCRIPTION", $"{Tokens.agilePrefix}. Swap to your bat.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(ScoutMasteryAchievement.identifier), "Scout: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(ScoutMasteryAchievement.identifier), "As Scout, beat the game or obliterate on Monsoon.");

            Language.Add(Tokens.GetAchievementNameToken(ScoutUnlockAchievement.identifier), "Batter Up");
            Language.Add(Tokens.GetAchievementDescriptionToken(ScoutUnlockAchievement.identifier), "Beat the stage 1 teleporter within 2 minutes without picking up a single item.");

            #endregion

            #endregion
        }
    }
}