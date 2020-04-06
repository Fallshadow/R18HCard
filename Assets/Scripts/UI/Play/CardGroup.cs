using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{
    public class CardGroup : MonoBehaviour
    {
        private RectTransform rect = null;
        private void Awake()
        {
            rect = transform as RectTransform;
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Refresh_HandCard_Delay, RefreshCardChildPosDelay);
        }
        private void OnDestroy()
        {
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Refresh_HandCard_Delay, RefreshCardChildPosDelay);
        }

        public void RefreshCardChildPosDelay()
        {
            Invoke("RefreshCardChildPos", 0.5f);
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
                    transform.GetChild(i).localPosition = new Vector2(childX * (i + 1), -(rect.sizeDelta.y / 2));
                    transform.GetChild(i).GetComponent<CardDisplay>().InitPos = new Vector2(childX * (i + 1), -(rect.sizeDelta.y / 2)); 
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).localPosition = new Vector2(space * (i + 1), -(rect.sizeDelta.y / 2));
                    transform.GetChild(i).GetComponent<CardDisplay>().InitPos = new Vector2(childX * (i + 1), -(rect.sizeDelta.y / 2));
                }
            }

        }
    }
}