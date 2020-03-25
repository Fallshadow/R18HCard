using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    [System.Serializable]
    public class ConditionInst
    {
        public ConditionData config = null;
        public string desc = null;
        public List<float> numVars = new List<float>();
        public TimePoint timePoint = TimePoint.TP_None;

        public ConditionInst(ConditionData conditionData)
        {
            config = conditionData;
            timePoint = (TimePoint)conditionData.timePoint;
            Debug.Log(conditionData.ID);
            desc = localization.LocalizationManager.instance.GetLocalizedString(conditionData.desc, "ui_system");
        }

        public bool Excute()
        {
            return ConditionMgr.instance.ExcuteConditionCheck((game.ConditionId)config.ID, numVars.ToArray());
        }
    }
}