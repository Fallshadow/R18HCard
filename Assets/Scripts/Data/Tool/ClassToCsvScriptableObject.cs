using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ClassToCsvScriptableObject : ScriptableObject
{
    [Description("ConfigId"), Sql("INT(10)")]
    public uint ConfigId;

    [Description("ClassList"), Sql("VARCHAR(500)"), TypeConverter(typeof(converter.StringListConverter))]
    public List<string> ClassList = new List<string>();

    public int A;
    public bool B;
}
