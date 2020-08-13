using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace act.game
{
    public class GameFlowMgr : Singleton<GameFlowMgr>
    {
        public bool CanReplay = true;
        //event 展示
        public bool eventDesc = false;
        //卡牌成功即解决事件？
        public bool cardSuccEventComp = true;
        public int TwoOneNum
        {
            get
            {
                return twoOneNum;
            }
            set
            {
                twoOneNum = value;
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_21Num_Change);

                //if(twoOneNum == 21)
                //{
                //    CurEvent.HasComplete = true;
                //    twoOneNum = 0;
                //}
                //else if(twoOneNum > 21)
                //{
                //    twoOneNum = 0;
                //}
                //evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Limit_TwoOne);
            }
        }
        private int twoOneNum = 0;

        public CardInst JustCard
        {
            get
            {
                return justCard;
            }
            set
            {
                justCard = value;
            }
        }
        private CardInst justCard;
        #region 当前使用的卡牌与事件
        public CardInst CurCard
        {
            get
            {
                return curCard;
            }
            set
            {
                curCard = value;
                evt.EventManager.instance.Send<CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Current_Change, curCard);
            }
        }
        private CardInst curCard;
        public EventInst CurEvent
        {
            get
            {
                return curEvent;
            }
            set
            {
                curEvent = value;
            }
        }
        private EventInst curEvent = null;
        #endregion

        #region 回合数、进度值、HP
        public int RoundNum
        {
            get
            {
                return roundNum;
            }
            set
            {
                roundNum = value;
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RoundNum_Change);
            }
        }
        private int roundNum = 1;

        private int processE29Value = 50;//气氛值达到50时
        private bool processE29Time = false;//气氛值达到50时第一次才出发29

        public float Process
        {
            get
            {
                return process;
            }
            set
            {
                if(processTwo)
                {
                    return;
                }

                if(process >= value)
                {

                }
                else
                {
                    AudioMgr.instance.PlaySound(AudioClips.AC_ProcessGet);
                }

                process = value;
                if(process < 0)
                {
                    process = 0;
                }

                if(process >= processE29Value && processE29Time == false)
                {
                    processE29Time = true;
                    PushEventToTable(29);
                }

                //process = Mathf.Clamp(value, 0, 100);
                //if(process == 100)
                //{

                //}
                if(process == 0)
                {
                    //TODO:游戏结束
                }
                evt.EventManager.instance.Send<bool>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_ProcessNum_Change, false);
            }
        }
        private float process = 0;

        public int Hp
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
                if(hp == 0)
                {
                    GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameOver);
                }
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_HpNum_Change);
            }
        }
        private int hp = 0;
        #endregion

        #region 是否跳过流程：投骰子
        public bool JumpUpTouzi
        {
            get
            {
                return jumpUpTouzi;
            }
            set
            {
                jumpUpTouzi = value;
            }
        }
        private bool jumpUpTouzi;
        #endregion
        public List<bool> curEventResults = new List<bool>();
        public List<EventInst> eventInsts = new List<EventInst>();
        public List<EventInst> hadSolveEventInsts = new List<EventInst>();
        public List<CardInst> cardInsts = new List<CardInst>();
        public List<CardInst> hadUsecardInsts = new List<CardInst>();
        public float RandomNum = 0;

        data.SaveData saveData = null;
        private const string SAVE_FILE_NAME = "SaveData";

        #region 初始化、注册、释放
        public void InitConfigData()
        {
            register();
        }
        public void Release()
        {
            unregister();
        }
        private void register()
        {
        }
        private void unregister()
        {

        }
        #endregion

        #region 存储相关
        //记录当前使用的卡牌
        public void RecordCurCard()
        {
            hadUsecardInsts.Add(curCard);
        }

        //清空重置数据
        public void ResetData()
        {
            RoundNum = 0;
            Hp = 6;
            Process = 0;
            CurCard = null;
            CurEvent = null;
            twoOneNum = 0;
            processE29Time = false;
            curEventResults.Clear();
            eventInsts.Clear();
            cardInsts.Clear();
            hadUsecardInsts.Clear();
            hadSolveEventInsts.Clear();
        }

        //加载数据： 重置-加载-展现 三步
        public void LoadData()
        {
            ResetData();
            saveData = data.DataArchiver.Load<data.SaveData>(SAVE_FILE_NAME);
            if(saveData == null)
            {
                return;
            }
            if(saveData.cardInsts.Count == 0)
            {
                saveData.gameobjectBool[0] = true;
                saveData.gameobjectBool[1] = true;
            }
            CurCard = saveData.curCard;
            CurCard = null;
            CurEvent = saveData.curEvent;
            CurEvent = null;
            curEventResults = saveData.curEventResults;
            eventInsts = saveData.eventInsts;
            twoOneNum = saveData.num21;
            cardInsts = saveData.cardInsts;
            hadUsecardInsts = saveData.hadUsecardInsts;
            hadSolveEventInsts = saveData.hadSolvecardInsts;
            RoundNum = saveData.RoundNum;
            Hp = saveData.HP;
            Process = saveData.process;
            processE29Time = saveData.processE29Time;

            Pleasant = saveData.pleasant;
            Vit = saveData.vit;
            processTwo = saveData.processTwo;

            FirstVit0 = saveData.FirstVit0;
            SecondVit0 = saveData.SecondVit0;
            ThrVit0 = saveData.ThrVit0;
            FourVit0 = saveData.FourVit0;

            FirstPlea0 = saveData.FirstPlea0;
            SecondPlea0 = saveData.SecondPlea0;
            ThrPlea0 = saveData.ThrPlea0;
            FourPlea0 = saveData.FourPlea0;
            GameController.instance.LoadGOpos(saveData.gameobjectBool);
            GameController.instance.LoadEventPos(saveData);
        }

        public void LoadSettingData()
        {
            data.SaveData settingData = data.DataArchiver.Load<data.SaveData>(SAVE_FILE_NAME);
            if(settingData == null)
            {
                settingData = new data.SaveData();
            }
            AudioMgr.instance.SetMusicVoice(settingData.musicVoice);
            AudioMgr.instance.SetEnvirVoice(settingData.envirVoice);
            AudioMgr.instance.SetSoundVoice(settingData.soundVoice);
            game.GameController.instance.isInNewPlayFlow = settingData.isPlayNewPlayer;
            game.GameController.instance.isInNewPlayFlow2 = settingData.isPlayNewPlayer;
        }

        public void SaveData()
        {
            saveData = new data.SaveData();
            saveData.curCard = CurCard;
            saveData.curEvent = CurEvent;
            saveData.curEventResults = curEventResults;
            saveData.eventInsts = eventInsts;
            saveData.num21 = twoOneNum;
            saveData.cardInsts = cardInsts;
            saveData.hadUsecardInsts = hadUsecardInsts;
            saveData.hadSolvecardInsts = hadSolveEventInsts;
            saveData.RoundNum = RoundNum;
            saveData.HP = Hp;
            saveData.process = Process;
            saveData.processE29Time = processE29Time;

            saveData.pleasant = Pleasant;
            saveData.vit = Vit;
            saveData.processTwo = processTwo;



            saveData.FirstVit0 = FirstVit0;
            saveData.SecondVit0 = SecondVit0;
            saveData.ThrVit0 = ThrVit0;
            saveData.FourVit0 = FourVit0;
            saveData.FirstPlea0 = FirstPlea0;
            saveData.SecondPlea0 = SecondPlea0;
            saveData.ThrPlea0 = ThrPlea0;
            saveData.FourPlea0 = FourPlea0;


            saveData.musicVoice = AudioMgr.instance.musicVol;
            saveData.envirVoice = AudioMgr.instance.envirVol;
            saveData.soundVoice = AudioMgr.instance.soundVol;
            saveData.isPlayNewPlayer = game.GameController.instance.isInNewPlayFlow;
            saveData.isPlayNewPlayer = game.GameController.instance.isInNewPlayFlow2;
            saveData.gameobjectBool = GameController.instance.SaveGOpos();
            data.DataArchiver.Save(saveData, SAVE_FILE_NAME);
        }

        //清空存储的数据
        public void ClearData()
        {
            saveData = new data.SaveData();
            data.DataArchiver.Save(saveData, SAVE_FILE_NAME);
        }

        #endregion

        #region 流程相关
        //使用卡牌
        public void UseCard()
        {
            if(curEvent.config.ID == 17)
            {
                if(curCard.Canuse)
                {
                    return;
                }
            }
            if(curEvent == null || curCard == null || curEvent.HasComplete)
            {
                return;
            }
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameFlowCardCheck);
        }
        //重新展示数据
        public void ReShowData()
        {
            if(eventInsts != null)
            {
                List<EventInst> tempEventList = new List<EventInst>();
                foreach(var item in eventInsts)
                {
                    tempEventList.Add(item);
                }
                eventInsts.Clear();
                foreach(var item in tempEventList)
                {
                    PushEventToTable(item);
                }
            }
            if(cardInsts != null)
            {
                List<CardInst> tempCardList = new List<CardInst>();
                foreach(var item in cardInsts)
                {
                    tempCardList.Add(item);
                }
                cardInsts.Clear();
                foreach(var item in tempCardList)
                {
                    PushCardToTable(item);
                }
            }
        }

        #region 二阶段
        public bool FirstVit0 = true;
        public bool SecondVit0 = true;
        public bool ThrVit0 = true;
        public bool FourVit0 = true;
        public bool FirstPlea0 = true;
        public bool SecondPlea0 = true;
        public bool ThrPlea0 = true;
        public bool FourPlea0 = true;

        public bool processTwo = false;
        private int vit = 0;
        public int Vit
        {
            get
            {
                return vit;
            }
            set
            {
                if(!processTwo)
                {
                    return;
                }
                int last = 0;
                last = value < 0 ? 0 : value;
                if(last == 0)
                {
                    if(FirstVit0)
                    {
                        FirstVit0 = false;
                        PushEventToTable(53);
                        PlayTimeLineFixed(game.TimeLineType.AiFu, game.TimeLineAssetType.AiFuDef);
                        last += 20;
                        Pleasant = 0;
                        game.GameController.instance.Carda1zPos();
                        HideAllEventWhenChange();

                    }
                    else if(SecondVit0)
                    {
                        SecondVit0 = false;
                        PushEventToTable(58);
                        PushEventToTable(59);
                        PlayTimeLineFixed(game.TimeLineType.ZhengMian, game.TimeLineAssetType.ZhengMianDef);
                        last += 20;
                        Pleasant = 0;
                        game.GameController.instance.Cardz2bPos();
                        HideAllEventWhenChange();

                    }
                    else if(ThrVit0)
                    {
                        ThrVit0 = false;
                        PushEventToTable(60);
                        PushEventToTable(61);
                        PlayTimeLineFixed(game.TimeLineType.BeiMian, game.TimeLineAssetType.BeiMianDef);
                        last += 20;
                        Pleasant = 0;
                        game.GameController.instance.Carda1zPos();
                        HideAllEventWhenChange();

                    }
                    else if(FourVit0)
                    {
                        FourVit0 = false;
                        PlayTimeLineFixed(game.TimeLineType.QiCheng, game.TimeLineAssetType.QiChengDef);
                        HideAllEventWhenChange();

                    }
                }
                vit = last;
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_VitNum_Change);
            }
        }

        private int pleasant = 0;
        public int Pleasant
        {
            get
            {
                return pleasant;
            }
            set
            {
                if(!processTwo)
                {
                    return;
                }
                int last = 0;
                last = value < 0 ? 0 : value;
                if(last >= 100)
                {
                    if(FirstPlea0)
                    {
                        FirstPlea0 = false;
                        PushEventToTable(53);
                        PushEventToTable(54);
                        PlayTimeLineFixed(game.TimeLineType.AiFu, game.TimeLineAssetType.AiFuSucc);
                        Vit += 20;
                        last = 0;
                        game.GameController.instance.Carda1zPos();
                        HideAllEventWhenChange();
                    }
                }
                if(last >= 150)
                {
                    if(SecondPlea0)
                    {
                        SecondPlea0 = false;
                        PushEventToTable(58);
                        PushEventToTable(59);
                        PlayTimeLineFixed(game.TimeLineType.ZhengMian, game.TimeLineAssetType.ZhengMianSucc);
                        Vit += 20;
                        last = 0;
                        game.GameController.instance.Cardz2bPos();
                        HideAllEventWhenChange();

                    }
                }
                if(last >= 200)
                {
                    if(ThrPlea0)
                    {
                        ThrPlea0 = false;
                        PushEventToTable(60);
                        PushEventToTable(61);
                        PlayTimeLineFixed(game.TimeLineType.BeiMian, game.TimeLineAssetType.BeiMianSucc);
                        Vit += 20;
                        last = 0;
                        HideAllEventWhenChange();

                    }
                }
                if(last >= 300)
                {
                    if(FourPlea0)
                    {
                        FourPlea0 = false;
                        PlayTimeLineFixed(game.TimeLineType.QiCheng, game.TimeLineAssetType.QiChengSucc);
                        HideAllEventWhenChange();

                    }
                }
                AudioMgr.instance.PlaySound(AudioClips.AC_14);
                pleasant = last;
                //通知显示
                evt.EventManager.instance.Send<bool>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_PleasantNum_Change, false);
            }
        }
        public void HideAllEventWhenChange()
        {
            GameObject[] goes = GameObject.FindGameObjectsWithTag("EventPrefab");
            tempPos = goes;
            for(int i = 0; i < goes.Length; i++)
            {
                goes[i].SetActive(false);
            }
        }
        GameObject[] tempPos = null;
        public void ActiveAllEventWhenChange()
        {
            if(tempPos != null)
            {
                for(int i = 0; i < tempPos.Length; i++)
                {
                    tempPos[i].SetActive(true);
                }
            }
        }
        public void PlayTimeLineFixed(game.TimeLineType timeLineType, TimeLineAssetType timeLineAssetType = 0, DirectorWrapMode directorWrapMode = DirectorWrapMode.None)
        {
            ui.UiManager.instance.SetUIAlpha(ui.UiManager.instance.CreateUi<ui.PlayCanvas>(), 0, 1);
            game.GameController.instance.PlayActivePlayableAsset(timeLineType, timeLineAssetType, directorWrapMode);
        }
        //进入二阶段
        public void EnterToProcessTwo()
        {
            DelectAllEvent();
            processTwo = true;
            //计算结算体力值
            vit = (20 - roundNum) + 2 * hp;
            act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>().ShowSpecialVitShow(()=> {
                if(GameController.instance.isInNewPlayFlow2)
                {
                    game.TimeLineMgr.instance.PlayTimeline(game.TimeLineMgr.instance.newPlayerDir, act.game.GameController.instance.xinShouEr);
                    game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameFlowRoundEnd);
                }
                game.GameController.instance.models[2].gameObject.SetActive(true);
                ui.UiManager.instance.ControlMouseInput(true);
                PushEventToTable(39);
                PushEventToTable(45);
                PushEventToTable(49);
                act.ui.UiManager.instance.SetUIAlpha(ui.UiManager.instance.CreateUi<ui.PlayCanvas>(), 1, time: 0);
            });

            Pleasant = (int)process;
            roundNum = 0;
            //act.game.GameController.instance.mainCamera.SetActive(false);
            //act.game.GameController.instance.mainCameraTwo.SetActive(true);
            
        }
        #endregion

        #endregion

        #region 功能相关

        #region 删除卡牌、事件
        public void DelectEventByID(int ID)
        {
            for(int i = 0; i < eventInsts.Count; i++)
            {
                if(eventInsts[i].config.ID == ID)
                {
                    eventInsts[i].RoundNum = -1;
                    i--;
                }
            }
        }
        public void DelectCardByID(int ID)
        {
            for(int i = 0; i < cardInsts.Count; i++)
            {
                if(cardInsts[i].config.ID == ID)
                {
                    cardInsts[i].DestorySelf();
                    i--;
                }
            }
        }
        public void DelectCardByUID(int uID)
        {
            for(int i = 0; i < cardInsts.Count; i++)
            {
                if(cardInsts[i].UniqueId == uID)
                {
                    cardInsts[i].DestorySelf();
                    return;
                }
            }
        }

        public void DelectAllEvent()
        {
            for(int i = 0; i < eventInsts.Count; i++)
            {
                eventInsts[i].RoundNum = -1;
                i--;
            }
        }

        public void DelectAllCard()
        {
            for(int i = 0; i < cardInsts.Count; i++)
            {
                cardInsts[i].DestorySelf();
                i--;
            }
        }
        #endregion


        //执行卡牌事件检测（且：必须都满足）
        public bool CheckCardOnEventByBlend(List<List<ConditionInst>> conditionInsts, List<List<EffectInst>> effectInsts)
        {
            List<bool> results = new List<bool>();
            if(ConditionMgr.instance.CheckConditionByBlend(conditionInsts, out results))
            {
                EffectMgr.instance.ExcuteResult(effectInsts, results);
                return true;
            }
            return false;
        }

        //执行卡牌事件检测（或：只要有一个就行）
        public bool CheckAndExcuteBySplit(List<List<ConditionInst>> conditionInsts, List<List<EffectInst>> effectInsts)
        {
            List<bool> results = new List<bool>();
            if(ConditionMgr.instance.CheckConditionBySplit(conditionInsts, out results))
            {
                EffectMgr.instance.ExcuteResult(effectInsts, results);
                return true;
            }
            return false;
        }
        //是否场上存在该ID事件
        public bool IsCurEventContain(int id)
        {
            foreach(var item in eventInsts)
            {
                if(item.config.ID == id)
                {
                    return true;
                }
            }
            return false;
        }

        //获取已经用过的卡牌
        public int GetGrayCard()
        {
            int tempInt = 0;
            foreach(CardInst item in cardInsts)
            {
                if(!item.Canuse)
                {
                    tempInt++;
                }
            }
            return tempInt;
        }
        #region 推送卡牌和事件
        public void PushEventToTable(int eventID)
        {
            EventInst inst = game.EventMgr.instance.GetEventInstByID(eventID);
            PushEventToTable(inst);
            if(eventID > 38)
            {
                game.GameController.instance.LoadEventPos();
            }
        }
        public void PushCardToTable(int cardId)
        {
            AudioMgr.instance.PlaySound(AudioClips.AC_11);
            CardInst inst = game.CardMgr.instance.GetCardInstByID(cardId);
            PushCardToTable(inst);
        }
        public void PushEventToTable(EventInst inst)
        {
            evt.EventManager.instance.Send<game.EventInst>(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Event_Create, inst);
        }
        public void PushCardToTable(CardInst inst)
        {
            evt.EventManager.instance.Send<game.CardInst>(evt.EventGroup.CARD, (short)evt.CardEvent.Card_Create, inst);
        }
        #endregion
        #endregion

    }
    [System.Serializable]
    public class GameFlowCdtAndEft : Singleton<GameFlowCdtAndEft>
    {
        public List<ConditionEffectConfig> curTotalCEC = new List<ConditionEffectConfig>();

        public List<ConditionEffectConfig> RoundStartCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardWaitCheckCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckStartCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckOverCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckSuccCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardNumCheckDeffCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> CardUseOverCEC = new List<ConditionEffectConfig>();
        public List<ConditionEffectConfig> RoundEndCEC = new List<ConditionEffectConfig>();

        public void CheckCdt(List<ConditionEffectConfig> cdtList)
        {
            ConditionEffectConfig tempCEC;
            for(int i = cdtList.Count - 1; i >= 0; i--)
            {
                if(i >= cdtList.Count)
                {
                    continue;
                }
                tempCEC = cdtList[i];
                bool tempBool = tempCEC.CECheckByBlend();
                Debug.Log($"当前时点：{System.Enum.GetName(typeof(TimePoint), tempCEC.timePoint)};当前ID：{tempCEC.id}当前结果：{tempBool}");
            }
        }

        public void AddCECToList(ConditionEffectConfig CEC)
        {
            curTotalCEC.Add(CEC);
            switch(CEC.timePoint)
            {
                case TimePoint.TP_None:
                    break;
                case TimePoint.TP_RoundStart:
                    RoundStartCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardCheck:
                    CardWaitCheckCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckStart:
                    CardNumCheckStartCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckOver:
                    CardNumCheckOverCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckSucc:
                    CardNumCheckSuccCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardNumCheckDeff:
                    CardNumCheckDeffCEC.Add(CEC);
                    break;
                case TimePoint.TP_CardUseOver:
                    CardUseOverCEC.Add(CEC);
                    break;
                case TimePoint.TP_RoundEnd:
                    RoundEndCEC.Add(CEC);
                    break;
                default:
                    break;
            }
            Debug.Log($"欢迎该CEC进入流程 ：时点{CEC.timePoint}," + "  " + $"ID{CEC.id}," + "  " +
                $"条件1ID{(CEC.conditionInsts[0] != null ? (CEC.conditionInsts[0].config.ID.ToString() + "  " + $"说明：{localization.LocalizationManager.instance.GetLocalizedString(CEC.conditionInsts[0].config.desc, "ui_system_ssc")}") : "无")}," + "  " +
                $"效果1ID{(CEC.effectInsts[0] != null ? (CEC.effectInsts[0].config.ID.ToString() + $"说明：{localization.LocalizationManager.instance.GetLocalizedString(CEC.effectInsts[0].config.desc, "ui_system_ssc")}") : "无")} ");
        }
        public void RemoveCECToListByID(int uID)
        {
            List<ConditionEffectConfig> tempList = new List<ConditionEffectConfig>();
            foreach(var cec in curTotalCEC)
            {
                if(cec.id == uID)
                {
                    tempList.Add(cec);
                }
            }
            foreach(var cec in tempList)
            {
                RemoveCECToList(cec);
            }
        }
        public void RemoveCECToList(ConditionEffectConfig CEC)
        {
            curTotalCEC.Remove(CEC);
            switch(CEC.timePoint)
            {
                case TimePoint.TP_None:
                    break;
                case TimePoint.TP_RoundStart:
                    RoundStartCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardCheck:
                    CardWaitCheckCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckStart:
                    CardNumCheckStartCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckOver:
                    CardNumCheckOverCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckSucc:
                    CardNumCheckSuccCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardNumCheckDeff:
                    CardNumCheckDeffCEC.Remove(CEC);
                    break;
                case TimePoint.TP_CardUseOver:
                    CardUseOverCEC.Remove(CEC);
                    break;
                case TimePoint.TP_RoundEnd:
                    RoundEndCEC.Remove(CEC);
                    break;
                default:
                    break;
            }
            Debug.Log($"欢送该CEC走出流程 ：时点{CEC.timePoint}," + "  " + $"ID{CEC.id}," + "  " +
    $"条件1ID{(CEC.conditionInsts[0] != null ? (CEC.conditionInsts[0].config.ID.ToString() + "  " + $"说明：{localization.LocalizationManager.instance.GetLocalizedString(CEC.conditionInsts[0].config.desc, "ui_system_ssc")}") : "无")}," + "  " +
    $"效果1ID{(CEC.effectInsts[0] != null ? (CEC.effectInsts[0].config.ID.ToString() + $"说明：{localization.LocalizationManager.instance.GetLocalizedString(CEC.effectInsts[0].config.desc, "ui_system_ssc")}") : "无")} ");
        }
    }
}

