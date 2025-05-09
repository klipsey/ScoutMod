using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.AddressableAssets;
using RoR2.Projectile;
using OfficialScoutMod.Scout.Content;
using OfficialScoutMod.Scout.Components;
using R2API;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class ThrowCleaver : GenericProjectileBaseState
    {
        public static float baseDuration = 0.2f;
        public static float baseDelayDuration = 0.1f * baseDuration;
        public GameObject cleaverPrefab = ScoutAssets.cleaverPrefab;
        public ScoutController scoutController;
        public override void OnEnter()
        {
            scoutController = base.gameObject.GetComponent<ScoutController>();
            base.attackSoundString = "sfx_scout_cleaver_throw";

            base.baseDuration = baseDuration;
            base.baseDelayBeforeFiringProjectile = baseDelayDuration;

            base.damageCoefficient = damageCoefficient;
            base.force = 120f;

            base.projectilePitchBonus = -3.5f;

            base.OnEnter();
            this.scoutController.SetupStockSecondary1();
        }

        public override void FireProjectile()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                aimRay = this.ModifyProjectileAimRay(aimRay);
                aimRay.direction = Util.ApplySpread(aimRay.direction, 0f, 0f, 1f, 1f, 0f, this.projectilePitchBonus);
                ProjectileDamage moddedDamage = cleaverPrefab.GetComponent<ProjectileDamage>();
                if (scoutController.ModdedDamageType == DamageTypes.AtomicCrits) moddedDamage.damageType.AddModdedDamageType(DamageTypes.AtomicCrits);
                ProjectileManager.instance.FireProjectile(cleaverPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), this.gameObject, 
                    this.damageStat * ScoutConfig.cleaverDamageCoefficient.Value, this.force, this.RollCrit(), scoutController.atomicDraining ? DamageColorIndex.Item : DamageColorIndex.Default, null, -1f);
                if (moddedDamage.damageType.HasModdedDamageType(DamageTypes.AtomicCrits)) moddedDamage.damageType.RemoveModdedDamageType(DamageTypes.AtomicCrits);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        public override void PlayAnimation(float duration)
        {
            if (base.GetModelAnimator())
            {
                base.PlayAnimation("Gesture, Override", "ThrowCleaver", "Cleaver.playbackRate", this.duration * 4.5f);
            }
        }
    }
}