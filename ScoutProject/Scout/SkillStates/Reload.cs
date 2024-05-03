using RoR2;
using UnityEngine;
using ScoutMod.Modules.BaseStates;
using EntityStates;

namespace ScoutMod.Scout.SkillStates
{
    public class Reload : BaseScoutSkillState
    {
        public static float baseDuration = 2f;
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
            this.startReload = 0.08f / attackSpeedStat;
            this.startShell = 0.1f / attackSpeedStat;
            this.shellsIn = 1f / attackSpeedStat;
            dontPlay = this.skillLocator.secondary.skillNameToken == ScoutSurvivor.SCOUT_PREFIX + "SECONDARY_SPIKEDBALL_NAME";
            if (dontPlay && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            base.PlayAnimation("Gesture, Override", "ReloadShotgun", "Shoot.playbackRate", this.duration);
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
                startReloadPlayed = true;
                Util.PlayAttackSpeedSound("sfx_scout_start_reload", base.gameObject, 1);
            }

            if (base.fixedAge >= startShell && !startReloadShell)
            {
                startReloadShell = true;
                Util.PlayAttackSpeedSound("sfx_scout_shells_out", base.gameObject, 1);
            }


            if (base.fixedAge >= shellsIn && !endReloadShell)
            {
                endReloadShell = true;
                Util.PlayAttackSpeedSound("sfx_scout_shells_in", base.gameObject, 1);
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                Util.PlayAttackSpeedSound("sfx_scout_end_reload", base.gameObject, 1);
                GiveStock();
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