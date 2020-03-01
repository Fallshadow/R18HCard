using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    public class ConditionInst
    {
        public ConditionData config = null;
        public string desc = null;
        public List<float> numVars = new List<float>();

        public ConditionInst(ConditionData conditionData)
        {
            config = conditionData;
            desc = localization.LocalizationManager.instance.GetLocalizedString(conditionData.desc, "ui_system");
        }

        public bool Excute()
        {
            return ConditionMgr.instance.ExcuteConditionCheck((game.ConditionId)config.ID, numVars.ToArray());
        }
    }
}