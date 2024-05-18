using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace ScoutMod.Scout.Components
{
    public class ScoutPassive : MonoBehaviour
    {
        public SkillDef doubleJumpPassive;

        public GenericSkill passiveSkillSlot;

        public SteppedSkillDef batSkillDef;

        public GenericSkill batSkillSlot;

        public SkillDef ballSkillDef;

        public GenericSkill ballSkillSlot;
        public bool isJump
        {
            get
            {
                if (doubleJumpPassive && passiveSkillSlot)
                {
                    return passiveSkillSlot.skillDef == doubleJumpPassive;
                }

                return false;
            }
        }
        public bool isBat
        {
            get
            {
                if (batSkillDef && batSkillSlot)
                {
                    return batSkillSlot.skillDef == batSkillDef;
                }

                return false;
            }
        }
        public bool isBall
        {
            get
            {
                if (ballSkillDef && ballSkillSlot)
                {
                    return ballSkillSlot.skillDef == ballSkillDef;
                }

                return false;
            }
        }
    }
}