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
        CI_12,
        CI_13,
        CI_14,
        CI_15,
        CI_16,
        CI_17,
        CI_18,
        CI_19,
        CI_20,
        CI_21,
        CI_22,   
        CI_23,
        CI_24,
        CI_25,
        CI_26,
        CI_27,   
        CI_28,
        CI_29,
        CI_30,
        CI_31,
        CI_32,
        CI_33,
        CI_34,
        CI_35,
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
        EI_14,
        EI_15,
        EI_16,
        EI_17,
        EI_18,
        EI_19,
        EI_20,
        EI_21,
        EI_22,
        EI_23,
        EI_24,
        EI_25,
        EI_26,
        EI_27,
        EI_28,
        EI_29,
        EI_30,
        EI_31,
    }

    public enum TimePoint
    {
        TP_None = 0,
        TP_RoundStart = 1,
        TP_CardCheck = 2,
        TP_CardNumCheckStart = 3,
        TP_CardNumCheckOver = 4,
        TP_CardNumCheckSucc = 5,
        TP_CardNumCheckDeff = 6,
        TP_CardUseOver = 7,
        TP_RoundEnd = 8,
    }

    public enum EntityType
    {
        ET_Self = 1,
        ET_World,
    }

}