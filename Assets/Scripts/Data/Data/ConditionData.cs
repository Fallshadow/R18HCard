using System.ComponentModel;

namespace act.data
{
    public class ConditionData
    {
        [Description("condition_id")]
        public int ID;
        [Description("condition_desc")]
        public string desc;
        [Description("condition_tp")]
        public int timePoint;
        [Description("condition_times")]
        public int times;
        [Description("condition_self")]
        public int self;
    }
}