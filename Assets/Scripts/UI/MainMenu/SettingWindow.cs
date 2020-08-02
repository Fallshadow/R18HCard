using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace act.ui
{
    [BindingResource("Window/Setting")]
    public class SettingWindow : UiWindowBase
    {
        public Slider musicSlider;
        public Slider voiceSlider;

        protected override void onShow()
        {
            base.onShow();
            musicSlider.maxValue = AudioMgr.instance.MaxVol;
            voiceSlider.maxValue = AudioMgr.instance.MaxVol;
            
        }
        public override void Initialize()
        {

        }

        public override void Refresh()
        {

        }

        public override void Release()
        {

        }
    }
}

