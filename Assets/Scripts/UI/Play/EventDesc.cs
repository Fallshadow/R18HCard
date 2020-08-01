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
        [SerializeField] private GameObject[] ImageLight_CardSlot = new GameObject[5];
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private CanvasGroup BGcanvasGroup;//desc 界面部分
        [SerializeField] private Button CheckBtn = null;

        public game.EventInst curEventInst = null;
        public game.CardInst curCardInst = null;

        private bool isShow = false;
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
            unRegister();
            evt.EventManager.instance.Register<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_Desc_Card_Slot_Exit, HideCardSlotLight);
            evt.EventManager.instance.Register<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_Desc_Card_Slot_Enter, ShowCardSlotLight);
            evt.EventManager.instance.Register(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Enter_Slot, ShowEventCardDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Exit_Slot, HideEventCardDescTip);
            evt.EventManager.instance.Register<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change, ChangeCurCard);

            evt.EventManager.instance.Register(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_Over, ShowRoundOverDescTip);
            //evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, HideDesc);

            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);

            evt.EventManager.instance.Register(evt.EventGroup.TOUZI, (short)evt.TouziEvent.T_Roll, RollEvent);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, ShowDefDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def_Anim, RollEventDef);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, ShowSuccDescTip);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success_Anim, RollEventSucc);

            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Limit_TwoOne, showEventID38);
            evt.EventManager.instance.Register<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_CHANGE, refreshRound);

        }

        private void unRegister()
        {
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_Desc_Card_Slot_Exit, HideCardSlotLight);
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_Desc_Card_Slot_Enter,ShowCardSlotLight);
            evt.EventManager.instance.Unregister(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Enter_Slot, ShowEventCardDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Exit_Slot, HideEventCardDescTip);
            evt.EventManager.instance.Unregister<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change, ChangeCurCard);

            evt.EventManager.instance.Unregister(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_Over, ShowRoundOverDescTip);
            //evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, HideDesc);

            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);

            evt.EventManager.instance.Unregister(evt.EventGroup.TOUZI, (short)evt.TouziEvent.T_Roll, RollEvent);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def, ShowDefDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def_Anim, RollEventDef);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success, ShowSuccDescTip);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success_Anim, RollEventSucc);

            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Limit_TwoOne, showEventID38);
            evt.EventManager.instance.Unregister<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_CHANGE, refreshRound);

        }

        private void refreshRound(int id)
        {
            if(curEventInst.UniqueId == id)
            {
                //TODO:做一个跳动动画
                SText_Event_Round.text = curEventInst.RoundNum.ToString();
            }
        }
        private void Update()
        {
            if(game.GameFlowMgr.instance.CurEvent == null && isShow)
            {
                game.GameFlowMgr.instance.CurEvent = curEventInst;
            }
        }
        public void ShowDesc(game.EventInst inst)
        {
            isShow = true;
            if(anim == null)
            {
                anim = GetComponent<Animator>();
            }
            anim.enabled = false;

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
            if(inst.RoundNum == -2)
            {
                SText_Event_Round.text = "无限";
                showEventID38();
            }

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
                        if(item.numVars[0] == 0)
                        {
                            continue;
                        }
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 3)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType{game.CardMgr.instance.GetCardDataByID((int)item.numVars[0]).type}", "PlayCanvas");
                        Img_Event_Bg.sprite = UiManager.instance.GetSprite($"EventShowType{game.CardMgr.instance.GetCardDataByID((int)item.numVars[0]).type}", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardTypeID{item.numVars[0]}";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 22)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType", "PlayCanvas");
                        Img_Event_Bg.sprite = UiManager.instance.GetSprite($"EventShowTypeAll", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardType";
                        tempCardSlot++;
                    }
                    else if(item.config.ID == 16)
                    {
                        Image_CardSlot[tempCardSlot].sprite = UiManager.instance.GetSprite($"EventType", "PlayCanvas");
                        Image_CardSlot[tempCardSlot].transform.parent.gameObject.name = $"CardTypeUnuse";
                        tempCardSlot++;
                    }
                    Image_CardSlot[tempCardSlot].gameObject.SetActive(true);
                }
            }
            Img_Bg.sprite = UiManager.instance.GetSprite($"determine_bg{tempCardSlot}", "PlayCanvas");
            CheckBtn.gameObject.SetActive(tempCardSlot != 0);
            Img_Bg.SetNativeSize();
            for(int i = 0; i < tempCardSlot; i++)
            {
                Image_CardSlot[i].transform.parent.gameObject.SetActive(true);
            }
            BGcanvasGroup.blocksRaycasts = true;
            if(curEventInst.HasRoundNum0)
            {
                ShowRoundOverDescTip();
            }
            
            (verLayputGroup.transform as RectTransform).anchorMin = new Vector2(0, 1);
            (verLayputGroup.transform as RectTransform).anchorMax = new Vector2(0, 1);
            LayoutRebuilder.ForceRebuildLayoutImmediate(verLayputGroup.transform as RectTransform);
            (verLayputGroup.transform as RectTransform).anchoredPosition = new Vector3(204, -202, 0);
            if(curEventInst.HasComplete)
            {
                ShowSuccDescTip();
                Img_Event_Bg.sprite = UiManager.instance.GetSprite($"card_sj_success", "PlayCanvas");
                foreach(var item in Image_CardSlot)
                {
                    item.transform.parent.gameObject.SetActive(false);
                }
                CheckBtn.gameObject.SetActive(false);
                (verLayputGroup.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
                (verLayputGroup.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
                LayoutRebuilder.ForceRebuildLayoutImmediate(verLayputGroup.transform as RectTransform);
                (verLayputGroup.transform as RectTransform).anchoredPosition = Vector3.zero;
            }
            LayoutRefresh();
        }

        private void showEventID38()
        {
            if(curEventInst.config.ID == 38)
            {
                SText_Event_Round.text = game.GameFlowMgr.instance.TwoOneNum.ToString();
            }
        }
        public void HideDesc()
        {
            isShow = false;
            InitAnim();
            canvasGroup.DOFade(0, 0.5f);
            canvasGroup.blocksRaycasts = false;
            game.GameFlowMgr.instance.eventDesc = false;
            if(game.GameFlowMgr.instance.CurEvent != null && !game.GameFlowMgr.instance.CurEvent.HasComplete)
            {
                game.GameFlowMgr.instance.CurEvent = null;
            }
            evt.EventManager.instance.Send(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide);
        }

        private void ChangeCurCard(game.CardInst cardInst)
        {
            curCardInst = cardInst;
        }

        #region 刷新显示
        //鼠标移动到对应槽位时发光
        private void ShowCardSlotLight(int index)
        {
            switch(Image_CardSlot[index].transform.parent.gameObject.name)
            {
                case "CardType1":
                    MoveAndSetActiveCardSlotLight(index,0);
                    break;
                case "CardType2":
                    MoveAndSetActiveCardSlotLight(index,1);
                    break;
                case "CardType3":
                    MoveAndSetActiveCardSlotLight(index,2);
                    break;
                case "CardType4":
                    MoveAndSetActiveCardSlotLight(index,3);
                    break;
                case "CardType":
                    MoveAndSetActiveCardSlotLight(index,4);
                    break;
                default:
                    Debug.Log("虽然进入了卡槽范围但是并没有对应该名字的背景光");
                    break;
            }
        }

        //将对应序号卡牌框光移动出对应槽位下并显隐藏
        private void MoveAndSetFalseCardSlotLight(int cardSlotIndex, int lightIndex)
        {
            ImageLight_CardSlot[lightIndex].SetActive(false);
        }
        //鼠标移动出对应槽位时暗淡
        private void HideCardSlotLight(int index)
        {
            switch(Image_CardSlot[index].transform.parent.gameObject.name)
            {
                case "CardType1":
                    MoveAndSetFalseCardSlotLight(index, 0);
                    break;
                case "CardType2":
                    MoveAndSetFalseCardSlotLight(index, 1);
                    break;
                case "CardType3":
                    MoveAndSetFalseCardSlotLight(index, 2);
                    break;
                case "CardType4":
                    MoveAndSetFalseCardSlotLight(index, 3);
                    break;
                case "CardType":
                    MoveAndSetFalseCardSlotLight(index, 4);
                    break;
                default:
                    Debug.Log("虽然进入了卡槽范围但是并没有对应该名字的背景光");
                    break;
            }
        }

        //将对应序号卡牌框光移动到对应槽位下并显示
        private void MoveAndSetActiveCardSlotLight(int cardSlotIndex, int lightIndex)
        {
            ImageLight_CardSlot[lightIndex].SetActive(true);
            ImageLight_CardSlot[lightIndex].transform.SetParent(Image_CardSlot[cardSlotIndex].transform.parent);
            ImageLight_CardSlot[lightIndex].transform.position = Image_CardSlot[cardSlotIndex].transform.position;
            ImageLight_CardSlot[lightIndex].transform.SetAsFirstSibling();
        }

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
            CheckComp();
        }

        private void CheckComp()
        {
            if(curEventInst.HasComplete)
            {
                CheckBtn.gameObject.SetActive(false);
            }
        }
        public void ShowDefDescTip()
        {
            SText_Com_Desc.gameObject.SetActive(true);
            SText_Com_Desc_Title.gameObject.SetActive(true);
            SText_Com_Desc.Localize(curEventInst.config.desc_DefResult, "ui_system");
            LayoutRefresh();
            CheckComp();
        }

        //事件roundover
        public void ShowRoundOverDescTip()
        {
            BGcanvasGroup.blocksRaycasts = false;
            SText_Com_Desc_Title.gameObject.SetActive(false);
            SText_Com_Desc.gameObject.SetActive(false);
            //SText_SP_Desc_Title.gameObject.SetActive(false);
            //SText_SP_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc.gameObject.SetActive(false);
            SText_EventCard_Desc_Title.gameObject.SetActive(false);
            SText_Desc.Localize(curEventInst.config.desc_DefResult, "ui_system");
            Img_Event_Bg.sprite = UiManager.instance.GetSprite($"card_sj_fail", "PlayCanvas");
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

            if(game.GameController.instance.isInNewPlayFlow)
            {
                act.game.TimeLineMgr.instance.ResumeTimeLine(act.game.TimeLineMgr.instance.newPlayerDir);
            }

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
            AudioMgr.instance.PlaySound(AudioClips.AC_PlayClick);
            if(game.GameController.instance.isInNewPlayFlow)
            {
                act.game.TimeLineMgr.instance.ResumeTimeLine(act.game.TimeLineMgr.instance.newPlayerDir);
            }
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
            anim.enabled = true;
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.HideAll);
            anim.SetFloat("Init", 1);
            anim.Play("Rotate");
        }
        public void RollEventSucc()
        {
            anim.enabled = true;
            anim.SetFloat("Init", 1);
            anim.SetTrigger("Succ");
        }
        public void RollEventDef()
        {
            anim.enabled = true;
            anim.SetFloat("Init", 1);
            anim.SetTrigger("Def");
        }

        public void Succ()
        {
            //转移到事件消失之后
            game.GameFlowMgr.instance.CurCard.CheckCdt();
            //game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckSuccCEC);
            //game.GameFlowMgr.instance.CurEvent.ExcuteResult(game.GameFlowMgr.instance.curEventResults);
            game.GameFlowMgr.instance.CurEvent.CheckHasComplete();
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success);
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.DisHideAll);
            evt.EventManager.instance.Send(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Use_Over);
        }

        public void Def()
        {
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckDeffCEC);
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def);
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.DisHideAll);
        }

        public void InitAnim()
        {
            anim.SetFloat("Init",0);
        }
        #endregion

    }
}

