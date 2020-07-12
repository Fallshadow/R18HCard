using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace act.ui
{
    [BindingResource("Window/TalkWindow")]

    public class TalkWindow : UiWindowBase
{
        public Text content;
        public float dur;
        public Image dark;
        public GameObject textShow;

        private string contenttext;
        private bool isFirst = true;
        private CanvasGroup canvasGroup;
        public void UseDark(bool isactive)
        {
            dark.enabled = isactive;
        }
        public void ResetText()
        {
            content.DOKill();
            contenttext = "";
            content.text = contenttext;
            textShow.SetActive(false);
        }
        public void ResetTextString(string text)
        {
            content.DOKill();
            contenttext = text;
            content.text = contenttext;
        }
        public void HideText()
        {
            contenttext = "";
            content.text = contenttext;
            textShow.SetActive(false);
        }

        public void FadeDark(float dur)
        {
            dark.DOFade(0, dur);
        }

        public void ShowText(string text)
        {
            ResetText();
            
            textShow.SetActive(true);
            contenttext = text;
            content.DOText(text, dur).OnComplete(()=> { isFirst = false; });
        }
        private void Start()
        {
            SetInteractable(true);
            canvasGroup = GetComponent<CanvasGroup>();
        }
        protected override void onShow()
        {
            base.onShow();
            canvasGroup.interactable = true;
        }
        protected override void onClose()
        {
            base.onClose();
            canvasGroup.interactable = false;
        }
        public void Click()
        {
            if(isFirst)
            {
                isFirst = false;
                ResetTextString(contenttext);
            }
            else
            {
                isFirst = true;
                game.TimeLineMgr.instance.ResumeTimeLine(game.TimeLineMgr.instance.newPlayerDir);
                Close();
            }
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
