using BepInEx;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using R2API.Networking;
using OfficialScoutMod.Modules;
using OfficialScoutMod.Scout.Content;
using ShaderSwapper;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace OfficialScoutMod
{
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [BepInDependency(NetworkingAPI.PluginGUID)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]

    public class ScoutPlugin : BaseUnityPlugin
    {
        // if you do not change this, you are giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.kenko.Scout";
        public const string MODNAME = "Scout";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "KENKO";

        public static ScoutPlugin instance;
        public static bool emotesInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI");

        void Awake()
        {
            instance = this;

            NetworkingAPI.RegisterMessageType<Scout.Components.SyncAtomic>();

            //easy to use logger
            Log.Init(Logger);

            // used when you want to properly set up language folders
            Modules.Language.Init();

            ScoutAssets.Init(Assets.LoadAssetBundle("scout"));
            StartCoroutine(ScoutAssets.mainAssetBundle.UpgradeStubbedShadersAsync());

            // character initialization
            new OfficialScoutMod.Scout.ScoutSurvivor().Initialize();

            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();

            //On.RoR2.Networking.NetworkManagerSystemSteam.OnClientConnect += (s, u, t) => { };
        }

        private void Start()
        {
            SoundBanks.Init();
        }
    }
}
