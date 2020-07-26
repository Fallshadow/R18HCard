using act.data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace act.game
{
    [Serializable]
    public class EventInst
    {
        public EventData config = null;
        public List<List<ConditionInst>> conditionInsts = new List<List<ConditionInst>>();
        public List<List<EffectInst>> effectInsts = new List<List<EffectInst>>();
        public List<List<ConditionInst>> conditionSpInsts = new List<List<ConditionInst>>();
        public List<List<EffectInst>> effectSpInsts = new List<List<EffectInst>>();


        public int RoundNum
        {
            get
            {
                return roundNum;
            }
            set
            {
                if(roundNum == -2)
                {
                    return;
                }
                roundNum = value;
                if (roundNum == 0)
                {
                    HasRoundNum0 = true;
                    GameFlowCdtAndEft.instance.RemoveCECToListByID(UniqueId);
                    evt.EventManager.instance.Send(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_Over);
                }
                if(roundNum == -1)
                {
                    DestorySelf();
                }
                evt.EventManager.instance.Send<int>(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_CHANGE,UniqueId);
            }
        }
        private int roundNum = 0;
        public bool HasComplete
        {
            get
            {
                return hasComplete;
            }
            set
            {
                hasComplete = value;
                //并不是一完成就要发送当前完成信号
                //if(hasComplete)
                //{
                //    evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed);
                //}
            }
        }
        private bool hasComplete = false;

        public bool HasCompleteWaitRoundOver
        {
            get
            {
                return hasCompleteWaitRoundOver;
            }
            set
            {
                hasCompleteWaitRoundOver = value;
            }
        }
        private bool hasCompleteWaitRoundOver = false;

        //生命为零但是不会消失，要
        public bool HasRoundNum0
        {
            get
            {
                return hasRoundNum0;
            }
            set
            {
                hasRoundNum0 = value;
                if(hasRoundNum0)
                {
                    evt.EventManager.instance.Send(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM0);
                }
            }
        }
        private bool hasRoundNum0 = false;

        public int UniqueId = 0;
        public EventInst(EventData eventData)
        {
            config = eventData;
            roundNum = eventData.rountNum;
            UniqueId = CECMgr.instance.GetUniqueId(eventData.ID, 2);
            if (config.specialCId != null) { conditionSpInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.specialCId, config.specialCVar)); }
            if (config.specialEId != null) { effectSpInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.specialEId, config.specialEVar)); }

            if (config.condition_var_1 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_1, config.condition_var_1)); }
            if (config.condition_var_2 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_2, config.condition_var_2)); }
            if (config.effect_var_1 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_1, config.effect_var_1)); }
            if (config.effect_var_2 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_2, config.effect_var_2)); }

            Debug.Log($"Event {config.ID}: Success!");
        }
        public void EnterToTable()
        {
            Debug.Log($"事件入场：  独有ID：{UniqueId}");
            if(conditionSpInsts.Count != 0)
            {
                for(int i = 0; i < conditionSpInsts[0].Count; i++)
                {
                    List<ConditionInst> tempCInstList = new List<ConditionInst>();
                    List<EffectInst> tempEInstList = new List<EffectInst>();
                    tempCInstList.Add(conditionSpInsts[0][i]);
                    tempEInstList.Add(effectSpInsts[0][i]);
                    ConditionEffectConfig conditionEffectConfig = new ConditionEffectConfig(UniqueId, tempCInstList, tempEInstList);
                    GameFlowCdtAndEft.instance.AddCECToList(conditionEffectConfig);
                }
            }
            GameFlowMgr.instance.eventInsts.Add(this);
        }

        public void DestorySelf()
        {
            GameFlowMgr.instance.eventInsts.Remove(this);
            GameFlowCdtAndEft.instance.RemoveCECToListByID(UniqueId);
        }
        public bool CheckAndExcuteSPByBlend()
        {
            return GameFlowMgr.instance.CheckCardOnEventByBlend(conditionSpInsts, effectSpInsts);
        }

        public bool CheckCardOnEventBySplit()
        {
            return ConditionMgr.instance.CheckConditionBySplit(conditionInsts);
        }

        public void ExcuteResult(List<bool> results)
        {
            EffectMgr.instance.ExcuteResult(effectInsts, results);
            game.GameFlowMgr.instance.CurEvent = null;
        }

        public void CheckHasComplete()
        {
            if(GameFlowMgr.instance.cardSuccEventComp)
            {
                HasComplete = true;
            }
            else
            {
                GameFlowMgr.instance.cardSuccEventComp = true;
            }
        }


    }
}
