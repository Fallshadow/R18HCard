﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class GameFlowRoundStart<T> : State<T>
    {
        public override void Enter()
        {
            game.GameFlowMgr.instance.SaveData();
            Debug.Log("进入状态：回合开始");
            GiveInitCard();
            CheckProcessCondition();
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.RoundStartCEC);
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowWaitForCheck);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
        public void CheckProcessCondition()
        {
            foreach (var item in game.ProcessMgr.instance.processInsts)
            {
                if (item.conditionInst.Excute())
                {
                    foreach (var eventid in item.eventIds)
                    {
                        game.GameFlowMgr.instance.PushEventToTable(eventid);
                    }
                }
            }
        }
        public void GiveInitCard()
        {
            if (game.GameFlowMgr.instance.RoundNum == 1)
            {
                Debug.Log("给予卡片：1，2，3");
                game.GameFlowMgr.instance.PushCardToTable(1);
                game.GameFlowMgr.instance.PushCardToTable(2);
                game.GameFlowMgr.instance.PushCardToTable(3);
            }

        }
    }
}

