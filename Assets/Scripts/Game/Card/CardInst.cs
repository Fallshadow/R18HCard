using act.data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace act.game
{
    public class CardInst
    {
        public CardData config = null;
        public List<ConditionEffectConfig> selfCEC = new List<ConditionEffectConfig>();
        public List<List<ConditionInst>> conditionInsts = new List<List<ConditionInst>>();
        public List<List<EffectInst>> effectInsts = new List<List<EffectInst>>();
        public int UniqueId = 0;
        public bool Canuse
        {
            get
            {
                return canUse;
            }
            set
            {
                canUse = value;
                evt.EventManager.instance.Send<int>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Refresh_Use, UniqueId);
            }
        }
        private bool canUse = false;
        public CardInst(CardData cardData)
        {
            UniqueId = CECMgr.instance.GetUniqueId(cardData.ID, 1);
            config = cardData;
            canUse = true;
            if (config.condition_1 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_1, config.condition_var_1)); }
            if (config.condition_2 != null) { conditionInsts.Add(ConditionMgr.instance.GetConditionListByConfig(config.condition_2, config.condition_var_2)); }

            if (config.effect_1 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_1, config.effect_var_1)); }
            if (config.effect_2 != null) { effectInsts.Add(EffectMgr.instance.GetEffectListByConfig(config.effect_2, config.effect_var_2)); }

            Debug.Log($"Card {config.ID}: Success!");
        }
        public void EnterTable()
        {
            Debug.Log($"卡牌入场：  独有ID：{UniqueId}");
            for (int i = 0; i < conditionInsts.Count; i++)
            {
                ConditionEffectConfig conditionEffectConfig = new ConditionEffectConfig(UniqueId, conditionInsts[i], effectInsts[i]);
                if (conditionEffectConfig.self == EntityType.ET_World)
                {
                    Debug.Log($"{UniqueId}卡牌入场：  加入全局：{i}");
                    GameFlowCdtAndEft.instance.AddCECToList(conditionEffectConfig);
                }
                else
                {
                    Debug.Log($"{UniqueId}卡牌入场：  加入私有：{i}");
                    selfCEC.Add(conditionEffectConfig);
                }
            }
            GameFlowMgr.instance.cardInsts.Add(this);
        }
        public void DestorySelf()
        {
            GameFlowMgr.instance.cardInsts.Remove(this);
            //TODO:通知表现
        }
        public bool ExcuteCheck()
        {
            Canuse = false;
            if (config.testNumber > GameFlowMgr.instance.RandomNum)//TODO:随机数生成,全局加持！！
            {
                return false;
            }
            return true;
        }
        public List<bool> CheckCdt()
        {
            List<bool> boolList = new List<bool>();
            Debug.Log("进入卡牌自身列表检测————————————————————————————————");
            ConditionEffectConfig tempCEC;
            for (int i = selfCEC.Count - 1; i >= 0; i--)
            {
                tempCEC = selfCEC[i];
                bool tempBool = tempCEC.CECheckByBlend();
                boolList.Add(tempBool);
                Debug.Log($"CECID：{tempCEC.id}结果{tempBool}");
            }
            return boolList;
        }
        public void RefreshUse()
        {
            canUse = true;
        }
    }
}