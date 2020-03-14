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
            game.GameFlowMgr.instance.CreatRandomNum();
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckOverCEC);
            if (!game.GameFlowMgr.instance.CurCard.ExcuteCheck())
            {

                //TODO:发信号！卡牌使用失败了
                Debug.Log("卡牌使用失败了QAQ");
                game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckDeffCEC);
            }
            else
            {
                Debug.Log("卡牌使用成功了！！！");
                game.GameFlowMgr.instance.CurCard.CheckCdt();
                game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckSuccCEC);
                game.GameFlowMgr.instance.CurEvent.ExcuteResult(game.GameFlowMgr.instance.curEventResults);
            }
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowCardUseOver);
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

