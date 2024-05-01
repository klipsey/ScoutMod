using RoR2;
using UnityEngine;
using ScoutMod.Modules.BaseStates;

namespace ScoutMod.Scout.SkillStates
{
    public class Reload : BaseScoutSkillState
    {
        public float duration = 1.75f;
        public float startReload = 0.08f;
        public bool startReloadPlayed = false;
        public float startShell = 0.1f;
        public bool startReloadShell = false;
        public float shellsIn = 1f;
        public bool endReloadShell = false;

        public override void OnEnter()
        {
            base.OnEnter();

            base.PlayAnimation("Gesture, Override", "ReloadShotgun", "Shoot.playbackRate", this.duration);
            Util.PlaySound("sfx_scout_start_reload", base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

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