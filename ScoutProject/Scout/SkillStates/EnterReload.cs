using EntityStates;
using ScoutMod.Modules.BaseStates;
using RoR2;
using System.Collections.Generic;
using System.Text;

namespace ScoutMod.Scout.SkillStates
{
    public class EnterReload : BaseScoutSkillState
    {
        public static float baseDuration = 0.1f;

        private float duration => baseDuration / attackSpeedStat;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge > duration)
            {
                outer.SetNextState(new Reload());
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
