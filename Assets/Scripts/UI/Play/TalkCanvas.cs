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
        public CanvasGroup canvasGroup;
        public override void Initialize()
        {

        }

        public override void Refresh()
        {

        }

        public override void Release()
        {

        }

        public void HideCanvas()
        {
            Debug.Log("____________________");
            showTalkContent.gameObject.SetActive(true);
            hideTalkContent.interactable = false;
            canvasGroup.DOKill();
            canvasGroup.DOFade(0, 0.5f);
        }

        public void ShowCanvas()
        {
            showTalkContent.gameObject.SetActive(false);
            hideTalkContent.interactable = true;
            canvasGroup.DOKill();
            canvasGroup.DOFade(1, 0.5f);
        }

        protected override void onShow()
        {
            talkContent.text = "";
            talkContent.DOText(talkContentString, talkTime);
        }
    }
}

