using System;
using OfficialScoutMod.Modules;
using OfficialScoutMod.Scout;
using OfficialScoutMod.Scout.Achievements;

namespace OfficialScoutMod.Scout.Content
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
            desc = desc + "< ! > Bat can gain the most Atomic Core charge if there are large groups of enemies." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Extra stocks for Atomic Blast increase its duration while cooldown reduction increases Atomic Core chargeup speed." + Environment.NewLine + Environment.NewLine;

            string lore = "An escape from the power plant. An accident that would spread like wildfire throughout the news, its cause unknown. A lone worker stumbles slowly away, his bat being his only support. He wandered aimlessly, pain and shock ringing throughout his body. Approaching what seemed to be an abandoned railroad, in the distance he could only attempt to make out what, logically, he considered was another person. Having a strange aura to them, he kept his guard up and tried to identify any potential threat from the figure.\r\n\r\n“It’s a nice time for a walk, mind if I join ya near these ol’ tracks?”\r\n\r\n“W-Who are you…? What d-do you want from me?”\r\n\r\n“My identity does not concern you.”\r\n“... however I can tell you had a pretty rough night. Nearing Death’s doorstep, hm?”\r\n“I may have something that can help you out, an… offer of some sorts.”\r\n\r\n“A… d-deal..?”\r\n\r\n“Precisely.”\r\n\r\nHis throat closed, heart began to race, and mind beginning to pretend that none of this was happening. Hesitating, he finally exhaled the breath he had been holding for what felt like an eternity as the person spoke. Was it fear he was feeling? Confusion? Or was it the idea that he could have the world at his fingertips that sent him into an almost euphoric temptation? Regardless, he still listened intently. Focusing and decoding every single word. Maybe it could get him out of this hell.\r\n\r\n“I can sense the radiation emitting from you… hm…”\r\n“... I got just the thing. How about I turn that into something that will power ya up, hm? Make you better and stronger than before, in exchange for… let's see… some favors. Sounds good, doesn’t it? I think it does.”\r\n“I have some old pals of mine that can help out with the procedure too.”\r\n\r\n“... B-but what about my job?”\r\n“Where will I go? I would rather not be unemployed…”\r\n\r\n“Funny that you mention that, I heard of a job that goes somewhere into the depths of space. I’d imagine they’d be able to utilize your new speed and agility for it once it’s all done.”\r\n“... also a word of advice, build up some confidence. It may improve your chances.”\r\n\r\n“Hm…”\r\n“... Alright, call it a deal… whoever you are.”\r\n\r\n“Wonderful.”\r\n\r\nThe sound of a distant train can be heard. As it slowed down and with the sound of its old whistle going off, the two were gone.";
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

            Language.Add(prefix + "PRIMARY_RIFLE_NAME", "Dastardly Dwarf");
            Language.Add(prefix + "PRIMARY_RIFLE_DESCRIPTION", $"Fire a high velocity round for <style=cIsDamage>{100f * ScoutStaticValues.rifleDamageCoefficient}% damage</style>. " +
                $"<style=cIsDamage>Headshots deal 2x damage</style>");
            #endregion

            #region PrimarySwap
            Language.Add(prefix + "PRIMARY_BONK_NAME", "Elephants Foot");
            Language.Add(prefix + "PRIMARY_BONK_DESCRIPTION", $"{Tokens.agilePrefix}. Swing your bat for <style=cIsDamage>{100f * ScoutStaticValues.baseballDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_CLEAVER_NAME", "Toxic Cleaver");
            Language.Add(prefix + "SECONDARY_CLEAVER_DESCRIPTION", $"{Tokens.agilePrefix}. Throw your cleaver <style=cIsDamage>Blighting</style> and dealing <style=cIsDamage>{100f * ScoutStaticValues.cleaverDamageCoefficient}% damage</style>. " +
                $"<style=cIsDamage>Critically Strikes</style> and <style=cIsHealing>Poisons</style> <style=cIsDamage>Stunned</style> enemies.");
            #endregion

            #region SecondarySwap
            Language.Add(prefix + "SECONDARY_SPIKEDBALL_NAME", "Spike Ball");
            Language.Add(prefix + "SECONDARY_SPIKEDBALL_DESCRIPTION", $"{Tokens.agilePrefix}. Hit your baseball <style=cIsDamage>Stunning</style> and dealing <style=cIsDamage>{100f * ScoutStaticValues.baseballDamageCoefficient}% damage </style>. " +
                "<style=cIsDamage>Stun</style> duration scales with distance traveled.");
            #endregion

            #region Utility 
            Language.Add(prefix + "UTILITY_ATOMICBLAST_NAME", "Atomic Blast");
            Language.Add(prefix + "UTILITY_ATOMICBLAST_DESCRIPTION", $"Drain your <style=cHumanObjective>Atomic Core</style> gaining " +
                $"<style=cIsDamage>Atomic Crits</style>, <style=cIsDamage>attack speed</style>, and <style=cIsUtility>movement speed</style>. " +
                $"Deal <style=cIsDamage>{100f}% - {100f * ScoutStaticValues.atomicBlastDamageCoefficient}% damage</style> around you based on charge.");

            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_SWAP_NAME", "Swap");
            Language.Add(prefix + "SPECIAL_SWAP_DESCRIPTION", $" Swap to your bat.");

            Language.Add(prefix + "SPECIAL_SCEPTER_SWAP_NAME", "Swap");
            Language.Add(prefix + "SPECIAL_SCEPTER_SWAP_DESCRIPTION", $" Swap to your bat." + Tokens.ScepterDescription("<style=cHumanObjective>Atomic Charge</style> can still be gained while <style=cHumanObjective>Atomic Core</style> drains ."));
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(ScoutMasteryAchievement.identifier), "Scout: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(ScoutMasteryAchievement.identifier), "As Scout, beat the game or obliterate on Monsoon.");

            //Language.Add(Tokens.GetAchievementNameToken(ScoutUnlockAchievement.identifier), "Batter Up");
            //Language.Add(Tokens.GetAchievementDescriptionToken(ScoutUnlockAchievement.identifier), "Beat the stage 1 teleporter within 3 minutes without picking up a single item.");

            #endregion

            #endregion
        }
    }
}