using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    public class EventMgr : Singleton<EventMgr>
    {
        Dictionary<int, EventData> dictEvent = new Dictionary<int, EventData>();
        Dictionary<int, EventInst> dictEventInst = new Dictionary<int, EventInst>();
        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictEvent, "ID", data.CONFIG_PATH.DICT_EVENT);
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
    }
}
