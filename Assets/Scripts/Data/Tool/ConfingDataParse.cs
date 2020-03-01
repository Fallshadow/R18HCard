using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace data
{
    public class ConfigDataParser : Singleton<ConfigDataParser>
    {
        public const char SEPARATOR = ',';

        private static System.ComponentModel.Int64Converter _unused = new System.ComponentModel.Int64Converter();
        private static System.ComponentModel.DecimalConverter _unused2 = new System.ComponentModel.DecimalConverter();
        private static System.ComponentModel.ByteConverter _unused3 = new System.ComponentModel.ByteConverter();
        private static System.ComponentModel.CollectionConverter _unused4 = new System.ComponentModel.CollectionConverter();
        private static System.ComponentModel.CharConverter _unused5 = new System.ComponentModel.CharConverter();
        private static System.ComponentModel.SByteConverter _unused6 = new System.ComponentModel.SByteConverter();
        private static System.ComponentModel.Int16Converter _unused7 = new System.ComponentModel.Int16Converter();
        private static System.ComponentModel.UInt16Converter _unused8 = new System.ComponentModel.UInt16Converter();
        private static System.ComponentModel.Int32Converter _unused9 = new System.ComponentModel.Int32Converter();
        private static System.ComponentModel.UInt32Converter _unused10 = new System.ComponentModel.UInt32Converter();
        private static System.ComponentModel.Int64Converter _unused11 = new System.ComponentModel.Int64Converter();
        private static System.ComponentModel.UInt64Converter _unused12 = new System.ComponentModel.UInt64Converter();
        private static System.ComponentModel.DoubleConverter _unused13 = new System.ComponentModel.DoubleConverter();
        private static System.ComponentModel.SingleConverter _unused14 = new System.ComponentModel.SingleConverter();
        private static System.ComponentModel.BooleanConverter _unused15 = new System.ComponentModel.BooleanConverter();
        private static System.ComponentModel.StringConverter _unused16 = new System.ComponentModel.StringConverter();
        private static System.ComponentModel.DateTimeConverter _unused17 = new System.ComponentModel.DateTimeConverter();
        private static System.ComponentModel.TimeSpanConverter _unused19 = new System.ComponentModel.TimeSpanConverter();

        private static Dictionary<string, System.ComponentModel.TypeConverter> customConverters = new Dictionary<string, System.ComponentModel.TypeConverter>();

        static ClassToCsvScriptableObject ClassToCsvSO
        {
            get
            {
                return act.utility.LoadResources.LoadAsset<ClassToCsvScriptableObject>(act.data.ResourcesPathSetting.ScriptableObject + "ClassToCsvScriptableObject");
            }
        }

        static public List<TData> ParseTable<TData>(ConfigTable table) where TData : new()
        {
            List<TData> dataList = new List<TData>();

            System.Reflection.FieldInfo[] dataFieldList = getDataFields<TData>();

            IEnumerator<ConfigRow> rowIterator = table.getRowEnumerator();
            while (rowIterator.MoveNext())
            {
                ConfigRow configRow = rowIterator.Current;

                if (configRow == null)
                    continue;

                TData data = parseData<TData>(dataFieldList, table, configRow);

                dataList.Add(data);
            }

            return dataList;
        }

        static private System.Reflection.FieldInfo[] getDataFields<TData>()
        {
            Type dataType = typeof(TData);

            return dataType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        }

        static private TData parseData<TData>(System.Reflection.FieldInfo[] fieldInfoList, ConfigTable table, ConfigRow configRow) where TData : new()
        {
            TData data = new TData();

            foreach (System.Reflection.FieldInfo fieldInfo in fieldInfoList)
            {
                string configValue = getConfigValue(fieldInfo, table, configRow);

                if (!string.IsNullOrEmpty(configValue))
                {
                    try
                    {
                        System.ComponentModel.TypeConverter typeConverter = getTypeConverter(fieldInfo);
                        object fieldValue = typeConverter.ConvertFromString(configValue);
                        fieldInfo.SetValue(data, fieldValue);
                    }
                    catch (Exception)
                    {
                        act.debug.PrintSystem.LogError("[CONFIG] error in field : " + fieldInfo.Name);
                        throw;
                    }
                }
            }

            return data;
        }

        static private System.ComponentModel.TypeConverter getTypeConverter(System.Reflection.FieldInfo fieldInfo)
        {
            string customConverterName = getTypeConverterName(fieldInfo);

            // Default type converter
            if (string.IsNullOrEmpty(customConverterName))
            {
                return System.ComponentModel.TypeDescriptor.GetConverter(fieldInfo.FieldType);
            }

            // Custom type converter
            if (!customConverters.ContainsKey(customConverterName))
            {
                Type converterType = Type.GetType(customConverterName);
                System.ComponentModel.TypeConverter customConverter = Activator.CreateInstance(converterType) as System.ComponentModel.TypeConverter;

                customConverters[customConverterName] = customConverter;
            }

            return customConverters[customConverterName];
        }

        static private string getTypeConverterName(System.Reflection.FieldInfo fieldInfo)
        {
            System.ComponentModel.TypeConverterAttribute[] attributes = (System.ComponentModel.TypeConverterAttribute[])fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.TypeConverterAttribute), false);
            return (attributes.Length > 0) ? attributes[0].ConverterTypeName : null;
        }

        static private string getConfigValue(System.Reflection.FieldInfo fieldInfo, ConfigTable table, ConfigRow configRow)
        {
            string[] columnNames = getDescriptionNames(fieldInfo);

            if (columnNames != null)
            {
                string configValue = string.Empty;

                int columnNamesLength = columnNames.Length;
                for (int i = 0; i < columnNamesLength; i++)
                {
                    if (i > 0)
                    {
                        configValue += SEPARATOR;
                    }

                    string columnName = columnNames[i];
                    int columnIndex = 0;
                    string cellValue = "";

                    ListAttribute[] attributes = (ListAttribute[])fieldInfo.GetCustomAttributes(typeof(ListAttribute), false);
                    if (attributes.Length > 0)
                    {
                        //如果是多欄位要合成list 就執行這邊
                        string columnNameTemp = columnName + 0;
                        columnIndex = table.getColumnIndex(columnNameTemp);
                        if (columnIndex != -1)
                        {
                            cellValue = configRow.getStringValue(columnIndex);
                            if (!string.IsNullOrEmpty(cellValue))
                                configValue = cellValue;
                        }

                        for (int j = 1; j < 100; j++)
                        {
                            columnNameTemp = columnName + j;
                            columnIndex = table.getColumnIndex(columnNameTemp);
                            if (columnIndex != -1)
                            {
                                cellValue = configRow.getStringValue(columnIndex);
                                if (!string.IsNullOrEmpty(cellValue))
                                    configValue = configValue + ":" + cellValue;
                            }
                        }
                    }
                    else
                    {
                        columnIndex = table.getColumnIndex(columnName);
                        cellValue = configRow.getStringValue(columnIndex);
                        configValue += cellValue;
                    }
                }

                return configValue;
            }

            return null;
        }

        static public string[] getDescriptionNames(System.Reflection.FieldInfo fieldInfo)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                string descriptionName = attributes[0].Description;
                string[] values = ((string)descriptionName).Split(new char[] { SEPARATOR });

                return values;
            }

            return null;
        }

        static public Dictionary<TKey, TData> ParseTable<TKey, TData>(ConfigTable table)
            where TKey : new()
            where TData : new()
        {
            Dictionary<TKey, TData> dataDictionary = new Dictionary<TKey, TData>();

            System.Reflection.FieldInfo[] keyFieldList = getDataFields<TKey>();
            System.Reflection.FieldInfo[] dataFieldList = getDataFields<TData>();

            IEnumerator<ConfigRow> rowIterator = table.getRowEnumerator();
            while (rowIterator.MoveNext())
            {
                ConfigRow configRow = rowIterator.Current;

                if (configRow == null)
                    continue;

                TKey key = parseData<TKey>(keyFieldList, table, configRow);
                TData data = parseData<TData>(dataFieldList, table, configRow);

                dataDictionary.Add(key, data);
            }

            return dataDictionary;
        }

        #region ClassToCsv
        public static void GetFieldInfoLine(out string fieldInfoDataLine, object obj)
        {
            List<string> stringData = new List<string>();
            System.Reflection.FieldInfo[] finfos;
            if (obj is IEnumerable)
            {
                List<object> oo = new List<object>((IEnumerable<object>)obj);
                finfos = oo[0].GetType().GetFields();
            }
            else
            {
                finfos = obj.GetType().GetFields();
            }

            fieldInfoDataLine = "";

            //set first colm is "ConfigId"
            for (int i = 0; i < finfos.Length; i++)
            {
                if (getDescriptionNames(finfos[i]) == null)
                {
                    continue;
                }
                if (finfos[i].Name == "ConfigId")
                {
                    fieldInfoDataLine += finfos[i].Name + ",";
                    break;
                }
            }

            for (int i = 0; i < finfos.Length; i++)
            {
                if (finfos[i].Name == "ConfigId")
                {
                    continue;
                }
                if (getDescriptionNames(finfos[i]) == null)
                {
                    continue;
                }
                fieldInfoDataLine += finfos[i].Name + ",";
            }
        }

        public static void GetFieldInfoValuesLine(object data, out string fieldInfoValue)
        {
            List<string> stringData = new List<string>();
            Type t = data.GetType();
            System.Reflection.FieldInfo[] finfos = data.GetType().GetFields();

            fieldInfoValue = "";

            for (int i = 0; i < finfos.Length; i++)
            {
                if (getDescriptionNames(finfos[i]) == null)
                {
                    continue;
                }
                if (finfos[i].Name == "ConfigId")
                {
                    System.ComponentModel.TypeConverter typeConverter = getTypeConverter(finfos[i]);
                    string valueString = typeConverter.ConvertToString(finfos[i].GetValue(data));
                    fieldInfoValue += valueString + ",";
                    break;
                }
            }


            for (int i = 0; i < finfos.Length; i++)
            {
                if (finfos[i].Name == "ConfigId")
                {
                    continue;
                }
                if (getDescriptionNames(finfos[i]) == null)
                {
                    continue;
                }

                if (finfos[i].FieldType.IsClass || finfos[i].FieldType.IsGenericType)
                {
                    if (finfos[i].FieldType != typeof(string))
                    {
                        GetGeneraterAndCreate(finfos[i].FieldType.ToString(), finfos[i].GetValue(data));
                    }
                }

                System.ComponentModel.TypeConverter typeConverter = getTypeConverter(finfos[i]);
                string valueString = typeConverter.ConvertToString(finfos[i].GetValue(data));
                fieldInfoValue += valueString + ",";

            }
        }

        /// <summary>
        /// only below define class can use
        /// </summary>
        /// <param name="typeString"></param>
        /// <param name="list"></param>
        public static void GetGeneraterAndCreate(string typeString, object list)
        {
            act.debug.PrintSystem.Log("[ClassToCsvEditor]GetGeneraterAndCreate! (" + typeString);

            if (ClassToCsvSO.ClassList.Contains(typeString))
            {
                CreateConfigCsv(list);
            }
            else
            {
                act.debug.PrintSystem.LogWarning("[ClassToCsvEditor]not get class type! (" + typeString);
            }
        }


        static void CreateConfigCsv(object obj)
        {
            string typeName = "";
            Dictionary<string, string> dicTable = new Dictionary<string, string>();
            List<object> list = new List<object>();
            if (obj is IEnumerable)
            {
                list = new List<object>((IEnumerable<object>)obj);
            }
            else
            {
                list.Add(obj);
            }

            foreach (object o in list)
            {
                string lineValue = "";
                GetFieldInfoValuesLine(o, out lineValue);
                if (!dicTable.ContainsKey(lineValue.Split(',')[0]))
                {
                    dicTable.Add(lineValue.Split(',')[0], lineValue);
                }
            }
            typeName = list[0].GetType().ToString();

            List<string> csvDataLines = new List<string>();
            string fieldInfoDatasLine = "";
            GetFieldInfoLine(out fieldInfoDatasLine, obj);
            csvDataLines.Add(fieldInfoDatasLine);

            Dictionary<string, string>.Enumerator _row = dicTable.GetEnumerator();
            while (_row.MoveNext())
            {
                csvDataLines.Add(_row.Current.Value);
            }

            data.CSVWriter cSVWriter = new data.CSVWriter();

            for (int i = 0; i < csvDataLines.Count; i++)
            {
                cSVWriter.changeConfigCsvByIndex(act.data.ResourcesPathSetting.ClassToConfigsFolder, typeName, csvDataLines[i]);
            }

            data.SQLWriter sqlWriter = new data.SQLWriter();

            for (int i = 0; i < csvDataLines.Count; i++)
            {
                sqlWriter.changeConfigSqlByIndex(act.data.ResourcesPathSetting.ClassToConfigsFolder, list[0], csvDataLines[i]);
            }

            act.debug.PrintSystem.Log("[ClassToCsvEditor]CreateMonsterConfigCsv Done!");
        }
        #endregion

        #region Sql
        static public string getSqlType(System.Reflection.FieldInfo fieldInfo)
        {
            SqlAttribute[] attributes = (SqlAttribute[])fieldInfo.GetCustomAttributes(typeof(SqlAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].sqlType;
            }

            return "[NULL]";
        }
        #endregion
    }
}

