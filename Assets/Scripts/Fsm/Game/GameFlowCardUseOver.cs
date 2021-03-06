﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class GameFlowCardUseOver<T> : State<T>
    {
        public override void Enter()
        {
            Debug.Log("进入状态：卡牌使用结束");

            game.GameFlowMgr.instance.RecordCurCard();
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardUseOverCEC);
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowWaitForCheck);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

