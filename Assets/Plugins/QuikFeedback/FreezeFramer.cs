using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pataya.QuikFeedback
{
    public class FreezeFramer : MonoBehaviour
    {
        private bool isFreezeFraming = false;
        private float progress = 0f;
        private FreezeFrameFeedback freeze;

        private void Update()
        {
            UpdateFreezeFrame();
        }

        public void FreezeFrame(FreezeFrameFeedback freeze)
        {
            this.freeze = freeze;
            isFreezeFraming = true;
            progress = 0f;
        }

        private void UpdateFreezeFrame()
        {
            if(isFreezeFraming)
            {
                if(progress < 1f)
                {
                    progress += Time.unscaledDeltaTime * freeze.speed;
                    float timeScale = Mathf.LerpUnclamped(freeze.minTimeScale, freeze.maxTimeScale, freeze.curve.Evaluate(progress));
                    Time.timeScale = timeScale;
                }

                if(progress >= 1f)
                {
                    isFreezeFraming = false;
                    Time.timeScale = 1f;
                }
            }
        }
    }
}