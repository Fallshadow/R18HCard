using act.data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace act.game
{
    public class CardInst
    {
        public CardData config = null;
        public List<List<ConditionInst>> conditionInsts = new List<List<ConditionInst>>();
        public List<List<EffectInst>> effectInsts = new List<List<EffectInst>>();
        public bool Canuse
        {
            get
            {
                return canUse;
            }
            set
            {
                canUse = value;
            }
        }
        private bool canUse = false;
        public CardInst(CardData cardData)
        {
            config = cardData;
            canUse = true;
            if (config.condition_var_1 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_1, config.condition_var_1)); }
            if (config.condition_var_2 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_2, config.condition_var_2)); }

            if (config.effect_var_1 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_1, config.effect_var_1)); }
            if (config.effect_var_2 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_2, config.effect_var_2)); }
        }


        public bool ExcuteCheck()
        {
            if (config.testNumber < GameFlowMgr.instance.CreatRandomNum())//TODO:随机数生成,全局加持！！
            {
                return false;
            }
            return true;
        }
        public bool CheckAndExcuteByBlend()
        {
            return GameFlowMgr.instance.CheckCardOnEventByBlend(conditionInsts, effectInsts);
        }

        public void RefreshUse()
        {
            canUse = true;
        }
    }
}