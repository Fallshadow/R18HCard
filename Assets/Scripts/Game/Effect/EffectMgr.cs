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
                    RandomNumMgr.instance.SetRandomNumByTime(6, 1);
                    return;
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

