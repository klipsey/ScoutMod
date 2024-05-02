using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScoutMod.Scout.Content
{
    public static class ScoutBuffs
    {
        public static BuffDef scoutAtomicBuff;
        public static BuffDef scoutStunMarker;

        public static void Init(AssetBundle assetBundle)
        {
            scoutAtomicBuff = Modules.Content.CreateAndAddBuff("ScoutAtomicBuff", Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/VoidSurvivor/texBuffVoidSurvivorCorruptionIcon.tif").WaitForCompletion(),
                Color.yellow, false, false, false);
            scoutStunMarker = Modules.Content.CreateAndAddBuff("ScoutStunBuff", Addressables.LoadAssetAsync<Sprite>("RoR2/Base/UI/texSniperCharge.tif").WaitForCompletion(),
                Color.white, false, false, false);
        }
    }
}
