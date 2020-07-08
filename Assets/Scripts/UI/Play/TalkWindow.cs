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

        private bool isFirst = true;
        private CanvasGroup canvasGroup;
        public void ShowText(string text)
        {
            content.DOText(text, dur);
        }
        private void Start()
        {
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
                content.DOKill();
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
