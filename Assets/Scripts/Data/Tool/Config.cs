using System;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
    public class Config<T> where T : struct, IConvertible
    {
        Dictionary<T, ConfigTable> _cfgList = new Dictionary<T, ConfigTable>();

        public Config()
        {
        }

        public void release()
        {
            this._cfgList.Clear();
        }

        //获取指定模块的配置对象
        public ConfigTable getCfg(T key)
        {
            ConfigTable config = null;
            if (this._cfgList.TryGetValue(key, out config))
                return config;
            //加载配置文件
            return this.initCfg(key);
        }

        //自动校验字段个数是否和枚举匹配
        public ConfigTable getCfg<T1>(T key) where T1 : struct, IConvertible
        {
            ConfigTable config = this.getCfg(key);
            if (config == null)
                return null;
            if (config.checkColCount<T1>())
            {
                //act.debug.PrintSystem.Log(string.Format("read config {0}  failed!", key));
                return null;
            }
            return config;
        }

        public void releaseCfg(T key)
        {
            this._cfgList.Remove(key);
        }

        public ConfigTable initCfg(T key)
        {
            ConfigTable config = new ConfigTable();
            string strFileName = act.utility.Common.GetDescription(key as System.Enum);
            if (strFileName == null)
                return null;

            if (config.init(strFileName))
            {
                this._cfgList[key] = config;
                return config;
            }
            else
            {
                //act.debug.PrintSystem.Log(string.Format("read config {0}  failed!", strFileName));
            }
            return null;
        }
    }
}

