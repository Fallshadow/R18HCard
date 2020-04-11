using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    public class RandomNumMgr : Singleton<RandomNumMgr>
    {
        public float curTouziNum = 1;
        public float curUseTouziTime = 1;
        public float initTouziNum = 1;
        public float initUseTouziTime = 1;
        public float curTouziCheckNum = 0;
        public List<float> futureTimeRandomNum = new List<float>();

        /// <summary>
        /// 设定接下来第几次骰子的数字
        /// </summary>
        public void SetRandomNumByTime(int number, int timeNum)
        {
            while (futureTimeRandomNum.Count < timeNum)
            {
                futureTimeRandomNum.Add(-1);
            }
            //要不要判断一下骰子数和当前要到达的数值。。
            futureTimeRandomNum[timeNum - 1] = number;
        }

        public void GetRandomNum(out List<float> result,out float touziNum,out float maxNum)
        {
            result = new List<float>();
            touziNum = curTouziNum;
            if (futureTimeRandomNum.Count >= 1)
            {
                if (futureTimeRandomNum[0] > 0)
                {
                    result.Add(futureTimeRandomNum[0]);
                }
                else
                {
                    for(int i = 0; i < curTouziNum; i++)
                    {
                        result.Add(Random.Range(1, 7));
                    }
                }
                futureTimeRandomNum.RemoveAt(0);
            }
            else
            {
                for(int i = 0; i < curTouziNum; i++)
                {
                    result.Add(Random.Range(1, 7));
                }
            }
            foreach(var item in result)
            {
                curTouziCheckNum = Mathf.Max(item, curTouziCheckNum);
            }
            maxNum = curTouziCheckNum;
            Debug.Log($"当前骰子应该的数值curTouziCheckNum:{curTouziCheckNum}");
            Debug.Log($"当前result数值:{result[0]}");
            ResetUseTouziTime();
        }
        public void ResetTouziNum()
        {
            curTouziNum = initTouziNum;
            curTouziCheckNum = 1;
        }
        public void ResetUseTouziTime()
        {
            curUseTouziTime = initUseTouziTime;
        }
    }
}