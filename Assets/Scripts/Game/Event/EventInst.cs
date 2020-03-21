using act.data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace act.game
{
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
                roundNum = value;
                if (roundNum == -1)
                {
                    DestorySelf();
                }
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE);
            }
        }
        private int roundNum = 0;
        public bool hasComplete = false;
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
            for (int i = 0; i < conditionSpInsts.Count; i++)
            {
                ConditionEffectConfig conditionEffectConfig = new ConditionEffectConfig(UniqueId, conditionSpInsts[i], effectSpInsts[i]);
                GameFlowCdtAndEft.instance.AddCECToList(conditionEffectConfig);
                Debug.Log($"{UniqueId}事件入场：  特殊CEC：{i}");
            }
            GameFlowMgr.instance.eventInsts.Add(this);
        }

        public void DestorySelf()
        {
            GameFlowMgr.instance.eventInsts.Remove(this);
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
            if (GameFlowMgr.instance.cardSuccEventComp)
            {
                hasComplete = true;
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed);
            }
            else
            {
                GameFlowMgr.instance.cardSuccEventComp = true;
            }

        }


    }
}
