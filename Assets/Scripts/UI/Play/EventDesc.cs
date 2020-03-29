using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace act.ui
{
    public class EventDesc : MonoBehaviour
    {
        [SerializeField] private Image Img_Bg = null;
        [SerializeField] private UiStaticText SText_Event_Name = null;
        [SerializeField] private UiStaticText SText_Event_Round = null;


        [SerializeField] private UiStaticText SText_Name = null;
        [SerializeField] private UiStaticText SText_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Com_Desc_Title = null;
        [SerializeField] private UiStaticText SText_SP_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Succ_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Def_Desc_Title = null;
        [SerializeField] private UiStaticText SText_RoundOver_Desc_Title = null;
        [SerializeField] private UiStaticText SText_EventCard_Desc_Title = null;

        [SerializeField] private UiStaticText SText_Desc = null;
        [SerializeField] private UiStaticText SText_Com_Desc = null;
        [SerializeField] private UiStaticText SText_SP_Desc = null;
        [SerializeField] private UiStaticText SText_Succ_Desc = null;
        [SerializeField] private UiStaticText SText_Def_Desc = null;
        [SerializeField] private UiStaticText SText_RoundOver_Desc = null;
        [SerializeField] private UiStaticText SText_EventCard_Desc = null;
        [SerializeField] private VerticalLayoutGroup verLayputGroup = null;
        [SerializeField] private Image[] Image_CardSlot = new Image[4];
        [SerializeField] private CanvasGroup canvasGroup;

        public game.CardInst curCardInst = null;
        private void Awake()
        {
            
        }
        private void OnDestroy()
        {
            
        }

        private void LayoutRefresh()
        {
            //SText_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            //SText_Com_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            //SText_SP_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            //SText_Succ_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            //SText_Def_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            for(int i = 0; i < verLayputGroup.transform.childCount; i++)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(verLayputGroup.transform.GetChild(i) as RectTransform);
            }
            verLayputGroup.SetLayoutVertical();
        }

    public void ShowDesc(game.EventInst inst)
        {
            evt.EventManager.instance.Register<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change,ChangeCurCard);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Enter_Slot, ShowEventCardDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Exit_Slot, HideEventCardDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE, ShowRoundOverDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, ShowSuccDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, ShowDefDescTip);



            game.GameFlowMgr.instance.CurEvent = inst;
            gameObject.SetActive(true);
            game.GameFlowMgr.instance.eventDesc = true;
            canvasGroup.interactable = false;
            canvasGroup.DOFade(1, 1).OnComplete(()=> { canvasGroup.interactable = true; });
            HideAll();
            if(inst.config.condition_1 != null)
            {
                SText_Com_Desc_Title.gameObject.SetActive(true);
                SText_Com_Desc.gameObject.SetActive(true);
            }
            if(inst.config.specialCId != null)
            {
                SText_SP_Desc.gameObject.SetActive(true);
                SText_SP_Desc_Title.gameObject.SetActive(true);
            }
            SText_Event_Name.Localize(inst.config.name, "ui_system");
            SText_Event_Round.text = inst.RoundNum.ToString();

            SText_Name.gameObject.SetActive(true);
            SText_Desc.gameObject.SetActive(true);
            SText_Name.Localize(inst.config.name, "ui_system");
            SText_Desc.Localize(inst.config.desc, "ui_system");
            SText_SP_Desc.Localize(inst.config.desc_SP, "ui_system");
            SText_Com_Desc.Localize(inst.config.desc_Common, "ui_system");
            SText_Succ_Desc.Localize(inst.config.desc_SuccResult, "ui_system");
            SText_Def_Desc.Localize(inst.config.desc_DefResult, "ui_system");
            SText_RoundOver_Desc.Localize(inst.config.desc_DefResult, "ui_system");

            if(curCardInst != null && curCardInst.UniqueId != 0)
            {
                SText_EventCard_Desc.text = game.EventMgr.instance.GetEventCardDesc(
                    game.GameFlowMgr.instance.CurEvent.config.ID,
                    game.GameFlowMgr.instance.CurCard.config.type,
                    game.GameFlowMgr.instance.CurCard.config.ID
                    );
            }
            int tempCardSlot = 0;
            foreach(var conditions in inst.conditionInsts)
            {
                foreach(var item in conditions)
                {
                    if(item.config.ID == 1)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType{item.numVars[0]}", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].gameObject.name = $"CardType{item.numVars[0]}";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 3)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType{game.CardMgr.instance.GetCardDataByID((int)item.numVars[0]).type}", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].gameObject.name = $"CardType{item.numVars[0]}";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 2)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].gameObject.name = $"CardType";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 16)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].gameObject.name = $"CardTypeUnuse";
                        tempCardSlot++;
                    }
                }
            }
            Img_Bg.sprite = UiManager.instance.GetSprite($"determine_bg{tempCardSlot}", "PlayCanvas");
            Img_Bg.SetNativeSize();
            for(int i = 0; i < tempCardSlot; i++)
            {
                Image_CardSlot[i].gameObject.SetActive(true);
            }
            LayoutRefresh();
        }


        private void ChangeCurCard(game.CardInst cardInst)
        {
            curCardInst = cardInst;
        }
        public void HideDesc()
        {
            evt.EventManager.instance.Unregister<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change, ChangeCurCard);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Enter_Slot, ShowEventCardDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Exit_Slot, HideEventCardDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE, ShowRoundOverDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, ShowSuccDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, ShowDefDescTip);
            canvasGroup.DOFade(0, 1);
            game.GameFlowMgr.instance.eventDesc = false;
            game.GameFlowMgr.instance.CurEvent = null;
            evt.EventManager.instance.Send(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide);
        }
        public void ChangeDescByCard()
        {
            
        }
        public void HideAll()
        {
            SText_Name.gameObject.SetActive(false);
            SText_EventCard_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc_Title.gameObject.SetActive(false);
            SText_RoundOver_Desc.gameObject.SetActive(false);
            SText_RoundOver_Desc_Title.gameObject.SetActive(false);
            SText_Desc_Title.gameObject.SetActive(false);
            SText_SP_Desc_Title.gameObject.SetActive(false);
            SText_Succ_Desc_Title.gameObject.SetActive(false);
            SText_Def_Desc_Title.gameObject.SetActive(false);
            SText_Desc.gameObject.SetActive(false);
            SText_SP_Desc.gameObject.SetActive(false);
            SText_Succ_Desc.gameObject.SetActive(false);
            SText_Def_Desc.gameObject.SetActive(false);
            for(int i = 0; i < Image_CardSlot.Length; i++)
            {
                Image_CardSlot[i].gameObject.SetActive(false);
            }
        }
        
        
        public void ShowSuccDescTip()
        {
            SText_Def_Desc.gameObject.SetActive(false);
            SText_Def_Desc_Title.gameObject.SetActive(false);
            SText_Succ_Desc.gameObject.SetActive(true);
            SText_Succ_Desc_Title.gameObject.SetActive(true);
            LayoutRefresh();
        }

        public void ShowDefDescTip()
        {
            SText_Succ_Desc.gameObject.SetActive(false);
            SText_Succ_Desc_Title.gameObject.SetActive(false);
            SText_Def_Desc.gameObject.SetActive(true);
            SText_Def_Desc_Title.gameObject.SetActive(true);
             LayoutRefresh();
        }

        public void ShowRoundOverDescTip()
        {
            SText_RoundOver_Desc.gameObject.SetActive(true);
            SText_RoundOver_Desc_Title.gameObject.SetActive(true);
            LayoutRefresh();
        }

        public void ShowEventCardDescTip()
        {
            SText_EventCard_Desc.gameObject.SetActive(true);
            SText_EventCard_Desc_Title.gameObject.SetActive(true);
            LayoutRefresh();
        }
        public void HideEventCardDescTip()
        {
            SText_EventCard_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc_Title.gameObject.SetActive(false);
            LayoutRefresh();
        }
        


        public void UseCard()
        {
            if(game.GameFlowMgr.instance.CurEvent != null && game.GameFlowMgr.instance.CurCard != null)
            {
                if(game.GameFlowMgr.instance.CurEvent.config.ID == 28)
                {
                    game.GameFlowMgr.instance.DelectCardByUID(game.GameFlowMgr.instance.CurCard.UniqueId);
                    game.GameFlowMgr.instance.PushCardToTable(
                        game.GameFlowMgr.instance.CurEvent.conditionSpInsts[0][0].config.ID);
                    game.GameFlowMgr.instance.CurEvent.RoundNum = -1;
                    game.GameFlowMgr.instance.CurEvent = null;
                }
                else
                {
                    game.GameFlowMgr.instance.UseCard();
                }
            }
        }
    }
}

