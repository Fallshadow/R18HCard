using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace act.ui
{
    public class EventDesc : MonoBehaviour
    {
        [SerializeField] private UiStaticText SText_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Com_Desc_Title = null;
        [SerializeField] private UiStaticText SText_SP_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Succ_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Def_Desc_Title = null;
        [SerializeField] private UiStaticText SText_Desc = null;
        [SerializeField] private UiStaticText SText_Com_Desc = null;
        [SerializeField] private UiStaticText SText_SP_Desc = null;
        [SerializeField] private UiStaticText SText_Succ_Desc = null;
        [SerializeField] private UiStaticText SText_Def_Desc = null;
        [SerializeField] private VerticalLayoutGroup verLayputGroup = null;
        [SerializeField] private Image[] Image_CardSlot = new Image[4];
        [SerializeField] private CanvasGroup canvasGroup;
        private void Awake()
        {
            
        }
        private void OnDestroy()
        {
            
        }

        private void LayoutRefresh()
        {
            SText_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            SText_Com_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            SText_SP_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            SText_Succ_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            SText_Def_Desc.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            verLayputGroup.enabled = false;
            Invoke("InvokeLayout", 0.2f);
    }
    private void InvokeLayout()
    {
        verLayputGroup.enabled = true;
    }
    public void ShowDesc(game.EventInst inst)
        {
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over, HideDesc);
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
            SText_Desc.Localize(inst.config.desc, "ui_system");
            SText_SP_Desc.Localize(inst.config.desc_SP, "ui_system");
            SText_Com_Desc.Localize(inst.config.desc_Common, "ui_system");
            SText_Succ_Desc.Localize(inst.config.desc_SuccResult, "ui_system");
            SText_Def_Desc.Localize(inst.config.desc_DefResult, "ui_system");
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
            for(int i = 0; i < tempCardSlot; i++)
            {
                Image_CardSlot[i].gameObject.SetActive(true);
            }
            LayoutRefresh();
        }

        public void HideDesc()
        {
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

