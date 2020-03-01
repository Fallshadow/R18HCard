using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{
    [RequireComponent(typeof(EventReference))]
    public class EventDisplay : MonoBehaviour, IUiItemLifeInterface
    {
        [SerializeField] protected EventReference config = null;
        [SerializeField] private game.EventInst event_inst = null;

        public void Hide()
        {

        }

        public void Init()
        {
            config = GetComponent<EventReference>();
        }
        public game.EventInst GetEventInst()
        {
            return event_inst;
        }
        public void SetInst(game.EventInst eventInst)
        {
            event_inst = eventInst;
            config.Text_Name.Localize(event_inst.config.name, "ui_system");
            config.Text_Desc.Localize(event_inst.config.desc, "ui_system");
            config.Text_ResultDesc.Localize(event_inst.config.resultDesc, "ui_system");
            config.Text_Round.text = event_inst.config.rountNum.ToString();

            int tempCSPCount = event_inst.conditionSpInsts.Count;
            int tempCCount = event_inst.conditionInsts.Count;

            for (int index = 0; index < tempCSPCount; index++)
            {
                for (int indexY = 0; indexY < event_inst.conditionSpInsts[index].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.conditionInsts[index][indexY].desc, "ui_system");
                    config.Text_Conditions[index].text += ss + " ";
                }
            }
            for (int index = tempCSPCount; index < tempCSPCount + tempCCount; index++)
            {
                for (int indexY = 0; indexY < event_inst.conditionInsts[index - tempCSPCount].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.conditionInsts[index - tempCSPCount][indexY].desc, "ui_system");
                    config.Text_Conditions[index].text += ss + " ";
                }
            }
            int tempESPCount = event_inst.effectSpInsts.Count;
            int tempECount = event_inst.effectInsts.Count;

            for (int index = 0; index < tempESPCount; index++)
            {
                for (int indexY = 0; indexY < event_inst.effectSpInsts[index].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.effectSpInsts[index][indexY].desc, "ui_system");
                    config.Text_Effects[index].text += ss + " ";
                }
            }
            for (int index = tempESPCount; index < tempESPCount + tempECount; index++)
            {
                for (int indexY = 0; indexY < event_inst.effectInsts[index - tempESPCount].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.effectInsts[index - tempESPCount][indexY].desc, "ui_system");
                    config.Text_Effects[index].text += ss + " ";
                }
            }
        }

        public void Release()
        {

        }

        public void Show()
        {

        }
    }
}

