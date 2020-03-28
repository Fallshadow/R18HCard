using System.ComponentModel;

namespace act.data
{
    public enum CONFIG_PATH
    {
        //範例
        [Description("dict/dict_example")] DICT_EXAMPLE,
        [Description("dict/dict_card")] DICT_CARD,
        [Description("dict/dict_event")] DICT_EVENT,
        [Description("dict/dict_event_card_desc")] DICT_EVENT_CARD_DESC,
        [Description("dict/dict_condition")] DICT_CONDITION,
        [Description("dict/dict_effect")] DICT_EFFECT,
        [Description("dict/dict_process")] DICT_PROCESS,
    }
}