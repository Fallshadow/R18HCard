using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuideEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    // 事件穿透对象
    public List<GameObject> targets;
    //监听按下
    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }
    //监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {

        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }
    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.submitHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }

    //把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for(int j = 0; j < targets.Count; j++)
        {
            //Debug.LogError($"target[{j}]={targets[j].name}");
            for(int i = 0; i < results.Count; i++)
            {
                //Debug.LogError($"results[{i}]={results[i].gameObject.name}");
                if(targets[j] == results[i].gameObject)
                {
                    //如果是目标物体，则把事件透传下去，然后break
                    //Debug.LogError($"RayCastTarget:[{j}]{targets[j].name}");
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                    break;
                }
            }
        }

    }
    private void Update()
    {

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.dragHandler);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.endDragHandler);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerEnterHandler);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerExitHandler);
    }
}