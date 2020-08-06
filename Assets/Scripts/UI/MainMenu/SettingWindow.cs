using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace act.ui
{
    [BindingResource("Window/SettingWindow")]
    public class SettingWindow : UiWindowBase
    {
        public Slider musicSlider;
        public Slider soundSlider;
        public Slider envirSlider;
        public Toggle toggle;

        protected override void onShow()
        {
            base.onShow();

        }
        public override void Initialize()
        {
            musicSlider.maxValue = 1;
            soundSlider.maxValue = 1;
            envirSlider.maxValue = 1;
            musicSlider.minValue = 0;
            soundSlider.minValue = 0;
            envirSlider.minValue = 0;
            musicSlider.value = AudioMgr.instance.musicVol;
            soundSlider.value = AudioMgr.instance.soundVol;
            envirSlider.value = AudioMgr.instance.envirVol;
            toggle.isOn = game.GameController.instance.isInNewPlayFlow;

            toggle.onValueChanged.AddListener(ChangeNewPlayerSetting);
            musicSlider.onValueChanged.AddListener(ChangeMusicVoice);
            soundSlider.onValueChanged.AddListener(ChangeSoundVoice);
            envirSlider.onValueChanged.AddListener(ChangeEnvirVoice);
        }

        public override void Refresh()
        {

        }

        public override void Release()
        {

        }

        public void ShowWindow()
        {
            GetComponent<GraphicRaycaster>().enabled = true;
            gameObject.layer = 5;
        }

        public void ChangeNewPlayerSetting(bool value)
        {
            game.GameController.instance.isInNewPlayFlow = value;
            game.GameController.instance.isInNewPlayFlow2 = value;
        }

        public void ChangeMusicVoice(float value)
        {
            AudioMgr.instance.SetMusicVoice(value);
        }
        
        public void ChangeSoundVoice(float value)
        {
            AudioMgr.instance.SetSoundVoice(value);
        }

        public void ChangeEnvirVoice(float value)
        {
            AudioMgr.instance.SetEnvirVoice(value);
        }
        public void OnClickClose()
        {
            ui.UiManager.instance.ControlMouseInput(UiManager.instance.CreateUi<MainMenuCanvas>(), true);
            Close();
            gameObject.SetActive(false);
        }
    }
}

