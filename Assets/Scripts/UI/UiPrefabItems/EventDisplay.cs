using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE, CheckDestoryEvent);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, CheckDestoryEvent);
        }

        public void Release()
        {
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE, CheckDestoryEvent);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, CheckDestoryEvent);
        }
        public game.EventInst GetEventInst()
        {
            return event_inst;
        }

        public void EnterToTable()
        {
            event_inst.EnterToTable();
            //TODO:表现上EnterToTable
        }
        public void SetInst(game.EventInst eventInst)
        {
            event_inst = eventInst;
            if (eventInst.conditionInsts.Count == 0)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"EventType{4}", "PlayCanvas");
            }
            else
            {
                if (eventInst.conditionInsts[0][0].config.ID == 1)
                {
                    config.Img_Type.sprite = UiManager.instance.GetSprite($"EventType{eventInst.conditionInsts[0][0].numVars[0]}", "PlayCanvas");
                }
                else if (eventInst.conditionInsts[0][0].config.ID == 3)
                {
                    config.Img_Type.sprite = UiManager.instance.GetSprite($"EventType{game.CardMgr.instance.GetCardDataByID((int)eventInst.conditionInsts[0][0].numVars[0]).type}", "PlayCanvas");
                }
            }

            config.Text_Name.Localize(event_inst.config.name, "ui_system");
            config.Text_Desc.Localize(event_inst.config.desc, "ui_system");
            config.Text_SPDesc.Localize(event_inst.config.desc_SP, "ui_system");
            config.Text_SuccResultDesc.Localize(event_inst.config.desc_SuccResult, "ui_system");
            config.Text_DefResultDesc.Localize(event_inst.config.desc_DefResult, "ui_system");
            config.Text_Round.text = event_inst.RoundNum.ToString();

            int tempCSPCount = event_inst.conditionSpInsts.Count;
            int tempCCount = event_inst.conditionInsts.Count;

            for (int index = 0; index < tempCSPCount; index++)
            {
                for (int indexY = 0; indexY < event_inst.conditionSpInsts[index].Count; indexY++)
                {
                    string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.conditionSpInsts[index][indexY].desc, "ui_system");
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

        public void Show()
        {

        }

        public void CheckDestoryEvent()
        {
            if (event_inst.hasComplete || event_inst.RoundNum == -1)
            {
                ShowDie();
            }

            config.Text_Round.text = event_inst.RoundNum.ToString();
        }
        public void ShowDie()
        {
            GetComponent<DOTweenAnimation>().DOPlay();
        }

        public void DestoryEvent()
        {
            Release();
            event_inst.DestorySelf();
            Destroy(this.gameObject);
        }
    }
}

