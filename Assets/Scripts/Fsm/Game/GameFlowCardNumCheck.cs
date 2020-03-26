using System.Collections;
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
            if(!game.GameFlowMgr.instance.CurCard.ExcuteCheck())
            {
                //TODO:发信号！卡牌使用失败了
                Debug.Log("卡牌使用失败了QAQ");
                game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckDeffCEC);
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Def);
            }
            else
            {
                Debug.Log("卡牌使用成功了！！！");
                game.GameFlowMgr.instance.CurCard.CheckCdt();
                game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckSuccCEC);
                game.GameFlowMgr.instance.CurEvent.ExcuteResult(game.GameFlowMgr.instance.curEventResults);
                evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Card_Event_Success);
            }
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowCardUseOver);
        }
    }
}

