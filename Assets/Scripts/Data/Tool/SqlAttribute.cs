using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "SMALLINT(5) UNSIGNED", 
/// "TINYINT(3) UNSIGNED",
/// "INT(10) UNSIGNED",
/// "VARCHAR(500)" => int, float, string array and list,
/// "DECIMAL(12,5)" => float, double
/// </summary>
public class SqlAttribute : Attribute
{
    public string sqlType { get; private set; }
    public SqlAttribute(string value)
    {
        this.sqlType = value;
    }
}

/// <summary>
/// 用來判斷是不是可擴增的表單
/// </summary>
public class ListAttribute : Attribute
{

}

//只是用于标识sql不用导出该字段
public class SqlRejectAttribute : Attribute
{

}
