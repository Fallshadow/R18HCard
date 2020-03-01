using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace act.editor.ui
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(act.ui.UiStaticText))]
    public class UiStaticTextEditor : TMP_UiEditorPanel
    {
        protected static string[] groupIds = new string[] { "ui_system", "quest", "monster" };

        [MenuItem("GameObject/Custom/UiStaticText", false, 0)]
        public static void CreateGameObject()
        {
            // Create a custom game object
            GameObject go = new GameObject("SText");

            // Set the selection object as the parent 
            GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create UiStaticText");
            Selection.activeObject = go;
            go.AddComponent<RectTransform>();
            go.AddComponent<act.ui.UiStaticText>();
        }

        protected act.ui.UiStaticText sText = null;
        protected SerializedProperty isLocalizeOnAwake = null;
        protected SerializedProperty localizationId = null;
        protected SerializedProperty localizationGroup = null;
        protected SerializedProperty specificFont = null;
        protected SerializedProperty specificMaterial = null;
        //protected SerializedProperty style = null;
        protected int groupIdIndex = 0;
        protected int fontIndex = 0;
        protected int materialIndex = 0;

        protected override void OnEnable()
        {
            isLocalizeOnAwake = serializedObject.FindProperty("isLocalizeOnAwake");
            localizationId = serializedObject.FindProperty("localizationId");

            localizationGroup = serializedObject.FindProperty("localizationGroup");
            groupIdIndex = getIndex(groupIds, localizationGroup.stringValue);

            specificFont = serializedObject.FindProperty("specificFont");
            fontIndex = getIndex(act.ui.UiStaticText.FontNames, specificFont.stringValue);
            specificMaterial = serializedObject.FindProperty("specificMaterial");
            materialIndex = getIndex(act.ui.UiStaticText.MaterialNames, specificMaterial.stringValue);
            (target as act.ui.UiStaticText).SetLocalizedFont();
            //style = serializedObject.FindProperty("style");

            localization.LocalizationManager.instance.LoadModule(localizationGroup.stringValue);
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Localization Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(isLocalizeOnAwake, new GUIContent("Localize OnAwake"));
            EditorGUILayout.PropertyField(localizationId, new GUIContent("ID"));
            groupIdIndex = EditorGUILayout.Popup("Group", groupIdIndex, groupIds);
            if (EditorGUI.EndChangeCheck())
            {
                localizationGroup.stringValue = groupIds[groupIdIndex];
                serializedObject.ApplyModifiedProperties();
                if (isLocalizeOnAwake.boolValue)
                {
                    localization.LocalizationManager.instance.LoadModule(groupIds[groupIdIndex]);
                    (target as act.ui.UiStaticText).Localize();
                }
                serializedObject.Update();
            }

            EditorGUI.BeginChangeCheck();
            fontIndex = EditorGUILayout.Popup("Font", fontIndex, act.ui.UiStaticText.FontNames);
            materialIndex = EditorGUILayout.Popup("Material", materialIndex, act.ui.UiStaticText.MaterialNames);
            //EditorGUILayout.PropertyField(style); // TODO: Not finish yet.
            if (EditorGUI.EndChangeCheck())
            {
                specificFont.stringValue = act.ui.UiStaticText.FontNames[fontIndex];
                specificMaterial.stringValue = act.ui.UiStaticText.MaterialNames[materialIndex];
                serializedObject.ApplyModifiedProperties();
                (target as act.ui.UiStaticText).SetLocalizedFont();
                serializedObject.Update();

                // Update index.
                fontIndex = getIndex(act.ui.UiStaticText.FontNames, specificFont.stringValue);
                materialIndex = getIndex(act.ui.UiStaticText.MaterialNames, specificMaterial.stringValue);
            }

            EditorGUI.indentLevel--;
            base.OnInspectorGUI();
        }

        protected int getIndex(string[] ids, string id)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == id)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}