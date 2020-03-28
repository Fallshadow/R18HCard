using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;
using System.Linq;

namespace act.game
{
    public class EventMgr : Singleton<EventMgr>
    {
        Dictionary<int, EventData> dictEvent = new Dictionary<int, EventData>();
        Dictionary<int, EventCardDescData> dictEventCardDesc = new Dictionary<int, EventCardDescData>();
        Dictionary<int, EventInst> dictEventInst = new Dictionary<int, EventInst>();
        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictEvent, "ID", data.CONFIG_PATH.DICT_EVENT);
            data.ConfigDataMgr.GetDataDictionary(dictEventCardDesc, "ID", data.CONFIG_PATH.DICT_EVENT_CARD_DESC);
            foreach (var item in dictEvent)
            {
                dictEventInst.Add(item.Key, new EventInst(item.Value));
            }
        }

        public EventData GetEventDataByID(int ID)
        {
            return dictEvent[ID];
        }
        public EventInst GetEventInstByID(int ID)
        {
            return new EventInst(GetEventDataByID(ID));
        }

        public string GetEventCardDesc(int eventID,int cardType, int cardID)
        {
            string[] eventCardDescs = dictEventCardDesc.Where(x => x.Value.eventID == eventID && x.Value.cardID == cardID).Select(x=>x.Value.desc).ToArray();
            if(eventCardDescs != null && eventCardDescs.Length != 0)
            {
                return localization.LocalizationManager.instance.GetLocalizedString(eventCardDescs[0], "ui_systen");
            }
            string desc = localization.LocalizationManager.instance.GetLocalizedString(dictEventCardDesc[cardType].desc, "ui_systen");
            if(desc != null)
            {
                return desc;
            }
            return null;
        }
    }
}
