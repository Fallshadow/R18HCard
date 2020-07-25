using UnityEngine;
using System.Collections.Generic;
using act.data;

namespace act.game
{
    public class ConditionMgr : Singleton<ConditionMgr>
    {
        Dictionary<int, ConditionData> dictCondition = new Dictionary<int, ConditionData>();
        #region 操作表（读取、查询、读取_;condition）
        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictCondition, "ID", data.CONFIG_PATH.DICT_CONDITION);
        }

        public ConditionData GetConditionDataById(ConditionId conditionId)
        {
            ConditionData conditionData = new ConditionData();
            dictCondition.TryGetValue((int)conditionId, out conditionData);
            return conditionData;
        }

        public ConditionInst GetConditionInstById(ConditionId conditionId)
        {
            ConditionInst conditionInst = new ConditionInst(GetConditionDataById(conditionId));
            return conditionInst;
        }
        /// <summary>
        /// 通过配置获取条件列表
        /// </summary>
        /// <param name="config_condition">_</param>
        /// <param name="config_chonditionVar">_;</param>
        /// <returns></returns>
        public List<ConditionInst> GetConditionListByConfig(string config_condition, string config_chonditionVar)
        {
            List<ConditionInst> resultCondition = new List<ConditionInst>();
            List<int> conditions = ConfigDataMgr.ReturnObjectArraryBySplitString<int>(config_condition, '_');
            List<string> conditionVars = ConfigDataMgr.ReturnObjectArraryBySplitString<string>(config_chonditionVar, ';');
            List<List<float>> contionVarDescs = new List<List<float>>();

            foreach (string item in conditionVars)
            {
                contionVarDescs.Add(ConfigDataMgr.ReturnObjectArraryBySplitString<float>(item, '_'));
            }

            ConditionInst conditionInst = null;
            for (int index = 0; index < conditions.Count; index++)
            {
                conditionInst = GetConditionInstById((ConditionId)conditions[index]);
                foreach (float floatItem in contionVarDescs[index])
                {
                    conditionInst.desc = string.Format($"{conditionInst.desc}", floatItem);
                    conditionInst.numVars.Add(floatItem);
                }
                resultCondition.Add(conditionInst);
            }
            return resultCondition;
        }
        #endregion
        public bool ExcuteConditionCheck(ConditionId condition, params float[] vars)
        {
            Debug.Log($"执行条件ID：{(int)condition}");
            switch (condition)
            {
                case ConditionId.CI_None:
                    return true;
                case ConditionId.CI_1:
                    return (GameFlowMgr.instance.CurCard.config.type == vars[0]
                        || GameFlowMgr.instance.CurCard.config.type == vars[1]
                        || GameFlowMgr.instance.CurCard.config.type == vars[2]
                        )
                        ? true : false;
                case ConditionId.CI_2:
                    return true;
                case ConditionId.CI_3:
                    return (GameFlowMgr.instance.CurCard.config.ID == vars[0])
                        ? true : false;
                case ConditionId.CI_4:
                    return (GameFlowMgr.instance.CurEvent.RoundNum == 0)
                        ? true : false;
                case ConditionId.CI_5:
                    return GameFlowMgr.instance.IsCurEventContain((int)vars[0]);
                case ConditionId.CI_6:
                    return (GameFlowMgr.instance.GetGrayCard() >= vars[0])
                        ? true : false;
                case ConditionId.CI_7:
                    return (GameFlowMgr.instance.CurEvent.config.ID == vars[0])
                        ? true : false;
                case ConditionId.CI_8:
                    return true;
                case ConditionId.CI_9:
                    return true;
                case ConditionId.CI_10:
                    return true;
                case ConditionId.CI_11:
                    return game.GameFlowMgr.instance.RoundNum == vars[0];
                case ConditionId.CI_12:
                    return game.GameFlowMgr.instance.Process >= vars[0];
                case ConditionId.CI_13:
                    return true;
                case ConditionId.CI_14:
                    return game.RandomNumMgr.instance.justTouziCheckNum == 6;
                case ConditionId.CI_15:
                    return true;
                case ConditionId.CI_16:
                    return !game.GameFlowMgr.instance.CurCard.Canuse;
                case ConditionId.CI_17:
                    return true;
                case ConditionId.CI_18:
                    return (GameFlowMgr.instance.CurEvent.config.ID == vars[0])
                        ? true : false;
                case ConditionId.CI_19:
                    return (GameFlowMgr.instance.CurEvent.config.ID == vars[0])
                        ? true : false;
                case ConditionId.CI_20:
                    return true;
                case ConditionId.CI_21:
                    return true;
                case ConditionId.CI_22:
                    return true;
                case ConditionId.CI_23:
                    return true;
                default:
                    break;
            }
            debug.PrintSystem.Log("[dictError/gameDefineError] : condition ID can't find! Please Check dict_condition or gameDefine!", Color.red);
            return false;
        }

        /// <summary>
        /// 单个 条件返回
        /// </summary>
        /// <param name="conditionInst"></param>
        /// <returns></returns>
        public bool ExcuteConditionCheck(ConditionInst conditionInst)
        {
            ConditionId ID = (ConditionId)conditionInst.config.ID;
            return ExcuteConditionCheck(ID, conditionInst.numVars.ToArray());
        }

        /// <summary>
        /// list 条件返回（且）
        /// </summary>
        /// <param name="conditionInsts">Insts</param>
        /// <returns></returns>
        public bool ExcuteConditionListCheckByBlend(List<ConditionInst> conditionInsts)
        {
            bool result = true;
            foreach (var item in conditionInsts)
            {
                result = result && ExcuteConditionCheck(item);
            }
            return result;
        }


        public bool CheckConditionByBlend(List<List<ConditionInst>> conditionInsts, out List<bool> results)
        {
            bool check = true;
            results = new List<bool>();
            foreach (var item in conditionInsts)
            {
                bool temp = ConditionMgr.instance.ExcuteConditionListCheckByBlend(item);
                results.Add(temp);
                check = check && temp;
            }
            return check;
        }

        public bool CheckConditionByBlend(List<List<ConditionInst>> conditionInsts)
        {
            bool check = true;
            foreach (var item in conditionInsts)
            {
                check = check && ConditionMgr.instance.ExcuteConditionListCheckByBlend(item);
            }
            return check;
        }
        public bool CheckConditionBySplit(List<List<ConditionInst>> conditionInsts, out List<bool> results)
        {
            bool check = false;
            results = new List<bool>();
            foreach (var item in conditionInsts)
            {
                bool temp = ConditionMgr.instance.ExcuteConditionListCheckByBlend(item);
                results.Add(temp);
                check = check || temp;
            }
            return check;
        }

        public bool CheckConditionBySplit(List<List<ConditionInst>> conditionInsts)
        {
            bool check = false;
            foreach (var item in conditionInsts)
            {
                check = check || ConditionMgr.instance.ExcuteConditionListCheckByBlend(item);
            }
            return check;
        }
    }
}

