
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    public class ProcessMgr : Singleton<ProcessMgr>
    {
        Dictionary<int, ProcessData> dictProcess = new Dictionary<int, ProcessData>();
        public List<ProcessInst> processInsts = new List<ProcessInst>();

        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictProcess, "ID", data.CONFIG_PATH.DICT_PROCESS);
            foreach (var item in dictProcess)
            {
                processInsts.Add(new ProcessInst(item.Value));
            }
        }

        public ProcessData GetProcessDataByID(int ID)
        {
            return dictProcess[ID];
        }
    }
}

