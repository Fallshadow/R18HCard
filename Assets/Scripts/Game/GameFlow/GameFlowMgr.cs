using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    public class GameFlowMgr : Singleton<GameFlowMgr>
    {
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
        public List<EventInst> eventInsts = new List<EventInst>();
        public List<CardInst> cardInsts = new List<CardInst>();
        public int RandomNum = 0;
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
        private int roundNum = 0;
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
            evt.EventManager.instance.Register<CardInst, EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Card_Event_Interact, UseCardOnEvent);
        }
        private void unregister()
        {
            evt.EventManager.instance.Unregister<CardInst, EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Card_Event_Interact, UseCardOnEvent);
        }
        public void UseCard()
        {
            if (curEvent == null || curCard == null)
            {
                return;
            }
            UseCardOnEvent(curCard, curEvent);
        }

        public void UseCardOnEvent(CardInst cardInst, EventInst eventInst)
        {
            List<bool> commonResults = new List<bool>();
            if (!ConditionMgr.instance.CheckConditionBySplit(eventInst.conditionInsts, out commonResults))
            {
                //TODO:发信号！卡牌根本使用不了
                Debug.Log("卡牌根本使用不了");
                return;
            }
            if (!cardInst.ExcuteCheck())
            {
                //TODO:发信号！卡牌使用失败了
                Debug.Log("卡牌使用失败了");
                eventInst.CheckAndExcuteSPByBlend();
            }
            else
            {
                eventInst.CheckAndExcuteSPByBlend();
                eventInst.ExcuteResult(commonResults);
                cardInst.CheckAndExcuteByBlend();
                cardInst.Canuse = false;
            }
        }


        public bool CheckCardOnEvent()
        {
            return curEvent.CheckCardOnEventBySplit(); ;
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

        public int CreatRandomNum()
        {
            RandomNum = UnityEngine.Random.Range(0, 7);
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change);
            return RandomNum;
        }
        public void StartRound()
        {

        }

        public void EndRound()
        {
            foreach (var item in eventInsts)
            {
                item.config.rountNum--;
            }
            foreach (var item in cardInsts)
            {
                item.RefreshUse();
            }
            CheckProcessCondition();
            RoundNum++;
        }
        public void ResetRound()
        {
            RoundNum = 0;
        }

        public void CheckProcessCondition()
        {
            foreach (var item in ProcessMgr.instance.processInsts)
            {
                if (item.conditionInst.Excute())
                {
                    foreach (var eventid in item.eventIds)
                    {
                        PushEventToTable(eventid);
                    }
                }
            }
        }
    }
}

