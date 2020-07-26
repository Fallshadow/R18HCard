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
        public Material material;

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
            //textShow.SetActive(false);
            textShow.GetOrAddComponent<CanvasGroup>().interactable = false;
            material.DOFloat(-1, "_IntensityU", 1);
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
            //textShow.SetActive(false);
            textShow.GetOrAddComponent<CanvasGroup>().interactable = false;
            material.DOFloat(-1, "_IntensityU", 1);
        }

        public void FadeDark(float dur)
        {
            dark.DOFade(0, dur);
        }

        public void ShowText(string text)
        {
            ResetText();
            
            textShow.SetActive(true);
            textShow.GetOrAddComponent<CanvasGroup>().interactable = true;
            material.DOFloat(1, "_IntensityU", 1);
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
            canvasGroup.blocksRaycasts = true;
        }
        protected override void onClose()
        {
            base.onClose();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

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
                AudioMgr.instance.PlaySound(AudioClips.AC_kuang);
                isFirst = true;
                game.TimeLineMgr.instance.ResumeTimeLine(game.TimeLineMgr.instance.newPlayerDir);
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
