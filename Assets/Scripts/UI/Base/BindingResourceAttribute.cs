using System;

namespace act.ui
{
    public class BindingResourceAttribute : Attribute
    {
        public string AssetId { get; private set; }

        public BindingResourceAttribute(string assetId)
        {
            AssetId = data.ResourcesPathSetting.UiPrefabFolder + assetId;
        }
    }
}