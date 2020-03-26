using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    public class GameFlowMgr : Singleton<GameFlowMgr>
    {
        //event 展示
        public bool eventDesc = false;
        //卡牌成功即解决事件？
        public bool cardSuccEventComp = true;

        #region 当前使用的卡牌与事件
        public CardInst CurCard
        {
            get
            {
                return curCard;
            }
            set
            {
                curCard = value;
            }
        }
        private CardInst curCard;
        public EventInst CurEvent
        {
            get
            {
                return curEvent;
            }
            set
            {
                curEvent = value;
            }
        }
        private EventInst curEvent = null;
        #endregion

        #region 回合数、进度值、HP
        public int RoundNum
        {
            get
            {
                return roundNum;
            }
            set
            {
                roundNum = value;
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RoundNum_Change);
            }
        }
        private int roundNum = 1;
        public float Process
        {
            get
            {
                return process;
            }
            set
            {
                process = Mathf.Clamp(value, 0, 100);
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_ProcessNum_Change);
            }
        }
        private float process = 0;

        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_HpNum_Change);
            }
        }
        private int hp = 0;
        #endregion
        public List<bool> curEventResults = new List<bool>();
        public List<EventInst> eventInsts = new List<EventInst>();
        public List<EventInst> hadSolveEventInsts = new List<EventInst>();
        public List<CardInst> cardInsts = new List<CardInst>();
        public List<CardInst> hadUsecardInsts = new List<CardInst>();
        public float RandomNum = 0;

        public bool JumpUpTouzi
        {
            get
            {
                return jumpUpTouzi;
            }
            set
            {
                jumpUpTouzi = value;
            }
        }
        private bool jumpUpTouzi;
        public bool canOverRound = false;
        data.SaveData saveData = null;
        private const string SAVE_FILE_NAME = "SaveData";

        public void InitConfigData()
        {
            register();
        }
        public void Release()
        {
            unregister();
        }
        private void register()
        {
        }
        private void unregister()
        {

        }
        public void RecordCurCard()
        {
            hadUsecardInsts.Add(curCard);
        }
        public void UseCard()
        {
            if (curEvent == null || curCard == null)
            {
                return;
            }
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameFlowCardCheck);
        }
        public void DelectEventByID(int ID)
        {
            for(int i = 0; i < eventInsts.Count; i++)
            {
                if(eventInsts[i].config.ID == ID)
                {
                    eventInsts[i].RoundNum = -1;
                    i--;
                }
            }
        }
        public void DelectCardByID(int ID)
        {
            for(int i = 0; i < cardInsts.Count; i++)
            {
                if(cardInsts[i].config.ID == ID)
                {
                    cardInsts[i].DestorySelf();
                    i--;
                }
            }
        }
        public void DelectCardByUID(int uID)
        {
            for(int i = 0; i < cardInsts.Count; i++)
            {
                if(cardInsts[i].UniqueId == uID)
                {
                    cardInsts[i].DestorySelf();
                    return;
                }
            }
        }
        //执行卡牌事件检测（且：必须都满足）
        public bool CheckCardOnEventByBlend(List<List<ConditionInst>> conditionInsts, List<List<EffectInst>> effectInsts)
        {
            List<bool> results = new List<bool>();
            if (ConditionMgr.instance.CheckConditionByBlend(conditionInsts, out results))
            {
                EffectMgr.instance.ExcuteResult(effectInsts, results);
                return true;
            }
            return false;
        }

        //执行卡牌事件检测（或：只要有一个就行）
        public bool CheckAndExcuteBySplit(List<List<ConditionInst>> conditionInsts, List<List<EffectInst>> effectInsts)
        {
            List<bool> results = new List<bool>();
            if (ConditionMgr.instance.CheckConditionBySplit(conditionInsts, out results))
            {
                EffectMgr.instance.ExcuteResult(effectInsts, results);
                return true;
            }
            return false;
        }
        #region 推送卡牌和事件
        public void PushEventToTable(int eventID)
        {
            EventInst inst = game.EventMgr.instance.GetEventInstByID(eventID);
            PushEventToTable(inst);
        }
        public void PushCardToTable(int cardId)
        {
            CardInst inst = game.CardMgr.instance.GetCardInstByID(cardId);
            PushCardToTable(inst);
        }
        public void PushEventToTable(EventInst inst)
        {
            evt.EventManager.instance.Send<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, inst);
        }
        public void PushCardToTable(CardInst inst)
        {
            evt.EventManager.instance.Send<game.CardInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Create, inst);
        }
        #endregion
        public int GetGrayCard()
        {
            int tempInt = 0;
            foreach (CardInst item in cardInsts)
            {
                if (!item.Canuse)
                {
                    tempInt++;
                }
            }
            return tempInt;
        }
        //public float CreatRandomNum()
        //{
        //    RandomNum = ;
        //    evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change);
        //    return RandomNum;
        //}

        public void ResetData()
        {
            RoundNum = 0;
            Hp = 0;
            Process = 0;
            CurCard = null;
            CurEvent = null;
            curEventResults.Clear();
            eventInsts.Clear();
            cardInsts.Clear();
            hadUsecardInsts.Clear();
            hadSolveEventInsts.Clear();
        }
        public void SaveData()
        {
            saveData = new data.SaveData();
            saveData.curCard                 = CurCard;
            saveData.curEvent                = CurEvent;
            saveData.curEventResults         = curEventResults;
            saveData.eventInsts              = eventInsts;
            saveData.cardInsts               = cardInsts;
            saveData.hadUsecardInsts         = hadUsecardInsts;
            saveData.hadSolvecardInsts       = hadSolveEventInsts;
            saveData.RoundNum = RoundNum;
            saveData.HP = Hp;

            data.DataArchiver.Save(saveData, SAVE_FILE_NAME);
        }

        public void LoadData()
        {
            ResetData();
            saveData = data.DataArchiver.Load<data.SaveData>(SAVE_FILE_NAME);
            if(saveData == null)
            {
                return;
            }
            CurCard = saveData.curCard;
            CurEvent = saveData.curEvent;
            curEventResults = saveData.curEventResults;
            eventInsts = saveData.eventInsts;
            cardInsts = saveData.cardInsts;
            hadUsecardInsts = saveData.hadUsecardInsts;
            hadSolveEventInsts = saveData.hadSolvecardInsts;
            RoundNum = saveData.RoundNum;
            Hp = saveData.HP;
            ReShowData();
        }

        public void ClearData()
        {
            saveData = new data.SaveData();
            data.DataArchiver.Save(saveData, SAVE_FILE_NAME);
        }
        private void ReShowData()
        {
            if(eventInsts != null)
            {
                List<EventInst> tempEventList = new List<EventInst>();
                foreach(var item in eventInsts)
                {
                    tempEventList.Add(item);
                }
                eventInsts.Clear();
                foreach(var item in tempEventList)
                {
                    PushEventToTable(item);
                }
            }
            if(cardInsts != null)
            {
                List<CardInst> tempCardList = new List<CardInst>();
                foreach(var item in cardInsts)
                {
                    tempCardList.Add(item);
                }
                cardInsts.Clear();
                foreach(var item in tempCardList)
                {
                    PushCardToTable(item);
                }
            }
        }
        #region 这个是应付各种检测方法
        public bool IsCurEventContain(int id)
        {
            foreach (var item in eventInsts)
            {
                if (item.config.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
    [System.Serializable]
    public class GameFlowCdtAndEft : Singleton<GameFlowCdtAndEft>
    {
        public List<ConditionEffectConfig> curTotalCEC = new List<ConditionEffectConfig>();

        public List<ConditionEffectConfig> RoundStartCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardCheckCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckStartCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckOverCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckSuccCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckDeffCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardUseOverCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> RoundEndCEC = new List<ConditionEffectConfig>();

        public void CheckCdt(List<ConditionEffectConfig> cdtList)
        {
            ConditionEffectConfig tempCEC;
            for (int i = cdtList.Count - 1; i >= 0; i--)
            {
                tempCEC = cdtList[i];
                bool tempBool = tempCEC.CECheckByBlend();
                Debug.Log($"当前时点：{System.Enum.GetName(typeof(TimePoint), tempCEC.timePoint)};当前ID：{tempCEC.id}当前结果：{tempBool}");
            }
        }

        public void AddCECToList(ConditionEffectConfig CEC)
        {
            curTotalCEC.Add(CEC);
            switch (CEC.timePoint)
            {
                case TimePoint.TP_None:
                    break;
                case TimePoint.TP_RoundStart:
                    RoundStartCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardCheck:
                    CardCheckCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckStart:
                    CardNumCheckStartCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckOver:
                    CardNumCheckOverCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckSucc:
                    CardNumCheckSuccCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckDeff:
                    CardNumCheckDeffCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardUseOver:
                    CardUseOverCEC.Add(CEC);
                    break;
                case TimePoint.TP_RoundEnd:
                    RoundEndCEC.Add(CEC);
                    break;
                default:
                    break;
            }
            Debug.Log($"欢迎该CEC进入流程 ：时点{CEC.timePoint}," +
                $"条件1ID{(CEC.conditionInsts[0] != null ? (CEC.conditionInsts[0].config.ID.ToString() + $"说明：{localization.LocalizationManager.instance.GetLocalizedString(CEC.conditionInsts[0].config.desc, "ui_system")}") : "无")}," +
                $"效果1ID{(CEC.effectInsts[0] != null ? (CEC.effectInsts[0].config.ID.ToString() + $"说明：{localization.LocalizationManager.instance.GetLocalizedString(CEC.effectInsts[0].config.desc, "ui_system")}"): "无")} ");
        }
        public void RemoveCECToList(ConditionEffectConfig CEC)
        {
            curTotalCEC.Remove(CEC);
            switch (CEC.timePoint)
            {
                case TimePoint.TP_None:
                    break;
                case TimePoint.TP_RoundStart:
                    RoundStartCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardCheck:
                    CardCheckCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckStart:
                    CardNumCheckStartCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckOver:
                    CardNumCheckOverCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckSucc:
                    CardNumCheckSuccCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckDeff:
                    CardNumCheckDeffCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardUseOver:
                    CardUseOverCEC.Remove(CEC);
                    break;
                case TimePoint.TP_RoundEnd:
                    RoundEndCEC.Remove(CEC);
                    break;
                default:
                    break;
            }
        }
    }
}

