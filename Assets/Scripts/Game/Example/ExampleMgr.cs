using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using act.data;

namespace act.game
{
    public class ExampleMgr : Singleton<ExampleMgr>
    {
        Dictionary<int, ExampleData> dictExample = new Dictionary<int, ExampleData>();

        public void InitConfigData()
        {
            data.ConfigDataMgr.GetDataDictionary(dictExample, "ID", data.CONFIG_PATH.DICT_EXAMPLE);
        }
    }
}

