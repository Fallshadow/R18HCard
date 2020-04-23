using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace act.input
{
    public class InputManager : SingletonMonoBehavior<InputManager>
    {
        public ParticleSystem mouseClick = null;
        public Camera uiCamera = null;
        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                evt.EventManager.instance.Send<bool>(evt.EventGroup.INPUT, (short)evt.InputEvent.IE_Mouse_Right_Down, true);
            }
            if(Input.GetMouseButtonUp(1))
            {
                evt.EventManager.instance.Send<bool>(evt.EventGroup.INPUT, (short)evt.InputEvent.IE_Mouse_Right_Up,false);
            }
            if(Input.GetMouseButtonDown(0))
            {
                mouseClick.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
                Debug.Log(mouseClick.GetComponent<RectTransform>().anchoredPosition);
                Debug.Log(Input.mousePosition);
                mouseClick.Play();
            }
        }
    }
}
