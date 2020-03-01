using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace data
{
    public class ConfigTable
    {
        protected int _colCount = 0;
        protected CsvReader reader = new CsvReader();

        public ConfigTable()
        {
        }

        static public int getEnumCount<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Length;
        }

        public bool checkColCount<T>() where T : struct, IConvertible
        {
            int nCount = getEnumCount<T>();
            return nCount == _colCount;
        }

        //初始化读取文件到队列
        public bool init(string strName, bool bExternal = false)
        {
            reader.release();
            try
            {
                if (bExternal)
                {
                    if (!reader.openCsvFile(strName))
                        return false;
                }
                else
                {
                    if (!reader.openCsvInRecesoure(strName))
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message + "\n" + ex.StackTrace);
                return false;
            }

            return true;
        }

        //初始化读取文件到队列
        public bool initByPath(string strName)
        {
            reader.release();
            try
            {
                if (!reader.openCsvByPath(strName))
                    return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }

        public int getColumnIndex(string columnName)
        {
            return reader.getColumnIndex(columnName);
        }

        public ConfigRow getRow(int index)
        {
            return reader.getRow(index);
        }

        public int getRowCount()
        {
            return reader.getRowCount();
        }

        public IEnumerator<ConfigRow> getRowEnumerator()
        {
            return reader.getRowEnumerator();
        }

        //public ConfigRow[] rows
        //{
        //    get { return this._rowList.ToArray(); }
        //}

        //public List<ConfigRow> RowList
        //{
        //    get { return this._rowList; }
        //}

        //根据条件值返回行
        public ConfigRow getRow(System.Enum key, string value)
        {
            int count = reader.getRowCount();
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (row == null)
                    continue;
                string temp = "";
                if (row.getStringValue(key, out temp))
                {
                    if (temp == value)
                        return row;
                }
            }
            return null;
        }

        //根据条件值返回行
        public ConfigRow getRow(System.Enum key, int value)
        {
            int count = reader.getRowCount();
            int col = -1;
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (row == null)
                    continue;

                if (col == -1)
                    col = row.getIndex(key);

                if (col >= 0 && row.equalsValue(col, value))
                    return row;
            }
            return null;
        }

        public ConfigRow[] getRows(ConfigRow[] srcList, System.Enum key, int value)
        {
            if (srcList == null)
                return null;
            List<ConfigRow> listRow = new List<ConfigRow>();

            int col = -1;
            int count = reader.getRowCount();
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (row == null)
                    continue;

                if (col == -1)
                    col = row.getIndex(key);

                if (col >= 0 && row.equalsValue(col, value))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }

        public ConfigRow[] getRows(System.Enum key, int value)
        {
            List<ConfigRow> listRow = new List<ConfigRow>();
            int count = reader.getRowCount();
            int col = -1;
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (col == -1)
                    col = row.getIndex(key);

                if (col >= 0 && row.equalsValue(key, value))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }

        public ConfigRow getRow(System.Enum key1, int value1, System.Enum key2, int value2)
        {
            int count = reader.getRowCount();
            int col1 = -1;
            int col2 = -1;
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (row == null)
                    continue;

                if (col1 == -1)
                    col1 = row.getIndex(key1);

                if (col2 == -1)
                    col2 = row.getIndex(key2);
                if (col1 >= 0 && col2 >= 0 && row.equalsValue(col1, value1) && row.equalsValue(col2, value2))
                    return row;
            }
            return null;
        }

        public ConfigRow[] getRows(System.Enum key1, int value1, System.Enum key2, int value2)
        {
            List<ConfigRow> listRow = new List<ConfigRow>();
            int count = reader.getRowCount();
            int col1 = -1;
            int col2 = -1;

            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (row == null)
                    continue;

                if (col1 == -1)
                    col1 = row.getIndex(key1);

                if (col2 == -1)
                    col2 = row.getIndex(key2);
                if (col1 >= 0 && col2 >= 0 && row.equalsValue(key1, value1) && row.equalsValue(key2, value2))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }

        public ConfigRow getRow(System.Enum key1, int value1, System.Enum key2, int value2, System.Enum key3, int value3)
        {
            int count = reader.getRowCount();
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = getRow(index);
                if (row == null)
                    continue;
                if (row.equalsValue(key1, value1) &&
                    row.equalsValue(key2, value2) &&
                    row.equalsValue(key3, value3))
                    return row;
            }
            return null;
        }

        public ConfigRow[] getRows(System.Enum key1, int value1, System.Enum key2, int value2, System.Enum key3, int value3)
        {
            List<ConfigRow> listRow = new List<ConfigRow>();

            int count = reader.getRowCount();
            for (int index = 0; index < count; index++)
            {
                ConfigRow row = reader.getRow(index);
                if (row == null)
                    continue;
                if (row.equalsValue(key1, value1) &&
                    row.equalsValue(key2, value2) &&
                    row.equalsValue(key3, value3))
                    listRow.Add(row);
            }
            return listRow.ToArray();
        }
    }
}
