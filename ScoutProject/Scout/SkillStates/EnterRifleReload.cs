using EntityStates;
using OfficialScoutMod.Modules.BaseStates;
using RoR2;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class EnterRifleReload : BaseScoutSkillState
    {
        public static float baseDuration = 0.1f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= baseDuration)
            {
                Util.PlaySound("sfx_scout_ooa", base.gameObject);
                if (this.scoutController)
                {
                    this.scoutController.DropCasing(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                }
                if (base.isAuthority) outer.SetNextState(new RifleReload());
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
