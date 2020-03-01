﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace act.data
{
    public class EventData
    {
        [Description("event_id")]
        public int ID;
        [Description("event_name")]
        public string name;
        [Description("event_desc")]
        public string desc;
        [Description("event_result_desc")]
        public string resultDesc;
        [Description("event_round")]
        public int rountNum;
        [Description("event_condition_SPId")]
        public string specialCId;
        [Description("event_condition_SP_var")]
        public string specialCVar;
        [Description("event_effect_SPId")]
        public string specialEId;
        [Description("event_effect_SP_var")]
        public string specialEVar;
        [Description("event_condition_1")]
        public string condition_1;
        [Description("event_condition_var_1")]
        public string condition_var_1;
        [Description("event_effect_1")]
        public string effect_1;
        [Description("event_effect_var_1")]
        public string effect_var_1;
        [Description("event_condition_2")]
        public string condition_2;
        [Description("event_condition_var_2")]
        public string condition_var_2;
        [Description("event_effect_2")]
        public string effect_2;
        [Description("event_effect_var_2")]
        public string effect_var_2;
    }
}