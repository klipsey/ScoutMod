using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using RoR2.HudOverlay;
using UnityEngine.AddressableAssets;
using static RoR2.CameraTargetParams;
using OfficialScoutMod.Scout.Content;
using OfficialScoutMod.Modules.BaseStates;
using OfficialScoutMod.Scout.SkillStates;
using R2API;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class ShootRifle : BaseScoutSkillState
    {
        public float damageCoefficient = ScoutConfig.rifleDamageCoefficient.Value;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.8f;
        public static float force = 200f;
        public static float recoil = ScoutConfig.adjustRifleRecoil.Value;
        public static float range = 9000f;

        protected float duration;
        protected string muzzleString;
        protected bool isCrit;
        protected int diamondbackStacks;

        protected virtual GameObject tracerPrefab => this.isCrit ? ScoutAssets.scoutTracerCrit : ScoutAssets.scoutTracer;
        public string shootSoundString = "sfx_scout_rifle_shoot";
        public virtual BulletAttack.FalloffModel falloff => BulletAttack.FalloffModel.None;

        private CameraParamsOverrideHandle camParamsOverrideHandle;
        private OverlayController overlayController;
        private float fireTimer;
        public bool hasFired;
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();

            this.duration = ShootRifle.baseDuration / this.attackSpeedStat;

            base.characterBody.SetAimTimer(4f);
            this.muzzleString = "GunMuzzle";

            this.isCrit = RollCrit();

            this.shootSoundString = "sfx_scout_rifle_shoot";

            this.overlayController = HudOverlayManager.AddOverlay(this.gameObject, new OverlayCreationParams
            {
                prefab = ScoutAssets.headshotOverlay,
                childLocatorEntry = "ScopeContainer"
            });
        }

        public override void FixedUpdate()
        {
            RefreshState();
            base.FixedUpdate();

            if(this.scoutController.jamTimer <= 0f)
            {
                this.fireTimer += Time.fixedDeltaTime;
                if (!hasFired)
                {
                    hasFired = true;
                    if (base.isAuthority)
                    {
                        this.Fire();
                    }
                }
                if (!this.inputBank.skill1.down && this.fireTimer >= this.duration)
                {
                    if (this.skillLocator.primary.stock <= 0)
                    {
                        Util.PlaySound("sfx_scout_ooa", base.gameObject);
                        if (this.scoutController)
                        {
                            this.scoutController.DropCasing(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                        }
                        if (base.isAuthority) this.outer.SetNextState(new RifleReload());
                    }
                    else if (base.isAuthority) this.outer.SetNextStateToMain();
                }
                else if (this.inputBank.skill1.down && this.fireTimer >= this.duration)
                {
                    if (this.skillLocator.primary.stock <= 0)
                    {
                        Util.PlaySound("sfx_scout_ooa", base.gameObject);
                        if (this.scoutController)
                        {
                            this.scoutController.DropCasing(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
                        }
                        if (base.isAuthority) this.outer.SetNextState(new RifleReload());
                    }
                    else
                    {
                        base.characterBody.SetAimTimer(4f);
                        this.skillLocator.primary.stock--;
                        this.Fire();
                    }
                }
            }
        }
        public void Fire()
        {
            this.characterBody.isSprinting = false;

            this.PlayAnimation("Gesture, Override", "FireRifle", "Shoot.playbackRate", this.duration * 1.5f);

            if (this.scoutController)
            {
                this.scoutController.DropShell(-this.GetModelBaseTransform().transform.right * -Random.Range(4, 12));
            }
            this.fireTimer = 0f;
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, this.gameObject, this.muzzleString, false);

            Util.PlaySound(this.shootSoundString, this.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                base.AddRecoil2(-1f * ShootRifle.recoil, -2f * ShootRifle.recoil, -0.5f * ShootRifle.recoil, 0.5f * ShootRifle.recoil);

                BulletAttack bulletAttack = new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = this.damageCoefficient * this.damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    falloffModel = this.falloff,
                    maxDistance = ShootRifle.range,
                    force = ShootRifle.force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = this.characterBody.spreadBloomAngle * 2f,
                    isCrit = this.isCrit,
                    owner = base.gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = this.tracerPrefab,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                };

                bulletAttack.damageType.damageSource = DamageSource.Primary;

                bulletAttack.AddModdedDamageType(scoutController.ModdedDamageType);
                bulletAttack.AddModdedDamageType(DamageTypes.FillAtomic);
                bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
                {
                    if (BulletAttack.IsSniperTargetHit(hitInfo))
                    {
                        damageInfo.damage *= 2f;
                        damageInfo.AddModdedDamageType(DamageTypes.FillAtomicHeadshot);
                        damageInfo.damageColorIndex = DamageColorIndex.Sniper;
                        EffectData effectData = new EffectData
                        {
                            origin = hitInfo.point,
                            rotation = Quaternion.LookRotation(-hitInfo.direction)
                        };

                        effectData.SetHurtBoxReference(hitInfo.hitHurtBox);
                        EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/Common/VFX/WeakPointProcEffect.prefab").WaitForCompletion(), effectData, true);
                        Util.PlaySound("sfx_driver_headshot", base.gameObject);
                    }
                };
                bulletAttack.Fire();
            }

            base.characterBody.AddSpreadBloom(1.25f);

            this.duration = ShootRifle.baseDuration / characterBody.attackSpeed;
        }
        public override void OnExit()
        {
            base.OnExit();

            if (this.overlayController != null)
            {
                HudOverlayManager.RemoveOverlay(this.overlayController);
                this.overlayController = null;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.fireTimer >= this.duration) return InterruptPriority.Any;
            else return InterruptPriority.PrioritySkill;
        }
    }
}
