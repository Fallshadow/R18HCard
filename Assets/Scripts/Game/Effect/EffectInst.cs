using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    [System.Serializable]
    public class EffectInst
    {
        public EffectData config = null;
        public string desc = null;
        public List<float> numVars = new List<float>();

        public EffectInst(EffectData effectData)
        {
            config = effectData;
            desc = localization.LocalizationManager.instance.GetLocalizedString(effectData.desc, "ui_system");
        }

        public void Excute()
        {
            EffectMgr.instance.ExcuteEffectResult((game.EffectId)config.ID, numVars.ToArray());
        }
    }
}
