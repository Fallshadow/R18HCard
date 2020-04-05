﻿
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
        CARD = 3,
        INPUT = 4,
        TOUZI = 5,
        EVENT = 6,
    }
    public enum UiEvent : short
    {
        UI_Event_Click,
        UI_Event_Desc_Hide,
    }
    public enum GameEvent : short
    {
        Card_Event_Interact = 1,
        Globe_RandomNum_Change,
        Globe_ProcessNum_Change,
        Globe_HpNum_Change,
        Globe_RoundNum_Change,
        Globe_Event_Create,
        Globe_CurEvent_Completed,
        Globe_Card_Event_Success,
        Globe_Card_Event_Def,
        Globe_Round_Over,
        Globe_IDEvent_ROUNDNUM_CHANGE,
        Globe_Card_Exit_Slot

    }

    public enum CardEvent : short
    {
        Card_Current_Change = 0,
        Card_Create,
        Card_Destory,
        Card_Refresh_Use,
        Card_Enter_Slot,
        Card_Exit_Slot
    }
    public enum EventEvent : short
    {
        Event_ID_ROUNDNUM_CHANGE = 0,
        Event_ID_ROUNDNUM_Over,
    }
    public enum InputEvent : short
    {
        IE_Mouse_Right_Down = 0,
        IE_Mouse_Right_Up = 1,
    }

    public enum TouziEvent : short
    {
        T_Roll,
    }

}