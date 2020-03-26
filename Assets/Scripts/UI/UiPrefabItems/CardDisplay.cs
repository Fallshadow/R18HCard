using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace act.ui
{
    [RequireComponent(typeof(CardReference))]
    public class CardDisplay : MonoBehaviour, IUiItemLifeInterface, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
    {
        [SerializeField] protected CardReference config = null;
        [SerializeField] private game.CardInst card_inst = null;
        private RectTransform rectTrans = null;
        private bool isDrag = false;
        private GameObject tempPointGo = null;
        private bool isLockedSlot = false;
        private Vector3 InitPos = Vector3.zero; 
         private Vector2 InitSizeDelta = Vector2.zero; 
        public game.CardInst GetCardInst()
        {
            return card_inst;
        }
        public void Hide()
        {

        }
        public void Show()
        {

        }

        public void EnterToTable()
        {
            card_inst.EnterTable();
            //TODO:表现上EnterToTable
        }
        public void Init()
        {
            config = GetComponent<CardReference>();
            rectTrans = GetComponent<RectTransform>();
            tempParent = transform.parent;
            InitPos = rectTrans.position;
            InitSizeDelta = rectTrans.sizeDelta;
        }

        public void SetInst(game.CardInst cardInst)
        {
            card_inst = cardInst;
            config.cardTypeBG.sprite = UiManager.instance.GetSprite($"CardType{cardInst.config.type}", "PlayCanvas");
            config.Text_Name.Localize(card_inst.config.name, "ui_system");
            config.Text_DescSP.Localize(card_inst.config.desc, "ui_system");
            config.Text_Desc.Localize(card_inst.config.descSP, "ui_system");
            config.Text_TestNum.text = card_inst.config.testNumber.ToString();
            config.Text_CardType.Localize(Enum.GetName(typeof(game.CardType), card_inst.config.type), "ui_system");
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

            evt.EventManager.instance.Register<int>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Destory, CheckCardDestroy);
            evt.EventManager.instance.Register<int>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Refresh_Use, ChangeDisplay);
            evt.EventManager.instance.Register(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, ResetToCardGroup);
    }

        public void Release()
        {

        }

        private Transform tempParent = null;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                return;
            }
            //if (!card_inst.Canuse)
            //{
            //    return;
            //}
            tempParent = transform.parent;
            //transform.SetParent(tempParent.parent);//TODO:表现

            game.GameFlowMgr.instance.CurCard = card_inst;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                return;
            }
            isDrag = true;
            config.DescShow.SetActive(false);
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                rectTrans.position = globalMousePos;
            }
            GraphicRaycaster gr = gameObject.GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(eventData, results);
            foreach (var item in results)
            {
                if (item.gameObject.tag == "CardSlot")
                {
                    tempPointGo = item.gameObject;
                    return;
                }
            }
            tempPointGo = null;
            //TODO:表现为不行
        }

        //能否把卡牌插入插槽
        public bool CheckCanUseCardInSlot()
        {
            if(tempPointGo != null
                && card_inst.Canuse
                &&
                (
                    tempPointGo.gameObject.name == $"CardType{card_inst.config.type}"
                    || tempPointGo.gameObject.name == $"CardType"
                    || tempPointGo.gameObject.name == $"CardType{card_inst.config.ID}"
                )
            )
            {
                return true;
            }

            if(tempPointGo != null && (!card_inst.Canuse) && tempPointGo.gameObject.name == $"CardTypeUnuse")
            {
                return true;
            }

            return false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            HideDownOthers();
            //transform.SetParent(tempParent);//TODO:根据卡片的不同有不同的表现形式，需要滞后表现
            if(CheckCanUseCardInSlot())
            {
                rectTrans.position = (tempPointGo.transform as RectTransform).position;
                rectTrans.sizeDelta = (tempPointGo.transform as RectTransform).sizeDelta;
                isLockedSlot = true;
            }
            else
            {
                tempParent.GetComponent<CardGroup>().RefreshCardChildPos();
            }
        }
        public void CheckCardDestroy(int id)
        {
            if(card_inst.UniqueId != id)
                return;
            //TODO:表现
            GameObject.Destroy(this.gameObject);
        }
        private void OnDestroy()
        {
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Destory, CheckCardDestroy);
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Refresh_Use, ChangeDisplay);
            evt.EventManager.instance.Unregister(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, ResetToCardGroup);
        }
        public void ChangeDisplay(int id)
        {
            if (card_inst.UniqueId != id)
                return;
            if (!card_inst.Canuse)
            {
                config.cardTypeBG.DOFade(0.6f, 1);
            }
            else
            {
                config.cardTypeBG.DOFade(1f, 1);
            }

        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                return;
            }
            ShowUpOthers();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                return;
            }
            HideDownOthers();
        }

        public void ShowUpOthers()
        {
            transform.localScale = config.enterScaleSize;
            config.DescShow.SetActive(true);
            //GetComponent<Canvas>().sortingOrder = 2;
        }

        public void HideDownOthers()
        {
            if (isDrag)
                return;
            transform.localScale = Vector3.one;
            config.DescShow.SetActive(false);
            //GetComponent<Canvas>().sortingOrder = 1;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                ResetToCardGroup();
            }
        }

        //回到手牌区
        private void ResetToCardGroup()
        {
            isLockedSlot = false;
            rectTrans.position = InitPos;
            rectTrans.sizeDelta = InitSizeDelta;
            tempParent.GetComponent<CardGroup>().RefreshCardChildPos();
        }
    }
}

