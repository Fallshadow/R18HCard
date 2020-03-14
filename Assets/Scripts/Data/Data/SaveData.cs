using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace act.data
{
    [Serializable]
    public class SaveData
    {
        public int curEventId;
        public int curCardId;
        public List<int> eventIds;
        public List<int> cardIds;
        
    }
}