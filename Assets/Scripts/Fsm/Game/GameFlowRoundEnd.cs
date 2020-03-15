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
            for (int i = 0; i < game.GameFlowMgr.instance.eventInsts.Count; i++)
            {
                game.GameFlowMgr.instance.eventInsts[i].RoundNum--;
                if (game.GameFlowMgr.instance.eventInsts[i].RoundNum == -1)
                {
                    i--;
                }
            }
            foreach (var item in game.GameFlowMgr.instance.cardInsts)
            {
                item.RefreshUse();
            }
            game.GameFlowMgr.instance.RoundNum++;

            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over);
        }



        public void SwitchToStart()
        {
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowRoundStart);
        }
    }
}

