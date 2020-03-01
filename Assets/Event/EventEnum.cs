
namespace act.evt
{
    /// <summary>
    /// 提供給不同模塊之間交互訊息，以降低偶合度。
    /// 事件分類以發送者的物件為依據，主要是各個Manager系統。
    /// </summary>
    public enum EventGroup : short
    {
        NONE = 0,
        UI = 1,
        GAME = 2,
    }
    public enum UiEvent : short
    {

    }
    public enum GameEvent : short
    {
        Card_Event_Interact = 1,
        Globe_RandomNum_Change,
        Globe_ProcessNum_Change,
        Globe_HpNum_Change,
        Globe_RoundNum_Change,
        Globe_Event_Create,
        Globe_Card_Create,
    }

}