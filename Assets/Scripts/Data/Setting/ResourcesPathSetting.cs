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
        #endregion
        #region Resources Path
        public const string Config = "Config/";
        public const string ScriptableObject = "ScriptableObject/";
        public const string Textures = "Textures/";
        #endregion
        #region Config
        public static string ClassToConfigsFolder = Application.dataPath + "/Resources/Config/EditorConfig/";
        #endregion 
    }
}