using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace act.data
{
    public class EventCardDescData
    {
        [Description("id")]
        public int ID;
        [Description("event_id")]
        public int eventID;
        [Description("card_type")]
        public game.CardType cardType;
        [Description("card_id")]
        public int cardID;
        [Description("desc")]
        public string desc;
    }
}
