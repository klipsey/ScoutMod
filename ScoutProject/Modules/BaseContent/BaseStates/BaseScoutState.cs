﻿using EntityStates;
using RoR2;
using OfficialScoutMod.Scout.Components;
using OfficialScoutMod.Scout.Content;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace OfficialScoutMod.Modules.BaseStates
{
    public abstract class BaseScoutState : BaseState
    {
        protected ScoutController scoutController;

        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            RefreshState();
        }
        protected void RefreshState()
        {
            if (!scoutController)
            {
                scoutController = base.GetComponent<ScoutController>();
            }
        }
    }
}
