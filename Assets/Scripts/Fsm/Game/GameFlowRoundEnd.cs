using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class GameFlowRoundEnd<T> : State<T>
    {
        public override void Enter()
        {
            Debug.Log("进入状态：回合结束");
            EndRound();
            SwitchToStart();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }

        public void EndRound()
        {
            int tempCount = game.GameFlowMgr.instance.eventInsts.Count;
            for (int i = 0; i < tempCount; i++)
            {
                game.GameFlowMgr.instance.eventInsts[i].RoundNum--;
                if (tempCount > game.GameFlowMgr.instance.eventInsts.Count)
                {
                    i--;
                }
                tempCount = game.GameFlowMgr.instance.eventInsts.Count;
            }
            foreach (var item in game.GameFlowMgr.instance.cardInsts)
            {
                item.RefreshUse();
            }
            game.GameFlowMgr.instance.RoundNum++;
            game.GameFlowMgr.instance.CurCard = null;
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over);

            
        }



        public void SwitchToStart()
        {
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowRoundStart);
        }
    }
}

