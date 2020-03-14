using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace act.ui
{
    public class CardReference : MonoBehaviour
    {
        [Header("Common")]
        public Image cardTypeBG = null;
        public UiStaticText Text_Name = null;
        public UiStaticText Text_Desc = null;
        public UiStaticText Text_TestNum = null;
        public UiStaticText Text_CardType = null;
        public UiStaticText[] Text_Conditions = null;
        public UiStaticText[] Text_Effects = null;

    }
}