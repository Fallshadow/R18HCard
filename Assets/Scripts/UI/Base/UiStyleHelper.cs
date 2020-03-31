using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace act.ui
{
    public class UiStyleHelper : SingletonMonoBehavior<UiStyleHelper>
    {
        private string ActionCardColorHex = "#bd6b39";
        private string WordCardColorHex = "#8e6e5e";
        private string EmotionCardColorHex = "#876492";
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