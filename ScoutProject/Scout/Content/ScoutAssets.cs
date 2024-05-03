using RoR2;
using UnityEngine;
using ScoutMod.Modules;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using R2API;
using RoR2.Skills;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using ThreeEyedGames;
using ScoutMod.Scout.Components;
using RoR2.EntityLogic;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.IO;
using System.Reflection;
using static UnityEngine.ParticleSystem.PlaybackState;
using Rewired.ComponentControls.Effects;

namespace ScoutMod.Scout.Content
{
    public static class ScoutAssets
    {
        //AssetBundle
        internal static AssetBundle mainAssetBundle;

        //Materials
        internal static Material commandoMat;

        //Shader
        internal static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");

        //Effects
        internal static GameObject atomicEffect;
        internal static GameObject atomicEndEffect;

        internal static GameObject atomicImpactEffect;
        internal static GameObject bloodSplatterEffect;

        internal static GameObject shotgunTracer;
        internal static GameObject shotgunTracerCrit;

        internal static GameObject batSwingEffect;
        internal static GameObject atomicSwingEffect;
        internal static GameObject batHitEffect;

        internal static GameObject scoutZoom;
        //Models
        internal static GameObject shotgunShell;
        internal static GameObject cleaverPrefab;
        internal static GameObject baseballPrefab;
        //Sounds
        internal static NetworkSoundEventDef batImpactSoundDef;
        public static void Init(AssetBundle assetBundle)
        {

            mainAssetBundle = assetBundle;

            CreateMaterials();

            CreateModels();

            CreateEffects();

            CreateSounds();

            CreateProjectiles();
        }

        #region heart

        private static void CleanChildren(Transform startingTrans)
        {
            for (int num = startingTrans.childCount - 1; num >= 0; num--)
            {
                if (startingTrans.GetChild(num).childCount > 0)
                {
                    CleanChildren(startingTrans.GetChild(num));
                }
                Object.DestroyImmediate(startingTrans.GetChild(num).gameObject);
            }
        }
        #endregion

        private static void CreateMaterials()
        {
        }

        private static void CreateModels()
        {
            shotgunShell = mainAssetBundle.LoadAsset<GameObject>("ShotgunShell");
            shotgunShell.GetComponentInChildren<MeshRenderer>().material = CreateMaterial("matShotgunShell");
            shotgunShell.AddComponent<Modules.Components.ShellController>();

        }
        #region effects
        private static void CreateEffects()
        {
            batHitEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/OmniImpactVFXLoader.prefab").WaitForCompletion().InstantiateClone("BatHitEffect");
            batHitEffect.AddComponent<NetworkIdentity>();
            ScoutMod.Modules.Content.CreateAndAddEffectDef(batHitEffect);
            batSwingEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion().InstantiateClone("ScoutBatSwing", false);
            batSwingEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Huntress/matHuntressSwingTrail.mat").WaitForCompletion();
            var swing = batSwingEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            swing.startLifetimeMultiplier *= 2f;
            atomicSwingEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion().InstantiateClone("ScoutAtomicSwing", false);
            atomicSwingEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Parent/matParentMeleeSwing.mat").WaitForCompletion();
            swing = atomicSwingEffect.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            swing.startLifetimeMultiplier *= 2f;
            GameObject wtf = new GameObject();
            scoutZoom = wtf.InstantiateClone("ScoutTrail", false);
            TrailRenderer pingus = scoutZoom.AddComponent<TrailRenderer>();
            pingus.startWidth = 4f;
            pingus.endWidth = 1f;
            pingus.time = 0.3f;
            pingus.emitting = true;
            pingus.numCornerVertices = 0;
            pingus.numCapVertices = 0;
            pingus.material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Beetle/matBeetleSpitTrail2.mat").WaitForCompletion();
            pingus.startColor = new Color(184f / 255f, 226f / 255f, 61f / 255f, 0.5f);
            pingus.endColor = Color.white;
            pingus.alignment = LineAlignment.TransformZ;

            shotgunTracer = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("ScoutShotgunTracer", true);

            if (!shotgunTracer.GetComponent<EffectComponent>()) shotgunTracer.AddComponent<EffectComponent>();
            if (!shotgunTracer.GetComponent<VFXAttributes>()) shotgunTracer.AddComponent<VFXAttributes>();
            if (!shotgunTracer.GetComponent<NetworkIdentity>()) shotgunTracer.AddComponent<NetworkIdentity>();

            Material bulletMat = null;

            foreach (LineRenderer i in shotgunTracer.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", new Color(0.68f, 0.58f, 0.05f));
                    i.material = bulletMat;
                    i.startColor = new Color(0.68f, 0.58f, 0.05f);
                    i.endColor = new Color(0.68f, 0.58f, 0.05f);
                }
            }

            shotgunTracerCrit = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoShotgun").InstantiateClone("ScoutShotgunTracerCritical", true);

            if (!shotgunTracerCrit.GetComponent<EffectComponent>()) shotgunTracerCrit.AddComponent<EffectComponent>();
            if (!shotgunTracerCrit.GetComponent<VFXAttributes>()) shotgunTracerCrit.AddComponent<VFXAttributes>();
            if (!shotgunTracerCrit.GetComponent<NetworkIdentity>()) shotgunTracerCrit.AddComponent<NetworkIdentity>();

            foreach (LineRenderer i in shotgunTracerCrit.GetComponentsInChildren<LineRenderer>())
            {
                if (i)
                {
                    bulletMat = UnityEngine.Object.Instantiate<Material>(i.material);
                    bulletMat.SetColor("_TintColor", Color.yellow);
                    i.material = bulletMat;
                    i.startColor = new Color(0.8f, 0.24f, 0f);
                    i.endColor = new Color(0.8f, 0.24f, 0f);
                }
            }
            Modules.Content.CreateAndAddEffectDef(shotgunTracer);
            Modules.Content.CreateAndAddEffectDef(shotgunTracerCrit);

            atomicEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/KillEliteFrenzy/NoCooldownEffect.prefab").WaitForCompletion().InstantiateClone("ScoutAtomicEffect");
            atomicEffect.AddComponent<NetworkIdentity>();
            atomicEffect.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", new Color(85f / 255f, 188f / 255f, 0f));
            atomicEffect.transform.GetChild(0).GetChild(1).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", new Color(85f / 255f, 188f / 255f, 0f));
            var main = atomicEffect.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
            main.startColor = new Color(184f / 255f, 226f / 255f, 61f / 255f);
            var what = main.startColor;
            what.m_Mode = ParticleSystemGradientMode.Color;

            atomicEndEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/LunarSkillReplacements/LunarDetonatorConsume.prefab").WaitForCompletion().InstantiateClone("ScoutAtomicEnd");
            atomicEndEffect.AddComponent<NetworkIdentity>();
            var fart = atomicEndEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().main;
            fart.startColor = Color.black;
            fart = atomicEndEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().main;
            fart.startColor = new Color(184f / 255f, 226f / 255f, 61f / 255f);
            atomicEndEffect.transform.GetChild(2).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", new Color(184f / 255f, 226f / 255f, 61f / 255f));
            atomicEndEffect.transform.GetChild(3).gameObject.SetActive(false);
            atomicEndEffect.transform.GetChild(4).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", new Color(184f / 255f, 226f / 255f, 61f / 255f));
            Material material = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarSkillReplacements/matLunarNeedleImpactEffect.mat").WaitForCompletion());
            material.SetColor("_TintColor", new Color(184f / 255f, 226f / 255f, 61f / 255f));
            atomicEndEffect.transform.GetChild(5).gameObject.GetComponent<ParticleSystemRenderer>().material = material;
            atomicEndEffect.transform.GetChild(6).gameObject.SetActive(false);
            Object.Destroy(atomicEndEffect.GetComponent<EffectComponent>());


            atomicImpactEffect = CreateImpactExplosionEffect("ScoutAtomicBlast", Addressables.LoadAssetAsync<Material>("RoR2/Base/Beetle/matBeetleSpitShockwave.mat").WaitForCompletion(), Addressables.LoadAssetAsync<Material>("RoR2/Base/Beetle/matBeetleQueenAcidDecal.mat").WaitForCompletion(), 2);

            bloodSplatterEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherSlamImpact.prefab").WaitForCompletion().InstantiateClone("ScoutSplat", true);
            bloodSplatterEffect.AddComponent<NetworkIdentity>();
            bloodSplatterEffect.transform.GetChild(0).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(1).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(2).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(3).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(4).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(5).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(6).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(7).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(8).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(9).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(10).gameObject.SetActive(false);
            bloodSplatterEffect.transform.Find("Decal").GetComponent<Decal>().Material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Imp/matImpDecal.mat").WaitForCompletion();
            bloodSplatterEffect.transform.Find("Decal").GetComponent<AnimateShaderAlpha>().timeMax = 10f;
            bloodSplatterEffect.transform.GetChild(12).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(13).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(14).gameObject.SetActive(false);
            bloodSplatterEffect.transform.GetChild(15).gameObject.SetActive(false);
            bloodSplatterEffect.transform.localScale = Vector3.one;
            ScoutMod.Modules.Content.CreateAndAddEffectDef(bloodSplatterEffect);
        }

        #endregion

        #region projectiles
        private static void CreateProjectiles()
        {
            cleaverPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2ShivProjectile.prefab").WaitForCompletion().InstantiateClone("ScoutCleaver");
            cleaverPrefab.AddComponent<NetworkIdentity>();
            cleaverPrefab.GetComponent<ProjectileSingleTargetImpact>().hitSoundString = "sfx_scout_cleaver_miss";
            cleaverPrefab.GetComponent<ProjectileSingleTargetImpact>().enemyHitSoundString = "sfx_scout_cleaver_hit";

            cleaverPrefab.GetComponent<ProjectileDamage>().damageType = DamageType.BlightOnHit;
            DamageAPI.ModdedDamageTypeHolderComponent moddedDamage = cleaverPrefab.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            moddedDamage.Add(DamageTypes.CleaverBonus);
            moddedDamage.Add(DamageTypes.FillAtomic);

            cleaverPrefab.GetComponent<ProjectileController>().ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2ShivGhostAlt.prefab").WaitForCompletion().InstantiateClone("ScoutCleaverGhost");
            cleaverPrefab.GetComponent<ProjectileController>().ghostPrefab.AddComponent<NetworkIdentity>();
            cleaverPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 120f;
            TrailRenderer trail = cleaverPrefab.AddComponent<TrailRenderer>();
            trail.startWidth = 0.5f;
            trail.endWidth = 0.1f;
            trail.time = 0.5f;
            trail.emitting = true;
            trail.numCornerVertices = 0;
            trail.numCapVertices = 0;
            trail.material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matSmokeTrail.mat").WaitForCompletion();
            trail.startColor = Color.white;
            trail.endColor = Color.gray;
            trail.alignment = LineAlignment.TransformZ;

            cleaverPrefab.GetComponent<ProjectileController>().ghostPrefab.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh = mainAssetBundle.LoadAsset<GameObject>("scoutCleaver").GetComponent<MeshFilter>().mesh;
            cleaverPrefab.GetComponent<ProjectileController>().ghostPrefab.transform.GetChild(0).localRotation = new Quaternion(90f, 0f, 90f, Quaternion.identity.w);
            cleaverPrefab.GetComponent<ProjectileController>().ghostPrefab.transform.GetChild(0).localScale = Vector3.one * 0.015f;
            cleaverPrefab.GetComponent<ProjectileController>().ghostPrefab.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CreateMaterial("matScout");

            Modules.Content.AddProjectilePrefab(cleaverPrefab);

            baseballPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2ShivProjectile.prefab").WaitForCompletion().InstantiateClone("ScoutBaseball");
            baseballPrefab.AddComponent<NetworkIdentity>();
            baseballPrefab.GetComponent <ProjectileStickOnImpact>().enabled = false;

            baseballPrefab.GetComponent<DelayedEvent>().enabled = false;

            baseballPrefab.GetComponent<ProjectileSingleTargetImpact>().impactEffect = batHitEffect;
            baseballPrefab.GetComponent<ProjectileSingleTargetImpact>().hitSoundString = "sfx_scout_baseball_miss";
            baseballPrefab.GetComponent<ProjectileSingleTargetImpact>().enemyHitSoundString = "sfx_scout_baseball_impact";

            baseballPrefab.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            DamageAPI.ModdedDamageTypeHolderComponent moddedDamage2 = baseballPrefab.AddComponent<DamageAPI.ModdedDamageTypeHolderComponent>();
            moddedDamage2.Add(DamageTypes.BallStun);
            moddedDamage2.Add(DamageTypes.FillAtomic);

            baseballPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 120f;

            baseballPrefab.AddComponent<DistanceLobController>();

            baseballPrefab.GetComponent<ProjectileController>().ghostPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bell/BellBallSmallGhost.prefab").WaitForCompletion().InstantiateClone("ScoutBaseballGhost");
            baseballPrefab.GetComponent<ProjectileController>().ghostPrefab.AddComponent<NetworkIdentity>();
            Object.Destroy(baseballPrefab.transform.GetChild(0).GetChild(0).gameObject);
            Object.Destroy(baseballPrefab.transform.GetChild(0).GetChild(1).gameObject);

            TrailRenderer trail2 = baseballPrefab.AddComponent<TrailRenderer>();
            trail2.startWidth = 0.5f;
            trail2.endWidth = 0.1f;
            trail2.time = 0.5f;
            trail2.emitting = true;
            trail2.numCornerVertices = 0;
            trail2.numCapVertices = 0;
            trail2.material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matSmokeTrail.mat").WaitForCompletion();
            trail2.startColor = Color.white;
            trail2.endColor = Color.gray;
            trail2.alignment = LineAlignment.TransformZ;

            Modules.Content.AddProjectilePrefab(baseballPrefab);
        }
        #endregion

        #region sounds
        private static void CreateSounds()
        {
            batImpactSoundDef = Modules.Content.CreateAndAddNetworkSoundEventDef("sfx_scout_bat_impact");

        }
        #endregion

        #region helpers
        private static GameObject CreateImpactExplosionEffect(string effectName, Material bloodMat, Material decal, float scale = 1f)
        {
            GameObject newEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherSlamImpact.prefab").WaitForCompletion().InstantiateClone(effectName, true);

            newEffect.transform.Find("Spikes, Small").gameObject.SetActive(false);

            newEffect.transform.Find("PP").gameObject.SetActive(false);
            newEffect.transform.Find("Point light").gameObject.SetActive(false);
            newEffect.transform.Find("Flash Lines").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Common/VFX/matOpaqueDustLargeDirectional.mat").WaitForCompletion();

            newEffect.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().material = bloodMat;
            newEffect.transform.Find("Flash Lines, Fire").GetComponent<ParticleSystemRenderer>().material = bloodMat;
            newEffect.transform.GetChild(6).GetComponent<ParticleSystemRenderer>().material = bloodMat;
            newEffect.transform.Find("Fire").GetComponent<ParticleSystemRenderer>().material = bloodMat;

            var boom = newEffect.transform.Find("Fire").GetComponent<ParticleSystem>().main;
            boom.startLifetimeMultiplier = 0.5f;
            boom = newEffect.transform.Find("Flash Lines, Fire").GetComponent<ParticleSystem>().main;
            boom.startLifetimeMultiplier = 0.3f;
            boom = newEffect.transform.GetChild(6).GetComponent<ParticleSystem>().main;
            boom.startLifetimeMultiplier = 0.4f;

            newEffect.transform.Find("Physics").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/MagmaWorm/matFracturedGround.mat").WaitForCompletion();

            newEffect.transform.Find("Decal").GetComponent<Decal>().Material = decal;
            newEffect.transform.Find("Decal").GetComponent<AnimateShaderAlpha>().timeMax = 10f;

            newEffect.transform.Find("FoamSplash").gameObject.SetActive(false);
            newEffect.transform.Find("FoamBilllboard").gameObject.SetActive(false);
            newEffect.transform.Find("Dust").gameObject.SetActive(false);
            newEffect.transform.Find("Dust, Directional").gameObject.SetActive(false);

            newEffect.transform.localScale = Vector3.one * scale;

            newEffect.AddComponent<NetworkIdentity>();

            ParticleSystemColorFromEffectData PSCFED = newEffect.AddComponent<ParticleSystemColorFromEffectData>();
            PSCFED.particleSystems = new ParticleSystem[]
            {
                newEffect.transform.Find("Fire").GetComponent<ParticleSystem>(),
                newEffect.transform.Find("Flash Lines, Fire").GetComponent<ParticleSystem>(),
                newEffect.transform.GetChild(6).GetComponent<ParticleSystem>(),
                newEffect.transform.GetChild(3).GetComponent<ParticleSystem>()
            };
            PSCFED.effectComponent = newEffect.GetComponent<EffectComponent>();

            ScoutMod.Modules.Content.CreateAndAddEffectDef(newEffect);

            return newEffect;
        }
        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
            Material tempMat = mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat) return commandoMat;

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }

        public static Material CreateMaterial(string materialName)
        {
            return CreateMaterial(materialName, 0f);
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return CreateMaterial(materialName, emission, Color.black);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return CreateMaterial(materialName, emission, emissionColor, 0f);
        }
        #endregion
    }
}