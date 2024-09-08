using UnityEngine;
using UnityEngine.UI;
using RoR2;
using RoR2.UI;

namespace OfficialScoutMod.Scout.Components
{
    public class AtomicGauge : MonoBehaviour
    {
        public HUD targetHUD;
        public ScoutController scoutController;

        public LanguageTextMeshController targetText;
        public GameObject durationDisplay;
        public Image durationBar;
        public Image durationBarRed;

        private void Start()
        {
            this.scoutController = this.targetHUD?.targetBodyObject?.GetComponent<ScoutController>();
            this.scoutController.onAtomicChange += SetDisplay;

            this.durationDisplay.SetActive(false);
            SetDisplay();
        }

        private void OnDestroy()
        {
            if (this.scoutController) this.scoutController.onAtomicChange -= SetDisplay;

            this.targetText.token = string.Empty;
            this.durationDisplay.SetActive(false);
            GameObject.Destroy(this.durationDisplay);
        }

        private void Update()
        {
            if(targetText.token != string.Empty) { targetText.token = string.Empty; }
            if (this.scoutController && this.scoutController.atomicGauge >= 0f)
            {
                float fill = Util.Remap(this.scoutController.atomicGauge, 0f, this.scoutController.maxAtomicGauge, 0f, 1f);

                if (this.durationBarRed)
                {
                    if (fill >= 1f) this.durationBarRed.fillAmount = 1f;
                    this.durationBarRed.fillAmount = Mathf.Lerp(this.durationBarRed.fillAmount, fill, Time.fixedDeltaTime * 2f);
                }

                this.durationBar.fillAmount = fill;
            }
        }

        private void SetDisplay()
        {
            if (this.scoutController)
            {
                this.durationDisplay.SetActive(true);
                this.targetText.token = string.Empty;

                if (this.scoutController.atomicGauge <=  99f) this.durationBar.color = new Color(85f / 255f, 188f / 255f, 0f);
                else this.durationBar.color = Color.red;
            }
            else
            {
                this.durationDisplay.SetActive(false);
            }
        }
    }
}