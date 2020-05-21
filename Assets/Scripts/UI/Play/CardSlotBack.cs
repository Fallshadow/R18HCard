using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace act.ui
{
    public class CardSlotBack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int ImageIndex = 0;
        public void OnPointerEnter(PointerEventData eventData)
        {
            evt.EventManager.instance.Send<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_Desc_Card_Slot_Enter, ImageIndex);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            evt.EventManager.instance.Send<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_Desc_Card_Slot_Exit, ImageIndex);
        }
    }
}