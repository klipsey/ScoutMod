using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using OfficialScoutMod.Scout.Content;

namespace OfficialScoutMod.Scout.Components
{
    public class ScoutController : MonoBehaviour
    {

        public bool atMaxGauge => atomicGauge >= maxAtomicGauge;
        private CharacterBody characterBody;
        private ModelSkinController skinController;
        private ChildLocator childLocator;
        private CharacterModel characterModel;
        private Animator animator;
        private SkillLocator skillLocator;
        private GameObject endEffect = ScoutAssets.atomicEndEffect;
        public DamageAPI.ModdedDamageType ModdedDamageType = DamageTypes.Default;

        private readonly int maxShellCount = 12;
        private GameObject[] shellObjects;
        private int currentShell;

        public float atomicGauge = 0f;

        public float maxAtomicGauge = 100f;

        public bool atomicDraining = false;

        public Action onAtomicChange;

        public float secondary1CdTimer = 0f;
        public float secondary1Cd = 6f;
        public int maxSecondary1Stock = 1;
        public int currentSecondary1Stock = 1;

        public float secondary2CdTimer = 0f;
        public float secondary2Cd = 6f;
        public int maxSecondary2Stock = 1;
        public int currentSecondary2Stock = 1;

        public float stagedReload = 0f;

        private uint playID1;
        private uint playID2;

        private float graceTimer = 0f;
        public bool hasGraced = false;

        private void Awake()
        {
            this.characterBody = this.GetComponent<CharacterBody>();
            ModelLocator modelLocator = this.GetComponent<ModelLocator>();
            this.childLocator = modelLocator.modelBaseTransform.GetComponentInChildren<ChildLocator>();
            this.animator = modelLocator.modelBaseTransform.GetComponentInChildren<Animator>();
            this.characterModel = modelLocator.modelBaseTransform.GetComponentInChildren<CharacterModel>();
            this.skillLocator = this.GetComponent<SkillLocator>();
            this.skinController = modelLocator.modelTransform.gameObject.GetComponent<ModelSkinController>();
        }
        private void Start()
        {
            InitShells();
            SetupStockSecondary1();
            SetupStockPrimary2();
        }
        public void FillAtomic(float amount, bool isCrit)
        {
            if (atomicDraining && !ScoutConfig.gainAtomicGaugeDuringAtomicBlast.Value) return;
            else if (atomicDraining && ScoutConfig.gainAtomicGaugeDuringAtomicBlast.Value) amount *= 0.25f;

            if (atomicGauge + amount <= maxAtomicGauge)
            {
                atomicGauge += amount;
                if (isCrit) atomicGauge += amount;
            }
            else atomicGauge = maxAtomicGauge;

            if(atomicGauge < 0f) atomicGauge = 0f;

            NetworkIdentity networkIdentity = base.gameObject.GetComponent<NetworkIdentity>();
            if (!networkIdentity)
            {
                return;
            }

            new SyncAtomic(networkIdentity.netId, (ulong)(this.atomicGauge * 100f)).Send(R2API.Networking.NetworkDestination.Clients);

            this.onAtomicChange?.Invoke();
        }
        private void InitShells()
        {
            this.currentShell = 0;

            this.shellObjects = new GameObject[this.maxShellCount + 1];

            GameObject desiredShell = ScoutAssets.shotgunShell;

            for (int i = 0; i < this.maxShellCount; i++)
            {
                this.shellObjects[i] = GameObject.Instantiate(desiredShell, this.childLocator.FindChild("Pistol"), false);
                this.shellObjects[i].transform.localScale = Vector3.one * 1.1f;
                this.shellObjects[i].SetActive(false);
                this.shellObjects[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

                this.shellObjects[i].layer = LayerIndex.ragdoll.intVal;
                this.shellObjects[i].transform.GetChild(0).gameObject.layer = LayerIndex.ragdoll.intVal;
            }
        }
        public void DropShell(Vector3 force)
        {
            if (this.shellObjects == null) return;

            if (this.shellObjects[this.currentShell] == null) return;

            Transform origin = this.childLocator.FindChild("Scattergun");

            this.shellObjects[this.currentShell].SetActive(false);

            this.shellObjects[this.currentShell].transform.position = origin.position;
            this.shellObjects[this.currentShell].transform.SetParent(null);

            this.shellObjects[this.currentShell].SetActive(true);

            Rigidbody rb = this.shellObjects[this.currentShell].gameObject.GetComponent<Rigidbody>();
            if (rb) rb.velocity = force;

            this.currentShell++;
            if (this.currentShell >= this.maxShellCount) this.currentShell = 0;
        }

        public void ActivateAtomic()
        {
            if (NetworkServer.active) this.characterBody.AddBuff(ScoutBuffs.scoutAtomicBuff);
            Transform scoutTransform = this.childLocator.FindChild("Chest").transform;
            atomicDraining = true;
            this.ModdedDamageType = DamageTypes.AtomicCrits;
            AkSoundEngine.StopPlayingID(this.playID1);
            AkSoundEngine.StopPlayingID(this.playID2);
            playID1 = Util.PlaySound("sfx_scout_atomic_on", this.gameObject);
            playID2 = Util.PlaySound("sfx_scout_atomic_duration", this.gameObject);
        }
        public void DeactivateAtomic()
        {
            AkSoundEngine.StopPlayingID(this.playID1);
            AkSoundEngine.StopPlayingID(this.playID2);
            Util.PlaySound("sfx_scout_atomic_off", base.gameObject);
            if (NetworkServer.active) this.characterBody.RemoveBuff(ScoutBuffs.scoutAtomicBuff);
            atomicDraining = false;
            atomicGauge = 0f;
            this.ModdedDamageType = DamageTypes.Default;
            Instantiate(endEffect, characterBody.modelLocator.transform);
        }
        public void SwitchLayer(string layerName)
        {
            if (!this.animator) return;

            if (layerName == "")
            {
                this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, Bat"), 0f);

                childLocator.FindChild("BatMesh").gameObject.SetActive(false);
                childLocator.FindChild("ScatterGunMesh").gameObject.SetActive(true);
                childLocator.FindChild("BackBatMesh").gameObject.SetActive(true);
            }
            else
            {
                childLocator.FindChild("BatMesh").gameObject.SetActive(true);
                childLocator.FindChild("ScatterGunMesh").gameObject.SetActive(false);
                childLocator.FindChild("BackBatMesh").gameObject.SetActive(false);

                this.animator.SetLayerWeight(this.animator.GetLayerIndex(layerName), 1f);
            }
        }
        public void SetupStockPrimary2()
        {
            secondary2CdTimer = this.skillLocator.secondary.rechargeStopwatch;
            secondary2Cd = this.skillLocator.secondary.finalRechargeInterval;
            currentSecondary2Stock = this.skillLocator.secondary.stock;
            maxSecondary2Stock = this.skillLocator.secondary.maxStock;
        }
        public void SetupStockSecondary1()
        {
            secondary1CdTimer = this.skillLocator.secondary.rechargeStopwatch;
            secondary1Cd = this.skillLocator.secondary.finalRechargeInterval;
            currentSecondary1Stock = this.skillLocator.secondary.stock;
            maxSecondary1Stock = this.skillLocator.secondary.maxStock;
        }
        public bool InGracePeriod()
        {
            return graceTimer <= 2f && atMaxGauge;
        }
        private void FixedUpdate()
        {
            graceTimer += Time.fixedDeltaTime;

            if (atomicDraining) 
            {
                atomicGauge -= maxAtomicGauge / (400f + (100f * this.skillLocator.utility.maxStock - 1));
                onAtomicChange?.Invoke();
                if(atomicGauge <= 0) DeactivateAtomic();
            }

            if (secondary2CdTimer < secondary2Cd)
            {
                secondary2CdTimer += Time.fixedDeltaTime;
            }
            else if (secondary2CdTimer >= secondary2Cd && currentSecondary2Stock < maxSecondary2Stock)
            {
                secondary2CdTimer = 0f;
                currentSecondary2Stock++;
            }

            if (secondary1CdTimer < secondary1Cd)
            {
                secondary1CdTimer += Time.fixedDeltaTime;
            }
            else if (secondary1CdTimer >= secondary1Cd && currentSecondary1Stock < maxSecondary1Stock)
            {
                secondary1CdTimer = 0f;
                currentSecondary1Stock++;
            }

            if(atMaxGauge && !hasGraced) 
            {
                graceTimer = 0f;
                hasGraced = true;

                Util.PlaySound("sfx_driver_plasma_cannon_shoot", this.gameObject);
                EffectManager.SimpleMuzzleFlash(ScoutAssets.scoutMaxGauge, base.gameObject, "Chest", false);
            }
            else if(!atMaxGauge && hasGraced)
            {
                hasGraced = false;
            }
        }

        private void OnDestroy()
        {
            AkSoundEngine.StopPlayingID(this.playID1);
            AkSoundEngine.StopPlayingID(this.playID2);
        }
    }
}
