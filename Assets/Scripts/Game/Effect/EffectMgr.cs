using UnityEngine;
using System.Collections.Generic;
using act.data;

namespace act.game
{
    public class EffectMgr : Singleton<EffectMgr>
    {
        Dictionary<int, EffectData> dictEffect = new Dictionary<int, EffectData>();
        #region 操作表（读取、查询、读取_;effect）
        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictEffect, "ID", data.CONFIG_PATH.DICT_EFFECT);
        }

        public EffectData GetEffectDataById(game.EffectId effectId)
        {
            EffectData effectData = new EffectData();
            dictEffect.TryGetValue((int)effectId, out effectData);
            return effectData;
        }

        public EffectInst GetEffectInstById(game.EffectId effectId)
        {
            EffectInst effectInst = new EffectInst(GetEffectDataById(effectId));
            return effectInst;
        }

        /// <summary>
        /// 从配置中获取效果
        /// </summary>
        /// <param name="config_effect"></param>
        /// <param name="config_effectVar"></param>
        /// <returns></returns>
        public List<EffectInst> GetEffectListByConfig(string config_effect, string config_effectVar)
        {
            List<EffectInst> resultEffect = new List<EffectInst>();
            List<int> effects = ConfigDataMgr.ReturnObjectArraryBySplitString<int>(config_effect, '_');
            List<string> effectVars = ConfigDataMgr.ReturnObjectArraryBySplitString<string>(config_effectVar, ';');
            List<List<float>> effectVarDescs = new List<List<float>>();

            foreach (string item in effectVars)
            {
                effectVarDescs.Add(ConfigDataMgr.ReturnObjectArraryBySplitString<float>(item, '_'));
            }

            EffectInst effectInst = null;
            for (int index = 0; index < effects.Count; index++)
            {
                effectInst = GetEffectInstById((EffectId)effects[index]);
                foreach (float floatItem in effectVarDescs[index])
                {
                    effectInst.desc = string.Format($"{effectInst.desc}", floatItem);
                    effectInst.numVars.Add(floatItem);
                }
                resultEffect.Add(effectInst);
            }
            return resultEffect;
        }
        #endregion


        public void ExcuteEffectResult(game.EffectId effect, params float[] vars)
        {
            Debug.Log($"执行效果ID：{(int)effect}");
            switch (effect)
            {
                case EffectId.EI_None:
                    return;
                case EffectId.EI_1:
                    GameFlowMgr.instance.Process += (int)vars[0];
                    Debug.Log($"进度值增加{(int)vars[0]}");
                    return;
                case EffectId.EI_2:
                    GameFlowMgr.instance.PushCardToTable((int)vars[0]);
                    return;
                case EffectId.EI_3:
                    if (vars.Length == 1)
                    {
                        RandomNumMgr.instance.SetRandomNumByTime((int)vars[0], 1);
                    }
                    else
                    {
                        RandomNumMgr.instance.SetRandomNumByTime((int)vars[0], (int)vars[1]);
                    }
                    return;
                case EffectId.EI_4:
                    GameFlowMgr.instance.PushEventToTable((int)vars[0]);
                    return;
                case EffectId.EI_5:
                    RandomNumMgr.instance.curUseTouziTime = (int)vars[0];
                    return;
                case EffectId.EI_6:
                    GameFlowMgr.instance.CurCard.Canuse = true;//TODO:需要能使用
                    return;
                case EffectId.EI_7:
                    GameFlowMgr.instance.CurEvent.RoundNum += (int)vars[0];
                    return;
                case EffectId.EI_8:
                    GameFlowMgr.instance.Hp += (int)vars[0];
                    return;
                case EffectId.EI_9:
                    GameFlowMgr.instance.CurCard.config.type =
                    GameFlowMgr.instance.hadUsecardInsts[GameFlowMgr.instance.hadUsecardInsts.Count - 1].config.type;
                    return;
                case EffectId.EI_10:
                    GameFlowMgr.instance.RandomNum += (int)vars[0];
                    return;
                case EffectId.EI_11:
                    //TODO:调用CG   ！临时用跳跃代替
                    act.game.ModelController.instance.ChangeAction((Action)vars[0], false);
                    return;
                case EffectId.EI_12:
                    if (GameFlowMgr.instance.CurEvent.config.ID == (int)vars[0])
                    {
                        GameFlowMgr.instance.CurCard.DestorySelf();
                    }
                    return;
                case EffectId.EI_13:
                    int index = 0;
                    List<ConditionInst> conditions = new List<ConditionInst>();
                    List<EffectInst> effectInsts = new List<EffectInst>();
                    conditions.Add(ConditionMgr.instance.GetConditionInstById((ConditionId)(int)vars[index++]));
                    for (int cdtx = index; cdtx < vars.Length; cdtx++)
                    {
                        if (vars[cdtx] != 111111)
                        {
                            conditions[0].numVars.Add(vars[index++]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    index++;
                    effectInsts.Add(EffectMgr.instance.GetEffectInstById((EffectId)(int)vars[index++]));
                    for (int eftx = index; eftx < vars.Length; eftx++)
                    {
                        effectInsts[0].numVars.Add(vars[index++]);
                    }
                    ConditionEffectConfig conditionEffectConfig = new ConditionEffectConfig(0, conditions, effectInsts);
                    GameFlowCdtAndEft.instance.AddCECToList(conditionEffectConfig);
                    return;
                case EffectId.EI_14:
                    GameFlowMgr.instance.cardSuccEventComp = false;
                    return;
                case EffectId.EI_15:
                    //删卡牌
                    GameFlowMgr.instance.DelectCardByID((int)vars[0]);
                    return;
                case EffectId.EI_16:
                    //删事件
                    GameFlowMgr.instance.DelectEventByID((int)vars[0]);
                    return;
                case EffectId.EI_17:
                    //跳过骰子
                    GameFlowMgr.instance.JumpUpTouzi = true;
                    return;
                //case EffectId.EI_10:
                //    GameFlowMgr.instance.PushEventToTable((int)vars[0]);
                //    return;
                default:
                    break;
            }
            debug.PrintSystem.Log("[dictError/gameDefineError] : effect ID can't find! Please Check dict_effect or gameDefine!", Color.red);
        }


        /// <summary>
        /// 单效果执行
        /// </summary>
        /// <param name="effectInst"></param>
        public void ExcuteEffectResult(EffectInst effectInst)
        {
            EffectId ID = (EffectId)effectInst.config.ID;
            ExcuteEffectResult(ID, effectInst.numVars.ToArray());
        }



        public void ExcuteEffectResult(bool conditionRe, List<EffectInst> effectInsts)
        {
            if (conditionRe)
            {
                foreach (var effectInst in effectInsts)
                {
                    effectInst.Excute();
                }
            }
        }
        /// <summary>
        /// 合成效果执行
        /// </summary>
        /// <param name="effectInsts"></param>
        public void ExcuteResult(List<EffectInst> effectInsts)
        {
            foreach (var effectInst in effectInsts)
            {
                effectInst.Excute();
            }
        }

        /// <summary>
        /// 一串合成效果（其中的效果是由多个效果合成的）的执行
        /// </summary>
        /// <param name="effectInsts"></param>
        /// <param name="results">需要知道那些是执行那些不执行</param>
        public void ExcuteResult(List<List<EffectInst>> effectInsts, List<bool> results)
        {
            for (int index = 0; index < effectInsts.Count; index++)
            {
                if (results[index] == true)
                {
                    EffectMgr.instance.ExcuteResult(effectInsts[index]);
                }
            }
        }
    }
}

