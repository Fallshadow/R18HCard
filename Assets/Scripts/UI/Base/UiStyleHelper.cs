using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace act.ui
{
    public class UiStyleHelper : SingletonMonoBehavior<UiStyleHelper>
    {
        private string ActionCardColorHex = "#BD6B39";
        private string WordCardColorHex = "#CF8A68";
        private string EmotionCardColorHex = "#BD86CF";
        private string SpecialCardColorHex = "#a15c6d";

        public Color ActionCardColor;
        public Color WordCardColor;
        public Color EmotionCardColor;
        public Color SpecialCardColor;
        private void Start()
        {
            ColorUtility.TryParseHtmlString(ActionCardColorHex, out ActionCardColor);
            ColorUtility.TryParseHtmlString(WordCardColorHex, out WordCardColor);
            ColorUtility.TryParseHtmlString(EmotionCardColorHex, out EmotionCardColor);
            ColorUtility.TryParseHtmlString(SpecialCardColorHex, out SpecialCardColor);

        }
    }
}