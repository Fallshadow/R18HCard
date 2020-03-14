using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace act.ui
{
    public class EventReference : MonoBehaviour
    {
        [Header("Common")]
        public UiStaticText Text_Name = null;
        public UiStaticText Text_Desc = null;
        public UiStaticText Text_SPResultDesc = null;
        public UiStaticText Text_ResultDesc = null;
        public UiStaticText Text_Round = null;
        public UiStaticText[] Text_Conditions = null;
        public UiStaticText[] Text_Effects = null;

    }
}
