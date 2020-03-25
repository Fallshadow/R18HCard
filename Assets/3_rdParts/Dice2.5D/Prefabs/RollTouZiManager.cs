using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.ui;

namespace act.game
{
    public class RollTouZiManager : SingletonMonoBehavior<RollTouZiManager>
    {
        public Touzi[] touzi;
        public float maxNum;
        public float touziNum;
        public void PlayRoll(float touziNum, List<float> resultList, CallBack continueCheck, float maxNum)
        {
            this.touziNum = touziNum;
            for(int i = 0; i < touziNum; i++)
            {
                touzi[i].gameObject.SetActive(true);
                touzi[i].rollTouZi(resultList[i],continueCheck, ResetTouzi);
            }
            this.maxNum = maxNum;
        }

        public void ResetTouzi()
        {
            game.GameFlowMgr.instance.RandomNum = maxNum;
            for(int i = 0; i < touziNum; i++)
            {
                touzi[i].gameObject.SetActive(false);
            }
            evt.EventManager.instance.Send(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_RandomNum_Change);
        }
    }
}