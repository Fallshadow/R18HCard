using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace act.ui
{

    [BindingResource("Play/PlayCanvas")]
    public class PlayCanvas : InteractableUiBase
    {
        [Header("Setting")]
        [SerializeField] private float ProcessDuration = 1;

        [SerializeField] private CardGroup cardGroup;
        [SerializeField] private Transform cardParentGroupTran;
        [SerializeField] private Transform eventParentGroupTran;
        [SerializeField] private EventDesc eventDesc;

        [SerializeField] private UiStaticText text_Touzi_Num;
        [SerializeField] private Text text_HP_Num;
        [SerializeField] private Text text_Process_Num;
        [SerializeField] private Material material_Process_Num;
        [SerializeField] private UiStaticText text_Round_Num;
        [SerializeField] private Image HideAll;

        [Header("Debug")]
        [SerializeField] private InputField tempCardId;
        [SerializeField] private InputField tempEventId;
        private List<GameObject> eventOGS = new List<GameObject>();
        private float processNum = 0;
        public override void Initialize()
        {
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change, ShowRandomNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_ProcessNum_Change, ShowProcessNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_HpNum_Change, ShowHPNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RoundNum_Change, ShowRoundNum);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.HideAll,ShowHideAllImage);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.DisHideAll,HideHideAllImage);

            evt.EventManager.instance.Register<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Create, CreateCard);
            evt.EventManager.instance.Register<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, CreateEvent);

            evt.EventManager.instance.Register<game.EventInst>(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Click, ShowEventDesc);
        }
        public override void Release()
        {
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change, ShowRandomNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_ProcessNum_Change, ShowProcessNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_HpNum_Change, ShowHPNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RoundNum_Change, ShowRoundNum);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.HideAll, ShowHideAllImage);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.DisHideAll, HideHideAllImage);

            evt.EventManager.instance.Unregister<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Create, CreateCard);
            evt.EventManager.instance.Unregister<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, CreateEvent);

            evt.EventManager.instance.Unregister<game.EventInst>(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Click, ShowEventDesc);
        }
        protected override void onShow()
        {
            base.onShow();
            game.GameFlowMgr.instance.LoadData();
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameFlowRoundStart);
        }

        
        public void CreateEvent(game.EventInst eventInst)
        {
            GameObject eventDisplayOB = utility.LoadResources.LoadPrefab(
                data.ResourcesPathSetting.UiPrefabFolder
                + data.ResourcesPathSetting.PlayUIEventPrefabBase
                + eventInst.config.ID, true);
            if(eventDisplayOB == null)
            {
                eventDisplayOB = utility.LoadResources.LoadPrefab(
                    data.ResourcesPathSetting.UiPrefabFolder
                    + data.ResourcesPathSetting.PlayUIEventPrefabBase);
            }
            eventOGS.Add(eventDisplayOB);
            ui.EventDisplay eventDisplay = eventDisplayOB.GetComponentInChildren<ui.EventDisplay>();
            eventDisplay.Init();
            eventDisplay.SetInst(eventInst);
            eventDisplay.EnterToTable();
        }

        public void CreateCard(game.CardInst cardInst)
        {
            ui.CardDisplay cardDisplay = utility.LoadResources.LoadPrefab<ui.CardDisplay>(data.ResourcesPathSetting.UiPrefabFolder + data.ResourcesPathSetting.PlayUICardPrefab,
                cardParentGroupTran);
            cardDisplay.Init();
            cardDisplay.SetInst(cardInst);
            cardDisplay.EnterToTable();
            cardGroup.RefreshCardChildPos();
        }


        #region 刷新展示
        public void ShowEventDesc(game.EventInst inst)
        {
            eventDesc.ShowDesc(inst);
        }
        public void ShowRandomNum()
        {
            text_Touzi_Num.text = game.GameFlowMgr.instance.RandomNum.ToString();
            Debug.Log(game.GameFlowMgr.instance.RandomNum);
        }
        public void ShowProcessNum()
        {
            //TODO:数字缓动！
            text_Process_Num.text = game.GameFlowMgr.instance.Process.ToString();
            material_Process_Num.DOFloat(game.GameFlowMgr.instance.Process / 100, "_Progress", ProcessDuration);
        }
        public void ShowHPNum()
        {
            text_HP_Num.text = game.GameFlowMgr.instance.Hp.ToString();
        }
        public void ShowRoundNum()
        {
            text_Round_Num.text = game.GameFlowMgr.instance.RoundNum.ToString();
        }

        public void ResetShow()
        {
            for(int i = 0; i < eventParentGroupTran.childCount; i++)
            {
                Destroy(eventParentGroupTran.GetChild(i).gameObject);
            }
            for(int i = 0; i < cardParentGroupTran.childCount; i++)
            {
                Destroy(cardParentGroupTran.GetChild(i).gameObject);
            }
        }
        #endregion
        #region Btn
        public void EndRound()
        {
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameFlowRoundEnd);
        }
        public void ReturnToMain()
        {
            Hide();
            ResetShow();
            foreach(var item in eventOGS)
            {
                if(item != null)
                {
                    Destroy(item);
                }
            }
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.MAINMENU);
        }
       
        public void CreateEvent()
        {
            int eventID = Convert.ToInt32(tempEventId.text);
            game.EventInst inst = game.EventMgr.instance.GetEventInstByID(eventID);
            CreateEvent(inst);
        }

        public void CreateCard()
        {
            int cardId = Convert.ToInt32(tempCardId.text);
            game.CardInst inst = game.CardMgr.instance.GetCardInstByID(cardId);
            CreateCard(inst);
        }


        #endregion
        public override void Refresh()
        {

        }

        public void ShowHideAllImage()
        {
            HideAll.enabled = true;
        }
        public void HideHideAllImage()
        {
            HideAll.enabled = false;
        }
    }
}