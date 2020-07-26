using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    //检查卡牌按钮点下
    public class GameFlowCardCheck<T> : State<T>
    {
        public bool NeedCheck = false;
        public override void Enter()
        {
            Debug.Log("进入状态：检测卡牌");
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardWaitCheckCEC);
            //if (!game.GameFlowMgr.instance.CurCard.Canuse)
            //{
            //    game.GameFlowMgr.instance.CurEvent = null;
            //    return;
            //}
            List<bool> commonResults = new List<bool>();

            if(game.GameFlowMgr.instance.CurEvent.config.ID == 17)
            {
                Debug.Log("卡牌使用了！");
                game.GameFlowMgr.instance.curEventResults = commonResults;
                m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowCardNumCheck);
            }
            else
            {
                if(!game.ConditionMgr.instance.CheckConditionBySplit(game.GameFlowMgr.instance.CurEvent.conditionInsts, out commonResults))
                {
                    //TODO:发信号！卡牌根本使用不了
                    Debug.Log("卡牌根本使用不了");
                    m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowWaitForCheck);
                    return;
                }
                Debug.Log("卡牌使用了！");
                game.GameFlowMgr.instance.curEventResults = commonResults;
                m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowCardNumCheck);
            }
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

