using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace act.data
{
    [System.Serializable]
    public class CardData
    {
        [Description("card_id")]
        public int ID;
        [Description("card_level")]
        public int level;
        [Description("card_name")]
        public string name;
        [Description("card_img")]
        public string showImg;
        [Description("card_desc")]
        public string desc;
        [Description("card_desc_SP")]
        public string descSP;
        [Description("card_verification_number")]
        public int testNumber;
        [Description("card_type")]
        public int type;
        [Description("card_condition_1")]
        public string condition_1;
        [Description("card_condition_var_1")]
        public string condition_var_1;
        [Description("card_effect_1")]
        public string effect_1;
        [Description("card_effect_var_1")]
        public string effect_var_1;
        [Description("card_condition_2")]
        public string condition_2;
        [Description("card_condition_var_2")]
        public string condition_var_2;
        [Description("card_effect_2")]
        public string effect_2;
        [Description("card_effect_var_2")]
        public string effect_var_2;
    }
}