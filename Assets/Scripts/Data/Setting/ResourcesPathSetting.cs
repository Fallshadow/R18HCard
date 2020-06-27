using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace act.data
{
    public static class ResourcesPathSetting
    {
        #region UI
        public const string UiPrefabFolder = "Prefab/UI/";
        public const string UiAtlasFolder = "Atlases/";
        public const string DefaultSpriteUiTexture = "PicMiss";
        public const string PlayUICardPrefab = "Play/CardPrefab";
        public const string PlayUIEventPrefabBase = "Play/Event/EventPrefabBase";
        #endregion
        #region Resources Path
        public const string Config = "Config/";
        public const string LocalizationConfig = "Config/Localization/";
        public const string LocalizationFontAssetPath = "Fonts/{0}{1} SDF"; // NOTE: {FontName}{_Language}
        public const string LocalizationMaterialPath = "Fonts/{0}_{1}{2} Material"; // NOTE: {FontName}{MaterialName}{_Language}
        public const string ScriptableObject = "ScriptableObject/";
        public const string Textures = "Textures/";
        #endregion
        #region Config
        public static string ClassToConfigsFolder = Application.dataPath + "/Resources/Config/EditorConfig/";
        #endregion 

        public const string event5TimeLine = "事件5Timeline";
    }
}