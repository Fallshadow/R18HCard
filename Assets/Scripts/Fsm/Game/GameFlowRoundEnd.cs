using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using act.utility;

namespace act.fsm
{
    public class GameFlowRoundEnd<T> : State<T>
    {

        public override void Enter()
        {
            Debug.Log("进入状态：回合结束");
            EndRound();
            SwitchToStart();
            //if(!EndRoundRTHasEventDie())
            //{
            //    SwitchToStart();
            //}
            //else
            //{
            //    CoroutineUtility.instance.EndRoundCoroutine();
            //    //转到Utility计时
            //    //SwitchToStart();
            //    //搬到如果有卡牌播放回合结束动画之后的回调去做
            //}

        }

        public override void Exit()
        {

        }
        
        public override void Update()
        {

        }

        //结束回合无返回
        public void EndRound()
        {
            game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.RoundEndCEC);

            //在场的事件数量
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
        public bool EndRoundRTHasEventDie()
        {
            //在场的事件数量
            int tempCount = game.GameFlowMgr.instance.eventInsts.Count;
            bool result = false;

            for(int i = 0; i < tempCount; i++)
            {
                game.GameFlowMgr.instance.eventInsts[i].RoundNum--;
                if(tempCount > game.GameFlowMgr.instance.eventInsts.Count)
                {
                    result = true;
                    i--;
                }
                tempCount = game.GameFlowMgr.instance.eventInsts.Count;
            }
            foreach(var item in game.GameFlowMgr.instance.cardInsts)
            {
                item.RefreshUse();
            }
            game.GameFlowMgr.instance.RoundNum++;
            game.GameFlowMgr.instance.CurCard = null;
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_Round_Over);
            return result;
        }


        public void SwitchToStart()
        {
            m_fsm.SwitchToState((int)fsm.GameFsmState.GameFlowRoundStart);
        }
    }
}

