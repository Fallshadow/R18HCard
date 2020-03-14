using System;

namespace act.game
{
    public enum Action
    {
        IDLE = 0,
        JUMP,
        WALK
    };
    public enum Face
    {
        DEFAULT,
        ANGRY,
        SMILE,
        ASHAMED,
        SURPRISE,
        EYE_CLOSE,
        MOUTH_A,
        MOUTH_E,
        MOUTH_I,
        MOUTH_O,
        MOUTH_U
    };
    public enum CardType
    {
        CT_None = 0,
        CT_Action,
        CT_Word,
        CT_Emotion,
        CT_Special,
    }

    public enum ConditionId
    {
        CI_None = 0,
        CI_1,//使用card typeX()的卡牌
        CI_2,//在第X回合开始时
        CI_3,//默认满足了条件
        CI_4,//使用特定cardID的卡牌
        CI_5,//检定X，即玩家掷出的点数大于等于X
        CI_6,
        CI_7,
        CI_8,
        CI_9,
        CI_10,
        CI_11,
    }

    public enum EffectId
    {
        EI_None = 0,
        EI_1,
        EI_2,
        EI_3,
        EI_4,
        EI_5,
        EI_6,
        EI_7,
        EI_8,
        EI_9,
        EI_10,
        EI_11,
        EI_12,
        EI_13,
    }

    public enum TimePoint
    {
        TP_None = 0,
        TP_RoundStart,
        TP_CardCheck,
        TP_CardNumCheckStart,
        TP_CardNumCheckOver,
        TP_CardNumCheckSucc,
        TP_CardNumCheckDeff,
        TP_CardUseOver,
        TP_RoundEnd,
    }

    public enum EntityType
    {
        ET_Self = 1,
        ET_World,
    }
}