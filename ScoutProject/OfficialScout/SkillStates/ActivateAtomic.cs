using UnityEngine;
using EntityStates;
using OfficialScoutMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using OfficialScoutMod.Scout.Content;
using UnityEngine.Networking;

namespace OfficialScoutMod.Scout.SkillStates
{
    public class ActivateAtomic : BaseScoutSkillState
    {
        DamageType damageType;
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            if (this.characterBody.HasBuff(ScoutBuffs.scoutAtomicBuff) || scoutController.atomicGauge < 1f)
            {
                return;
            }

            if (this.scoutController)
            {
                this.scoutController.ActivateAtomic();

                if (this.scoutController.atomicGauge >= 10f)
                {
                    if(NetworkServer.active) this.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1.5f);

                    if (base.isAuthority)
                    {
                        this.damageType = DamageType.AOE;
                        this.damageType |= scoutController.atomicGauge >= scoutController.maxAtomicGauge / 2f ? DamageType.Stun1s : DamageType.Generic;
                        BlastAttack.Result result = new BlastAttack
                        {
                            attacker = base.gameObject,
                            procChainMask = default(ProcChainMask),
                            impactEffect = EffectIndex.Invalid,
                            losType = BlastAttack.LoSType.None,
                            damageColorIndex = DamageColorIndex.Default,
                            damageType = this.damageType,
                            procCoefficient = Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 0.1f, 1f),
                            bonusForce = Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 50f, 400f) * Vector3.up,
                            baseForce = Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 250f, 2000f),
                            baseDamage = Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 1f * this.damageStat, ScoutStaticValues.atomicBlastDamageCoefficient * this.damageStat),
                            falloffModel = BlastAttack.FalloffModel.None,
                            radius = Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 1f, 16f),
                            position = this.characterBody.corePosition,
                            attackerFiltering = AttackerFiltering.NeverHitSelf,
                            teamIndex = base.GetTeam(),
                            inflictor = base.gameObject,
                            crit = base.RollCrit()
                        }.Fire();

                        EffectManager.SpawnEffect(ScoutAssets.atomicImpactEffect, new EffectData
                        {
                            origin = this.transform.position + (Vector3.up * 1.8f),
                            rotation = Quaternion.identity,
                            scale = Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 0.2f,  3f)
                        }, false);
                    }
                }
                if (base.isAuthority)
                {
                    if (!this.isGrounded)
                    {
                        this.SmallHop(this.characterMotor, Util.Remap(scoutController.atomicGauge, 10f, scoutController.maxAtomicGauge, 1f, 16f));
                    }
                    this.outer.SetNextStateToMain();
                }
            }
        }
    }
}