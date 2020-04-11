using DG.Tweening;
using UnityEngine;

namespace act.ui
{
    public class CardGroup : MonoBehaviour
    {
        private RectTransform rect = null;
        [SerializeField] private float refreshDuration = 0.5f;
        void Start()
        {
            rect = transform as RectTransform;
        }
        public void RefreshCardChildPos()
        {
            if (transform.childCount == 0)
                return;
            if (rect == null)
                rect = transform as RectTransform;

            float space = rect.sizeDelta.x / transform.childCount;
            float childX = (transform.GetChild(0) as RectTransform).sizeDelta.x;
            if (space > childX)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).DOLocalMove(new Vector2(childX * (i + 1), -(rect.sizeDelta.y / 2)), refreshDuration);
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).DOLocalMove(new Vector2(space * (i + 1), -(rect.sizeDelta.y / 2)), refreshDuration);
                }
            }

        }
    }
}
