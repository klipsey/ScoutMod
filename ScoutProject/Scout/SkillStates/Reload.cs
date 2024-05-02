using RoR2;
using UnityEngine;
using ScoutMod.Modules.BaseStates;

namespace ScoutMod.Scout.SkillStates
{
    public class Reload : BaseScoutSkillState
    {
        public static float baseDuration = 1.75f;
        public float duration;
        public float startReload = 0.08f;
        public bool startReloadPlayed = false;
        public float startShell = 0.1f;
        public bool startReloadShell = false;
        public float shellsIn = 1f;
        public bool endReloadShell = false;
        public bool dontPlay = false;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration * skillLocator.primary.cooldownScale - skillLocator.primary.flatCooldownReduction;
            dontPlay = this.skillLocator.secondary.skillNameToken == ScoutSurvivor.SCOUT_PREFIX + "SECONDARY_SPIKEDBALL_NAME";
            if (dontPlay && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            base.PlayAnimation("Gesture, Override", "ReloadShotgun", "Shoot.playbackRate", this.duration);
            Util.PlaySound("sfx_scout_start_reload", base.gameObject);
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
                Util.PlaySound("sfx_scout_start_reload", base.gameObject);
            }

            if (base.fixedAge >= startShell && !startReloadShell)
            {
                startReloadShell = true;
                Util.PlaySound("sfx_scout_shells_out", base.gameObject);
            }


            if (base.fixedAge >= shellsIn && !endReloadShell)
            {
                endReloadShell = true;
                Util.PlaySound("sfx_scout_shells_in", base.gameObject);
            }

            if (base.fixedAge >= this.duration)
            {
                Util.PlaySound("sfx_scout_end_reload", base.gameObject);
                this.outer.SetNextStateToMain();
            }
        }
    }
}