using HG;
using Newtonsoft.Json.Linq;
using R2API;
using RoR2;
using RoR2.Projectile;
using ScoutMod.Modules;
using ScoutMod.Scout.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using static RoR2.DotController;

namespace ScoutMod.Scout.Content
{
    public static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType Default;
        public static DamageAPI.ModdedDamageType FillAtomic;
        public static DamageAPI.ModdedDamageType FillAtomicShotgun;
        public static DamageAPI.ModdedDamageType MiniCrit;
        public static DamageAPI.ModdedDamageType BallStun;
        public static DamageAPI.ModdedDamageType CleaverBonus; 
        internal static void Init()
        {
            Default = DamageAPI.ReserveDamageType();
            FillAtomic = DamageAPI.ReserveDamageType();
            FillAtomicShotgun = DamageAPI.ReserveDamageType();
            CleaverBonus = DamageAPI.ReserveDamageType();
            MiniCrit = DamageAPI.ReserveDamageType();
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
            if (NetworkServer.active)
            {
                if(damageInfo.HasModdedDamageType(FillAtomic))
                {
                    attackerBody.GetComponent<ScoutController>().FillAtomic(5f, damageInfo.crit);
                    attackerBody.RecalculateStats();
                }
                else if(damageInfo.HasModdedDamageType(FillAtomicShotgun))
                {
                    attackerBody.GetComponent<ScoutController>().FillAtomic(2f, damageInfo.crit);
                    attackerBody.RecalculateStats();
                }
            }
        }
    }
}
