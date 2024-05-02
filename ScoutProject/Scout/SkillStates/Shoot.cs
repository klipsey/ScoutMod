using RoR2;
using UnityEngine;
using EntityStates;
using RobDriver.Modules.Components;
using R2API;
using ScoutMod.Modules.BaseStates;
using ScoutMod.Scout.Content;

namespace ScoutMod.Scout.SkillStates
{
    public class Shoot : BaseScoutSkillState
    {
        public static float damageCoefficient = ScoutStaticValues.shotgunDamageCoefficient;
        public static float procCoefficient = 0.7f;
        public float baseDuration = 1.2f; // the base skill duration. i.e. attack speed
        public static int bulletCount = 12;
        public static float bulletSpread = 8f;
        public static float bulletRecoil = 40f;
        public static float bulletRange = 150f;
        public static float bulletThiccness = 1f;
        public float selfForce = 3000f;

        private float earlyExitTime;
        protected float duration;
        protected float fireDuration;
        protected bool hasFired;
        private bool isCrit;
        protected string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.SetAimTimer(5f);
            this.muzzleString = "GunMuzzle";
            this.hasFired = false;
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.isCrit = base.RollCrit();
            this.earlyExitTime = 0.75f * this.duration;

            if (this.isCrit) Util.PlaySound("sfx_scout_shoot_crit", base.gameObject);
            else Util.PlaySound("sfx_scout_shoot", base.gameObject);

            base.PlayAnimation("Gesture, Override", "FireShotgun", "Shoot.playbackRate", this.duration);

            this.fireDuration = 0;
        }

        public virtual void FireBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (this.scoutController)
                {
                    this.scoutController.DropShell(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                }

                float recoilAmplitude = Shoot.bulletRecoil / this.attackSpeedStat;

                base.AddRecoil2(-0.4f * recoilAmplitude, -0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);
                this.characterBody.AddSpreadBloom(4f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, false);

                GameObject tracer = ScoutAssets.shotgunTracer;
                if (this.isCrit) tracer = ScoutAssets.shotgunTracerCrit;

                if (base.isAuthority)
                {
                    float damage = Shoot.damageCoefficient * this.damageStat;

                    Ray aimRay = GetAimRay();

                    float spread = Shoot.bulletSpread;
                    float thiccness = Shoot.bulletThiccness;
                    float force = 50;

                    BulletAttack bulletAttack = new BulletAttack
                    {
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damage,
                        damageColorIndex = scoutController.atomicDraining ? DamageColorIndex.Item : DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = bulletRange,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        isCrit = this.isCrit,
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = thiccness,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracer,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                        HitEffectNormal = false,
                    };
                    bulletAttack.AddModdedDamageType(scoutController.ModdedDamageType);
                    bulletAttack.AddModdedDamageType(DamageTypes.FillAtomicShotgun);
                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = 0;
                    bulletAttack.bulletCount = 1;
                    bulletAttack.Fire();

                    uint secondShot = (uint)Mathf.CeilToInt(bulletCount / 2f) - 1;
                    bulletAttack.minSpread = 0;
                    bulletAttack.maxSpread = spread / 1.45f;
                    bulletAttack.bulletCount = secondShot;
                    bulletAttack.Fire();

                    bulletAttack.minSpread = spread / 1.45f;
                    bulletAttack.maxSpread = spread;
                    bulletAttack.bulletCount = (uint)Mathf.FloorToInt(bulletCount / 2f);
                    bulletAttack.Fire();

                    if(!this.characterMotor.isGrounded) this.characterMotor.ApplyForce((aimRay.direction * -this.selfForce) * 0.5f);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                this.FireBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextState(new Reload());
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.earlyExitTime) return InterruptPriority.Any;
            return InterruptPriority.Skill;
        }
    }
}