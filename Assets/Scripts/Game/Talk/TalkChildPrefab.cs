using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using act.ui;

namespace act.worldui
{
    public class TalkChildPrefab : MonoBehaviour
    {
        public UiStaticText content;
        public Image spriteBackGround;
        public GameObject followGameObject;

        private CanvasGroup canvasGroup;
        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public void ChangeContent(string text, Sprite sprite = null)
        {
            content.text = text;
            if(null != sprite)
            {
                spriteBackGround.sprite = sprite;
            }
        }

        public void ChangeAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }
    }

}
