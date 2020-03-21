using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    public class GameFlowMgr : Singleton<GameFlowMgr>
    {
        public bool cardSuccEventComp = true;
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
        public List<bool> curEventResults = new List<bool>();

        public List<EventInst> eventInsts = new List<EventInst>();
        public List<CardInst> cardInsts = new List<CardInst>();
        public List<CardInst> hadUsecardInsts = new List<CardInst>();
        public List<CardInst> hadSolvecardInsts = new List<CardInst>();

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

        public float RandomNum = 0;
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
        public int Hp = 0;
        public bool canOverRound = false;
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
        public void PushEventToTable(int eventID)
        {
            EventInst inst = game.EventMgr.instance.GetEventInstByID(eventID);
            evt.EventManager.instance.Send<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, inst);
            eventInsts.Add(inst);
        }
        public void PushCardToTable(int cardId)
        {
            CardInst inst = game.CardMgr.instance.GetCardInstByID(cardId);
            evt.EventManager.instance.Send<game.CardInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Create, inst);
            cardInsts.Add(inst);
        }
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
        public float CreatRandomNum()
        {
            RandomNum = RandomNumMgr.instance.GetRandomNum();
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change);
            return RandomNum;
        }

        public void ResetRound()
        {
            RoundNum = 0;
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

