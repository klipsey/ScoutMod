using RoR2;
using UnityEngine;
using OfficialScoutMod.Modules.BaseStates;
using EntityStates;
using OfficialScoutMod.Scout.Components;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class RifleReload : BaseScoutSkillState
    {
        public static float baseDuration = 1.4f;
        private float duration;
        private bool dontPlay = false;
        private bool hasGivenStock;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / attackSpeedStat;
            if (this.scoutController.stagedReload > 0f) this.duration = this.scoutController.stagedReload;
            else this.scoutController.stagedReload = this.duration;
            dontPlay = this.skillLocator.secondary == scoutController.isSwapped;
            if (dontPlay && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            base.PlayAnimation("Gesture, Override", "ReloadRifle", "Shoot.playbackRate", this.duration);
            Util.PlayAttackSpeedSound("sfx_scout_start_reload_rifle", base.gameObject, 1);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (dontPlay && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                Util.PlayAttackSpeedSound("sfx_scout_finish_rifle_reload", base.gameObject, 1);
                GiveStock();
                this.scoutController.stagedReload = 0f;
                this.outer.SetNextStateToMain();
            }
        }

        private void GiveStock()
        {
            if (!hasGivenStock)
            {
                for (int i = base.skillLocator.primary.stock; i < base.skillLocator.primary.maxStock; i++) base.skillLocator.primary.AddOneStock();
                hasGivenStock = true;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}