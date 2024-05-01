using RoR2;
using UnityEngine;
using ScoutMod.Modules;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using R2API;
using RoR2.UI;
using ScoutMod.Scout.Components;

namespace ScoutMod.Scout.Content
{
    public static class ScoutCrosshair
    {
        internal static GameObject scoutCrosshair;

        private static AssetBundle _assetBundle;
        public static void Init(AssetBundle assetBundle)
        {
            _assetBundle = assetBundle;
            CreateCrosshair();
        }

        private static void CreateCrosshair()
        {
            scoutCrosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageCrosshair.prefab").WaitForCompletion().InstantiateClone("ScoutCrosshair", false);
        }
    }
}