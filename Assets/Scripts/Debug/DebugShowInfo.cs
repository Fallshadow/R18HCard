using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace act.debug
{
    public class DebugShowInfo : SingletonMonoBehavior<DebugShowInfo>
    {
        public bool ShowDebugCanvas = true;
        [SerializeField] private Canvas DebugCanvas = null;
        [SerializeField] private act.ui.UiStaticText FPSText = null;

        private int frameCount = 0;
        private float fpsByDeltatime = 1.5f;
        private float passedTime = 0.0f;
        private float realTimeFPS = 0.0f;
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
        private void Update()
        {
            if(ShowDebugCanvas)
            {
                GetFPS();
                if(DebugCanvas.gameObject.activeSelf == ShowDebugCanvas)
                {
                    DebugCanvas.gameObject.SetActive(true);
                }
            }
            else
            {
                DebugCanvas.gameObject.SetActive(false);
            }

        }
        public void GetFPS()
        {

            frameCount++;
            passedTime += Time.deltaTime;

            if(passedTime >= fpsByDeltatime)
            {
                realTimeFPS = frameCount / passedTime;
                FPSText.text = realTimeFPS.ToString();
                passedTime = 0;
                frameCount = 0;
            }
        }
    }
}
