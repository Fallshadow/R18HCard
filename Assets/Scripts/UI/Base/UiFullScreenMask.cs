using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace act.ui
{
    public class UiFullScreenMask : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnMaskClickHandler = null;

        [SerializeField] private effect.KawaseBlur blurImg = null;
        [SerializeField] private Image maskImg = null;

        public void SetFrontMask(float alpha, bool isBlur = false)
        {
            if (alpha > 0f)
            {
                if (isBlur)
                {
                    UiManager.instance.StartCoroutine(blurFrontMask());
                }
                else
                {
                    blurImg.Hide();
                }

                Color color = maskImg.color;
                color.a = alpha;
                maskImg.color = color;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                blurImg.Hide();
            }
        }

        private IEnumerator blurFrontMask()
        {
            yield return new WaitForEndOfFrame();
            blurImg.Blur(utility.CameraUtility.TakeShot(Screen.width, Screen.height));
            blurImg.Show();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnMaskClickHandler != null)
            {
                OnMaskClickHandler();
            }
        }
    }
}