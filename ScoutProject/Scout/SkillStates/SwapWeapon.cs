using UnityEngine;
using EntityStates;
using OfficialScoutMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using OfficialScoutMod.Scout.Content;
using UnityEngine.Networking;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class SwapWeapon : BaseScoutSkillState
    {
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            Util.PlaySound("sfx_scout_swap_weapon", this.gameObject);
            //return to gun
            if (this.skillLocator.secondary.skillNameToken == ScoutSurvivor.SCOUT_PREFIX + "SECONDARY_SPIKEDBALL_NAME")
            {
                PlayAnimation("Gesture, Override", "SwapToGun", "Cleaver.playbackRate", 0.5f / base.characterBody.attackSpeed);
                this.scoutController.SwitchLayer("");
                this.skillLocator.primary.UnsetSkillOverride(this.gameObject, this.scoutSwapPassive.batSkillDef, GenericSkill.SkillOverridePriority.Network);
                this.skillLocator.secondary.UnsetSkillOverride(this.gameObject, this.scoutSwapPassive.ballSkillDef, GenericSkill.SkillOverridePriority.Network);
                if (base.isAuthority)
                {
                    this.skillLocator.secondary.RemoveAllStocks();
                    for (int i = 0; i < this.scoutController.currentSecondary1Stock; i++) this.skillLocator.secondary.AddOneStock();
                }
                if (this.skillLocator.secondary.stock < this.skillLocator.secondary.maxStock)
                {
                    this.skillLocator.secondary.rechargeStopwatch = this.scoutController.secondary1CdTimer;
                }
            }
            else
            {
                //swap to bat
                PlayAnimation("Gesture, Override", "SwapToBat", "Cleaver.playbackRate", 0.5f / base.characterBody.attackSpeed);
                this.scoutController.SwitchLayer("Body, Bat");
                this.skillLocator.primary.SetSkillOverride(this.gameObject, this.scoutSwapPassive.batSkillDef, GenericSkill.SkillOverridePriority.Network);
                this.skillLocator.secondary.SetSkillOverride(this.gameObject, this.scoutSwapPassive.ballSkillDef, GenericSkill.SkillOverridePriority.Network);
                if(base.isAuthority)
                {
                    this.skillLocator.secondary.RemoveAllStocks();
                    for (int i = 0; i < this.scoutController.currentSecondary2Stock; i++) this.skillLocator.secondary.AddOneStock();
                }
                if (this.skillLocator.secondary.stock < this.skillLocator.secondary.maxStock)
                {
                    this.skillLocator.secondary.rechargeStopwatch = this.scoutController.secondary2CdTimer;
                }
            }

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}