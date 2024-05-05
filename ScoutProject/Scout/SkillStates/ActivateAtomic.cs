using UnityEngine;
using EntityStates;
using ScoutMod.Modules.BaseStates;
using RoR2;
using UnityEngine.AddressableAssets;
using ScoutMod.Scout.Content;
using UnityEngine.Networking;

namespace ScoutMod.Scout.SkillStates
{
    public class ActivateAtomic : BaseScoutSkillState
    {
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
            if (this.characterBody.HasBuff(ScoutBuffs.scoutAtomicBuff) || scoutController.atomicGauge < 1f)
            {
                if(this.skillLocator.utility.stock < 1f) this.skillLocator.utility.AddOneStock();
                return;
            }

            if (this.scoutController)
            {
                this.scoutController.ActivateAtomic();

                if (this.scoutController.atomicGauge >= this.scoutController.maxAtomicGauge)
                {
                    if(NetworkServer.active) this.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1.5f);

                    if (base.isAuthority)
                    {
                        BlastAttack.Result result = new BlastAttack
                        {
                            attacker = base.gameObject,
                            procChainMask = default(ProcChainMask),
                            impactEffect = EffectIndex.Invalid,
                            losType = BlastAttack.LoSType.None,
                            damageColorIndex = DamageColorIndex.Default,
                            damageType = DamageType.Stun1s,
                            procCoefficient = 1f,
                            bonusForce = 400 * Vector3.up,
                            baseForce = 2000f,
                            baseDamage = ScoutStaticValues.atomicBlastDamageCoefficient * this.damageStat,
                            falloffModel = BlastAttack.FalloffModel.None,
                            radius = 16f,
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
                            scale = 3f
                        }, false);
                    }
                }
                if (base.isAuthority)
                {
                    if (!this.isGrounded)
                    {
                        this.SmallHop(this.characterMotor, 16f);
                    }
                    this.outer.SetNextStateToMain();
                }
            }
        }
    }
}