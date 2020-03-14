using System.ComponentModel;

namespace act.data
{
    public class EffectData
    {
        [Description("effect_id")]
        public int ID;
        [Description("effect_desc")]
        public string desc;
        [Description("effect_dietp")]
        public int dieTimePoint;
        [Description("effect_times")]
        public int times;
    }
}