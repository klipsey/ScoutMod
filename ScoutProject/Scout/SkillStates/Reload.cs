using RoR2;
using UnityEngine;
using OfficialScoutMod.Modules.BaseStates;
using EntityStates;
using OfficialScoutMod.Scout.Components;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class Reload : BaseScoutSkillState
    {
        public static float baseDuration = 1.4f;
        private float duration;
        private float startReload;
        private bool startReloadPlayed = false;
        private float startShell;
        private bool startReloadShell = false;
        private float shellsIn;
        private bool endReloadShell = false;
        private bool dontPlay = false;
        private bool hasGivenStock;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / attackSpeedStat;
            if (this.scoutController.stagedReload > 0f) this.duration = this.scoutController.stagedReload;
            else this.scoutController.stagedReload = this.duration;
            this.startReload = 0.04f * duration;
            this.startShell = 0.05f * duration;
            this.shellsIn = 0.5f * duration;
            dontPlay = scoutController.isSwapped;
            if (dontPlay && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            base.PlayCrossfade("Gesture, Override", "ReloadShotgun", "Shoot.playbackRate", this.duration, 0.05f);
            Util.PlayAttackSpeedSound("sfx_scout_start_reload", base.gameObject, 1);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (dontPlay && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            if(base.fixedAge >= startReload && !startReloadPlayed)
            {
                this.scoutController.stagedReload = duration - startReload;
                startReloadPlayed = true;
                Util.PlayAttackSpeedSound("sfx_scout_start_reload", base.gameObject, 1);
            }

            if (base.fixedAge >= startShell && !startReloadShell)
            {
                this.scoutController.stagedReload = duration - startShell;
                startReloadShell = true;
                Util.PlayAttackSpeedSound("sfx_scout_shells_out", base.gameObject, 1);
            }


            if (base.fixedAge >= shellsIn && !endReloadShell)
            {
                this.scoutController.stagedReload = duration - shellsIn;
                endReloadShell = true;
                Util.PlayAttackSpeedSound("sfx_scout_shells_in", base.gameObject, 1);
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                Util.PlayAttackSpeedSound("sfx_scout_end_reload", base.gameObject, 1);
                GiveStock();
                this.scoutController.stagedReload = 0f;
                this.outer.SetNextStateToMain();
            }
        }

        private void GiveStock()
        {
            if (!hasGivenStock)
            {
                for(int i = base.skillLocator.primary.stock; i < base.skillLocator.primary.maxStock; i++) base.skillLocator.primary.AddOneStock();
                hasGivenStock = true;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}