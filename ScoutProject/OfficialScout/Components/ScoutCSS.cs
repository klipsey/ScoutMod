using UnityEngine;
using RoR2;

namespace OfficialScoutMod.Scout.Components
{
    public class ScoutCSS : MonoBehaviour
    {
        private bool hasPlayed = false;
        private float timer = 0f;
        private void Awake()
        {
        }
        private void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if (!hasPlayed && timer >= 0.8f)
            {
                hasPlayed = true;
                Util.PlaySound("sfx_driver_gun_throw", this.gameObject);
            }
        }
    }
}
