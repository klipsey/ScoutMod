using EntityStates;
using RoR2;
using OfficialScoutMod.Scout.Components;
using OfficialScoutMod.Scout.Content;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;

namespace OfficialScoutMod.Modules.BaseStates
{
    public abstract class BaseScoutSkillState : BaseSkillState
    {
        protected ScoutController scoutController;

        protected ScoutPassive scoutPassive;

        protected ScoutSwap scoutSwapPassive;

        protected bool isAtomic;

        public float fixedAge;
        public virtual void AddRecoil2(float x1, float x2, float y1, float y2)
        {
            this.AddRecoil(x1, x2, y1, y2);
        }
        public override void OnEnter()
        {
            RefreshState();
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            fixedAge += Time.fixedDeltaTime;
        }
        protected void RefreshState()
        {
            if (!scoutController)
            {
                scoutController = base.GetComponent<ScoutController>();
            }
            if (scoutController)
            {
                isAtomic = scoutController.atomicDraining;
            }
            if(!scoutPassive)
            {
                scoutPassive = base.GetComponent<ScoutPassive>();
            }
            if(!scoutSwapPassive)
            {
                scoutSwapPassive = base.GetComponent<ScoutSwap>();
            }
        }
    }
}
