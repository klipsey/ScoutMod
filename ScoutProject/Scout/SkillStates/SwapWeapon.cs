using UnityEngine;
using EntityStates;
using ScoutMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using ScoutMod.Scout.Content;
using UnityEngine.Networking;

namespace ScoutMod.Scout.SkillStates
{
    public class SwapWeapon : BaseScoutSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound("sfx_scout_swap_weapon", this.gameObject);
            if (this.skillLocator.secondary.skillNameToken == ScoutSurvivor.SCOUT_PREFIX + "SECONDARY_SPIKEDBALL_NAME")
            {
                this.scoutController.SwitchLayer("");
                this.skillLocator.primary.UnsetSkillOverride(this.gameObject, ScoutSurvivor.batSkillDef, GenericSkill.SkillOverridePriority.Network);
                this.skillLocator.secondary.UnsetSkillOverride(this.gameObject, ScoutSurvivor.spikeBallSkillDef, GenericSkill.SkillOverridePriority.Network);
                if (base.isAuthority)
                {
                    this.skillLocator.secondary.RemoveAllStocks();
                    for (int i = 0; i < this.scoutController.currentCleaverStock; i++) this.skillLocator.secondary.AddOneStock();
                }
                if (this.skillLocator.secondary.stock < this.skillLocator.secondary.maxStock)
                {
                    this.skillLocator.secondary.rechargeStopwatch = this.scoutController.cleaverCdTimer;
                }
            }
            else
            {
                this.scoutController.SwitchLayer("Body, Bat");
                this.skillLocator.primary.SetSkillOverride(this.gameObject, ScoutSurvivor.batSkillDef, GenericSkill.SkillOverridePriority.Network);
                this.skillLocator.secondary.SetSkillOverride(this.gameObject, ScoutSurvivor.spikeBallSkillDef, GenericSkill.SkillOverridePriority.Network);
                if(base.isAuthority)
                {
                    this.skillLocator.secondary.RemoveAllStocks();
                    for (int i = 0; i < this.scoutController.currentBallStock; i++) this.skillLocator.secondary.AddOneStock();
                }
                if (this.skillLocator.secondary.stock < this.skillLocator.secondary.maxStock)
                {
                    this.skillLocator.secondary.rechargeStopwatch = this.scoutController.ballCdTimer;
                }
            }

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}