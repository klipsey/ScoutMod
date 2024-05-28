using HG;
using Newtonsoft.Json.Linq;
using R2API;
using RoR2;
using RoR2.Projectile;
using OfficialScoutMod.Modules;
using OfficialScoutMod.Scout.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static RoR2.DotController;

namespace OfficialScoutMod.Scout.Content
{
    public static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType Default;
        public static DamageAPI.ModdedDamageType FillAtomic;
        public static DamageAPI.ModdedDamageType FillAtomicShotgun;
        public static DamageAPI.ModdedDamageType FillAtomicHeadshot;
        public static DamageAPI.ModdedDamageType AtomicCrits;
        public static DamageAPI.ModdedDamageType BallStun;
        public static DamageAPI.ModdedDamageType CleaverBonus; 
        internal static void Init()
        {
            Default = DamageAPI.ReserveDamageType();
            FillAtomic = DamageAPI.ReserveDamageType();
            FillAtomicShotgun = DamageAPI.ReserveDamageType();
            FillAtomicHeadshot = DamageAPI.ReserveDamageType();
            CleaverBonus = DamageAPI.ReserveDamageType();
            AtomicCrits = DamageAPI.ReserveDamageType();
            BallStun = DamageAPI.ReserveDamageType();
            Hook();
        }
        private static void Hook()
        {
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.SetStateOnHurt.OnTakeDamageServer += SetStateOnHurt_OnTakeDamageServer;
            On.RoR2.GlobalEventManager.OnHitEnemy += new On.RoR2.GlobalEventManager.hook_OnHitEnemy(GlobalEventManager_OnHitEnemy);
        }

        private static void SetStateOnHurt_OnTakeDamageServer(On.RoR2.SetStateOnHurt.orig_OnTakeDamageServer orig, SetStateOnHurt self, DamageReport damageReport)
        {
            if (!NetworkServer.active) return;
            orig.Invoke(self, damageReport);
            DamageInfo damageInfo = damageReport.damageInfo;
            GameObject inflictorObject = damageInfo.inflictor;
            if (damageInfo.HasModdedDamageType(BallStun))
            {
                self.SetStun(inflictorObject.GetComponent<DistanceLobController>().timer * 2f + 1.5f);
            }
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {

            if (!damageInfo.attacker || !victim)
            {
                return;
            }
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            orig.Invoke(self, damageInfo, victim);
        }

        private static void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            DamageInfo damageInfo = damageReport.damageInfo;
            if (!damageReport.attackerBody || !damageReport.victimBody)
            {
                return;
            }
            HealthComponent victim = damageReport.victim;
            GameObject inflictorObject = damageInfo.inflictor;
            CharacterBody victimBody = damageReport.victimBody;
            EntityStateMachine victimMachine = victimBody.GetComponent<EntityStateMachine>();
            CharacterBody attackerBody = damageReport.attackerBody;
            GameObject attackerObject = damageReport.attacker.gameObject;
            ScoutController attackerScout = attackerBody.GetComponent<ScoutController>();
            if (NetworkServer.active)
            {
                if(attackerScout)
                {
                    if (damageInfo.HasModdedDamageType(FillAtomic))
                    {
                        if (damageInfo.HasModdedDamageType(FillAtomicHeadshot))
                        {
                            attackerScout.FillAtomic(10f / attackerBody.skillLocator.utility.cooldownScale + attackerBody.skillLocator.utility.flatCooldownReduction, damageInfo.crit);
                        }
                        else
                        {
                            attackerScout.FillAtomic(5f / attackerBody.skillLocator.utility.cooldownScale + attackerBody.skillLocator.utility.flatCooldownReduction, damageInfo.crit);
                        }
                        attackerBody.RecalculateStats();
                    }
                    else if (damageInfo.HasModdedDamageType(FillAtomicShotgun))
                    {
                        attackerScout.FillAtomic(1f / attackerBody.skillLocator.utility.cooldownScale + attackerBody.skillLocator.utility.flatCooldownReduction, damageInfo.crit);
                        attackerBody.RecalculateStats();
                    }

                    if (damageInfo.HasModdedDamageType(BallStun) && inflictorObject)
                    {
                        damageReport.victimBody.AddTimedBuff(ScoutBuffs.scoutStunMarker, inflictorObject.GetComponent<DistanceLobController>().timer * 2f + 1.5f);
                    }
                }
            }
        }
    }
}
