using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace act.ui
{
    [BindingResource("MainMenu/MainMenuCanvas")]
    public class MainMenuCanvas : InteractableUiBase
    {
        public override UiOpenType OpenType => base.OpenType;
        public bool firstShow = true;
        public CanvasGroup title;
        public CanvasGroup interBtn;
        public CanvasGroup allShow;
        public CanvasGroup textBtn;
        public float fadeIn = 1.5f;
        public float hold = 2;
        public float fadeOut = 1.5f;
        public float allShowTime = 2;

        [Header("重洗开始按钮")]
        public Button playbtn;
        public override void Initialize()
        {

        }
        private void HideAll()
        {
            title.gameObject.SetActive(false);
            allShow.gameObject.SetActive(false);
            interBtn.gameObject.SetActive(false);
            textBtn.interactable = false;
        }
        protected override void onShow()
        {
            game.GameFlowMgr.instance.LoadSettingData();
            HideAll();
            title.gameObject.SetActive(true);
            allShow.gameObject.SetActive(true);

            var titleFadeSeq = DOTween.Sequence();
            if(firstShow)
            {
                titleFadeSeq.Append(title.DOFade(0, 0));
                titleFadeSeq.Append(allShow.DOFade(0, 0));
                titleFadeSeq.Append(title.DOFade(1, fadeIn));
                   // .AppendCallback(() => { AudioMgr.instance.PlaySound(AudioClips.AC_Title); });
                var audio = DOTween.Sequence();
                titleFadeSeq.Append(audio);
                audio.Append(title.DOFade(1, hold));
                titleFadeSeq.Append(title.DOFade(1,1));
                titleFadeSeq.Append(title.DOFade(0, fadeOut));
                firstShow = false;
                titleFadeSeq.Append(allShow.DOFade(1, allShowTime));

            }
            else
            {
                allShow.gameObject.SetActive(true);
                titleFadeSeq.Append(allShow.DOFade(0, 0));
                titleFadeSeq.Append(allShow.DOFade(1, allShowTime));
            }
            titleFadeSeq.AppendCallback(() =>
            {
                title.gameObject.SetActive(false);
                textBtn.interactable = true;
                PingPong(0,0.6f,1);
                AudioMgr.instance.PlayMusicFade(AudioClips.AC_TitleBGM);
            });
        }
        private void PingPong(float from,float to,float dur)
        {
            textBtn.DOFade(to, dur).OnComplete(()=> { PingPong(to, from, dur); });
        }


        public override void Refresh()
        {

        }

        public override void Release()
        {

        }
        #region Btn
        public void ShowInterBtn()
        {
            if(game.GameFlowMgr.instance.CanReplay)
            {
                playbtn.interactable = true;
            }
            else
            {
                playbtn.interactable = false;
            }
            textBtn.DOKill();
            textBtn.interactable = false;
            interBtn.gameObject.SetActive(true);
            var FadeSeq = DOTween.Sequence();
            FadeSeq.Append(interBtn.DOFade(0, 0));
            FadeSeq.Append(textBtn.DOFade(1, 0));
            FadeSeq.Append(textBtn.DOFade(0, 1));
            FadeSeq.Append(interBtn.DOFade(1, 1));
            AudioMgr.instance.PlaySound(AudioClips.AC_TitleBtn);
        }
        public void ExitBtn()
        {
            AudioMgr.instance.PlaySound(AudioClips.AC_TitleBtn);
            Application.Quit();
        }
        public void Play()
        {
            Hide();
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.PLAP);
            AudioMgr.instance.PlaySound(AudioClips.AC_TitleBtn);
            AudioMgr.instance.PlayMusicFade(AudioClips.AC_OneBGM);
        }
        public void RePlay()
        {
            Hide();
            game.GameFlowMgr.instance.CanReplay = true;
            game.GameFlowMgr.instance.ClearData();
            game.GameFlowMgr.instance.LoadData();
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.PLAP);
            AudioMgr.instance.PlaySound(AudioClips.AC_TitleBtn);
            AudioMgr.instance.PlayMusicFade(AudioClips.AC_OneBGM);
        }

        public void ShowSetting()
        {
            ui.UiManager.instance.ControlMouseInput(UiManager.instance.CreateUi<MainMenuCanvas>(), false);

            SettingWindow settingWindow = act.ui.UiManager.instance.CreateUi<act.ui.SettingWindow>();
            settingWindow.gameObject.SetActive(true);
            settingWindow.ShowWindow();
        }


        #endregion
    }
}