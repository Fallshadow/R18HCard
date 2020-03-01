using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace act.ui
{

    [BindingResource("Play/PlayCanvas")]
    public class PlayCanvas : InteractableUiBase
    {
        [SerializeField] private Transform cardParentGroupTran;
        [SerializeField] private Transform eventParentGroupTran;
        [SerializeField] private InputField tempCardId;
        [SerializeField] private InputField tempEventId;
        [SerializeField] private UiStaticText text_Touzi_Num;
        [SerializeField] private UiStaticText text_HP_Num;
        [SerializeField] private UiStaticText text_Process_Num;
        [SerializeField] private UiStaticText text_Round_Num;
        public override void Initialize()
        {
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change, ShowRandomNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_ProcessNum_Change, ShowProcessNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_HpNum_Change, ShowHPNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change, ShowRandomNum);
            evt.EventManager.instance.Register<game.CardInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Create, CreateCard);
            evt.EventManager.instance.Register<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, CreateEvent);
        }

        public override void Refresh()
        {

        }

        public override void Release()
        {
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change, ShowRandomNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_ProcessNum_Change, ShowProcessNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_HpNum_Change, ShowHPNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change, ShowRandomNum);
            evt.EventManager.instance.Unregister<game.CardInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Create, CreateCard);
            evt.EventManager.instance.Unregister<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, CreateEvent);
        }

        #region Btn
        public void EndRound()
        {
            game.GameFlowMgr.instance.EndRound();
        }
        public void ReturnToMain()
        {
            Hide();
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.MAINMENU);
        }

        public void CreateEvent(game.EventInst eventInst)
        {
            ui.EventDisplay eventDisplay = utility.LoadResources.LoadPrefab<ui.EventDisplay>(data.ResourcesPathSetting.UiPrefabFolder + data.ResourcesPathSetting.PlayUIEventPrefab,
                eventParentGroupTran);
            eventDisplay.Init();
            eventDisplay.SetInst(eventInst);
        }

        public void CreateCard(game.CardInst cardInst)
        {
            ui.CardDisplay cardDisplay = utility.LoadResources.LoadPrefab<ui.CardDisplay>(data.ResourcesPathSetting.UiPrefabFolder + data.ResourcesPathSetting.PlayUICardPrefab,
                cardParentGroupTran);
            cardDisplay.Init();
            cardDisplay.SetInst(cardInst);
        }
        public void CreateEvent()
        {
            int eventID = Convert.ToInt32(tempEventId.text);
            ui.EventDisplay eventDisplay = utility.LoadResources.LoadPrefab<ui.EventDisplay>(data.ResourcesPathSetting.UiPrefabFolder + data.ResourcesPathSetting.PlayUIEventPrefab,
                eventParentGroupTran);
            eventDisplay.Init();
            game.EventInst inst = game.EventMgr.instance.GetEventInstByID(eventID);
            eventDisplay.SetInst(inst);
            game.GameFlowMgr.instance.eventInsts.Add(inst);
        }

        public void CreateCard()
        {
            int cardId = Convert.ToInt32(tempCardId.text);
            ui.CardDisplay cardDisplay = utility.LoadResources.LoadPrefab<ui.CardDisplay>(data.ResourcesPathSetting.UiPrefabFolder + data.ResourcesPathSetting.PlayUICardPrefab,
                cardParentGroupTran);
            cardDisplay.Init();
            game.CardInst inst = game.CardMgr.instance.GetCardInstByID(cardId);
            cardDisplay.SetInst(inst);
            game.GameFlowMgr.instance.cardInsts.Add(inst);
        }

        public void ShowRandomNum()
        {
            text_Touzi_Num.text = game.GameFlowMgr.instance.RandomNum.ToString();
        }
        public void ShowProcessNum()
        {
            text_Process_Num.text = game.GameFlowMgr.instance.Process.ToString();
        }
        public void ShowHPNum()
        {
            text_HP_Num.text = game.GameFlowMgr.instance.Hp.ToString();
        }
        public void ShowRoundNum()
        {
            text_Round_Num.text = game.GameFlowMgr.instance.RoundNum.ToString();
        }
        #endregion
    }
}