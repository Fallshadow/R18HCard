using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace act.ui
{
    [BindingResource("Play/TalkCanvas")]
    public class TalkCanvas : FullScreenCanvasBase
    {
        public Text talkContent;
        public UiStaticText talkerName;
        public string talkContentString;
        public float talkTime;
        public Button hideTalkContent;
        public Button showTalkContent;
        public Button NextBtn;
        public CanvasGroup canvasGroup;
        public override void Initialize()
        {

        }

        public override void Refresh()
        {
            onShow();
        }

        public override void Release()
        {

        }

        public void ShowCanvas()
        {
            Debug.Log("____________________");
            showTalkContent.gameObject.SetActive(true);
            hideTalkContent.interactable = false;
            canvasGroup.DOKill();
            canvasGroup.DOFade(1, 0.5f);
        }

        public void HideCanvas()
        {
            showTalkContent.gameObject.SetActive(false);
            hideTalkContent.interactable = true;
            canvasGroup.DOKill();
            canvasGroup.DOFade(0, 0.5f);
        }

        protected override void onShow()
        {
            talkContent.text = "";
            talkContent.DOText(talkContentString, talkTime);
        }

        public void SetNextBtn(UnityEngine.Events.UnityAction action)
        {
            NextBtn.onClick.RemoveAllListeners();
            NextBtn.onClick.AddListener(action);
            NextBtn.onClick.AddListener(() => {
                AudioMgr.instance.PlaySound(AudioClips.AC_kuang);
            });
        }
    }
}

