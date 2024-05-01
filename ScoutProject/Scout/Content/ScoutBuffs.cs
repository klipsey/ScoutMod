using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ScoutMod.Scout.Content
{
    public static class ScoutBuffs
    {
        public static BuffDef atomicBuff;

        public static void Init(AssetBundle assetBundle)
        {
            atomicBuff = Modules.Content.CreateAndAddBuff("ButcheredBuff", Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/VoidSurvivor/texBuffVoidSurvivorCorruptionIcon.tif").WaitForCompletion(),
                Color.yellow, false, false, false);
        }
    }
}
