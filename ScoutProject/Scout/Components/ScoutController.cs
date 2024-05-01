using R2API;
using R2API.Networking;
using R2API.Networking.Interfaces;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using ScoutMod.Scout.Content;

namespace ScoutMod.Scout.Components
{
    public class ScoutController : MonoBehaviour
    {
        private CharacterBody characterBody;
        private ModelSkinController skinController;
        private ChildLocator childLocator;
        private CharacterModel characterModel;
        private Animator animator;
        private SkillLocator skillLocator;

        public DamageAPI.ModdedDamageType ModdedDamageType = DamageTypes.Default;

        private readonly int maxShellCount = 12;
        private GameObject[] shellObjects;
        private int currentShell;

        public float atomicGauge = 0f;

        public float maxAtomicGauge = 100f;

        public bool atomicDraining = false;

        public Action onAtomicChange;

        public float ballCdTimer = 0f;
        public float ballCd = 6f;
        public int maxBallStock = 1;
        public int currentBallStock = 1;

        public float cleaverCdTimer = 0f;
        public float cleaverCd = 6f;
        public int maxCleaverStock = 1;
        public int currentCleaverStock = 1;

        private uint playID1;
        private uint playID2;

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
        }
        public void FillAtomic(float amount, bool isCrit)
        {
            if (atomicDraining) return;
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
            if (NetworkServer.active) this.characterBody.AddBuff(ScoutBuffs.atomicBuff);
            atomicDraining = true;
            this.ModdedDamageType = DamageTypes.MiniCrit;
            playID1 = Util.PlaySound("sfx_scout_atomic_on", this.gameObject);
            playID2 = Util.PlaySound("sfx_scout_atomic_duration", this.gameObject);
        }
        public void DeactivateAtomic()
        {
            AkSoundEngine.StopPlayingID(this.playID1);
            AkSoundEngine.StopPlayingID(this.playID2);
            Util.PlaySound("sfx_scout_atomic_off", base.gameObject);
            if (NetworkServer.active) this.characterBody.RemoveBuff(ScoutBuffs.atomicBuff);
            atomicDraining = false;
            atomicGauge = 0f;
            this.ModdedDamageType = DamageTypes.Default;
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
        public void SetupStockBaseball()
        {
            ballCdTimer = this.skillLocator.secondary.rechargeStopwatch;
            ballCd = this.skillLocator.secondary.finalRechargeInterval;
            currentBallStock = this.skillLocator.secondary.stock;
            maxBallStock = this.skillLocator.secondary.maxStock;
        }
        public void SetupStockCleaver()
        {
            cleaverCdTimer = this.skillLocator.secondary.rechargeStopwatch;
            cleaverCd = this.skillLocator.secondary.finalRechargeInterval;
            currentCleaverStock = this.skillLocator.secondary.stock;
            maxCleaverStock = this.skillLocator.secondary.maxStock;
        }
        private void FixedUpdate()
        {
            if(atomicDraining) 
            {
                atomicGauge -= maxAtomicGauge / 400f;
                onAtomicChange?.Invoke();
                if(atomicGauge <= 0) DeactivateAtomic();
            }

            if (ballCdTimer < ballCd)
            {
                ballCdTimer += Time.fixedDeltaTime;
            }
            else if (ballCdTimer >= ballCd && currentBallStock < maxBallStock)
            {
                ballCdTimer = 0f;
                currentBallStock++;
            }

            if (cleaverCdTimer < cleaverCd)
            {
                cleaverCdTimer += Time.fixedDeltaTime;
            }
            else if (cleaverCdTimer >= cleaverCd && currentCleaverStock < maxCleaverStock)
            {
                cleaverCdTimer = 0f;
                currentCleaverStock++;
            }
        }
    }
}
