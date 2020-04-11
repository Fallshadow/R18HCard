using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace act.ui
{
    public class CardGroup : MonoBehaviour
    {
        [SerializeField] private float doLocalMoveTime = 1;
        private RectTransform rect = null;
        private List<RectTransform> childrenRects = new List<RectTransform>();
        private List<CardDisplay> childrenDisplays = new List<CardDisplay>();
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

            float space = (rect.sizeDelta.x - 80) / transform.childCount;
            float childX = (transform.GetChild(0) as RectTransform).sizeDelta.x;
            childrenRects.Clear();
            childrenDisplays.Clear();
            for(int i = 0; i < transform.childCount; i++)
            {
                childrenDisplays.Add(transform.GetChild(i).GetComponent<CardDisplay>());
            }
            childrenDisplays = childrenDisplays.OrderBy(x => x.GetCardInst().config.type).ToList();
            if (space > childX)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    childrenDisplays[i].InitPos = new Vector2(childX * (i + 1), -(rect.sizeDelta.y / 2)); 
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    childrenDisplays[i].InitPos = new Vector2(space * (i + 1), -(rect.sizeDelta.y / 2));
                }
            }
            foreach(var item in childrenDisplays)
            {
                (item.transform as RectTransform).SetAsLastSibling();
            }
            int tempDis = 0;
            int tempType = 1;
            for(int i = 0; i < transform.childCount; i++)
            {
                childrenRects.Add(transform.GetChild(i).transform as RectTransform);
            }
            foreach(var item in childrenRects)
            {
                CardDisplay tempDisplay = item.GetComponent<CardDisplay>();
                
                if(tempDisplay.GetCardInst().config.type != tempType)
                {
                    tempType = tempDisplay.GetCardInst().config.type;
                    tempDis += 40;
                }
                item.DOLocalMove(tempDisplay.InitPos + new Vector3(tempDis, 0, 0), doLocalMoveTime).SetEase(Ease.OutQuad).OnComplete(()=> { tempDisplay.InitPos = item.localPosition; });
            }

        }
    }
}