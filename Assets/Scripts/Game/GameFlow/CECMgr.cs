using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CECMgr : Singleton<CECMgr>
{
    public const int cardStartID = 1000;
    public const int eventStartID = 1000000;
    public Dictionary<int, int> dictIDNum = new Dictionary<int, int>();
    public int GetUniqueId(int ID,int type)
    {
        int temp = 0;
        temp = ID * (type == 1 ? cardStartID : eventStartID);
        if(dictIDNum.ContainsKey(temp))
        {
            dictIDNum[temp] += 1; 
        }
        else
        {
            dictIDNum.Add(temp, 1);
        }
        return temp + dictIDNum[temp];
    }
}
