using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace converter
{
    public class StringListConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string stringValue = value as string;
                string[] arr = stringValue.Split(':');
                List<string> list = new List<string>();
                foreach (string s in arr)
                {
                    list.Add(s);
                }
                return list;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(List<string>))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is List<string>)
            {
                List<string> stringList = value as List<string>;
                string stringValues = "";
                for (int i = 0; i < stringList.Count; i++)
                {
                    stringValues += stringList[i];
                    if (i != stringList.Count - 1)
                    {
                        stringValues += ":";
                    }
                }
                return stringValues;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
