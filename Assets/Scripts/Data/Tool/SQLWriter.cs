using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

namespace data
{
    public class SQLWriter : MonoBehaviour
    {
        Dictionary<string, string[]> dicTable = new Dictionary<string, string[]>();

        public bool changeConfigSqlByIndex(string saveFileFolder, object obj, string strContent)
        {
            string strTable = saveFileFolder + "/" + obj.GetType() + ".sql";

            try
            {
                if (File.Exists(strTable))
                {
                    //读取
                    FileStream fs = new FileStream(strTable, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);

                    bool startInsertData = false;
                    for (int line = 0; !sr.EndOfStream; line++)
                    {
                        string strLine = sr.ReadLine();
                        if (strLine == null)
                            break;

                        if (strLine.Contains("INSERT BEGIN"))
                        {
                            startInsertData = true;
                            // next line brgin
                            continue;
                        }

                        if (!startInsertData)
                        {
                            continue;
                        }

                        string[] cols = GetDataArray(strLine);
                        dicTable[cols[0]] = cols;
                    }

                    sr.Close();
                    fs.Close();
                }

                //修改
                string[] _cols = GetDataArray(strContent);
                dicTable[_cols[0]] = _cols;

                //写入
                StreamWriter table = new StreamWriter(strTable, false);
                WriteTitle(table);
                WriteCreateTable(table, obj);
                WriteInsertTable(table, obj, dicTable);

                table.Flush();
                table.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }

        void WriteTitle(StreamWriter table)
        {
            table.WriteLine("-- --------------------------------------------------------");
            table.WriteLine("-- Create Time:   " + System.DateTime.Now);
            table.WriteLine("-- Author:   Silver");
            table.WriteLine("-- @copyright   CopyRight By IGG Devp7");
            table.WriteLine("-- --------------------------------------------------------");
            table.WriteLine("");
            table.WriteLine("");
        }

        void WriteCreateTable(StreamWriter table, object obj)
        {
            table.WriteLine("--");
            table.WriteLine("-- Create Table: " + obj.GetType());
            table.WriteLine("--");
            table.WriteLine("DROP TABLE IF EXISTS `" + obj.GetType() + "`;");
            table.WriteLine("CREATE TABLE IF NOT EXISTS `" + obj.GetType() + "`(");
            System.Reflection.FieldInfo[] fieldInfos = obj.GetType().GetFields();
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                if (ConfigDataParser.getDescriptionNames(fieldInfos[i]) == null)
                {
                    continue;
                }
                string sqlType = ConfigDataParser.getSqlType(fieldInfos[i]);
                table.WriteLine("`" + fieldInfos[i].Name + "` " + sqlType + " COMMENT `" + fieldInfos[i].Name + "`,");
            }
            table.WriteLine(") ENGINE=MyISAM DEFAULT CHARSET=latin1;");
            table.WriteLine("");
            table.WriteLine("");
        }

        void WriteInsertTable(StreamWriter table, object obj, Dictionary<string, string[]> dicTable)
        {
            table.WriteLine("--");
            table.WriteLine("-- Initialization Table: " + obj.GetType());
            table.WriteLine("--");
            //below is reload data
            table.WriteLine("-- INSERT BEGIN");

            //write first line
            Dictionary<string, string[]>.Enumerator _row = dicTable.GetEnumerator();
            string strRow = "";
            int rowValueLength;
            if (dicTable.ContainsKey("ConfigId"))
            {
                _row.MoveNext();
                strRow = strRow = "INSERT INTO `" + GetSqlType(obj.GetType().ToString()) + "`(`";
                string[] rowValues = dicTable["ConfigId"];
                rowValueLength = rowValues.Length;
                for (int i = 0; i < rowValueLength; ++i)
                {
                    strRow += rowValues[i].Replace("`", "");
                    strRow += (i != rowValueLength - 1) ? "`,`" : "`) VALUES";

                }
                table.WriteLine(strRow);
                // remove parameters line and sort by first colm
                dicTable.Remove("ConfigId");
                IOrderedEnumerable<KeyValuePair<string, string[]>> items = from pair in dicTable
                                                                           orderby int.Parse(pair.Value[0]) ascending
                                                                           select pair;
                dicTable = items.ToDictionary(t => t.Key, t => t.Value);
            }

            _row = dicTable.GetEnumerator();
            while (_row.MoveNext())
            {
                strRow = "(";
                rowValueLength = _row.Current.Value.Length;
                for (int i = 0; i < rowValueLength; ++i)
                {
                    strRow += _row.Current.Value[i].Replace("`", "");
                    strRow += (i != rowValueLength - 1) ? "," : "),";
                }

                table.WriteLine(strRow);
            }
        }


        string[] GetDataArray(string str)
        {
            if (str.Contains("INSERT INTO"))
            {
                str = str.Split('(')[1];
                str = str.Split(')')[0];
                str = str.Replace("`", "");
                str += ',';
            }
            else
            {
                str = str.Replace("(", "");
                str = str.Replace(")", "");
            }

            string[] arr = str.Split(',');
            List<string> list = new List<string>();
            // remove last empty data
            for (int i = 0; i < arr.Length - 1; i++)
            {
                list.Add(arr[i]);
            }

            arr = list.ToArray();

            return arr;
        }

        string GetSqlType(string str)
        {
            str = str.Replace("`", "");

            return str;
        }
    }
}
