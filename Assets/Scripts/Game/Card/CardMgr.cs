using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    public class CardMgr : Singleton<CardMgr>
    {
        Dictionary<int, CardData> dictCard = new Dictionary<int, CardData>();
        Dictionary<int, CardInst> dictCardInst = new Dictionary<int, CardInst>();

        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictCard, "ID", data.CONFIG_PATH.DICT_CARD);
            foreach (var item in dictCard)
            {
                dictCardInst.Add(item.Key, new CardInst(item.Value));
            }
        }

        public CardData GetCardDataByID(int ID)
        {
            return dictCard[ID];
        }

        public CardInst GetCardInstByID(int ID)
        {
            return dictCardInst[ID];
        }
    }
}
