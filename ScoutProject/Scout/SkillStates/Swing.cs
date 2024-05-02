using UnityEngine;
using EntityStates;
using ScoutMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using ScoutMod.Scout.Content;
using static R2API.DamageAPI;

namespace ScoutMod.Scout.SkillStates
{
    public class Swing : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            hitboxGroupName = "Bat";

            damageType = DamageType.Generic;
            damageCoefficient = ScoutStaticValues.swingDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1.5f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.3f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.5f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "sfx_driver_swing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingMuzzle1" : "SwingMuzzle2";
            playbackRateParam = "Swing.playbackRate";
            swingEffectPrefab = this.isAtomic ? ScoutAssets.atomicSwingEffect : ScoutAssets.batSwingEffect;
            if (this.isAtomic) moddedDamageTypeHolder.Add(this.scoutController.ModdedDamageType);
            moddedDamageTypeHolder.Add(DamageTypes.FillAtomic);
            hitEffectPrefab = ScoutAssets.batHitEffect;

            impactSound = ScoutAssets.batImpactSoundDef.index;

            base.OnEnter();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void FireAttack()
        {
            if (base.isAuthority)
            {
                Vector3 direction = this.GetAimRay().direction;
                direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
                this.FindModelChild("MeleePivot").rotation = Util.QuaternionSafeLookRotation(direction);
            }

            base.FireAttack();
        }

        protected override void PlaySwingEffect()
        {
            Util.PlaySound(this.swingSoundString, this.gameObject);
            if (this.swingEffectPrefab)
            {
                Transform muzzleTransform = this.FindModelChild(this.muzzleString);
                if (muzzleTransform)
                {
                    this.swingEffectPrefab = Object.Instantiate<GameObject>(this.swingEffectPrefab, muzzleTransform);
                }
            }
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "BatSwing" + (1 + swingIndex), playbackRateParam, duration * 1.2f, 0.05f);
        }
    }
}