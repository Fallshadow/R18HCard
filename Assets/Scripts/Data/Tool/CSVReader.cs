using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace data
{
    public class CsvReader
    {
        protected List<string> _columnNameList = new List<string>();
        protected List<ConfigRow> _dataList = new List<ConfigRow>();
        protected int _colAmount = 0;

        public int rowAmount { get { return this._dataList.Count; } }
        public int columnAmount { get { return this._colAmount; } }

        public bool openCsvInRecesoure(string strFileName)
        {
            if (strFileName.Equals(""))
                return false;

            string strPath = act.data.ResourcesPathSetting.Config + strFileName;

            try
            {
                TextAsset buildText = null;
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    buildText = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/" + strPath + ".csv");
                }
                else

#endif
                {
                    buildText = act.utility.LoadResources.LoadAsset<TextAsset>(strPath);
                }
                StreamReader sr = new StreamReader(new MemoryStream(buildText.bytes)/*, Encoding.ASCII*/);
                bool bRet = openStream(sr);
                return bRet;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public bool openCsvByPath(string strFilePath)
        {
            if (strFilePath.Equals(""))
                return false;

            try
            {
                FileStream fs = new FileStream(strFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.ASCII);
                bool bRet = openStream(sr);
                return bRet;
            }
            catch
            {
                return false;
            }
        }

        protected virtual bool openStream(StreamReader sr)
        {
            if (sr == null)
                return false;
            int line = 0;
            try
            {

                for (; !sr.EndOfStream; line++)
                {
                    string strLine = sr.ReadLine();
                    if (strLine == null)
                        break;
                    string[] cols = strLine.Split(',');

                    if (line == 0)
                    {
                        _colAmount = cols.Length;
                        for (int i = 0; i < _colAmount; i++)
                        {
                            _columnNameList.Add(cols[i]);
                        }
                        continue;
                    }

                    ConfigRow newRow = new ConfigRow(_colAmount);

                    for (int col = 0; col < _colAmount; col++)
                    {
                        newRow.addValue(cols[col]);
                    }
                    _dataList.Add(newRow);
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message + " in line " + line + "\n" + ex.StackTrace);
                return false;
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
            return true;
        }

        public bool openCsvFile(string strFileName)
        {
            if (strFileName.Equals(""))
                return false;

            try
            {
                FileStream fs = new FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                bool bRet = openStream(sr);
                fs.Close();
                return bRet;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public int getColumnIndex(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return -1;

            return _columnNameList.IndexOf(columnName);
        }

        public ConfigRow getRow(int row)
        {
            if (row < 0 || row >= _dataList.Count)
                return null;

            return this._dataList[row];
        }

        public virtual void release()
        {
            if (_dataList != null)
            {
                _dataList.Clear();
            }
        }

        public int getRowCount()
        {
            return _dataList.Count;
        }

        public IEnumerator<ConfigRow> getRowEnumerator()
        {
            return _dataList.GetEnumerator();
        }
    }
}

