using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace act.utility
{
    public static class Common
    {
        public static string GetDescription(System.Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            System.ComponentModel.DescriptionAttribute[] attributes =
                (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static void ChooseEventTimeLineToPlay(int eventID)
        {
            string filename = "";
            filename = $"事件{eventID}Timeline";
            game.TimeLineMgr.instance.PlayPlayableAsset(filename);
        }
    }
}