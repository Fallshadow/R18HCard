using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.game
{
    public class RandomNumMgr : Singleton<RandomNumMgr>
    {
        public float curTouziNum = 1;
        public float initTouziNum = 1;
        public float curTouziCheckNum = 0;
        public float justTouziCheckNum = 0;
        public float nextAddCheckNum = 0;//加持骰子点数
        public int resultChengNum = 1;//加持骰子翻倍
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
        //result才是骰子投出来的
        public void GetRandomNum(out List<float> result,out float touziNum,out float maxNum)
        {
            bool doSet = false;
            result = new List<float>();
            touziNum = curTouziNum;
            if (futureTimeRandomNum.Count >= 1)
            {
                if (futureTimeRandomNum[0] > 0)
                {
                    result.Add(futureTimeRandomNum[0]);
                    doSet = true;
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
            maxNum = curTouziCheckNum + nextAddCheckNum;
            maxNum *= resultChengNum;
            Debug.Log($"当前骰子正常数值:{maxNum}（是否被设定{doSet}）");

            Debug.Log($"当前骰子加持数值:{nextAddCheckNum}");

            Debug.Log($"当前骰子最终数值:{maxNum}");
            justTouziCheckNum = maxNum;
            nextAddCheckNum = 0;
            resultChengNum = 1;
        }
        public void ResetTouziNum()
        {
            curTouziNum = initTouziNum;
            curTouziCheckNum = 1;
        }
    }
}