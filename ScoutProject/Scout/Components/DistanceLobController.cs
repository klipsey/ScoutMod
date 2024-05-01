using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ScoutMod.Scout.Components
{
    public class DistanceLobController : MonoBehaviour
    {
        public float timer = 0f;
        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
        }
    }
}
