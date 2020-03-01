using act.data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace act.game
{
    public class ProcessInst
    {
        public ProcessData config = null;
        public ConditionInst conditionInst = null;
        public List<int> eventIds = new List<int>();
        public ProcessInst(ProcessData processData)
        {
            config = processData;
            conditionInst = ConditionMgr.instance.GetConditionInstById((ConditionId)processData.condition_id);
            conditionInst.numVars = ConfigDataMgr.ReturnObjectArraryBySplitString<float>(config.conditions_var, '_');
            foreach (var item in conditionInst.numVars)
            {
                conditionInst.desc = string.Format(conditionInst.desc, item);
            }
            eventIds = ConfigDataMgr.ReturnObjectArraryBySplitString<int>(config.events_id, '_');
        }
    }
}
