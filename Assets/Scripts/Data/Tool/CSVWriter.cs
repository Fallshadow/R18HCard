using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;

namespace data
{
    public class CSVWriter
    {
        public void writeConfigCsv(string saveFileFolder, string fileName, List<string> contents)
        {
            string strTable = saveFileFolder + "/" + fileName + ".csv";
            StreamWriter table = new StreamWriter(strTable, false);

            for (int i = 0; i < contents.Count; i++)
            {
                table.WriteLine(contents[i]);
            }

            table.Flush();
            table.Close();
        }

        public void writeConfigCsv(string fullName, List<string> contents)
        {
            StreamWriter table = new StreamWriter(fullName, false);

            for (int i = 0; i < contents.Count; i++)
            {
                table.WriteLine(contents[i]);
            }

            table.Flush();
            table.Close();
        }

        public void CreateTextFile(string fileName, string textToAdd)
        {
            string logFile = DateTime.Now.ToShortDateString()
                .Replace(@"/", @"-").Replace(@"\", @"-") + ".log";

            StreamWriter swFromFile = new StreamWriter(logFile);
            swFromFile.Write(textToAdd);
            swFromFile.Flush();
            swFromFile.Close();

        }

        Dictionary<string, string[]> dicTable = new Dictionary<string, string[]>();

        public bool changeConfigCsvByIndex(string saveFileFolder, string strFileName, string strContent)
        {

            if (strFileName.Equals(""))
                return false;

            string strTable = saveFileFolder + "/" + strFileName + ".csv";

            try
            {
                if (File.Exists(strTable))
                {
                    //读取
                    FileStream fs = new FileStream(strTable, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);

                    for (int line = 0; !sr.EndOfStream; line++)
                    {
                        string strLine = sr.ReadLine();
                        if (strLine == null)
                            break;

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

                // if contain "ConfigId" sort by it
                if (dicTable.ContainsKey("ConfigId"))
                {
                    // write parameters line
                    string[] paraLine = dicTable["ConfigId"];
                    string firstStrRow = "";
                    int firstRowValueLength = paraLine.Length;
                    for (int i = 0; i < firstRowValueLength; ++i)
                    {
                        firstStrRow += paraLine[i];
                        firstStrRow += ",";
                    }
                    table.WriteLine(firstStrRow);

                    // remove parameters line and sort by first colm
                    dicTable.Remove("ConfigId");
                    IOrderedEnumerable<KeyValuePair<string, string[]>> items = from pair in dicTable
                                                                               orderby int.Parse(pair.Value[0]) ascending
                                                                               select pair;
                    dicTable = items.ToDictionary(t => t.Key, t => t.Value);
                }

                Dictionary<string, string[]>.Enumerator _row = dicTable.GetEnumerator();
                while (_row.MoveNext())
                {
                    string strRow = "";
                    int rowValueLength = _row.Current.Value.Length;
                    for (int i = 0; i < rowValueLength; ++i)
                    {
                        strRow += _row.Current.Value[i];
                        strRow += ",";
                    }
                    table.WriteLine(strRow);
                }

                table.Flush();
                table.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }

        string[] GetDataArray(string str)
        {
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
    }
}