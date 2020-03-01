using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace act.data
{
    public class ProcessData
    {
        [Description("process_id")]
        public int ID;
        [Description("p_condition_id")]
        public int condition_id;
        [Description("p_condition_var")]
        public string conditions_var;
        [Description("p_events_id")]
        public string events_id;
    }
}