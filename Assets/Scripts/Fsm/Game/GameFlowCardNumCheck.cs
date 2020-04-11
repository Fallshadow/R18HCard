﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class GameFlowCardNumCheck<T> : State<T>
    {
        public override void Enter()
        {
            Debug.Log("进入状态：检定卡牌成功或失败");
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckStartCEC);

            if(game.GameFlowMgr.instance.JumpUpTouzi)
            {
                game.GameFlowMgr.instance.JumpUpTouzi = false;
                m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowCardUseOver);
            }
            else
            {
                Touzi();
            }
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }


        public void Touzi()
        {
            float touziNum = 0;
            float maxNum = 0;
            List<float> result = new List<float>();
            game.RandomNumMgr.instance.GetRandomNum(out result, out touziNum, out maxNum);
            game.RollTouZiManager.instance.PlayRoll(touziNum, result, TouziCallBack, maxNum);
        }
        public void TouziCallBack()
        {
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckOverCEC);
            game.GameFlowMgr.instance.RandomNum = game.RollTouZiManager.instance.maxNum;
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change);
            if(!game.GameFlowMgr.instance.CurCard.ExcuteCheck())
            {
                //TODO:发信号！卡牌使用失败了
                Debug.Log("卡牌使用失败了QAQ");

                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def_Anim);
            }
            else
            {
                Debug.Log("卡牌使用成功了！！！");
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success_Anim);
            }
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowCardUseOver);
        }


    }
}

