using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AudioMgr))]
public class AudioMgrEditor : Editor
{
    private ReorderableList audioClips;
    private SerializedProperty Dur;
    private SerializedProperty outDur;
    private SerializedProperty MU1;
    private SerializedProperty MU2;
    private SerializedProperty Envir;


    private SerializedProperty SOUND; 
    private void OnEnable()
    {
        audioClips = new ReorderableList(serializedObject, serializedObject.FindProperty("audioClips")
          , true, true, true, true);




        //自定义列表名称
        audioClips.drawHeaderCallback = (Rect rect) =>
        {
            GUI.Label(rect, "audioClips");
        };

        //自定义绘制列表元素
        audioClips.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
        {
            //根据index获取对应元素
            SerializedProperty item = audioClips.serializedProperty.GetArrayElementAtIndex(index);
            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2;
            EditorGUI.PropertyField(rect, item, new GUIContent("Element " + index));
        };

        //当添加新元素时的回调函数，自定义新元素的值
        audioClips.onAddCallback = (ReorderableList list) =>
        {
            if(list.serializedProperty != null)
            {
                list.serializedProperty.arraySize++;
                list.index = list.serializedProperty.arraySize - 1;
                SerializedProperty item = list.serializedProperty.GetArrayElementAtIndex(list.index);
                item.stringValue = "Default Value";
            }
            else
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
            }
        };

        //当删除元素时候的回调函数，实现删除元素时，有提示框跳出
        audioClips.onRemoveCallback = (ReorderableList list) =>
        {
            if(EditorUtility.DisplayDialog("Warnning", "Do you want to remove this element?", "Remove", "Cancel"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            }
        };



        Dur = serializedObject.FindProperty("dur");
        MU1 = serializedObject.FindProperty("musicAS1");
        
MU2 = serializedObject.FindProperty("musicAS2");
        Envir = serializedObject.FindProperty("musicEnvir");
        SOUND = serializedObject.FindProperty("soundAS");
        outDur = serializedObject.FindProperty("outDur");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        audioClips.DoLayoutList();
        EditorGUILayout.PropertyField(Dur, true);
        EditorGUILayout.PropertyField(MU1, true);
        EditorGUILayout.PropertyField(MU2, true);
        EditorGUILayout.PropertyField(Envir, true);
        EditorGUILayout.PropertyField(SOUND, true);
        EditorGUILayout.PropertyField(outDur, true);
        
        serializedObject.ApplyModifiedProperties();
    }
}
