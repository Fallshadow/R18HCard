using UnityEditor;
using UnityEngine;

namespace act.editor.ui
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(act.ui.UiDynamicText))]
    public class UiDynamicTextTextEditor : UnityEditor.UI.TextEditor
    {
        [MenuItem("GameObject/Custom/UiDynamicText", false, 0)]
        public static void CreateGameObject()
        {
            // Create a custom game object
            GameObject go = new GameObject("DText");

            // Set the selection object as the parent 
            GameObjectUtility.SetParentAndAlign(go, Selection.activeGameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create UiDynamicText");
            Selection.activeObject = go;
            go.AddComponent<RectTransform>();
            go.AddComponent<act.ui.UiDynamicText>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}