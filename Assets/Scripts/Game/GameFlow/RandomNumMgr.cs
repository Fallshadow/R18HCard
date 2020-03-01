using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumMgr : Singleton<RandomNumMgr>
{
    public float curTouziNum = 1;
    public float initTouziNum = 1;
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
    public float GetRandomNum()
    {
        float result = 0;
        if (futureTimeRandomNum.Count >= 1)
        {
            if (futureTimeRandomNum[0] > 0)
            {
                result = futureTimeRandomNum[0];
            }
            else
            {
                result = Random.Range(1, 7) * curTouziNum;
            }
            futureTimeRandomNum.RemoveAt(0);
        }
        else
        {
            result = Random.Range(1, 7) * curTouziNum;
        }
        return result;
    }
    public void Reset()
    {
        curTouziNum = initTouziNum;
    }
}
