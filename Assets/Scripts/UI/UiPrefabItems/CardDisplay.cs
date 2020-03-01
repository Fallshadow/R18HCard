﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace act.ui
{
    [RequireComponent(typeof(CardReference))]
    public class CardDisplay : MonoBehaviour, IUiItemLifeInterface, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] protected CardReference config = null;
        [SerializeField] private game.CardInst card_inst = null;
        private RectTransform rectTrans = null;
        public game.CardInst GetCardInst()
        {
            return card_inst;
        }
        public void Hide()
        {

        }

        public void Init()
        {
            config = GetComponent<CardReference>();
            rectTrans = GetComponent<RectTransform>();
        }

        public void SetInst(game.CardInst cardInst)
        {
            card_inst = cardInst;
            config.Text_Name.Localize(card_inst.config.name, "ui_system");
            config.Text_Desc.Localize(card_inst.config.desc, "ui_system");
            config.Text_TestNum.text = card_inst.config.testNumber.ToString();
            config.Text_CardType.text = Enum.GetName(typeof(game.CardType), card_inst.config.type);
            for (int index = 0; index < card_inst.conditionInsts.Count; index++)
            {
                for (int indexY = 0; indexY < card_inst.conditionInsts[index].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(card_inst.conditionInsts[index][indexY].desc, "ui_system");
                    config.Text_Conditions[index].text += ss + " ";
                }
            }

            for (int index = 0; index < card_inst.effectInsts.Count; index++)
            {
                for (int indexY = 0; indexY < card_inst.effectInsts[index].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(card_inst.effectInsts[index][indexY].desc, "ui_system");
                    config.Text_Effects[index].text += ss + " ";
                }
            }
        }

        public void Release()
        {

        }

        public void Show()
        {

        }

        private Transform tempParent = null;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!card_inst.Canuse)
            {
                return;
            }
            tempParent = transform.parent;
            transform.SetParent(tempParent.parent);//TODO:表现

            game.GameFlowMgr.instance.CurCard = card_inst;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!card_inst.Canuse)
            {
                return;
            }
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rectTrans.position = globalMousePos;
            }
            GraphicRaycaster gr = gameObject.GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(eventData, results);
            bool tempExistEvent = false;
            foreach (var item in results)
            {
                if (item.gameObject.tag == "EventPrefab")
                {
                    tempExistEvent = true;
                    game.EventInst eventInst = item.gameObject.GetComponent<EventDisplay>().GetEventInst();
                    if (game.GameFlowMgr.instance.CurEvent != eventInst)
                    {
                        game.GameFlowMgr.instance.CurEvent = eventInst;
                        game.GameFlowMgr.instance.CheckCardOnEvent();//TODO:如果成了表现为行
                    }
                    return;
                }
            }
            if (!tempExistEvent) { game.GameFlowMgr.instance.CurEvent = null; }
            //TODO:表现为不行
        }



        public void OnEndDrag(PointerEventData eventData)
        {
            if (!card_inst.Canuse)
            {
                game.GameFlowMgr.instance.CurEvent = null;
                return;
            }
            transform.SetParent(tempParent);//TODO:根据卡片的不同有不同的表现形式，需要滞后表现
            game.GameFlowMgr.instance.UseCard();
            game.GameFlowMgr.instance.CurEvent = null;
        }
    }
}
