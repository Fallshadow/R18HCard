using act.game;
using System.Collections;
using UnityEngine;

public delegate void CallBack();
namespace act.ui
{
    public class CardEffect : SingletonMonoBehaviorNoDestroy<CardEffect>
    {   /// <summary>
        /// 翻转卡牌时间间隔
        /// </summary>
        [SerializeField]
        private float flipDuration = 1.0f;
        /// <summary>
        /// 隐藏卡牌时间间隔
        /// </summary>
        [SerializeField]
        private float hideDuration = 1.0f;
        /// <summary>
        /// 显示卡牌时间间隔
        /// </summary>
        [SerializeField]
        private float showDuration = 1.0f;

        IEnumerator DoFlipCard(CardDisplay display, CallBack callack, CardInst newCardInst)
        {
            while (display.transform.rotation.eulerAngles.y < 90.0f)
            {
                display.transform.rotation *= Quaternion.Euler(0.0f, Time.deltaTime / flipDuration * 90.0f, 0.0f);
                if (display.transform.rotation.eulerAngles.y > 90.0f)
                {
                    display.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            if (newCardInst != null)
                display.SetInst(newCardInst);
            while (display.transform.rotation.eulerAngles.y > 0.0f)
            {
                display.transform.rotation *= Quaternion.Euler(0.0f, -Time.deltaTime / flipDuration * 90.0f, 0.0f);
                if (display.transform.rotation.eulerAngles.y > 90.0f)
                {
                    display.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            callack?.Invoke();
        }

        /// <summary>
        /// 翻转卡牌
        /// </summary>
        /// <param name="display">卡牌控制</param>
        /// <param name="callBack">回调</param>
        /// <param name="newCardInst">背面参数 翻到背面时替换</param>
        public void FlipCard(CardDisplay display, CallBack callBack = null, CardInst newCardInst = null)
        {
            if (display == null)
                return;
            StartCoroutine(DoFlipCard(display, callBack, newCardInst));
        }
        IEnumerator DoHideCard(CanvasGroup group, CallBack callBack = null)
        {
            while (group.alpha > 0.0f)
            {
                group.alpha -= Time.deltaTime / hideDuration;
                yield return new WaitForFixedUpdate();
            }
        }
        /// <summary>
        /// 隐藏卡牌
        /// </summary>
        /// <param name="display">卡牌控制</param>
        /// <param name="callBack">回调</param>
        public void HideCard(CardDisplay display, CallBack callBack = null)
        {
            if (display == null)
                return;
            CanvasGroup group = display.GetOrAddComponent<CanvasGroup>();
            StartCoroutine(DoHideCard(group, callBack));
        }
        IEnumerator DoShowCard(CanvasGroup group, CallBack callBack = null)
        {
            group.alpha = 0.0f;
            while (group.alpha < 1.0f)
            {
                group.alpha += Time.deltaTime / showDuration;
                yield return new WaitForFixedUpdate();
            }
        }
        /// <summary>
        /// 显示卡牌
        /// </summary>
        /// <param name="display">卡牌控制</param>
        /// <param name="callBack">回调</param>
        public void ShowCard(CardDisplay display, CallBack callBack = null)
        {
            if (display == null)
                return;
            CanvasGroup group = display.GetOrAddComponent<CanvasGroup>();
            StartCoroutine(DoShowCard(group, callBack));
        }
    }
}
