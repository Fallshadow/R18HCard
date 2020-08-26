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
            Debug.Log($"执行效果ID：{(int)effect}:{localization.LocalizationManager.instance.GetLocalizedString($"desc{(int)effect}", "ui_system_ssc")}");
            foreach(var item in vars)
            {
                Debug.Log($"参数{item}");
            }

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
                    foreach(var item in vars)
                    {
                        GameFlowMgr.instance.PushEventToTable((int)item);
                    }
                    if(vars.Length > 0)
                    {
                        if(vars[0] == 19)
                        {
                            GameController.instance.models[0].SetActive(false);
                        }
                    }
                    return;
                case EffectId.EI_5:
                    RandomNumMgr.instance.curTouziNum += (int)vars[0];
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
                    foreach(var item in GameFlowMgr.instance.cardInsts)
                    {
                        if(item.config.ID == (int)vars[0])
                        {
                            item.config.type =
                            GameFlowMgr.instance.hadUsecardInsts[GameFlowMgr.instance.hadUsecardInsts.Count - 1].config.type;
                        }
                    }
                    evt.EventManager.instance.Send(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Refresh);
                    return;
                case EffectId.EI_10:
                    RandomNumMgr.instance.nextAddCheckNum += (int)vars[0];
                    return;
                case EffectId.EI_11:
                    //TODO:调用CG   ！临时用跳跃代替
                    act.game.ModelController.instance.ChangeAction((Action)vars[0], false);
                    return;
                case EffectId.EI_12:
                    if (GameFlowMgr.instance.CurCard.config.ID == (int)vars[0])
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
                    foreach(var item in vars)
                    {
                        GameFlowMgr.instance.DelectCardByID((int)item);
                    }
                    return;
                case EffectId.EI_16:
                    //删事件
                    foreach(var item in vars)
                    {
                        GameFlowMgr.instance.DelectEventByID((int)item);
                    }
                    if(vars.Length == 5)
                    {
                        if(vars[0] == 21 && vars[1] == 22 && vars[2] == 28 && vars[3] == 29 && vars[4] == 9)
                        {
                            GameController.instance.models[1].SetActive(true);
                        }
                        if(vars[0] == 20 && vars[1] == 21 && vars[2] == 22 && vars[3] == 29 && vars[4] == 9)
                        {
                            GameController.instance.models[1].SetActive(true);
                        }
                    }
                    
                    return;
                case EffectId.EI_17:
                    //跳过骰子
                    GameFlowMgr.instance.JumpUpTouzi = true;
                    return;
                case EffectId.EI_18:
                    //21点事件，将游戏21点数值增加
                    GameFlowMgr.instance.TwoOneNum += (int)RandomNumMgr.instance.curTouziCheckNum;
                    return;
                case EffectId.EI_19:
                    GameFlowMgr.instance.Pleasant += (int)RandomNumMgr.instance.justTouziCheckNum * (int)vars[0];
                    return;
                case EffectId.EI_20:
                    if(RandomNumMgr.instance.justTouziCheckNum >= vars[0])
                    {
                        GameFlowMgr.instance.Vit += (int)vars[1];
                    }
                    return;
                case EffectId.EI_21:
                    GameFlowMgr.instance.Vit += (int)vars[0];
                    return;
                case EffectId.EI_22:
                    GameFlowMgr.instance.Pleasant += (int)vars[0];
                    return;
                case EffectId.EI_23:
                    RandomNumMgr.instance.resultChengNum = (int)vars[0]; 
                        return;
                case EffectId.EI_24:
                    foreach(var item in GameFlowMgr.instance.cardInsts)
                    {
                        if(item.config.ID == (int)vars[0])
                        {
                            item.RefreshUse();
                        }
                    }
                    return;
                case EffectId.EI_25:
                    int count = GameFlowMgr.instance.cardInsts.Count;
                    GameFlowMgr.instance.DelectAllCard();
                    for(int i = 0; i < count; i++)
                    {
                        GameFlowMgr.instance.PushCardToTable((int)vars[0]);
                    }
                    return;
                case EffectId.EI_26:
                    GameFlowMgr.instance.CurEvent.RoundNum += (int)game.RandomNumMgr.instance.justTouziCheckNum;
                    return;
                case EffectId.EI_27:
                    //事件27的专属 转入啪啪啪流程 解决自己
                    if(GameFlowMgr.instance.TwoOneNum > 21)
                    {
                        GameFlowMgr.instance.TwoOneNum = 0;
                    }
                    else if(GameFlowMgr.instance.TwoOneNum == 21)
                    {
                        game.GameController.instance.YUJINuseTimerNorModel = false;
                        game.GameController.instance.models[6].gameObject.SetActive(true);
                        if(act.game.GameController.instance.unlocktest.GetifR18())
                        {
                            game.GameController.instance.isshowtalkcanvas = false;
                            ui.UiManager.instance.CreateUi<ui.TalkCanvas>().setobjs(false);
                            game.GameFlowMgr.instance.PlayTimeLineFixed(game.TimeLineType.ZuJiaoHard, game.TimeLineAssetType.ZuJiaoHard);
                        }
                        else
                        {
                            act.game.GameFlowMgr.instance.ClearData();
                            act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>().ReturnToMain();
                        }
                        
                    }
                    return;
                case EffectId.EI_28:
                    GameFlowMgr.instance.CurEvent.RoundNum = (int)vars[0];
                    return;
                case EffectId.EI_29:
                    game.GameController.instance.YUJINuseTimerNorModel = false;
                    game.GameController.instance.models[6].gameObject.SetActive(true);
                    if(act.game.GameController.instance.unlocktest.GetifR18())
                    {
                        game.GameController.instance.isshowtalkcanvas = false;
                        ui.UiManager.instance.CreateUi<ui.TalkCanvas>().setobjs(false);
                        game.GameFlowMgr.instance.PlayTimeLineFixed(game.TimeLineType.ZuJiaoHard, game.TimeLineAssetType.ZuJiaoHard);
                    }
                    else
                    {
                        act.game.GameFlowMgr.instance.ClearData();
                        act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>().ReturnToMain();
                    }
                    return;
                case EffectId.EI_30:
                    GameFlowMgr.instance.TwoOneNum += (int)RandomNumMgr.instance.justTouziCheckNum;
                    return;
                case EffectId.EI_31:
                    if(GameFlowMgr.instance.CurCard.config.type == 1)
                    {
                        GameFlowMgr.instance.PushCardToTable(12);
                    }
                    else
                    {
                        GameFlowMgr.instance.CurEvent.RoundNum += -1;
                    }
                    return;
                case EffectId.EI_32:
                    GameFlowMgr.instance.hadUsecardInsts[GameFlowMgr.instance.hadUsecardInsts.Count - 1].DestorySelf();
                    return;
                case EffectId.EI_33:
                    int[] num = new int[vars.Length];
                    for(int i = 0; i < vars.Length; i++)
                    {
                        num[i] = (int)vars[i];
                    }
                    GameFlowMgr.instance.DelectAllEventUnSelfID(num);
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

