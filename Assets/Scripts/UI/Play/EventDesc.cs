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
        [SerializeField] private Image Img_Event_Bg = null;
        [SerializeField] private UiStaticText SText_Event_Name = null;
        [SerializeField] private UiStaticText SText_Event_Round = null;


        [SerializeField] private UiStaticText SText_Name = null;
        [SerializeField] private UiStaticText SText_Com_Desc_Title = null;
        //[SerializeField] private UiStaticText SText_SP_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Succ_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Def_Desc_Title = null;
        [SerializeField] private UiStaticText SText_RoundOver_Desc_Title = null;
        [SerializeField] private UiStaticText SText_EventCard_Desc_Title = null;

        [SerializeField] private UiStaticText SText_Desc = null;
        [SerializeField] private UiStaticText SText_Com_Desc = null;
        //[SerializeField] private UiStaticText SText_SP_Desc = null;
        [SerializeField] private UiStaticText SText_Succ_Desc = null;
        [SerializeField] private UiStaticText SText_Def_Desc = null;
        [SerializeField] private UiStaticText SText_RoundOver_Desc = null;
        [SerializeField] private UiStaticText SText_EventCard_Desc = null;
        [SerializeField] private VerticalLayoutGroup verLayputGroup = null;
        [SerializeField] private Image[] Image_CardSlot = new Image[4];
        [SerializeField] private CanvasGroup canvasGroup;

        public game.EventInst curEventInst = null;
        public game.CardInst curCardInst = null;

        private Animator anim = null;
        private string SText_Desc_String = "";
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        private void OnDestroy()
        {
            
        }
        private void register()
        {
            evt.EventManager.instance.Register(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Enter_Slot, ShowEventCardDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Exit_Slot, HideEventCardDescTip);
            evt.EventManager.instance.Register<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change, ChangeCurCard);

            evt.EventManager.instance.Register(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_Over, ShowRoundOverDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, HideDesc);

            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);

            evt.EventManager.instance.Register(evt.EventGroup.TOUZI, (short)evt.TouziEvent.T_Roll, RollEvent);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, ShowDefDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, RollEventDef);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, ShowSuccDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, RollEventSucc);
        }

        private void unRegister()
        {
            evt.EventManager.instance.Unregister(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Enter_Slot, ShowEventCardDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Exit_Slot, HideEventCardDescTip);
            evt.EventManager.instance.Unregister<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change, ChangeCurCard);

            evt.EventManager.instance.Unregister(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_Over, ShowRoundOverDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, HideDesc);

            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);

            evt.EventManager.instance.Unregister(evt.EventGroup.TOUZI, (short)evt.TouziEvent.T_Roll, RollEvent);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, ShowDefDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, RollEventDef);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, ShowSuccDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, RollEventSucc);
        }

        public void ShowDesc(game.EventInst inst)
        {
            register();
            gameObject.SetActive(true);
            canvasGroup.interactable = false;
            canvasGroup.DOFade(1, 0.5f).OnComplete(() => { canvasGroup.interactable = true; canvasGroup.blocksRaycasts = true; });
            curCardInst = null;
            curEventInst = inst;
            game.GameFlowMgr.instance.CurEvent = inst;
            game.GameFlowMgr.instance.eventDesc = true;
            
            HideAll();
            SText_Com_Desc_Title.gameObject.SetActive(true);
            SText_Com_Desc.gameObject.SetActive(true);
            SText_Desc_String = "";
            if(inst.config.condition_1 != null)
            {
                SText_Desc_String = localization.LocalizationManager.instance.GetLocalizedString(inst.config.desc_Common, "ui_system");
            }
            if(inst.config.specialCId != null)
            {
                SText_Desc_String += "/n" + localization.LocalizationManager.instance.GetLocalizedString(inst.config.desc_SP, "ui_system");
                

                //SText_SP_Desc.gameObject.SetActive(true);
                //SText_SP_Desc_Title.gameObject.SetActive(true);
            }
            SText_Event_Name.Localize(inst.config.name, "ui_system");
            SText_Event_Round.text = inst.RoundNum.ToString();
            SText_Name.gameObject.SetActive(true);
            SText_Desc.gameObject.SetActive(true);
            SText_Name.Localize(inst.config.name, "ui_system");
            SText_Desc.Localize(inst.config.desc, "ui_system");
            //SText_SP_Desc.Localize(inst.config.desc_SP, "ui_system");
            SText_Com_Desc.text = SText_Desc_String;
            SText_Succ_Desc.Localize(inst.config.desc_SuccResult, "ui_system");
            SText_Def_Desc.Localize(inst.config.desc_DefResult, "ui_system");
            SText_RoundOver_Desc.Localize(inst.config.desc_DefResult, "ui_system");

            int tempCardSlot = 0;
            Img_Event_Bg.sprite = UiManager.instance.GetSprite($"EventShowType4", "PlayCanvas");
            foreach(var conditions in inst.conditionInsts)
            {
                foreach(var item in conditions)
                {
                    if(item.config.ID == 1)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType{item.numVars[0]}", "PlayCanvas");
                        Img_Event_Bg.sprite = UiManager.instance.GetSprite($"EventShowType{item.numVars[0]}", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardType{item.numVars[0]}";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 3)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType{game.CardMgr.instance.GetCardDataByID((int)item.numVars[0]).type}", "PlayCanvas");
                        Img_Event_Bg.sprite = UiManager.instance.GetSprite($"EventShowType{game.CardMgr.instance.GetCardDataByID((int)item.numVars[0]).type}", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardType{item.numVars[0]}";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 2)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardType";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 16)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardTypeUnuse";
                        tempCardSlot++;
                    }
                }
            }
            Img_Bg.sprite = UiManager.instance.GetSprite($"determine_bg{tempCardSlot}", "PlayCanvas");
            Img_Bg.SetNativeSize();
            for(int i = 0; i < tempCardSlot; i++)
            {
                Image_CardSlot[i].transform.parent.gameObject.SetActive(true);
            }
            LayoutRefresh();
        }


        public void HideDesc()
        {
            canvasGroup.DOFade(0, 0.5f);
            canvasGroup.blocksRaycasts = false;
            game.GameFlowMgr.instance.eventDesc = false;
            game.GameFlowMgr.instance.CurEvent = null;
            evt.EventManager.instance.Send(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide);
        }

        private void ChangeCurCard(game.CardInst cardInst)
        {
            curCardInst = cardInst;
        }

        #region 刷新显示
        //隐藏所有UI
        public void HideAll()
        {
            SText_Com_Desc_Title.gameObject.SetActive(false);
            SText_Com_Desc.gameObject.SetActive(false);
            SText_Name.gameObject.SetActive(false);
            SText_EventCard_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc_Title.gameObject.SetActive(false);
            SText_RoundOver_Desc.gameObject.SetActive(false);
            SText_RoundOver_Desc_Title.gameObject.SetActive(false);
            //SText_SP_Desc_Title.gameObject.SetActive(false);
            SText_Succ_Desc_Title.gameObject.SetActive(false);
            SText_Def_Desc_Title.gameObject.SetActive(false);
            SText_Desc.gameObject.SetActive(false);
            //SText_SP_Desc.gameObject.SetActive(false);
            SText_Succ_Desc.gameObject.SetActive(false);
            SText_Def_Desc.gameObject.SetActive(false);
            for(int i = 0; i < Image_CardSlot.Length; i++)
            {
                Image_CardSlot[i].transform.parent.gameObject.SetActive(false);
            }
        }

        public void ShowSuccDescTip()
        {
            SText_Com_Desc_Title.gameObject.SetActive(false);
            SText_Com_Desc.gameObject.SetActive(false);
            //SText_SP_Desc_Title.gameObject.SetActive(false);
            //SText_SP_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc_Title.gameObject.SetActive(false);
            SText_Desc.Localize(curEventInst.config.desc_SuccResult, "ui_system");
            LayoutRefresh();
        }

        public void ShowDefDescTip()
        {
            SText_Desc.Localize(curEventInst.config.desc_DefResult, "ui_system");
            LayoutRefresh();
        }

        //事件roundover
        public void ShowRoundOverDescTip()
        {
            SText_RoundOver_Desc.gameObject.SetActive(true);
            SText_RoundOver_Desc_Title.gameObject.SetActive(true);
            LayoutRefresh();
        }

        //卡牌进入槽
        public void ShowEventCardDescTip()
        {
            SText_Com_Desc.gameObject.SetActive(true);
            SText_Com_Desc_Title.gameObject.SetActive(true);
            if(curCardInst != null && curCardInst.UniqueId != 0)
            {
                SText_Com_Desc.text = game.EventMgr.instance.GetEventCardDesc(
                    game.GameFlowMgr.instance.CurEvent.config.ID,
                    game.GameFlowMgr.instance.CurCard.config.type,
                    game.GameFlowMgr.instance.CurCard.config.ID
                    );
            }
            LayoutRefresh();
        }

        //卡牌退出槽
        public void HideEventCardDescTip()
        {
            SText_Com_Desc.text = SText_Desc_String;
            LayoutRefresh();
        }

        //排布
        private void LayoutRefresh()
        {
            for(int i = 0; i < verLayputGroup.transform.childCount; i++)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(verLayputGroup.transform.GetChild(i) as RectTransform);
            }
            verLayputGroup.SetLayoutVertical();
        }
        #endregion


        #region BTN
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
        #endregion


        #region 播放动画
        public void RollEvent()
        {
            anim.Play("Rotate");
        }
        public void RollEventSucc()
        {
            anim.Play("EventSucc");
        }
        public void RollEventDef()
        {
            anim.Play("EventDef");
        }
        #endregion

    }
}

