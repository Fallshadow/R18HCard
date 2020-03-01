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
        public bool hasComplete = false;
        public EventInst(EventData eventData)
        {
            config = eventData;

            if (config.specialCId != null) { conditionSpInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.specialCId, config.specialCVar)); }
            if (config.specialEId != null) { effectSpInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.specialEId, config.specialEVar)); }

            if (config.condition_var_1 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_1, config.condition_var_1)); }
            if (config.condition_var_2 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_2, config.condition_var_2)); }
            if (config.effect_var_1 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_1, config.effect_var_1)); }
            if (config.effect_var_2 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_2, config.effect_var_2)); }
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
            hasComplete = true;
        }


    }
}
