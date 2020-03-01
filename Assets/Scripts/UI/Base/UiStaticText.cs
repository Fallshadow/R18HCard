using TMPro;
using UnityEngine;

namespace act.ui
{
    public enum UiTextStyle
    {
        None = 0,
        Title = 1,
        NormalWhite = 2,
        NormalBlack,
        Player,
        Enemy,
        Free,
        Chat,
        Read,
        NormalMW,
        PlayerSP,
        EnemySP
    }

    public class UiStaticText : TextMeshProUGUI, localization.ILocalizable
    {
        public static string[] FontNames = new string[] { "HYQiHei", "impact" , "BEYNO", "ArialMT" };
        public static string[] MaterialNames = new string[] { "Default", "Outline" };

        [SerializeField] protected bool isLocalizeOnAwake = true;
        [SerializeField] protected string localizationId = string.Empty;
        [SerializeField] protected string localizationGroup = string.Empty;
        [SerializeField] protected string specificFont = FontNames[0];
        [SerializeField] protected string specificMaterial = MaterialNames[0];

        // TODO: Not finish yet.
        //[SerializeField] protected UiTextStyle style;


#if UNITY_EDITOR // NOTE: 因應底層的define.
        protected override void Reset()
        {
            base.Reset();
            raycastTarget = false;
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            SetLocalizedFont();

            if (isLocalizeOnAwake)
            {
                Localize();
            }
        }

        public void SetLocalizedFont(string specificFont)
        {
            TMP_FontAsset tempFont = localization.LocalizationManager.instance.GetLocalizedFont(specificFont);
            if (tempFont == null)
            {
                return;
            }
            font = tempFont;

            Material tempMaterial = localization.LocalizationManager.instance.GetLocalizedMaterial(specificMaterial, specificFont);
            if (tempMaterial == null)
            {
                specificMaterial = MaterialNames[0];
                fontMaterial = font.material;
                return;
            }

            fontMaterial = tempMaterial;
        }

        public void SetLocalizedFont()
        {
            SetLocalizedFont(specificFont);
        }

        public void Localize()
        {
            if (string.IsNullOrEmpty(localizationId))
            {
                return;
            }

            text = localization.LocalizationManager.instance.GetLocalizedString(localizationId, localizationGroup);
        }

        public void Localize(string[] args)
        {
            text = string.Format(
                localization.LocalizationManager.instance.GetLocalizedString(localizationId, localizationGroup),
                args);
        }
        public void Localize(string key)
        {
            text = localization.LocalizationManager.instance.GetLocalizedString(key, localizationGroup);
        }

        public void Localize(string key, string group)
        {
            text = localization.LocalizationManager.instance.GetLocalizedString(key, group);
        }

        public void Localize(string key, string group, params string[] args)
        {
            text = string.Format(
                localization.LocalizationManager.instance.GetLocalizedString(key, group),
                args);
        }
    }
}