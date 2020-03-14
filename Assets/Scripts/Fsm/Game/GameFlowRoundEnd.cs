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
            foreach (var item in game.GameFlowMgr.instance.eventInsts)
            {
                item.RoundNum--;
            }
            foreach (var item in game.GameFlowMgr.instance.cardInsts)
            {
                item.RefreshUse();
            }
            CheckProcessCondition();
            game.GameFlowMgr.instance.RoundNum++;

            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over);
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

        public void SwitchToStart()
        {
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowRoundStart);
        }
    }
}

