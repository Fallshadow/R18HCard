using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    [System.Serializable]
    public class ConditionEffectConfig
    {
        public int id = 0;
        public List<ConditionInst> conditionInsts = new List<ConditionInst>();
        public List<EffectInst> effectInsts = new List<EffectInst>();
        public TimePoint timePoint = TimePoint.TP_None;
        public int times = 0;
        public EntityType self = 0;
        public ConditionEffectConfig(int ID,List<ConditionInst> conditionInsts, List<EffectInst> effectInsts)
        {
            id = ID;
            times = conditionInsts[0].config.times;
            self = (EntityType)conditionInsts[0].config.self;
            timePoint = conditionInsts[0].timePoint;
            this.conditionInsts = conditionInsts;
            this.effectInsts = effectInsts;
        }

        public bool CECheckByBlend()
        {
            //if(GameFlowMgr.instance.CurEvent.UniqueId != id && GameFlowMgr.instance.CurCard.UniqueId != id)
            //{
            //    return false;
            //}
            if(times != -2)
            {
                times--;
            }
            
            if(times == 0)
            {
                DestorySelf();
            }
            
            if(ConditionMgr.instance.ExcuteConditionListCheckByBlend(conditionInsts))
            {
                EffectMgr.instance.ExcuteResult(effectInsts);
                return true;
            }
            return false;

        }
        public void DestorySelf()
        {
            GameFlowCdtAndEft.instance.RemoveCECToList(this);
        }
    }
}
