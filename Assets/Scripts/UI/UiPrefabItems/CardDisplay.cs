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
        public Vector3 InitPos = Vector3.zero; 
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
            evt.EventManager.instance.Register<int>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Destory, CheckCardDestroy);
            evt.EventManager.instance.Register<int>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Refresh_Use, ChangeDisplay);
            evt.EventManager.instance.Register(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, ResetToCardGroup);
        }

        private void SetTextColor(Color color)
        {
            config.Text_Name.color = color;
            config.Text_DescSP.color = color;
            config.Text_Desc.color = color;
            config.Text_TestNum.color = color;
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
            switch(cardInst.config.type)
            {
                case (int)game.CardType.CT_Action:
                    SetTextColor(UiStyleHelper.instance.ActionCardColor);
                    break;
                case (int)game.CardType.CT_Emotion:
                    SetTextColor(UiStyleHelper.instance.EmotionCardColor);
                    break;
                case (int)game.CardType.CT_Special:
                    SetTextColor(UiStyleHelper.instance.SpecialCardColor);
                    break;
                case (int)game.CardType.CT_Word:
                    SetTextColor(UiStyleHelper.instance.WordCardColor);
                    break;
                default:
                    break;
            }

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

        private Transform tempParent = null;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                return;
            }
            tempParent = transform.parent;
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
        public void OnEndDrag(PointerEventData eventData)
        {
            isDrag = false;
            HideDownOthers();

            if(CheckCanUseCardInSlot())
            {
                rectTrans.position = (tempPointGo.transform.GetChild(0) as RectTransform).position;
                rectTrans.localPosition += new Vector3(0, 5.5f, 0);
                rectTrans.sizeDelta = (tempPointGo.transform.GetChild(0) as RectTransform).sizeDelta * 1.17f;
                isLockedSlot = true;
                evt.EventManager.instance.Send(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Enter_Slot);
            }
            else
            {
                Debug.Log("卡牌无法使用，不满足条件");
                ResetToCardGroup();
            }
        }


        //能否把卡牌插入插槽
        public bool CheckCanUseCardInSlot()
        {
            Debug.Log($"CurCard：{game.GameFlowMgr.instance.CurCard == null}");
            if(game.GameFlowMgr.instance.CurCard != null)
            {
                return false;
            }
            game.GameFlowMgr.instance.CurCard = card_inst;
            Debug.Log($"tempPointGo：{tempPointGo}");
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

        public void CheckCardDestroy(int id)
        {
            if(card_inst.UniqueId != id)
                return;
            //TODO:表现
            GameObject.Destroy(this.gameObject);
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Refresh_HandCard_Delay);
        }
        private void OnDestroy()
        {
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Destory, CheckCardDestroy);
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Refresh_Use, ChangeDisplay);
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
        public void OnPointerClick(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                game.GameFlowMgr.instance.CurCard = null;
                evt.EventManager.instance.Send(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Exit_Slot);
                ResetToCardGroup();
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(isLockedSlot)
            {
                return;
            }
            HideDownOthers();
        }
        private int showIndexID = 0;
        public void ShowUpOthers()
        {
            showIndexID = rectTrans.GetSiblingIndex();
            rectTrans.SetAsLastSibling();
            transform.localScale = config.enterScaleSize;
            config.DescShow.SetActive(true);
            //GetComponent<Canvas>().sortingOrder = 2;
        }

        public void HideDownOthers()
        {
            rectTrans.SetAsFirstSibling();
            rectTrans.SetSiblingIndex(showIndexID);
            if (isDrag)
                return;
            transform.localScale = Vector3.one;
            config.DescShow.SetActive(false);
            //GetComponent<Canvas>().sortingOrder = 1;
        }

        //回到手牌区
        public void ResetToCardGroup()
        {
            if(game.GameFlowMgr.instance.CurCard == card_inst)
            {
                game.GameFlowMgr.instance.CurCard = null;
            }
            isLockedSlot = false;
            rectTrans.DOAnchorPos(InitPos, 0.5f).SetEase(Ease.OutQuad);
            rectTrans.DOSizeDelta(InitSizeDelta,0.5f).SetEase(Ease.InQuad);
        }
    }
}

