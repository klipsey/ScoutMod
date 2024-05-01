using UnityEngine;
using RoR2;

namespace ScoutMod.Scout.Components
{
    public class ScoutCSS : MonoBehaviour
    {
        private void Awake()
        {
        }
        private void OnEnable()
        {
            Util.PlaySound("sfx_driver_gun_throw", this.gameObject);

        }
    }
}
